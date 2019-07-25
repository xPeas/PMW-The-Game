using UnityEngine;

namespace Assets.Scripts.Scriptables
{
    [CreateAssetMenu(menuName ="Single Instances/Controller Stats")]
    public class ControllerStats : ScriptableObject
    {
        public float moveSpeed;
        public float sprintSpeed;
        public float rotateSpeed;
    }
}
