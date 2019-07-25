using UnityEngine;
using Assets.Scripts.References;

namespace Assets.Scripts.Managers
{
    [CreateAssetMenu(menuName = "Single Instances/Resources Manager")]
    public class ResourcesManager : ScriptableObject
    {
        public Inventory.Inventory inventory;
        public RuntimeReferences runtime;

        public void Init()
        {
            inventory.Init();
        }

        public Inventory.Item GetItem(string id)
        {
            return inventory.GetItem(id);
        }

        public Inventory.Weapon GetWeapon(string id)
        {
            Inventory.Item item = GetItem(id);
            return (Inventory.Weapon)item.obj;
        }

        public Inventory.Armor GetArmor(string id)
        {
            Inventory.Item item = GetItem(id);
            return (Inventory.Armor)item.obj;
        }
    }

}
