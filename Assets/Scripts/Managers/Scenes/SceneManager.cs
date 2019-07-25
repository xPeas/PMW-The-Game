using Assets.Scripts.References;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class SceneManager : Singleton<SceneManager>
    {
        public SceneLoaderAsync SceneLoader { get; private set; }
        public Scene currentScene { get; private set; }
        
        public void OnEnable()
        {
            SceneLoader = this.gameObject.AddComponent<SceneLoaderAsync>();
        }
        public void LoadScene(Scene scene)
        {
            SceneLoader.LoadScene(SceneLibrary.GetSceneName(scene));
            currentScene = scene;
        }
    }
}
