using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildManager : MonoBehaviour
{
    [SerializeField] private NodeUI nodeUI;
    [SerializeField] private CameraController cameraController;
    [SerializeField] private Color hoverColor;
    [SerializeField] private Color notEnoughMoneyColor;
    [SerializeField] private Vector3 positionOffset;

    private TurretBlueprint turretToBuild;
    private Node _node;
    private Node selectedNode;
    private GameManager _gameManager;
    private Renderer rend;
    private Color startColor;
    private GameObject turret;
    private bool isUpgrade;
    private TurretBlueprint turretBlueprint;


    public GameObject buildEffect;
    public GameObject sellEffect;
    // public static BuildManager instance;

    public bool CanBuild => (turretToBuild != null && !cameraController.isDragging && !cameraController.isZooming);

    public bool HasMoney => _gameManager.PlayerMoney >= turretToBuild.cost;

    public BuildManager(GameManager gameManager)
    {
        _gameManager = gameManager;
    }

    private void Start()
    {
        rend = GetComponent<Renderer>();
        startColor = rend.material.color;
        
        _node.onMouseUpButton += () => ClickOnNode();
        _node.onMouseUpButton += () => rend.material.color = startColor;;
    }

    public BuildManager(Node node)
    {
        _node = node;
    }

    private void SelectNode(Node node)
    {
        if (selectedNode == node)
        {
            DeselectNode();
            return;
        }

        selectedNode = node;
        turretToBuild = null;

        nodeUI.SetTarget(node);
    }

    private void DeselectNode()
    {
        selectedNode = null;
        nodeUI.HidePopUpMenu();
    }
    
    public void SellTurret()
    {
        PlayerStats.instance.PlayerMoney += turretBlueprint.GetSellAmount();

        GameObject effect = (GameObject) Instantiate(buildManager.sellEffect, GetBuildPosition(), Quaternion.identity);
        Destroy(effect, 5f);

        Destroy(turret);
        turretBlueprint = null;
    }

    public void SelectTurretToBuild(TurretBlueprint turret)
    {
        turretToBuild = turret;
        DeselectNode();
    }

    public TurretBlueprint GetTurretToBuild()
    {
        return turretToBuild;
    }

    private void ClickOnNode()
    {
        IndicateCanBuild();
            
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (turret != null)
        {
            SelectNode(this);
            return;
        }

        if (!CanBuild)
            return;

        BuildTurret(buildManager.GetTurretToBuild());
    }
    
    private void BuildTurret(TurretBlueprint blueprint)
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

        GameObject effect = (GameObject) Instantiate(buildEffect, GetBuildPosition(), Quaternion.identity);
        Destroy(effect, 5f);

        isUpgrade = true;

        Debug.Log("Turret upgraded");
    }

    private Vector3 GetBuildPosition()
    {
        return transform.position + positionOffset;
    }
    
    private void IndicateCanBuild()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (!CanBuild)
            return;

        rend.material.color = HasMoney ? hoverColor : notEnoughMoneyColor;
    }
}