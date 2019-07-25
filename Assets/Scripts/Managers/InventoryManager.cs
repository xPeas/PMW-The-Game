using Assets.Scripts.Inventory;

namespace Assets.Scripts.Managers
{
    [System.Serializable]
    public class InventoryManager
    {
        public RuntimeWeapon rh;
        public RuntimeWeapon lh;

        public Item rh_item;
        public Item lh_item;
        public Item consumable;
        public Item spell;
    }
}
