using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;

    private static bool isPaused = false;

    [Header("Pause")]
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button exitButton;
    
    [Header("Error Message")]
    [SerializeField] private TMP_Text errorMessage;
    [SerializeField] private float deleteTime = 3f;

    [Header("Display")]
    [SerializeField] private TMP_Text tokenText;
    [SerializeField] private TMP_Text allTokensText;
    [Space]
    [SerializeField] private TMP_Text currentTurnText;
    [SerializeField] private TMP_Text currentAreaTypeText;
    [Space]
    [SerializeField] private GameObject winnerPanel;
    [SerializeField] private TMP_Text winnerText;   

    [Header("Select")]
    [SerializeField] private GameObject selectPanel;
    [SerializeField] private GameObject rerollPanel;
    [Space]
    [SerializeField] private Button moveButton;
    [SerializeField] private Button placeButton;
    [SerializeField] private Button rerollButton;
    [SerializeField] private GameObject rerollPlz; 

    private void Awake()
    {
        instance = this;

        LocalPlayer player = FindAnyObjectByType<LocalPlayer>();
        moveButton.onClick.AddListener(() => player.SetState(Player.State.Move));
        placeButton.onClick.AddListener(() => player.SetState(Player.State.Place));
        rerollButton.onClick.AddListener(player.Reroll);

        resumeButton.onClick.AddListener(Pause);
        exitButton.onClick.AddListener(() =>
        {
            Time.timeScale = 1f;
            UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("Title");
            isPaused = false;
        });
    }

    private void Start()
    {
        TurnManager.gameSetEvent.AddListener(() =>
        {
            KeyValuePair<PlaceColor, Player> winner = GameManager.Players.Single(kv => kv.Value.CurrentState != Player.State.Die);
            winnerText.text = $"Game Set\nWinner is <color={winner.Key}>{winner.Value.name}</color>";
            winnerPanel.SetActive(true);
        });
    }

    public static void Pause()
    {
        if (isPaused)
        {
            Time.timeScale = 1f;
            instance.pausePanel.SetActive(false);
        }
        else
        {
            Time.timeScale = 0f;
            instance.pausePanel.SetActive(true);
        }
        isPaused = !isPaused;
    }

    public static void ErrorPresent(string message)
    {
        instance.errorMessage.text += message + '\n';
        instance.StartCoroutine(DeleteMessage(message.Length + 1));
    }

    private static IEnumerator DeleteMessage(int trimStart)
    {
        yield return MyUtilities.YieldCache.GetWaitForSeconds(instance.deleteTime);
        instance.errorMessage.text = instance.errorMessage.text[trimStart..];
    }

    public static void SetTokenCount(int count)
    {
        instance.tokenText.text = $"Token(s) : {count}";
    }

    public static void SetAllTokenCounts()
    {
        System.Text.StringBuilder sb = new();
        foreach (var item in GameManager.Players)
        {
            sb.AppendLine($"<color={item.Key}>{item.Value.name} : {item.Value.Token}</color>");
        }
        instance.allTokensText.text = sb.ToString();
    }

    public static void SetCurrentPlayer(string name)
    {
        instance.currentTurnText.text = name;
    }

    public static void SetCurrentAreaType(AreaType type)
    {
        instance.currentAreaTypeText.text = type.ToString();
    }

    public static void SetActiveSelectPanel(bool active)
    {
        instance.selectPanel.SetActive(active);
    }

    public static void SetActiveRerollPanel(bool active, bool warning = false)
    {
        instance.rerollPanel.SetActive(active);
        instance.rerollPlz.SetActive(warning);
    }
}
