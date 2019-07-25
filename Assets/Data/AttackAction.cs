﻿using Assets.Scripts.Scriptables.Variables;
using UnityEngine;

namespace Assets.Data
{
    [CreateAssetMenu(menuName = "Actions/Attack Action")]
    public class AttackAction : ScriptableObject
    {
        public StringVariable attack_anim;
        public bool canBeParried = true;
        public bool changeSpeed = false;
        public float animSpeed = 1;
        public bool canParry = false;
        public bool canBackStab = false;
    }
}