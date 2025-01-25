using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button playBtn = null;
    [SerializeField] private Button quitBtn = null;

    private void Awake()
    {
        playBtn?.onClick.AddListener(StartGame);
        quitBtn?.onClick.AddListener(QuitGame);
    }
    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        Debug.Log("Game Exited");
        Application.Quit();
    }
}
