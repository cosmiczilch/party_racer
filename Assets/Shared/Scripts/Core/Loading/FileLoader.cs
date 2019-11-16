using System.IO;
using TimiShared.Debug;
using UnityEngine;

namespace TimiShared.Loading {
    public class FileLoader {

        #region Public API
        /**
         Currently supports loading files from persistent data path everywhere, and
         from streaming assets and data path iff editor
         */
        public static Stream GetFileStreamSync(TimiSharedURI fileURI, FileMode mode, FileAccess accessType) {
            if (!FileLoader.CheckParameters(fileURI, mode, accessType)) {
                return null;
            }

            if (fileURI.BasePathType != FileBasePathType.LocalPersistentDataPath) {
                if (!Application.isEditor ||
                    (fileURI.BasePathType != FileBasePathType.LocalDataPath &&
                     fileURI.BasePathType != FileBasePathType.LocalStreamingAssetsPath)) {
                    DebugLog.LogErrorColor("Synchronous loads disallowed on uri: " + fileURI.ToString(), LogColor.red);
                    return null;
                }
            }

            FileStream fileStream = new FileStream(fileURI.GetFullPath(), mode, accessType);
            return fileStream;
        }

        public static FileLoadRequest GetFileStreamAsync(TimiSharedURI fileURI, FileMode mode, FileAccess accessType) {
            if (!FileLoader.CheckParameters(fileURI, mode, accessType)) {
                return null;
            }

            return new FileLoadRequest(fileURI.GetFullPath(), mode, accessType);
        }
        #endregion


        private static bool CheckParameters(TimiSharedURI fileURI, FileMode mode, FileAccess accessType) {
            if (accessType == FileAccess.Write || accessType == FileAccess.ReadWrite) {
                if (fileURI.BasePathType != FileBasePathType.LocalPersistentDataPath) {
                    if (!Application.isEditor ||
                    (fileURI.BasePathType != FileBasePathType.LocalDataPath &&
                    fileURI.BasePathType != FileBasePathType.LocalStreamingAssetsPath)) {
                        DebugLog.LogErrorColor("Writes disallowed on uri: " + fileURI.ToString(), LogColor.red);
                        return false;
                    }
                }
            }

            if (string.IsNullOrEmpty(fileURI.GetFullPath())) {
                DebugLog.LogErrorColor("File path empty", LogColor.red);
                return false;
            }

            return true;
        }
    }
}
