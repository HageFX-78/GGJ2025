using TMPro;
using UnityEngine;

public class TimerUI : MonoBehaviour
{
    bool gameStart;
    float gameTimer;
    TextMeshProUGUI _gameTimer = null;

    private void Awake()
    {
        _gameTimer = GetComponent<TextMeshProUGUI>();
    }
    void Start()
    {
        gameStart = true;
    }

    void Update()
    {
        if (gameStart)
        {
            gameTimer += Time.deltaTime;
            _gameTimer.text = gameTimer.ToString("00:00");
        }
    }
}
