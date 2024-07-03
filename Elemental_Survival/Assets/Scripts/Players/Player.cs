using UnityEngine;
using UnityEngine.Events;

public enum PlaceColor
{
    Red,
    Yellow,
    Green,
    Blue,
    Black,
    White
}

public abstract class Player : MonoBehaviour, ITurnObject
{
    [SerializeField] private Transform man;

    protected Dice dice;

    public PlaceColor playerColor;

    private sbyte token = 0;

    protected int x = 0, z = 0;

    protected State currentState = State.Move;
    protected AreaType areaType = AreaType.Start;

    private readonly UnityEvent TurnEndEvent = new();
    private readonly UnityEvent DieEvent = new();

    protected readonly Ground[] groundArea = new Ground[16];

    public sbyte Token
    {
        get
        {
            return token;
        }
        protected set
        {
            token = value;
            UIManager.SetAllTokenCounts();
        }
    }

    public State CurrentState => currentState;

    public event UnityAction TurnEndAction
    {
        add => TurnEndEvent.AddListener(value);
        remove => TurnEndEvent.RemoveListener(value);
    }

    public event UnityAction DiedAction
    {
        add => DieEvent.AddListener(value);
        remove => DieEvent.RemoveListener(value);
    }

    private void Start()
    {
        man = GameObject.Find($"ES_man_{playerColor}").transform;
        dice = FindAnyObjectByType<Dice>();
        Token = 0;
    }

    protected bool Move(Ground ground)
    {
        if (ground.Walkable(playerColor, out bool isDiedOrMyColor))
        {
            if (Token < 5 && !isDiedOrMyColor)
            {
                Token++;
            }

            ground.ColoringPlace(PlaceColor.White);

            x = (int)ground.PlacePosition.x;
            z = (int)ground.PlacePosition.z;
            man.position = ground.PlacePosition;

            OnesTurnEnd();

            return true;
        }
        return false;
    }

    protected bool Place(Ground ground)
    {
        if (Token >= 1 && ground.Placable(playerColor, out bool isDiedColor))
        {
            if (isDiedColor)
            {
                if (Token >= 2)
                {
                    Token--;
                }
                else
                {
                    return false;
                }
            }
            Token--;

            ground.ColoringPlace(playerColor);

            OnesTurnEnd();

            return true;
        }
        return false;
    }

    public void Die()
    {
        currentState = State.Die;
        DieEvent?.Invoke();
        man.gameObject.SetActive(false);
    }

    public abstract void OnesTurn();

    public void OnesTurnEnd()
    {
        currentState = State.Idle;
        TurnEndEvent?.Invoke();
    }

    public enum State
    {
        Idle,
        Select,
        Move,
        Place,
        Die
    }
}
