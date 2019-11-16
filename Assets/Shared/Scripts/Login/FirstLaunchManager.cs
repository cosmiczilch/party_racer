using System.Collections;
using System.Collections.Generic;
using TimiShared.Debug;
using TimiShared.Init;
using TimiShared.Loading;
using TimiShared.Utils;
using UnityEngine;

namespace TimiShared.Login {

    public class FirstLaunchManager : MonoBehaviour, IInitializable {

        #region IInitializable
        public void StartInitialize() {
            this.StartCoroutine(DetectAndHandleFirstLaunch());
        }

        public bool IsFullyInitialized {
            get; private set;
        }

        public string GetName {
            get {
                return this.GetType().Name;
            }
        }
        #endregion

        [System.Serializable]
        private class TextAssetInfo {
            public string fileName = null;
            public TextAsset textAsset = null;
        }
        [SerializeField] private List<TextAssetInfo> _bootstrapTextAssetInfos = null;

        private IEnumerator DetectAndHandleFirstLaunch() {
            if (this.IsFirstLaunch()) {

                if (FileUtils.DoesDirectoryExist(this.FirstLaunchDestinationURI)) {
                    DebugLog.LogColor("Deleting previous first launch data because app version doesn't match", LogColor.green);
                    FileUtils.DeleteDirectory(this.FirstLaunchDestinationURI);
                }

                DebugLog.LogColor("Copying first launch data", LogColor.green);
                yield return this.CopyFirstLaunchTextAssets();

                PlayerPrefs.SetString(kAppVersionKey, kAppVersion);
            }

            this.IsFullyInitialized = true;
            yield break;
        }

        private IEnumerator CopyFirstLaunchTextAssets() {
            FileUtils.CreateDirectory(this.FirstLaunchDestinationURI);

            var enumerator = this._bootstrapTextAssetInfos.GetEnumerator();
            while (enumerator.MoveNext()) {
                TextAsset textAsset = enumerator.Current.textAsset;
                TimiSharedURI destinationURI = TimiSharedURI.Combine(this.FirstLaunchDestinationURI, new TimiSharedURI(FileBasePathType.LocalPersistentDataPath, enumerator.Current.fileName));

                FileUtils.WriteFile(destinationURI, textAsset.text);
            }
            enumerator.Dispose();

            yield break;
        }

        private TimiSharedURI FirstLaunchDestinationURI {
            get {
                return new TimiSharedURI(FileBasePathType.LocalPersistentDataPath, "AppData/DataModels");
            }
        }

        private const string kAppVersionKey = "app_version";
        private const string kAppVersion = "1.1.2";
        private bool IsFirstLaunch() {
            string appVersion = PlayerPrefs.GetString(kAppVersionKey, "");
            return !FileUtils.DoesDirectoryExist(this.FirstLaunchDestinationURI) ||
                appVersion != kAppVersion;
        }
    }
}