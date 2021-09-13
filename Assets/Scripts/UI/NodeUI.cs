using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class NodeUI : MonoBehaviour
{
    [SerializeField] private GameObject ui;
    [SerializeField] private Text upgradeCost;
    [SerializeField] private Button upgradeButton;
    [SerializeField] private Text sellAmount;
    [SerializeField] private float appearPopUpSpeed;
    [SerializeField] private CanvasGroup canvasGroup;

    private Node selectedTurrel;
    private float popUpAlpha;

    private void Start()
    {
        
    }

    private IEnumerator AnimateAppear()
    {
        while (canvasGroup.alpha < 1 && ui.activeInHierarchy)
        {
            popUpAlpha += appearPopUpSpeed * Time.deltaTime;
            canvasGroup.alpha = popUpAlpha;
            
            yield return null;
        }
    }

    public void SetTarget(Node _target)
    {
        selectedTurrel = _target;

        transform.position = selectedTurrel.GetBuildPosition();

        if (!selectedTurrel.isUpgrade)
        {
            upgradeCost.text = "$" + selectedTurrel.turretBlueprint.upgradeCost;
            upgradeButton.interactable = true;
        } else
        {
            upgradeCost.text = "DONE";
            upgradeButton.interactable = false;
        }

        sellAmount.text = "$" + selectedTurrel.turretBlueprint.GetSellAmount();

        ui.SetActive(true);
        popUpAlpha = 0;
        canvasGroup.alpha = 0;

        StartCoroutine(AnimateAppear());
    }

    public void  HidePopUpMenu ()
    {
        ui.SetActive(false);
    }

    public void UpgradeTurret()
    {
        selectedTurrel.UpgradeTurret();
        BuildManager.instance.DeselectNode();
    }

    public void SellTurret ()
    {
        selectedTurrel.SellTurret();
        BuildManager.instance.DeselectNode();
    }
}
