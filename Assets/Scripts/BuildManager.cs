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
    // [SerializeField] private NodeUI nodeUI;
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
    private Node _node;
    private Node _selectedNode;
    private readonly GameManager _gameManager;
    private Renderer _rend;
    private Color _startColor;
    private GameObject _turret;
    private bool _isUpgrade; 
    private TurretBlueprint _turretBlueprint;
    private TurretInfoSO _turretInfoSo;
    private float _popUpAlpha;
    
    private bool CanShowPopUp => !cameraController.isDragging && !cameraController.isZooming;

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
        _rend = _selectedNode.GetComponent<Renderer>();
        _startColor = _rend.material.color;

        _node.ONMouseUpButton += ClickOnNode;
        _node.ONMouseExitButton += ResetNodeColor;
    }

    private void ResetNodeColor(Node currentNode)
    {
        currentNode.rend.material.color = _startColor;
    }

    public void Initialize(Node node, TurretInfoSO turretInfoSo)
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

        SetTarget(currentNode);
    }

    private void DeselectNode()
    {
        _selectedNode = null;
        HidePopUpMenu();
    }

    public void SellTurret()
    {
        SellTurretAction.Invoke(_turretBlueprint.GetSellAmount());

        GameObject effect = Instantiate(sellEffect, GetBuildPosition(), Quaternion.identity);
        Destroy(effect, 5f);

        Destroy(_turret);
        _turretBlueprint = null;
    }

    public void SelectTurretToBuild(TurretType turretType)
    {
        _turretToBuild = _turretInfoSo.turretArray.First(someTurret => someTurret.type == turretType);
        Debug.Log("Selected turret type: " + _turretToBuild.type);
        DeselectNode();
    }

    private TurretBlueprint GetTurretToBuild()
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

        GameObject turret = Instantiate(blueprint.prefab, GetBuildPosition(), Quaternion.identity);
        _turret = turret;

        _turretBlueprint = blueprint;

        GameObject effect = Instantiate(buildEffect, GetBuildPosition(), Quaternion.identity);
        Destroy(effect, 5f);
    }

    public void UpgradeTurret()
    {
        UpgradeTurretAction.Invoke(_turretBlueprint.upgradeCost);

        Destroy(_turret);

        GameObject turret = Instantiate(_turretBlueprint.upgradedPrefab,
            GetBuildPosition(), Quaternion.identity);
        
        _turret = turret;

        GameObject effect = Instantiate(buildEffect, GetBuildPosition(), Quaternion.identity);
        Destroy(effect, 5f);

        _isUpgrade = true;

        Debug.Log("Turret upgraded");
    }

    public Vector3 GetBuildPosition()
    {
        return transform.position + positionOffset;
    }
    
    public void SetTarget(Node target)
    {
        if (!CanShowPopUp)
        {
            return;
        }
        
        _selectedNode = target;

        transform.position = GetBuildPosition();

        if (!_isUpgrade)
        {
            upgradeCost.text = "$" + _turretBlueprint.upgradeCost;
            upgradeButton.interactable = true;
        } else
        {
            upgradeCost.text = "DONE";
            upgradeButton.interactable = false;
        }

        sellAmount.text = "$" + _turretBlueprint.GetSellAmount();

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
    
    public void  HidePopUpMenu ()
    {
        nodeCanvasUI.SetActive(false);
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