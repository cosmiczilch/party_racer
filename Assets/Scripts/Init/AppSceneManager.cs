using Game;
using Init;
using Lobby;
using TimiShared.Debug;
using TimiShared.Instance;
using TimiShared.Loading;
using UnityEngine.SceneManagement;

public class AppSceneManager : IInstance {

    public static AppSceneManager Instance {
        get {
            return InstanceLocator.Instance<AppSceneManager>();
        }
    }

    public enum AppScene {
        LOBBY_SCENE,
        GAME_SCENE
    }

    private const string kLobbySceneName = "LobbyScene";
    private const string kGameSceneName = "GameScene";

    #region Public API
    public void LoadLobbyScene(System.Action callback = null) {
        this.LoadScene(AppScene.LOBBY_SCENE, (success) => {
            if (success) {
                LobbyController lobbyController = new LobbyController();
                if (callback != null) {
                    callback.Invoke();
                }
            }
        });
    }

    public void LoadGameScene(GameController.Config_t config, System.Action callback = null) {
        LoadingScreenManager.Instance.ShowLoadingScreen(true, false);

        this.LoadScene(AppScene.GAME_SCENE, (success) => {
            if (success) {
                GameController gameController = new GameController(config);
                if (callback != null) {
                    callback.Invoke();
                }
            }
        });
    }
    #endregion

    private void LoadScene(AppScene appScene, System.Action<bool /* success */> callback = null) {
        LoadSceneMode loadSceneMode = LoadSceneMode.Single;

        string sceneName = null;
        switch (appScene) {

            case AppScene.LOBBY_SCENE:
                sceneName = kLobbySceneName;
                loadSceneMode = LoadSceneMode.Single;
                break;

            case AppScene.GAME_SCENE:
                sceneName = kGameSceneName;
                loadSceneMode = LoadSceneMode.Single;
                break;

            default:
                DebugLog.LogErrorColor("Invalid scene: " + appScene, LogColor.red);
                return;
        }

        SceneLoader.Instance.LoadSceneAsync(sceneName, loadSceneMode, (loadedSceneName, success) => {
            if (!success) {
                DebugLog.LogErrorColor("Failed to load scene: " + appScene, LogColor.red);
                if (callback != null) {
                    callback.Invoke(false);
                }
            }
            else {
                if (callback != null) {
                    callback.Invoke(true);
                }
            }
        });
    }

}
