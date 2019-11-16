using System.Collections;
using Init;
using TimiMultiPlayer;
using TimiShared.Debug;
using TimiShared.Extensions;
using TimiShared.Identity;
using TimiShared.Init;
using TimiShared.Login;
using UnityEngine;

public class Core : MonoBehaviour {

    [SerializeField] private SharedInit _sharedInit = null;
    [SerializeField] private FirstLaunchManager _firstLaunchManager = null;
    [SerializeField] private AppConfig _appConfig = null;
    [SerializeField] private MultiPlayerManager _multiPlayerManager = null;
    [SerializeField] private DataModelsLoader _dataModelsLoader = null;
    [SerializeField] private IdentityManager _identityManager = null;
    [SerializeField] private AppInitBase _appInit = null;
    [SerializeField] private AppLoaderBase _appLoader = null;
    [SerializeField] private LoadingScreenManager _loadingScreenManager = null;

    private void Awake () {
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start() {
        this.StartCoroutine(this.InitializationSequence());
    }

    private IEnumerator InitializationSequence() {
        float startTimeSeconds = Time.realtimeSinceStartup;

        InitializableSerialGroup initializables = new InitializableSerialGroup("Core");


        /** Start initialization sequence. Order matters here!! **/

        this._sharedInit.AssertNotNull("Shared Init");
        initializables.AddInitializable(this._sharedInit);

        this._firstLaunchManager.AssertNotNull("First Launch Manager");
        initializables.AddInitializable(this._firstLaunchManager);

        this._appConfig.AssertNotNull("App Config");
        initializables.AddInitializable(this._appConfig);

        this._identityManager.AssertNotNull("Identity Manager");
        initializables.AddInitializable(this._identityManager);

        this._multiPlayerManager.AssertNotNull("Multiplayer Manager");
        initializables.AddInitializable(this._multiPlayerManager);

        this._appInit.AssertNotNull("App Init");
        initializables.AddInitializable(this._appInit);

        this._dataModelsLoader.AssertNotNull("Data Models Loader");
        initializables.AddInitializable(this._dataModelsLoader);

        /** End initialization sequence. Order matters here!! **/

        initializables.StartInitialize();
        while (!initializables.IsFullyInitialized) {
            yield return null;
        }

        float timeElapsedSeconds = Time.realtimeSinceStartup - startTimeSeconds;
        DebugLog.LogColor("Initialization complete in " + timeElapsedSeconds.ToString("F2") + " seconds", LogColor.green);

        /** Initialization complete. Load App **/

        this._appLoader.AssertNotNull("App Loader");
        this._appLoader.StartInitialize();
        while (!this._appLoader.IsFullyInitialized) {
            yield return null;
        }

        this._loadingScreenManager.AssertNotNull("Loading Screen Manager");
        this._loadingScreenManager.StartInitialize();
        while (!this._loadingScreenManager.IsFullyInitialized) {
            yield return null;
        }

        Input.backButtonLeavesApp = true;
    }

    private void OnApplicationPause(bool isPaused) {
        if (Application.isEditor) {
            return;
        }

        if (!isPaused) {
            return;
        }

        if (this._sharedInit != null) {
            this._sharedInit.Cleanup();
        }
    }
}
