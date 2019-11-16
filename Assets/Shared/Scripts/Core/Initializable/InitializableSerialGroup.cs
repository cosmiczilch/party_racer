using System.Collections;
using System.Collections.Generic;
using TimiShared.Debug;
using UnityEngine;

namespace TimiShared.Init {

    public class InitializableSerialGroup : InitializableGroup {

        public InitializableSerialGroup(string groupName, List<IInitializable> initializables = null) : base(groupName, initializables) {
        }

        #region IInitializable
        public override void StartInitialize() {
            CoroutineHelper.Instance.RunCoroutine(this.InitializeGroup(() => {
                this.IsFullyInitialized = true;
            }));
        }
        #endregion

        private IEnumerator InitializeGroup(System.Action callback) {
            if (this._initializables == null || this._initializables.Count == 0) {
                yield break;
            }

            var enumerator = this._initializables.GetEnumerator();
            while (enumerator.MoveNext()) {
                if (enumerator.Current == null) {
                    DebugLog.LogErrorColor("Initializable object list in group " + this._groupName + " has a null object", LogColor.red);
                    continue;
                }
                IInitializable initializable = enumerator.Current;
                DebugLog.LogColor("Initializing " + initializable.GetName, LogColor.green);

                float startTimeInSeconds = Time.fixedTime;
                initializable.StartInitialize();

                while (!initializable.IsFullyInitialized) {
                    if ((Time.fixedTime - startTimeInSeconds) > this._timeoutSeconds) {
                        DebugLog.LogErrorColor("Initialization timed out waiting for: " + initializable.GetName, LogColor.red);
                        break;
                    }
                    yield return null;
                }
            }

            callback.Invoke();
        }
    }
}