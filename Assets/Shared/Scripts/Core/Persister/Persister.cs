using System.IO;
using TimiShared.Debug;
using TimiShared.Extensions;
using TimiShared.Loading;
using TimiShared.Utils;

namespace TimiShared.Persister {

    public class Persister {

        private IPersistable _target;

        public Persister(IPersistable target) {
            this._target = target;
        }

        public bool Save() {
            if (this._target == null) {
                DebugLog.LogErrorColor("No persistable target set", LogColor.red);
                return false;
            }

            TimiSharedURI baseURI = this.BaseURI;
            TimiSharedURI fileURI = this.FullFileURI;

            if (!FileUtils.DoesDirectoryExist(baseURI)) {
                FileUtils.CreateDirectory(baseURI);
            }

            using (Stream fileStream = FileLoader.GetFileStreamSync(fileURI, FileMode.Create, FileAccess.ReadWrite)) {
                if (fileStream != null) {
                    TimiSharedSerializer.Serialize(fileStream, this._target);
                    fileStream.Flush();
                    fileStream.Close();
                    return true;

                } else {
                    DebugLog.LogErrorColor("Unable to get file stream for writing for " + fileURI.GetFullPath(), LogColor.grey);
                }
            }
            return false;
        }

        public bool Restore() {
            TimiSharedURI fileURI = this.FullFileURI;

            if (!FileUtils.DoesFileExist(fileURI)) {
                return false;
            }

            using (Stream fileStream = FileLoader.GetFileStreamSync(fileURI, FileMode.Open, FileAccess.Read)) {
                if (fileStream != null) {
                    var obj = TimiSharedSerializer.DeserializeNonGeneric(this._target.GetType(), fileStream);
                    return this._target.CopyObjectFieldsFrom(obj);

                } else {
                    DebugLog.LogErrorColor("Unable to get file stream for reading for " + fileURI.GetFullPath(), LogColor.grey);
                }
            }

            return false;
        }

        #region Helpers
        public TimiSharedURI BaseURI {
            get {
                return new TimiSharedURI(FileBasePathType.LocalPersistentDataPath, this._target.GetBaseFolderName());
            }
        }

        public TimiSharedURI FullFileURI {
            get {
                TimiSharedURI relativeURI = new TimiSharedURI(FileBasePathType.LocalPersistentDataPath, this._target.GetFileName());
                return TimiSharedURI.Combine(this.BaseURI, relativeURI);
            }
        }
        #endregion

    }
}