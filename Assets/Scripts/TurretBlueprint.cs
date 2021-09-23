using UnityEngine;

[System.Serializable]
public class TurretBlueprint
{
    public TurretType type;
    public Turret prefab;
    public Turret upgradedPrefab;
    public int upgradeCost;
    public int cost;

    public int GetSellAmount()
    {
        return cost / 2;
    }
}
