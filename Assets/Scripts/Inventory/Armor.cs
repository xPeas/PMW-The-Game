using Assets.Scripts.Inventory.Enumerations;
using UnityEngine;

namespace Assets.Scripts.Inventory
{
    [CreateAssetMenu(menuName = "Items/Armor")]
    public class Armor : ScriptableObject
    {
        public ArmorType armorType;
        public Mesh armorMesh;
        public UnityEngine.Material[] materials;
        public bool baseBodyEnabled;
    }
}
