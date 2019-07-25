using System;
using System.Collections.Generic;

namespace Assets.Scripts.References
{
    public static class SceneLibrary
    {
        private static Dictionary<Scene, String> SceneList = new Dictionary<Scene, String>
        {
            {Scene.MainMenu, SceneName.MainMenu },
            {Scene.TestRoom, SceneName.TestRoom },
            {Scene.WubHub, SceneName.WubHub }
        };

        public static string GetSceneName(Scene scene)
        {
            return SceneList[scene];
        }
    }

    public enum Scene
    {
        MainMenu,
        TestRoom,
        WubHub
    }
}
