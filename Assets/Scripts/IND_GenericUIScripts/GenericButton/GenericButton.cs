using UnityEngine;
using System;

[RequireComponent(typeof(BoxCollider2D))]
public class GenericButton : GenericButtonBase
{
    void Awake()
    {
        GetComponent<BoxCollider2D>().isTrigger = true;
    }

#region Mouse Interaction
    protected override void InitializeButton()
    {
        // Custom initialization for GameObject button
    }
    void OnMouseEnter()
    {
        HandleMouseEnter();
    }

    void OnMouseExit()
    {
        HandleMouseExit();
    }

    void OnMouseDown()
    {
        HandleMouseClick();
    }


    #endregion
}
