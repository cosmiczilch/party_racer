using System.Collections;
using System.Collections.Generic;
using TimiShared.Debug;
using UnityEngine;

namespace TimiShared.Init {

    public abstract class InitializableGroup : IInitializable {

        private const float MAX_TIMEOUT_IN_SECONDS = 10;

        protected string _groupName;
        protected float _timeoutSeconds;
        protected List<IInitializable> _initializables;

        public InitializableGroup(string groupName, List<IInitializable> initializables = null) {
            this._groupName = groupName;
            this._timeoutSeconds = MAX_TIMEOUT_IN_SECONDS;
            this._initializables = initializables ?? new List<IInitializable>();
        }

        public void AddInitializable(IInitializable initializable) {
            if (initializable != null) {
                this._initializables.Add(initializable);
            } else {
                DebugLog.LogWarningColor("Trying to add null initializable to " + this.GetName, LogColor.orange);
            }
        }

        public void SetTimeoutSeconds(float seconds) {
            this._timeoutSeconds = seconds;
        }

        #region IInitializable
        public abstract void StartInitialize();

        public bool IsFullyInitialized {
            get; protected set;
        }

        public string GetName {
            get {
                return "group " + this._groupName;
            }
        }
        #endregion
    }
}