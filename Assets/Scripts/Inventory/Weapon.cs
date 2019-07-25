using UnityEngine;
using Assets.Scripts.Enumerations;
using Assets.Scripts.Scriptables.Variables;
using Assets.Data;

namespace Assets.Scripts.Inventory
{
    [CreateAssetMenu(menuName ="Items/Weapon")]
    public class Weapon : ScriptableObject
    {
        public StringVariable oh_idle;
        public StringVariable th_idle;
        public GameObject modelPrefab;
        public ActionHolder[] actions;

        public LeftHandPosition lh_position;

        public ActionHolder GetActionHolder(InputType inp)
        {
            for (int i = 0; i < actions.Length; i++)
            {
                if (actions[i].input == inp)
                    return actions[i];
            }
            return null;
        }
        public Action GetAction(InputType inp)
        {
            ActionHolder ah = GetActionHolder(inp);
            return ah.action;
        }
    }

    [System.Serializable]
    public class ActionHolder
    {
        public InputType input;
        public Action action;
    }
}
