using System.IO;
using TimiShared.Loading;
using UnityEditor;
using UnityEngine;

public class AppConfigWindow : EditorWindow {

    private int _appID;
    private string _appPhotonID;
    private AppConfig.Environment _currentEnvironment;
    private string _deviceId;
    private bool _initialized = false;

    [MenuItem("TimiShared/AppConfig")]
    public static void ShowWindow() {
        AppConfigWindow thisWindow = EditorWindow.GetWindow(typeof(AppConfigWindow)) as AppConfigWindow;

        if (!thisWindow._initialized) {
            thisWindow.Initialize();
        }
    }

    private void Initialize() {
        TextAsset appConfigTextAsset = Resources.Load<TextAsset>("BootstrapData/AppConfig");
        AppConfigData appConfigData = AppConfigHelper.LoadAppConfigDataFromTextAsset(appConfigTextAsset);
        if (appConfigData != null) {
            this._appID = appConfigData.appID;
            this._appPhotonID = appConfigData.appPhotonID;
            this._currentEnvironment = appConfigData.currentEnvironment;
            this._deviceId = appConfigData.debugDeviceId;
        } else {
            this._currentEnvironment = AppConfig.Environment.LOCAL;
        }

        this._initialized = true;
    }

    void OnGUI() {
        if (!this._initialized) {
            this.Initialize();
        }

        string newAppIDString = EditorGUILayout.TextField("App ID:", this._appID.ToString());
        string newAppPhotonIDString = EditorGUILayout.TextField("App Photon ID:", this._appPhotonID.ToString());
        int newAppID;
        if (!int.TryParse(newAppIDString, out newAppID)) {
            newAppID = 0;
        }
        AppConfig.Environment newEnvironment = (AppConfig.Environment)EditorGUILayout.EnumPopup("Current Environment:", this._currentEnvironment);
        string newDeviceId = EditorGUILayout.TextField("Debug Device ID", this._deviceId);

        if (newAppID != this._appID ||
            newAppPhotonIDString != this._appPhotonID ||
            newEnvironment != this._currentEnvironment ||
            newDeviceId != this._deviceId) {
            this.SaveEnvironmentSelection(newAppID, newAppPhotonIDString, newEnvironment, newDeviceId);
            this._appID = newAppID;
            this._appPhotonID = newAppPhotonIDString;
            this._currentEnvironment = newEnvironment;
            this._deviceId = newDeviceId;
        }
    }

    private void SaveEnvironmentSelection(int appID, string appPhotonID, AppConfig.Environment environment, string deviceId) {
        AppConfigData newAppConfigData = new AppConfigData();
        newAppConfigData.appID = appID;
        newAppConfigData.appPhotonID = appPhotonID;
        newAppConfigData.currentEnvironment = environment;
        newAppConfigData.debugDeviceId = deviceId;
        AppConfigHelper.SaveAppConfigData(newAppConfigData);
    }


}
