using UnityEngine;
using UnityEngine.UI;

public class PlayerInfoUI : MonoBehaviour
{
    //Health
    //Special Attack Bar
    //Combine threshold? EXP bar?
    [SerializeField] Slider _playerHp;
    [SerializeField] Slider _altFire;

    float altFireCD;
    public void Start()
    {
        EventManager.ConnectEvent(GameEvents.AltFireSetCD, SetAltFireCD);
    }

    private void Update()
    {
        // _playerHp.value = playerCurrentHp / playerMaxHP

        if (_altFire.value < 1)
        {
            _altFire.value += Time.deltaTime * altFireCD;
        }
    }

    public void SetAltFireCD(object cd)
    { 
        _altFire.value = 0;
        altFireCD = (float)cd;
    }

    private void OnDisable()
    {
        EventManager.DisconnectEvent(GameEvents.AltFireSetCD, SetAltFireCD);
    }
}
