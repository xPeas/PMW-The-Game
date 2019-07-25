using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Data
{
    [System.Serializable]
    public class Action
    {
        public bool isMirrored;
        public ActionType actionType;
        public Object action_obj;
    }
    
    public enum ActionType
    {
        attack,block,spell,parry
    }
}
