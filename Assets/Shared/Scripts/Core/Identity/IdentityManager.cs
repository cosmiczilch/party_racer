using System;
using System.Security.Cryptography;
using System.Text;
using TimiShared.Debug;
using TimiShared.Init;
using TimiShared.Instance;
using UnityEngine;

namespace TimiShared.Identity {

    public class IdentityManager : MonoBehaviour, IInstance, IInitializable {

        #region Public Api
        public static IdentityManager Instance {
            get {
                return InstanceLocator.Instance<IdentityManager>();
            }
        }

        private User _currentUser;
        public User CurrentUser {
            get {
                return this._currentUser;
            }
        }
        #endregion

        #region IInitializable
        public void StartInitialize() {
            InstanceLocator.RegisterInstance<IdentityManager>(this);

            string duid = SystemInfo.deviceUniqueIdentifier;
            if (string.IsNullOrEmpty(duid)) {
                DebugLog.LogErrorColor("Failed to get device unique identifier", LogColor.red);
                return;
            }
            MD5 md5hasher = MD5.Create();
            byte[] duid_bytes = md5hasher.ComputeHash(Encoding.UTF8.GetBytes(SystemInfo.deviceUniqueIdentifier));
            int userId = BitConverter.ToInt32(duid_bytes, 0);

            this._currentUser = new User(userId);

            this.IsFullyInitialized = true;
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
    }
}