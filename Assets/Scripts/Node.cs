using UnityEngine;
using UnityEngine.EventSystems;

public class Node : MonoBehaviour
{
    [SerializeField] private BuildManager buildManager;
    [SerializeField] private Color hoverColor;
    [SerializeField] private Color notEnoughMoneyColor;
    [SerializeField] private Vector3 positionOffset;

    private GameObject turret;
    private Renderer rend;
    private Color startColor;
    
    [HideInInspector] public TurretBlueprint turretBlueprint;
    [HideInInspector] public bool isUpgrade = false;
    
    private void Start()
    {
        rend = GetComponent<Renderer>();
        startColor = rend.material.color;

        buildManager = BuildManager.instance;
    }

    public Vector3 GetBuildPosition()
    {
        return transform.position + positionOffset;
    }

    public void OnMouseUp()
    {
        IndicateCanBuild();
        
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (turret != null)
        {
            buildManager.SelectNode(this);
            return;
        }

        if (!buildManager.CanBuild)
            return;

        BuildTurret(buildManager.GetTurretToBuild());
    }

    public void BuildTurret(TurretBlueprint blueprint)
    {
        if (PlayerStats.instance.PlayerMoney < blueprint.cost)
        {
            Debug.Log("Not enough money to build that!");
            return;
        }

        PlayerStats.instance.PlayerMoney -= blueprint.cost;

        GameObject _turret = (GameObject) Instantiate(blueprint.prefab, GetBuildPosition(), Quaternion.identity);
        turret = _turret;

        turretBlueprint = blueprint;

        GameObject effect = (GameObject) Instantiate(buildManager.buildEffect, GetBuildPosition(), Quaternion.identity);
        Destroy(effect, 5f);

        Debug.Log("Money left: " + PlayerStats.instance.PlayerMoney);
    }

    public void UpgradeTurret()
    {
        if (PlayerStats.instance.PlayerMoney < turretBlueprint.upgradeCost)
        {
            Debug.Log("Not enough money to upgrade that!");
            return;
        }

        PlayerStats.instance.PlayerMoney -= turretBlueprint.upgradeCost;
        
        Destroy(turret);
        
        GameObject _turret = (GameObject) Instantiate(turretBlueprint.upgradedPrefab, GetBuildPosition(), Quaternion.identity);
        turret = _turret;

        GameObject effect = (GameObject) Instantiate(buildManager.buildEffect, GetBuildPosition(), Quaternion.identity);
        Destroy(effect, 5f);

        isUpgrade = true;

        Debug.Log("Turret upgraded");
    }

    public void SellTurret()
    {
        PlayerStats.instance.PlayerMoney += turretBlueprint.GetSellAmount();

        GameObject effect = (GameObject) Instantiate(buildManager.sellEffect, GetBuildPosition(), Quaternion.identity);
        Destroy(effect, 5f);

        Destroy(turret);
        turretBlueprint = null;
    }

    public void IndicateCanBuild()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (!buildManager.CanBuild)
            return;

        if (buildManager.HasMoney)
        {
            rend.material.color = hoverColor;
        }
        else
        {
            rend.material.color = notEnoughMoneyColor;
        }
    }

    public void OnMouseExit()
    {
        rend.material.color = startColor;
    }
}