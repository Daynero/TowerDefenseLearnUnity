using System;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Node : MonoBehaviour
{
    private Action <Node> _onMouseUpButton;
    private Action <Node> _onMouseExitButton;
    public Renderer rend;

    public void Initialize(Action<Node> clickOnNode,Action<Node> resetNodeColor)
    {
        _onMouseUpButton = clickOnNode;
        _onMouseExitButton = resetNodeColor;
    }
    private void Start()
    {
        rend = new Renderer();
    }

    public void OnMouseUp()
    {
       Debug.Log("Click on Node");
       _onMouseUpButton?.Invoke(this);
    }

    public void OnMouseExit()
    {
        _onMouseExitButton?.Invoke(this);
    }
}