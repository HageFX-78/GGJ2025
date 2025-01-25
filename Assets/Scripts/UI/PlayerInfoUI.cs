using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerInfoUI : MonoBehaviour
{
    //Health
    //Special Attack Bar
    //Combine threshold? EXP bar?
    [SerializeField] Slider _playerHp;
    [SerializeField] Slider _altFire;

    public static bool altFireReady = false;
    [SerializeField] float altFireCD = 5;
    private void Update()
    {
        // _playerHp.value = playerCurrentHp / playerMaxHP

        altFireReady = _altFire.value >= 1;

        if (_altFire.value < 1 )
        {
            _altFire.value += Time.deltaTime *  (1 / altFireCD);
        }

        if (Input.GetMouseButtonDown(0) && altFireReady) //Move to player fire later and add functionality 
        {
            _altFire.value = 0; 
            EventManager.FireEvent(GameEvents.IncreasePopCount, true);
        }
    }
}
