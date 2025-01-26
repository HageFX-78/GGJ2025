using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;
    
    [Header("Current Stats")]
    public int currentWave = 0;
    public int bossKilled = 0;
    public bool bossModeActivated = false;
    public bool IS_GAMEOVER = false;

    [Header("Boss UI")]
    [SerializeField] private GameObject bossIndicator;
    
    [Header("Win Lose Panel")]
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject losePanel;
    
    private PlayerController player;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        
        winPanel.SetActive(false);
        losePanel.SetActive(false);
        bossIndicator.SetActive(bossModeActivated);
    }

    private void Start()
    {
        Time.timeScale = 1;
        AudioManager.PlayBGM(EAudio.GameBGM);
        Cursor.visible = false;
   
        EventManager.ConnectEvent(GameEvents.OnWinGame, HandleOnWinGame);
        EventManager.ConnectEvent(GameEvents.OnLoseGame, HandleOnLoseGame);
        EventManager.ConnectEvent(GameEvents.OnBossSpawn, HandleOnBossSpawn);
        EventManager.ConnectEvent(GameEvents.OnBossDefeated, HandleOnBossDefeat);
    }

    private void OnDisable()
    {
        EventManager.DisconnectEvent(GameEvents.OnWinGame, HandleOnWinGame);
        EventManager.DisconnectEvent(GameEvents.OnLoseGame, HandleOnLoseGame);
        EventManager.DisconnectEvent(GameEvents.OnBossSpawn, HandleOnBossSpawn);
        EventManager.DisconnectEvent(GameEvents.OnBossDefeated, HandleOnBossDefeat);

    }

    private void HandleOnWinGame()
    {
        IS_GAMEOVER = true;
        winPanel.SetActive(true);
        Cursor.visible = true;
    }

    private void HandleOnLoseGame()
    {
        IS_GAMEOVER = true;
        losePanel.SetActive(true);
        Cursor.visible = true;
    }

    private void HandleOnBossSpawn()
    {
        //Debug.Log("BOSS MODE ENABLED, NO MORE SPAWNING");
        bossIndicator.SetActive(true);
        bossModeActivated = true;
    }

    private void HandleOnBossDefeat()
    {
        //Debug.Log("BOSS MODE DISABLED, RESUME SPAWNING");
        bossIndicator.SetActive(false);
        bossModeActivated = false;
        bossKilled++;
    }
}
