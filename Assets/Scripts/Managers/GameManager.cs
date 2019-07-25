using Assets.Scripts.References;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class GameManager : Singleton<GameManager>
    {
        public SceneManager SceneManager;

        public void OnEnable()
        {
            SceneManager = this.gameObject.AddComponent<SceneManager>();            
            SceneManager.LoadScene(Scene.MainMenu);
        }
    }
}
