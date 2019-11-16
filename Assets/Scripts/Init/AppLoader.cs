using TimiShared.Init;

public class AppLoader : AppLoaderBase {

    #region IInitializable
    private bool _isFullyInitialized = false;

    public override void StartInitialize() {
        AppSceneManager.Instance.LoadLobbyScene(() => {
            this._isFullyInitialized = true;
        });
    }

    public override bool IsFullyInitialized {
        get {
            return this._isFullyInitialized;
        }
    }
    #endregion
}
