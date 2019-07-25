using Assets.Scripts.Managers;
using Assets.Scripts.References;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.MainMenu
{
    public class StartButton : MonoBehaviour
    {
        GameManager game;
        Button theButton;
        public void Awake()
        {
            game = FindObjectOfType<GameManager>();
            theButton = this.gameObject.GetComponent<Button>();
        }
        public void Start()
        {
            theButton.onClick.AddListener(() => { game.SceneManager.LoadScene(Scene.TestRoom);} );
        }
    }
}
