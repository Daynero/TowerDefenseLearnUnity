using System;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Node : MonoBehaviour
{
    public Action <Node> OnMouseUpButton;
    public Action <Node> OnMouseExitButton;
    public Renderer rend;
    private BuildManager _buildManager;

    private void Start()
    {
        rend = new Renderer();
    }

    public void Initialize (BuildManager buildManager)
    {
        _buildManager = buildManager;
    }

    public void OnMouseUp()
    {
        _buildManager.ClickOnNode(this);
        // Debug.Log("Click on Node");
        // OnMouseUpButton?.Invoke(this);
    }

    public void OnMouseExit()
    {
        _buildManager.ResetNodeColor(this);   
        // OnMouseExitButton?.Invoke(this);
    }
}