using TMPro;
using UnityEngine;

public class PopCountUI : MonoBehaviour
{
    TextMeshProUGUI _popCount = null;
    static int popCount = 0;

    private void Start()
    {
        _popCount = GetComponent<TextMeshProUGUI>();
        popCount = 0;
        _popCount.text = $"Bubbles Popped: {popCount.ToString()}";
        EventManager.ConnectEvent(GameEvents.IncreasePopCount, IncreasePopCount);
    }
    
    public void IncreasePopCount()
    {
        popCount++;
        _popCount.text = $"Bubbles Popped: {popCount.ToString()}";
    }

    private void OnDestroy()
    {
        EventManager.DisconnectEvent(GameEvents.IncreasePopCount, IncreasePopCount);
    }
}
