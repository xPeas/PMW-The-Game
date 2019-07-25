using UnityEngine;

namespace Assets.Scripts.Inventory
{
    [CreateAssetMenu(menuName ="Items/Item")]
    public class Item : ScriptableObject
    {
        public ItemType type;
        public UI_Info ui_info;
        public Object obj;

        [System.Serializable]
        public class UI_Info
        {
            public string itemName;
            public string itemDescription;
            public string skillDescription;
            public Sprite icon;
        }
    }

    public enum ItemType
    {
        weapon,armor,consumable,spell
    }

}
