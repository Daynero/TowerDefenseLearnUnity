using UnityEngine;
using UnityEngine.UI;

public class NodeUI : MonoBehaviour
{
    [SerializeField] private GameObject ui;
    [SerializeField] private Text upgradeCost;
    [SerializeField] private Button upgradeButton;
    [SerializeField] private Text sellAmount;

    private Node target;

    public void SetTarget(Node _target)
    {
        target = _target;

        transform.position = target.GetBuildPosition();

        if (!target.isUpgrade)
        {
            upgradeCost.text = "$" + target.turretBlueprint.upgradeCost;
            upgradeButton.interactable = true;
        } else
        {
            upgradeCost.text = "DONE";
            upgradeButton.interactable = false;
        }

        sellAmount.text = "$" + target.turretBlueprint.GetSellAmount();

        ui.SetActive(true);
    }

    public void  HidePopUpMenu ()
    {
        ui.SetActive(false);
    }

    public void UpgradeTurret()
    {
        target.UpgradeTurret();
        BuildManager.instance.DeselectNode();
    }

    public void SellTurret ()
    {
        target.SellTurret();
        BuildManager.instance.DeselectNode();
    }
}
