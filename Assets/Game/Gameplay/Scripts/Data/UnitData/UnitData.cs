using UnityEngine;

namespace Game.Gameplay.Data.UnitData
{
    [CreateAssetMenu(fileName = "UnitData", menuName = "Data/UnitData/UnitData")]
    public class UnitData : ScriptableObject
    {
        public string unitName;
        [Range(1, 300)] public int damage;
        [Range(10, 300)] public int health;
        [Range(1, 10)] public int attackRange;
        [Range(0.1f, 2)] public float moveDelay;
    }
}