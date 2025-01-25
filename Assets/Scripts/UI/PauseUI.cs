using UnityEngine;

public class PauseUI : MonoBehaviour
{
    bool isPaused = false;
    [SerializeField] GameObject pauseCanvas;
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
    }

    public void QuitGame()
    { 
        //return to main menu (wait for scenes to be finalize)
    }
}
