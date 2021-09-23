using System;
using System.Linq;
using Data;
using UnityEngine;
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
    [SerializeField] private Button upgradeTurretButton;
    [SerializeField] private Button sellTurretButton;
    [SerializeField] private TMP_Text sellAmount;
    [SerializeField] private float appearPopUpSpeed;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private GameObject nodeCanvasUI;

    private TurretBlueprint _turretToBuild;
    private Node _selectedNode;
    private GameManager _gameManager;
    private Color _startColor = Color.white;
    private TurretBlueprint _turretBlueprint;
    private TurretInfoSO _turretInfoSo;
    private float _popUpAlpha;
    private Node[] _nodes;
    private Node _popUpPositionOnNode;

    private bool CanShowPopUp => !cameraController.isDragging && !cameraController.isZooming;

    public Action<int> SellTurretAction;
    public Action<int> BuildTurretAction;
    public Action<int> UpgradeTurretAction;
    public Action<TurretInfoSO> DisplayCurrentTurretPrice;

    public GameObject buildEffect;
    public GameObject sellEffect;


    private bool CanBuild => _turretToBuild != null && !cameraController.isDragging && !cameraController.isZooming;

    private bool HasMoney => _turretToBuild != null && _gameManager.PlayerMoney >= _turretToBuild.cost;

    public void Initialize(TurretInfoSO turretInfoSo, GameManager gameManager)
    {
        _turretInfoSo = turretInfoSo;
        _gameManager = gameManager;
        DisplayCurrentTurretPrice?.Invoke(_turretInfoSo);

        _nodes = FindObjectsOfType<Node>();

        foreach (var node in _nodes)
        {
            node.Initialize(ClickOnNode, ResetNodeColor);
        }
        
        upgradeTurretButton.onClick.AddListener(UpgradeTurret);
        sellTurretButton.onClick.AddListener(SellTurret);
    }

    private void ClickOnNode(Node currentNode, Renderer rend)
    {
        _selectedNode = currentNode;

        if (_selectedNode.AssignedTurret != null)
        {
            SelectNodeWithTurret(_selectedNode);
            return;
        }

        if (!CanBuild)
            return;

        if (!HasMoney)
        {
            rend.material.color = notEnoughMoneyColor;
            return;
        }

        rend.material.color = hoverColor;

        BuildTurret(_turretToBuild);
    }

    private void SelectNodeWithTurret(Node currentNode)
    {
        _turretToBuild = null;
        
        if (_popUpPositionOnNode == _selectedNode)
        {
            HidePopUpMenu();
            return;
        }

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
        _selectedNode.TurretBluePrint = blueprint;
        
        BuildTurretAction.Invoke(_selectedNode.TurretBluePrint.cost);

        Turret turret = Instantiate(_selectedNode.TurretBluePrint.prefab, GetBuildPosition(_selectedNode), Quaternion.identity);
        _selectedNode.AssignedTurret = turret;
        
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

        nodeCanvasUI.transform.position = GetNodeUIPosition(target);

        _popUpPositionOnNode = target;

        if (!target.IsUpgrade)
        {
            upgradeCost.text = "$" + _selectedNode.TurretBluePrint.upgradeCost;
            upgradeTurretButton.interactable = true;
        }
        else
        {
            upgradeCost.text = "DONE";
            upgradeTurretButton.interactable = false;
        }

        Debug.Log("set target end ");

        sellAmount.text = "$" + _selectedNode.TurretBluePrint.GetSellAmount();

        nodeCanvasUI.SetActive(true);
        _popUpAlpha = 0;
        canvasGroup.alpha = 0;

        StartCoroutine(AnimateAppear());
    }

    private IEnumerator AnimateAppear()
    {
        while (canvasGroup.alpha < 1 && nodeCanvasUI.activeInHierarchy)
        {
            _popUpAlpha += appearPopUpSpeed * Time.deltaTime;
            canvasGroup.alpha = _popUpAlpha;

            yield return null;
        }
    }

    private void HidePopUpMenu()
    {
        _popUpPositionOnNode = null;
        nodeCanvasUI.SetActive(false);
    }

    private void SellTurret()
    {
        Debug.Log("sell turret");
        SellTurretAction.Invoke(_selectedNode.TurretBluePrint.GetSellAmount());

        GameObject effect = Instantiate(sellEffect, GetBuildPosition(_selectedNode), Quaternion.identity);
        Destroy(effect, 5f);

        Destroy(_selectedNode.AssignedTurret);
        _selectedNode.TurretBluePrint = null;

        HidePopUpMenu();
    }

    private void UpgradeTurret()
    {
        Debug.Log("upgraded turret");
        UpgradeTurretAction.Invoke(_selectedNode.TurretBluePrint.upgradeCost);

        Destroy(_selectedNode.AssignedTurret.gameObject);

        Turret turret = Instantiate(_selectedNode.TurretBluePrint.upgradedPrefab,
            GetBuildPosition(_selectedNode), Quaternion.identity);

        _selectedNode.AssignedTurret = turret;

        GameObject effect = Instantiate(buildEffect, GetBuildPosition(_selectedNode), Quaternion.identity);
        Destroy(effect, 5f);

        _selectedNode.IsUpgrade = true;

        HidePopUpMenu();
    }
}