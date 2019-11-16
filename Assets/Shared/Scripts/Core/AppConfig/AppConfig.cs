using System.Collections;
using TimiShared.Debug;
using TimiShared.Init;
using TimiShared.Instance;
using UnityEngine;

public class AppConfig : MonoBehaviour, IInitializable, IInstance {

    [SerializeField] private TextAsset _appConfigTextAsset = null;

    public static AppConfig Instance {
        get {
            return InstanceLocator.Instance<AppConfig>();
        }
    }

    public enum Environment {
        LOCAL,
        DEV,
        STAGING,
        PROD
    }

    private AppConfigData _appConfigData;


    #region Public API
    public int GetAppID() {
        return _appConfigData.appID;
    }

    public Environment GetCurrentEnvironment() {
        if (_appConfigData == null) {
            DebugLog.LogErrorColor("AppConfig not set", LogColor.red);
            return Environment.LOCAL;
        }
        return _appConfigData.currentEnvironment;
    }

    public string GetAppPhotonID() {
        if (_appConfigData == null) {
            DebugLog.LogErrorColor("AppConfig not set", LogColor.red);
            return "";
        }
        return _appConfigData.appPhotonID;
    }

    public string GetDebugDeviceID() {
        if (UnityEngine.Debug.isDebugBuild) {
            return _appConfigData.debugDeviceId;
        }
        return null;
    }
    #endregion

    #region IInitializable
    public void StartInitialize() {
        InstanceLocator.RegisterInstance<AppConfig>(this);
        this.StartCoroutine(this.InitializeAsync(() => {
            this.IsFullyInitialized = true;
        }));
    }

    public string GetName {
        get {
            return this.GetType().Name;
        }
    }

    public bool IsFullyInitialized {
        get; private set;
    }

    private IEnumerator InitializeAsync(System.Action callback) {
        this._appConfigData = AppConfigHelper.LoadAppConfigDataFromTextAsset(this._appConfigTextAsset);
        callback.Invoke();
        yield break;
    }
    #endregion

}
