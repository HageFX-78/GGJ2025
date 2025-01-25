using System;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    public CinemachineCamera virtualMachine;
    public float zoomSpeed = 1f;
    public float minZoom = 5f;
    public float maxZoom = 10f;

    [SerializeField] private InputActionReference scrollWheel;

    // Update is called once per frame
    void Update()
    {
        HandleZoom();
    }

    void HandleZoom()
    {
        float scrollValue = scrollWheel.action.ReadValue<Vector2>().y;
        if (scrollValue != 0.0f)
        {
            float newSize = virtualMachine.Lens.OrthographicSize - scrollValue * zoomSpeed;
            virtualMachine.Lens.OrthographicSize = Mathf.Clamp(newSize, minZoom, maxZoom);
        }

    }
}
