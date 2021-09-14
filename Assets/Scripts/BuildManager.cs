using UnityEngine;

public class BuildManager : MonoBehaviour
{
    [SerializeField] private NodeUI nodeUI;
    [SerializeField] private CameraController cameraController;

    private TurretBlueprint turretToBuild;
    private Node selectedNode;

    public GameObject buildEffect;
    public GameObject sellEffect;
    public static BuildManager instance;

    public bool CanBuild
    {
        get { return (turretToBuild != null && !cameraController.isDragging && !cameraController.isZooming); }
    }

    public bool HasMoney
    {
        get { return PlayerStats.instance.PlayerMoney >= turretToBuild.cost; }
    }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("More than one BuildManager in the scene!");
            return;
        }

        instance = this;
    }

    public void SelectNode(Node node)
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

    public void DeselectNode()
    {
        selectedNode = null;
        nodeUI.HidePopUpMenu();
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
}