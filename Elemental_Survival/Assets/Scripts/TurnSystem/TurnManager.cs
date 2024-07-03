using System.Linq;
using UnityEngine;
using MyUtilities;
using System.Collections;
using UnityEngine.Events;

public class TurnManager : MonoBehaviour
{
    [SerializeField] private OrderMode orderMode;

    private static TurnObject[] turnObjects;

    public static int playerCount;

    public static UnityEvent gameSetEvent = new();

    private void Awake()
    {
        var foundTurnObjects = FindObjectsByType<TurnObject>(FindObjectsSortMode.None);

        turnObjects = orderMode switch
        {
            OrderMode.InstanceID     => foundTurnObjects.OrderBy(t => t.GetInstanceID()).ToArray(),
            OrderMode.CustomPriority => foundTurnObjects.OrderBy(t => t.priority).ToArray(),
            OrderMode.Random         => ArrayShuffle.Shuffle(foundTurnObjects),
            _                        => throw new UnityException($"Unassigned Mode : {{{orderMode}}}"),
        };

        playerCount = turnObjects.Length;
        for (int i = 0; i < playerCount; i++)
        {
            turnObjects[i].priority = i;
        }
    }

    private void Start()
    {
        StartCoroutine(TurnEveryTurn());
    }

    private static IEnumerator TurnEveryTurn()
    {
        int index = 0;
        while (playerCount > 1)
        {
            if (!turnObjects[index].Losed)
            {
                turnObjects[index].TurnStart();
                UIManager.SetCurrentPlayer(turnObjects[index].name);

                yield return turnObjects[index].Wait;
            }
            index++;
            index %= turnObjects.Length;
        }

        gameSetEvent.Invoke();
    }

    private enum OrderMode
    {
        InstanceID,
        CustomPriority,
        Random
    }
}
