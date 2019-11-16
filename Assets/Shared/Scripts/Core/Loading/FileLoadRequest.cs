using System.IO;
using TimiShared.Debug;
using UnityEngine.Networking;

namespace TimiShared.Loading {

    public class FileLoadRequest : AsyncRequest {

        private string _filePath;
        private FileMode _fileMode;
        private FileAccess _accessType;

        private UnityWebRequest _wwwRequest;

        private Stream _loadedFileStream;
        public Stream LoadedFileStream {
            get {
                return this._loadedFileStream;
            }
        }

        public FileLoadRequest(string filePath, FileMode fileMode, FileAccess accessType) {
            this._filePath = filePath;
            this._fileMode = fileMode;
            this._accessType = accessType;
        }

        ~FileLoadRequest() {
            if (this._loadedFileStream != null) {
                this._loadedFileStream.Close();
            }
        }

        #region CustomYieldInstruction
        public override void StartRequest() {
            if (string.IsNullOrEmpty(this._filePath)) {
                DebugLog.LogErrorColor("No such file", LogColor.red);
                return;
            }

            if (!this._filePath.Contains("://")) {
                // Regular File i/o should work here
                this._loadedFileStream = new FileStream(this._filePath, this._fileMode, this._accessType);
            } else {
                this._wwwRequest = UnityWebRequest.Get(this._filePath);
                if (this._wwwRequest == null) {
                    DebugLog.LogErrorColor("www object is null for file path: " + this._filePath, LogColor.red);
                    return;
                }
                this._wwwRequest.downloadHandler = new FileLoadRequestDownloadHandler(onCompleteCallback: (Stream loadedStream) => {
                    this._loadedFileStream = loadedStream;
                });
                this._wwwRequest.SendWebRequest();
            }
        }

        public override bool keepWaiting {
            get {

                if (this._wwwRequest != null) {
                    if (!this._wwwRequest.isDone) {
                        return true;
                    }
                    if (this._wwwRequest.isNetworkError || this._wwwRequest.isHttpError) {
                        DebugLog.LogErrorColor("Error loading " + this._filePath + ": " + this._wwwRequest.error, LogColor.red);
                        this._loadedFileStream = null;
                        return false;
                    }
                }

                if (this._loadedFileStream != null) {
                    return false;
                }

                return true;
            }
        }
        #endregion

        private class FileLoadRequestDownloadHandler : DownloadHandlerScript {

            private MemoryStream _dataStream = new MemoryStream();
            private int _dataLength = 0;

            private System.Action<Stream> _onCompleteCallback;

            public FileLoadRequestDownloadHandler(System.Action<Stream> onCompleteCallback) : base() {
                this._onCompleteCallback = onCompleteCallback;
            }

            protected override byte[] GetData() { return null; }

            protected override bool ReceiveData(byte[] data, int dataLength) {
                if(data == null || data.Length < 1) {
                    DebugLog.LogWarningColor("Received a null/empty buffer", LogColor.orange);
                    return false;
                }

                this._dataStream.Write(data, 0, dataLength);
                this._dataLength += dataLength;

                return true;
            }

            protected override void CompleteContent() {
                this._dataStream.Flush();
                this._dataStream.Seek(0, SeekOrigin.Begin);

                this._onCompleteCallback.Invoke(this._dataStream);
            }
        }

    }
}