using UnityEditor;
using UnityEditor.SceneManagement;

namespace Debug {

    public static class Shortcuts {

        [MenuItem("HungryNerds/Shortcuts/Switch to Login scene #&1")]
        static void SwitchToLoginScene() {
            SwitchScene("Assets/Scenes/LoginScene.unity");
        }

        [MenuItem("HungryNerds/Shortcuts/Switch to Lobby scene #&2")]
        static void SwitchToMainScene() {
            SwitchScene("Assets/Scenes/LobbyScene.unity");
        }

        [MenuItem("HungryNerds/Shortcuts/Switch to Test UI scene #&3")]
        static void SwitchToTestUIScene() {
            SwitchScene("Assets/Scenes/TestUIScene.unity");
        }

        [MenuItem("HungryNerds/Shortcuts/Switch to Game scene #&4")]
        static void SwitchToGameScene() {
            SwitchScene("Assets/Scenes/GameScene.unity");
        }

        [MenuItem("HungryNerds/Shortcuts/Switch to Test scene #&5")]
        static void SwitchToTestScene() {
            SwitchScene("Assets/Scenes/TestScene.unity");
        }

        private static void SwitchScene(string sceneName) {
            EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
            EditorSceneManager.OpenScene(sceneName);
        }
    }

}