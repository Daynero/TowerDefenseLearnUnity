using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "TurretInfo", menuName = "Turret Info", order = 51)]
    public class TurretInfoSO: ScriptableObject
    {
        public TurretBlueprint[] turretArray = new TurretBlueprint[3];
    }
}