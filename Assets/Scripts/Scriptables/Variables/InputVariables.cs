using UnityEngine;

namespace Assets.Scripts.Scriptables.Variables
{
    [System.Serializable]
    public class InputVariables : MonoBehaviour
    {
        public float moveAmount;
        public float horizontal;
        public float vertical;
        public float jump;
        public Vector3 moveDir;
        public Vector3 animDelta;
        public Transform lockOnTransform;

        public bool rt;
        public bool lt;
        public bool rb;
        public bool lb;
        public bool a;
    }
}
