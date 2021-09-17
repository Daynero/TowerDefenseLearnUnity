using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class Node : MonoBehaviour
{
    public Action onMouseUpButton;
    public Action onMouseExitButton;

    public void OnMouseUp()
    {
        onMouseUpButton.Invoke();
    }

    public void OnMouseExit()
    {
        onMouseExitButton.Invoke();
    }
    
    // Из Node в buildManager нужно передавать какой node сейчас выбран

}