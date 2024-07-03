using System.Collections;
using System.Linq;
using UnityEngine;

public class LocalPlayer : Player
{
#pragma warning disable IDE0051 // Remove unused private members
    private void OnClick()
    {
        if (GameManager.IsStoped)
        {
            return;
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, float.PositiveInfinity, 0b1000000))
        {
            if (hit.transform.TryGetComponent(out Ground ground))
            {
                if (currentState == State.Idle)
                {
                    UIManager.ErrorPresent("It is not your turn!");
                }
                else if (currentState == State.Select)
                {
                    UIManager.ErrorPresent("You need to select!");
                }
                else if (currentState == State.Die)
                {
                    UIManager.ErrorPresent("You was lose!");
                }
                else if (groundArea.Contains(ground))
                {
                    switch (currentState)
                    {
                    case State.Move:
                        if (Move(ground))
                        {
                            UIManager.SetTokenCount(Token);
                            UIManager.SetActiveRerollPanel(false);
                        }
                        break;

                    case State.Place:
                        if (Place(ground))
                        {
                            UIManager.SetTokenCount(Token);
                            UIManager.SetActiveRerollPanel(false);
                        }
                        break;

                    default:
                        break;
                    }
                }
                else
                {
                    UIManager.ErrorPresent("You can't select there");
                }
            }
        }
    }

    private void OnPause()
    {
        UIManager.Pause();
    }
#pragma warning restore IDE0051 // Unity Input System Method

    public override void OnesTurn()
    {
        if (currentState == State.Idle)
        {
            currentState = State.Select;
            UIManager.SetActiveSelectPanel(true);
        }
        else
        {
            Board.GetGrounds(areaType, x, z, groundArea);
        }
    }

    public void SetState(State state)
    {
        if (state == State.Place && Token == 0)
        {
            UIManager.ErrorPresent("Not enough tokens");
            return;
        }

        currentState = state;
        StartCoroutine(SetStateCoroutine());

        UIManager.SetActiveSelectPanel(false);
    }

    private IEnumerator SetStateCoroutine()
    {
        yield return dice.Roll();
        areaType = (AreaType)(2 - dice.Value % 2);
        Board.GetGrounds(areaType, x, z, groundArea);
        UIManager.SetCurrentAreaType(areaType);

        bool walkable = AbleGroundsCheck(currentState);
        if (Token >= 2)
        {
            UIManager.SetActiveRerollPanel(true, !walkable);
        }
        else if (!walkable)
        {
            Die();
        }
    }

    public void Reroll()
    {
        Token -= 2;
        UIManager.SetTokenCount(Token);

        StartCoroutine(RerollCoroutine());
        UIManager.SetActiveRerollPanel(false);
    }

    private IEnumerator RerollCoroutine()
    {
        yield return dice.Roll();
        areaType = (AreaType)(2 - dice.Value % 2);

        Board.GetGrounds(areaType, x, z, groundArea);

        if (!AbleGroundsCheck(currentState))
        {
            Die();
        }

        UIManager.SetCurrentAreaType(areaType);
    }

    private bool AbleGroundsCheck(State state)
    {
        if (state == State.Move)
        {
            return groundArea.Any(g => g && g.Walkable(playerColor, out bool isDiedColor) && (!isDiedColor || Token >= 2));
        }
        else if (state == State.Place)
        {
            return groundArea.Any(g => g && g.Placable(playerColor, out bool isDiedColor) && (!isDiedColor || Token >= 2));
        }
        throw new UnityException("It is unselectable state or unassigned State : " + state);
    }
}
