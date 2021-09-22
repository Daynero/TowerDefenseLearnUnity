using System;
using System.Linq;
using Data;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public enum TurretType
{
    DefaultTurret,
    MissileLauncher,
    LaserBeamer
}

public class BuildManager : MonoBehaviour
{
    [SerializeField] private CameraController cameraController;
    [SerializeField] private Color hoverColor;
    [SerializeField] private Color notEnoughMoneyColor;
    [SerializeField] private Vector3 positionOffset;

    [SerializeField] private TMP_Text upgradeCost;
    [SerializeField] private Button upgradeButton;
    [SerializeField] private TMP_Text sellAmount;
    [SerializeField] private float appearPopUpSpeed;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private GameObject nodeCanvasUI;

    private TurretBlueprint _turretToBuild;
    private Node _selectedNode;
    private GameManager _gameManager;
    private Color _startColor = Color.white;
    private bool _isUpgrade;
    private TurretBlueprint _turretBlueprint;
    private TurretInfoSO _turretInfoSo;
    private float _popUpAlpha;
    private Node[] _nodes;

    private bool CanShowPopUp => !cameraController.isDragging && !cameraController.isZooming;

    public Action<int> SellTurretAction;
    public Action<int> BuildTurretAction;
    public Action<int> UpgradeTurretAction;

    public GameObject buildEffect;
    public GameObject sellEffect;


    private bool CanBuild => _turretToBuild != null && !cameraController.isDragging && !cameraController.isZooming;

    private bool HasMoney => _turretToBuild != null && _gameManager.PlayerMoney >= _turretToBuild.cost;

    public void Initialize(TurretInfoSO turretInfoSo, GameManager gameManager)
    {
        _turretInfoSo = turretInfoSo;
        _gameManager = gameManager;

        _nodes = FindObjectsOfType<Node>();

        foreach (var node in _nodes)
        {
            node.Initialize(ClickOnNode, ResetNodeColor);
        }
    }

    private void ClickOnNode(Node currentNode, Renderer rend)
    {
        // if (EventSystem.current.IsPointerOverGameObject())
        //     return;

        if (currentNode.AssignedTurret != null)
        {
            // todo: не с первого клика по турели вылазит попап
            //_selectedNode = null;
            SelectNode(currentNode);
            return;
        }

        if (!CanBuild)
            return;

        if (!HasMoney)
            return;

        rend.material.color = HasMoney ? hoverColor : notEnoughMoneyColor;

        _selectedNode = currentNode;

        BuildTurret(_turretToBuild);
    }

    private void SelectNode(Node currentNode)
    {
        if (_selectedNode == currentNode)
        {
            Debug.Log("_selectedNode == currentNode");
            DeselectNode();
            return;
        }

        //_selectedNode = currentNode;
        _turretToBuild = null;

        SetTargetForNodeUI(currentNode);
    }

    private void ResetNodeColor(Renderer rend)
    {
        rend.material.color = _startColor;
    }

    private void DeselectNode()
    {
        _selectedNode = null;
        HidePopUpMenu();
    }


    public void SelectTurretToBuild(TurretType turretType)
    {
        _turretToBuild = _turretInfoSo.turretArray.First(someTurret => someTurret.type == turretType);
        Debug.Log("turret to build cost" + _turretToBuild.cost);
        DeselectNode();
    }


    private void BuildTurret(TurretBlueprint blueprint)
    {
        Debug.Log("create turret " + blueprint.prefab);
        BuildTurretAction.Invoke(blueprint.cost);

        GameObject turret = Instantiate(blueprint.prefab, GetBuildPosition(_selectedNode), Quaternion.identity);
        _selectedNode.AssignedTurret = turret;

        _turretBlueprint = blueprint;

        GameObject effect = Instantiate(buildEffect, GetBuildPosition(_selectedNode), Quaternion.identity);
        Destroy(effect, 5f);
    }


    private Vector3 GetBuildPosition(Node target)
    {
        return target.transform.position;
    }
    
    private Vector3 GetNodeUIPosition(Node target)
    {
        return target.transform.position + positionOffset;
    }

    private void SetTargetForNodeUI(Node target)
    {
        Debug.Log("set target");
        if (!CanShowPopUp)
            return;
        
        _selectedNode = target;

        nodeCanvasUI.transform.position = GetNodeUIPosition(_selectedNode);

        if (!_isUpgrade)
        {
            upgradeCost.text = "$" + _turretBlueprint.upgradeCost;
            upgradeButton.interactable = true;
        }
        else
        {
            upgradeCost.text = "DONE";
            upgradeButton.interactable = false;
        }

        Debug.Log("set target end ");

        sellAmount.text = "$" + _turretBlueprint.GetSellAmount();

        nodeCanvasUI.SetActive(true);
        _popUpAlpha = 0;
        canvasGroup.alpha = 0;

        StartCoroutine(AnimateAppear());
    }

    private IEnumerator AnimateAppear()
    {
        Debug.Log("AmimateAppear");
        while (canvasGroup.alpha < 1 && nodeCanvasUI.activeInHierarchy)
        {
            _popUpAlpha += appearPopUpSpeed * Time.deltaTime;
            canvasGroup.alpha = _popUpAlpha;

            yield return null;
        }
    }

    private void HidePopUpMenu()
    {
        nodeCanvasUI.SetActive(false);
    }

    public void SellTurret()
    {
        SellTurretAction.Invoke(_turretBlueprint.GetSellAmount());

        GameObject effect = Instantiate(sellEffect, GetBuildPosition(_selectedNode), Quaternion.identity);
        Destroy(effect, 5f);

        Destroy(_selectedNode.AssignedTurret);
        _turretBlueprint = null;
    }

    public void UpgradeTurret()
    {
        UpgradeTurretAction.Invoke(_turretBlueprint.upgradeCost);

        Destroy(_selectedNode.AssignedTurret);

        GameObject turret = Instantiate(_turretBlueprint.upgradedPrefab,
            GetBuildPosition(_selectedNode), Quaternion.identity);

        _selectedNode.AssignedTurret = turret;

        GameObject effect = Instantiate(buildEffect, GetBuildPosition(_selectedNode), Quaternion.identity);
        Destroy(effect, 5f);

        _isUpgrade = true;

        Debug.Log("Turret upgraded");
    }
}