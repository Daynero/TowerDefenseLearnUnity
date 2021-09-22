using System;
using UnityEngine;

public class Node : MonoBehaviour
{
    private Action<Node, Renderer> _onMouseUpButton;
    private Action<Renderer> _onMouseExitButton;
    private Renderer _rend;

    public GameObject AssignedTurret { get; set; }

    public void Initialize(Action<Node, Renderer> clickOnNode, 
        Action<Renderer> resetNodeColor)
    {
        _onMouseUpButton = clickOnNode;
        _onMouseExitButton = resetNodeColor;
    }

    private void Start()
    {
        _rend = GetComponent<Renderer>();
    }

    public void OnMouseUp()
    {
        _onMouseUpButton?.Invoke(this, _rend);
    }

    public void OnMouseExit()
    {
        _onMouseExitButton?.Invoke(_rend);
    }
}