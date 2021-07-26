using UnityEngine;

public class Shop : MonoBehaviour
{
    public TorretBluePrint standartTurret;
    public TorretBluePrint missleLauncher;

    private BuildManager buildManager;

    void Start()
    {
        buildManager = BuildManager.instance;
    }
    public void PurchaseStandardTurret()
    {
        buildManager.SetTurretToBuild(buildManager.standartTurretPrefab);
    }

    public void PurchaseMissileLauncher()
    {
       buildManager.SetTurretToBuild(buildManager.MissileLauncherPrefab);
    }
}
