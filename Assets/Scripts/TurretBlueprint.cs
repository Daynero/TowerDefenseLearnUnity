using UnityEngine;

[System.Serializable]
public class TurretBlueprint
{
    public TurretType type;
    public GameObject prefab;
    public GameObject upgradedPrefab;
    public int upgradeCost;
    public int cost;

    public int GetSellAmount()
    {
        return cost / 2;
    }
}
