using System.IO;
using TimiShared.Debug;
using UnityEngine;

namespace TimiShared.Loading {
    public enum FileBasePathType {
        LocalDataPath = 1,
        LocalStreamingAssetsPath = 2,
        LocalPersistentDataPath = 3,
        RemoteWebURL = 10
    }

    public class TimiSharedURI {

        public TimiSharedURI(FileBasePathType basePathType, string relativePath) {
            this.BasePathType = basePathType;
            this.RelativePath = relativePath;
        }

        #region Public API
        public FileBasePathType BasePathType {
            get; private set;
        }

        public string RelativePath {
            get; private set;
        }

        public string FileName {
            get {
                return Path.GetFileName(this.RelativePath);
            }
        }

        public override string ToString() {
           return this.GetFullPath();
        }

        public string GetFullPath() {
            string basePath = null;
            switch (this.BasePathType) {
                case FileBasePathType.LocalDataPath: {
                    basePath = Application.dataPath;
                } break;
                case FileBasePathType.LocalStreamingAssetsPath: {
                    basePath = Application.streamingAssetsPath;
                } break;
                case FileBasePathType.LocalPersistentDataPath: {
                    basePath = Application.persistentDataPath;
                } break;
                case FileBasePathType.RemoteWebURL: {
                    basePath = "";
                } break;
            }
            return Path.Combine(basePath, this.RelativePath);
        }

        public static TimiSharedURI Combine(TimiSharedURI uri1, TimiSharedURI uri2) {
            if (uri1.BasePathType != uri2.BasePathType) {
                DebugLog.LogErrorColor("Cannot combine " + uri1 + " with " + uri2, LogColor.grey);
                return null;
            }
            TimiSharedURI result = new TimiSharedURI(uri1.BasePathType, Path.Combine(uri1.RelativePath, uri2.RelativePath));
            return result;
        }
        #endregion


    }
}