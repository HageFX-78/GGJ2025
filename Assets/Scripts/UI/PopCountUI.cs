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
    }

    private void Update()
    {
        _popCount.text = popCount.ToString();
    }
    
    public static void IncreasePopCount() // probably want to change to event system after merge
    { 
        popCount++;
    }
}
