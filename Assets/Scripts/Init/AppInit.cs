using TimiShared.Init;
using TimiShared.Instance;
using UnityEngine;

public class AppInit : AppInitBase {

    #region Events
    public static System.Action OnAppInitComplete = delegate {};
    #endregion

    #region IInitializable
    private bool _isFullyInitialized = false;

    public override void StartInitialize() {

        Application.targetFrameRate = 30;

        AppData appData = new AppData();
        InstanceLocator.RegisterInstance<AppData>(appData);

        AppSceneManager appSceneManager = new AppSceneManager();
        InstanceLocator.RegisterInstance<AppSceneManager>(appSceneManager);

        this._isFullyInitialized = true;
        OnAppInitComplete.Invoke();
    }

    public override bool IsFullyInitialized {
        get {
            return this._isFullyInitialized;
        }
    }
    #endregion

}
