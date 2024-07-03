using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleControl : MonoBehaviour
{
    [SerializeField] private GameObject selectCanvas;
    
    private Board board;

    public void StartButton()
    {
        if (board)
        {
            selectCanvas.SetActive(true);
        }
        else
        {
            StartCoroutine(LoadScene());
        }
    }

    private IEnumerator LoadScene()
    {
        yield return SceneManager.LoadSceneAsync("Board", LoadSceneMode.Additive);

        board = FindAnyObjectByType<Board>(FindObjectsInactive.Include);

        selectCanvas.SetActive(true);
    }

    public void SelectButton(string typeStr)
    {
        if (System.Enum.TryParse(typeStr, out BoardType type))
        {
            board.boardType = type;
            board.gameObject.SetActive(true);

            SceneManager.UnloadSceneAsync("Title");
            SceneManager.LoadSceneAsync("Main", LoadSceneMode.Additive);
        }
    }

    public void ExitButton()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
