﻿using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Inventory
{
    [CreateAssetMenu(menuName ="Single Instances/Inventory")]
    public class Inventory : ScriptableObject
    {
        public List<Item> all_items = new List<Item>();
        Dictionary<string, int> dict = new Dictionary<string, int>();

        public void Init()
        {
#if UNITY_EDITOR
            all_items = EditorUtilities.FindAssetsByType<Item>();
#endif
            for (int i = 0; i < all_items.Count; i++)
            {
                if (dict.ContainsKey(all_items[i].name))
                {

                }
                else
                {
                    dict.Add(all_items[i].name, i);
                }
            }
        }

        public Item GetItem(string id)
        {
            Item retVal = null;
            int index = -1;
            if(dict.TryGetValue(id,out index))
            {
                retVal = all_items[index];
            }

            if(index == -1)
            {
                Debug.Log("No item with " + id + " found!");
            }

            return retVal;
        }
    }
}
