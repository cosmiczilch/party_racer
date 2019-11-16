using TimiShared.Debug;
using TimiShared.Loading;
using TimiShared.Utils;
using UnityEditor;

namespace TimiShared.Menu {

    public static class TimiSharedMenu {

        [MenuItem("TimiShared/Clear Persistent AppData")]
        static void ClearPersistentAppData() {
            TimiSharedURI appDataURI = new TimiSharedURI(FileBasePathType.LocalPersistentDataPath, "AppData");
            if (FileUtils.DoesDirectoryExist(appDataURI)) {
                FileUtils.DeleteDirectory(appDataURI);
                DebugLog.LogColor("Cleared persistent app data", LogColor.grey);
            }
            else {
                DebugLog.LogColor("No persistent app data exists", LogColor.grey);
            }
        }

    }
}