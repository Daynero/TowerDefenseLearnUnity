using System;
using UnityEngine;

public class Node : MonoBehaviour
{
    public Action <Node> ONMouseUpButton;
    public Action <Node> ONMouseExitButton;
    public Renderer rend;

    private void Start()
    {
        rend = new Renderer();
    }

    public void OnMouseUp()
    {
        ONMouseUpButton.Invoke(this);
    }

    public void OnMouseExit()
    {
        ONMouseExitButton.Invoke(this);
    }
}