using System.Collections;
using System.Collections.Generic;
using TimiShared.Debug;
using UnityEngine;

namespace TimiShared.Init {

    public class InitializableParallelGroup : InitializableGroup {

        public InitializableParallelGroup(string groupName, List<IInitializable> initializables = null) : base(groupName, initializables) {
        }

        #region IInitializable
        public override void StartInitialize() {
            CoroutineHelper.Instance.RunCoroutine(this.InitializeGroup());
            CoroutineHelper.Instance.RunCoroutine(this.WaitForGroupToFinishInitializing());
        }
        #endregion

        private IEnumerator InitializeGroup() {
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
                initializable.StartInitialize();
            }
        }

        private IEnumerator WaitForGroupToFinishInitializing() {

            float startTimeInSeconds = Time.fixedTime;

            bool fullyInitialized;
            do {
                fullyInitialized = true;

                var enumerator = this._initializables.GetEnumerator();
                while (enumerator.MoveNext()) {
                    IInitializable initializable = enumerator.Current;
                    if (!initializable.IsFullyInitialized) {
                        fullyInitialized = false;
                        break;
                    }
                }

                if (!fullyInitialized) {
                    if ((Time.fixedTime - startTimeInSeconds) > this._timeoutSeconds) {
                        DebugLog.LogErrorColor("Initialization timed out waiting for: " + this.GetName, LogColor.red);
                        break;
                    }
                    yield return null;
                }
            } while (!fullyInitialized);
            this.IsFullyInitialized = true;
        }

    }
}