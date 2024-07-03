using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public interface ITurnObject
{
    void OnesTurn();

    void OnesTurnEnd();

    event UnityAction TurnEndAction;

    event UnityAction DiedAction;
}

public class TurnObject : MonoBehaviour
{
    public int priority;

    private ITurnObject turnObj;
    private readonly TurnEndYield turnEnd = new();

    public IEnumerator Wait => turnEnd;

    public bool IsTurnEnd => turnEnd.isTurnEnd;
    public bool Losed => turnEnd.losed;

    private void Awake()
    {
        if (TryGetComponent(out turnObj))
        {
            turnObj.TurnEndAction += TurnEnd;
            turnObj.DiedAction += Lose;
        }
    }

    public void TurnStart()
    {
        turnEnd.isTurnEnd = false;
        turnObj.OnesTurn();
    }

    public void TurnEnd()
    {
        turnEnd.isTurnEnd = true;
    }

    public void Lose()
    {
        turnEnd.losed = true;
        TurnManager.playerCount--;
    }

    private class TurnEndYield : CustomYieldInstruction
    {
        public bool isTurnEnd = false;
        public bool losed = false;

        public override bool keepWaiting
        {
            get
            {
                return !isTurnEnd && !losed;
            }
        }
    }
}