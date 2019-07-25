using Assets.Scripts.Managers;
using UnityEngine;

namespace Assets.Scripts
{
    public class Persistent : MonoBehaviour
    {
        GameManager theGame;
        // Start is called before the first frame update
        public void Start()
        {
            theGame = gameObject.AddComponent<GameManager>();
            DontDestroyOnLoad(this.gameObject);            
        }
    }
}
