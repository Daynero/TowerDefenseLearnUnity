using UnityEngine;

[System.Serializable]
public struct TurretBlueprint
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
