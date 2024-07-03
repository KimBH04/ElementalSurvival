using MyUtilities;
using System.Collections;
using System.Linq;
using UnityEngine;

public class Computer : Player
{
    public override void OnesTurn()
    {
        StartCoroutine(TurnAction());
    }

    private IEnumerator TurnAction()
    {
        if (currentState == State.Idle)
        {
            if (Token == 0)
            {
                currentState = State.Move;
            }
            else
            {
                currentState = (State)Random.Range(2, 4);
            }
            yield return dice.Roll();
            areaType = (AreaType)(2 - dice.Value % 2);
        }
        UIManager.SetCurrentAreaType(areaType);

        int n = Board.GetGrounds(areaType, x, z, groundArea);
        var around = ArrayShuffle.Shuffle(groundArea[..n]).Where(g => g);
        foreach (var g in around)
        {
            yield return YieldCache.GetWaitForSeconds(0.1f);
            if (currentState == State.Move)
            {
                if (Move(g))
                {
                    yield break;
                }
            }
            else if (currentState == State.Place)
            {
                if (Place(g))
                {
                    yield break;
                }
            }
        }

        if (Token >= 2)
        {
            Token -= 2;

            yield return dice.Roll();
            areaType = (AreaType)(2 - dice.Value % 2);

            foreach (var g in around)
            {
                yield return YieldCache.GetWaitForSeconds(0.1f);
                if (currentState == State.Move)
                {
                    if (Move(g))
                    {
                        yield break;
                    }
                }
                else if (currentState == State.Place)
                {
                    if (Place(g))
                    {
                        yield break;
                    }
                }
            }
        }

        Die();
    }
}
