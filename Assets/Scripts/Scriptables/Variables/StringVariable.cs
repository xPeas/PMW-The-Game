using UnityEngine;

namespace Assets.Scripts.Scriptables.Variables
{
    [CreateAssetMenu(menuName = "Variables/String")]
    public class StringVariable : ScriptableObject
    {
        public string value;
    }
}