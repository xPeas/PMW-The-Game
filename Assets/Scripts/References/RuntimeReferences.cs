using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.References
{
    [CreateAssetMenu(menuName = "SingleInstances/Runtime References")]
    public class RuntimeReferences : ScriptableObject
    {
        public List<Inventory.RuntimeWeapon> runtimeWeapons = new List<Inventory.RuntimeWeapon>();

        public void Init()
        {
            runtimeWeapons.Clear();
        }

        public void RegesterRW(Inventory.RuntimeWeapon rw)
        {
            runtimeWeapons.Add(rw);
        }

        public void UnRegisterRW(Inventory.RuntimeWeapon rw)
        {
            if (runtimeWeapons.Contains(rw))
            {
                if (rw.instance)
                    Destroy(rw.instance);
                runtimeWeapons.Remove(rw);
            }
        }
    }
}
