using TMPro;
using UnityEngine;

public class PopCountUI : MonoBehaviour
{
    TextMeshProUGUI _popCount = null;
    static int popCount = 0;

    private void Awake()
    {
        _popCount = GetComponent<TextMeshProUGUI>();
        popCount = 0;
        _popCount.text = "0";
        EventManager.ConnectEvent(GameEvents.IncreasePopCount, IncreasePopCount);
    }
    
    public void IncreasePopCount()
    {
        popCount++;
        _popCount.text = popCount.ToString();
    }
}
