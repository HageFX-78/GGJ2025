using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseUI : MonoBehaviour
{
    bool isPaused = false;
    [SerializeField] GameObject pauseCanvas;
    [SerializeField] GameObject settingsCanvas;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        pauseCanvas.SetActive(isPaused);
        Time.timeScale = isPaused ? 0.0f : 1.0f;
    }

    public void ToggleSettings()
    { 
        //Toggle settings menu, if needed depending on what settings we have
        settingsCanvas.SetActive(!settingsCanvas.activeSelf);

    }

    public void QuitGame()
    { 
        SceneManager.LoadScene(0);
    }
}
