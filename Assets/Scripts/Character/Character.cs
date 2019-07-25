using Assets.Scripts.Managers;
using UnityEngine;

namespace Assets.Scripts.Character
{
    public class Character : MonoBehaviour
    {
        public StatesManager statesManager;

        public Character()
        {
            statesManager = new StatesManager();
            
        }
    }
}
