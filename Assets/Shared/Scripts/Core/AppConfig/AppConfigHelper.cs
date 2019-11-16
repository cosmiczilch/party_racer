using System;
using System.IO;
using TimiShared.Loading;
using TimiShared.Utils;
using UnityEngine;

public static class AppConfigHelper {

    private static string kAppConfigFileName = "AppConfig.json";

    private static TimiSharedURI _appConfigFileURI = null;
    public static TimiSharedURI AppConfigFileURI {
        get {
            if (_appConfigFileURI == null) {
                _appConfigFileURI = new TimiSharedURI(FileBasePathType.LocalDataPath, "Resources/BootstrapData/" + kAppConfigFileName);
            }
            return _appConfigFileURI;
        }
    }

    public static AppConfigData LoadAppConfigDataFromTextAsset(TextAsset textAsset) {
        AppConfigData appConfigData = null;

        string appConfigDataJson = textAsset.text;
        if (!string.IsNullOrEmpty(appConfigDataJson)) {
            appConfigData = TimiSharedSerializer.Deserialize<AppConfigData>(appConfigDataJson);
        }

        return appConfigData;
    }

    public static void SaveAppConfigData(AppConfigData appConfigData) {
        if (!Application.isEditor) {
            throw new NotImplementedException("Not yet supported to edit app config on device");
        }

        string appConfigDataJson = TimiSharedSerializer.Serialize(appConfigData);
        if (!string.IsNullOrEmpty(appConfigDataJson)) {
            TimiSharedURI appConfigFileUri = AppConfigHelper.AppConfigFileURI;
            using (Stream appConfigFileStream = FileLoader.GetFileStreamSync(appConfigFileUri, FileMode.Create, FileAccess.Write)) {
                FileUtils.PutStreamContents(appConfigFileStream, appConfigDataJson);
                appConfigFileStream.Close();
            }
        }
    }


}
