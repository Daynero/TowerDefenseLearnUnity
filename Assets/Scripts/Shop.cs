using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
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
