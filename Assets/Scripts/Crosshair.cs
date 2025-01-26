using UnityEngine;

public class Crosshair : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {
        //Set Cursor to not be visible
        Cursor.visible = false;
    }
    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        transform.position = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, Camera.main.nearClipPlane + 2));
    }
}
