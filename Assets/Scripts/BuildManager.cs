using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using UnityEngine;
using UnityEngine.EventSystems;

public enum TurretType
{
    DefaultTurret,
    MissileLauncher,
    LaserBeamer
}

public class BuildManager : MonoBehaviour
{
    [SerializeField] private NodeUI nodeUI;
    [SerializeField] private CameraController cameraController;
    [SerializeField] private Color hoverColor;
    [SerializeField] private Color notEnoughMoneyColor;
    [SerializeField] private Vector3 positionOffset;

    private TurretBlueprint _turretToBuild;
    private Node _node;
    private Node _selectedNode;
    private GameManager _gameManager;
    private Renderer _rend;
    private Color _startColor;
    private GameObject _turret;
    private bool _isUpgrade; // Todo: Она где-то тягалась еще
    private TurretBlueprint _turretBlueprint;
    private TurretInfoSO _turretInfoSo;

    public Action<int> SellTurretAction;
    public Action<int> BuildTurretAction;
    public Action<int> UpgradeTurretAction;

    public GameObject buildEffect;
    public GameObject sellEffect;

    public bool CanBuild => (_turretToBuild != null && !cameraController.isDragging && !cameraController.isZooming);

    public bool HasMoney => _gameManager.PlayerMoney >= _turretToBuild.cost;

    public BuildManager(GameManager gameManager)
    {
        _gameManager = gameManager;
    }

    private void Start()
    {
        _rend = GetComponent<Renderer>();
        _startColor = _rend.material.color;
        
        _node.ONMouseUpButton += ClickOnNode;
        _node.ONMouseExitButton += ResetNodeColor;
    }

    private void ResetNodeColor(Node currentNode)
    {
        currentNode.rend.material.color = _startColor;
    }

    public void Initialize (Node node, TurretInfoSO turretInfoSo)
    {
        _node = node;
        _turretInfoSo = turretInfoSo;
    }

    private void SelectNode(Node currentNode)
    {
        if (_selectedNode == currentNode)
        {
            DeselectNode();
            return;
        }

        _selectedNode = currentNode;
        _turretToBuild = null;

        nodeUI.SetTarget(currentNode);
    }

    private void DeselectNode()
    {
        _selectedNode = null;
        nodeUI.HidePopUpMenu();
    }
    
    public void SellTurret()
    {
        SellTurretAction.Invoke(_turretBlueprint.GetSellAmount());

        GameObject effect = (GameObject) Instantiate(sellEffect, GetBuildPosition(), Quaternion.identity);
        Destroy(effect, 5f);

        Destroy(_turret);
        _turretBlueprint = null;
    }

    public void SelectTurretToBuild(TurretType turretType)
    {
        _turretToBuild = _turretInfoSo.turretArray.First(someTurret => someTurret.type == turretType);
        DeselectNode();
    }

    public TurretBlueprint GetTurretToBuild()
    {
        return _turretToBuild;
    }

    private void ClickOnNode(Node currentNode)
    {
        IndicateCanBuild();
            
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (_turret != null)
        {
            SelectNode(currentNode);
            return;
        }

        if (!CanBuild)
            return;

        BuildTurret(GetTurretToBuild());
    }
    
    private void BuildTurret(TurretBlueprint blueprint)
    {
        BuildTurretAction.Invoke(blueprint.cost);

        GameObject _turret = (GameObject) Instantiate(blueprint.prefab, GetBuildPosition(), Quaternion.identity);
        this._turret = _turret;

        _turretBlueprint = blueprint;

        GameObject effect = (GameObject) Instantiate(buildEffect, GetBuildPosition(), Quaternion.identity);
        Destroy(effect, 5f);
    }
    
    public void UpgradeTurret()
    {
        UpgradeTurretAction.Invoke(_turretBlueprint.upgradeCost);

        Destroy(this._turret);
        
        GameObject _turret = (GameObject) Instantiate(_turretBlueprint.upgradedPrefab, GetBuildPosition(), Quaternion.identity);
        this._turret = _turret;

        GameObject effect = (GameObject) Instantiate(buildEffect, GetBuildPosition(), Quaternion.identity);
        Destroy(effect, 5f);

        _isUpgrade = true;

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

        _rend.material.color = HasMoney ? hoverColor : notEnoughMoneyColor;
    }
}