using System.Collections;
using TimiShared.Extensions;
using TimiShared.Loading;
using UnityEngine;

namespace TimiShared.Init {
    public class SharedInit : MonoBehaviour, IInitializable {

        [SerializeField] private PrefabLoader _prefabLoader = null;
        [SerializeField] private SceneLoader _sceneLoader = null;

        #region IInitializable
        public void StartInitialize() {
            this.StartCoroutine(this.InitializationSequence(() => {
                this.IsFullyInitialized = true;
            }));
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

        public void Cleanup() {
            if (!this.IsFullyInitialized) {
                return;
            }
        }

        private IEnumerator InitializationSequence(System.Action callback) {

            InitializableSerialGroup initializables = new InitializableSerialGroup("Shared Init");

            this._prefabLoader.AssertNotNull("Prefab loader");
            initializables.AddInitializable(this._prefabLoader);

            this._sceneLoader.AssertNotNull("Scene loader");
            initializables.AddInitializable(this._sceneLoader);

            initializables.StartInitialize();
            while (!initializables.IsFullyInitialized) {
                yield return null;
            }

            callback.Invoke();
        }

    }
}