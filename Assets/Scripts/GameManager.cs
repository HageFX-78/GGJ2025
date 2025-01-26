using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;
    
    [Header("Current Stats")]
    public int currentWave = 1;
    public int bossKilled = 0;
    public bool bossModeActivated = false;


    public bool IS_GAMEOVER = false;
   

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
    }

    private void Start()
    {
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
        
    }

    private void HandleOnLoseGame()
    {
        
    }

    private void HandleOnBossSpawn()
    {
        bossModeActivated = true;
        Debug.Log("BOSS MODE ENABLED, NO MORE SPAWNING");
    }

    private void HandleOnBossDefeat()
    {
        bossModeActivated = false;
        Debug.Log("BOSS MODE DISABLED, RESUME SPAWNING");

        bossKilled++;
    }
}
