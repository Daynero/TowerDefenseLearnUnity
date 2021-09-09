using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    [SerializeField] private TurretBlueprint standartTurret;
    [SerializeField] private TurretBlueprint missleLauncher;
    [SerializeField] private TurretBlueprint laserBeamer;
    [SerializeField] private Button selectDefaultTurretButton;
    [SerializeField] private Button selectMissileLauncherButton;
    [SerializeField] private Button selectLaserBeamerButton;

    private BuildManager buildManager;

    private void Start()
    {
        buildManager = BuildManager.instance;

        selectDefaultTurretButton.onClick.AddListener(delegate { buildManager.SelectTurretToBuild(standartTurret); });

        selectMissileLauncherButton.onClick.AddListener(delegate { buildManager.SelectTurretToBuild(missleLauncher); });

        selectLaserBeamerButton.onClick.AddListener(delegate { buildManager.SelectTurretToBuild(laserBeamer); });
    }
}