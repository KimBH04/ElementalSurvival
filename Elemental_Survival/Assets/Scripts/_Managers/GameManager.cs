using MyUtilities;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static Dictionary<PlaceColor, Player> Players { get; private set; }

    public static bool IsStoped => Time.deltaTime == 0f;

    private void Awake()
    {
        Application.targetFrameRate = 60;

        Players = new();
        var foundPlayers = FindObjectsByType<Player>(FindObjectsSortMode.None);

        if (foundPlayers.Length > 4)
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        try
        {
            foreach (Player player in foundPlayers)
            {
                Players.Add(player.playerColor, player);
            }
        }
        catch (System.ArgumentException)
        {
            Players.Clear();
            int cnt = foundPlayers.Length;
            var cocktail = ArrayShuffle.Shuffle(new int[] { 0, 1, 2, 3 }[..cnt]);
            for (int i = 0; i < cnt; i++)
            {
                Players.Add(foundPlayers[i].playerColor = (PlaceColor)cocktail[i], foundPlayers[i]);
            }
        }

        Players = Players.OrderBy(p => p.Value.GetComponent<TurnObject>().priority).ToDictionary(x => x.Key, x => x.Value);
    }
}
