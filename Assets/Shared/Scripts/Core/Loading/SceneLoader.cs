using System.Collections;
using TimiShared.Debug;
using TimiShared.Init;
using TimiShared.Instance;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TimiShared.Loading {
    // TODO: Change this to not be a MonoBehaviour; right now the only reason this is a MonoBehaviour is for this.StartCoroutine
    // TODO: Add templated loaders
    public class SceneLoader : MonoBehaviour, IInitializable, IInstance {

        public static SceneLoader Instance {
            get {
                return InstanceLocator.Instance<SceneLoader>();
            }
        }

        #region IInitializable
        public void StartInitialize() {
            InstanceLocator.RegisterInstance<SceneLoader>(this);
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


        #region Public API
        public void LoadSceneAsync(string sceneName, LoadSceneMode mode, System.Action<string, bool> callback) {
            this.StartCoroutine(this.LoadSceneAsyncInternal(sceneName, mode, callback));
        }

        public void LoadSceneSync(string sceneName, LoadSceneMode mode) {
            if (!this.CanLoadScene(sceneName)) {
                DebugLog.LogErrorColor("Scene name not set", LogColor.red);
                return;
            }
            SceneManager.LoadScene(sceneName, mode);
        }

        public void UnloadSceneAsync(string sceneName, System.Action callback) {
            this.StartCoroutine(this.UnloadSceneAsyncInternal(sceneName,  callback));
        }
        #endregion

        private bool CanLoadScene(string sceneName) {
            if (string.IsNullOrEmpty(sceneName)) {
                return false;
            }
            return true;
        }

        private IEnumerator LoadSceneAsyncInternal(string sceneName, LoadSceneMode mode, System.Action<string, bool> callback) {
            AsyncOperation asyncOperation = this.CanLoadScene(sceneName) ? SceneManager.LoadSceneAsync(sceneName, mode) : null;
            if (asyncOperation != null) {
                while (!asyncOperation.isDone) {
                    yield return null;
                }
                if (callback != null) {
                    callback.Invoke(sceneName, true);
                }
            } else {
                DebugLog.LogErrorColor("Could not load scene: " + sceneName, LogColor.red);
                if (callback != null) {
                    callback.Invoke(sceneName, false);
                }
            }
        }

        private IEnumerator UnloadSceneAsyncInternal(string sceneName, System.Action callback) {
            AsyncOperation asyncOperation = SceneManager.UnloadSceneAsync(sceneName);
            if (asyncOperation != null) {
                while (!asyncOperation.isDone) {
                    yield return null;
                }
                if (callback != null) {
                    callback.Invoke();
                }
            }
            else {
                DebugLog.LogErrorColor("Could not unload scene: " + sceneName, LogColor.red);
            }
        }

    }
}
