using System.Collections;
using TimiShared.Init;
using TimiShared.Instance;
using UnityEngine;

namespace Init {
    public class LoadingScreenManager : MonoBehaviour, IInstance, IInitializable {

        public static LoadingScreenManager Instance {
            get {
                return InstanceLocator.Instance<LoadingScreenManager>();
            }
        }

        [SerializeField] private Transform _loadingScreenTransform = null;
        [SerializeField] private Animator _loadingScreenAnimator = null;
        [SerializeField] private Animator _textAnimator = null;

        [SerializeField] private float _loadingScreenMinDurationSeconds = 1.5f;
        public float MinDurationSeconds {
            get {
                return this._loadingScreenMinDurationSeconds;
            }
        }

        private float _lastLoadingScreenShownTime = float.MinValue;

        private void Awake() {
            InstanceLocator.RegisterInstance<LoadingScreenManager>(this);
        }

        public void StartInitialize() {
            this._lastLoadingScreenShownTime = Time.time;
            this.ShowLoadingScreen(false);
            this.IsFullyInitialized = true;
        }

        public bool IsFullyInitialized { get; private set; }

        public string GetName {
            get {
                return this.GetType().Name;
            }
        }


        public void ShowLoadingScreen(bool show, bool animate = false) {
            if (show) {
                this._lastLoadingScreenShownTime = Time.time;
                this.ShowLoadingScreenInternal(true, animate);
            }
            else {
                if (Time.time - this._lastLoadingScreenShownTime < this._loadingScreenMinDurationSeconds) {
                    this.StartCoroutine(this.DismissLoadingScreenAfterDelay(this._loadingScreenMinDurationSeconds - (Time.time - this._lastLoadingScreenShownTime)));
                }
                else {
                    this.ShowLoadingScreenInternal(false, false);
                }
            }
        }

        private void ShowLoadingScreenInternal(bool show, bool animate) {
            if (this._loadingScreenAnimator != null) {
                this._loadingScreenAnimator.enabled = animate;
            }
            if (this._textAnimator != null) {
                this._textAnimator.enabled = animate;
            }
            this._loadingScreenTransform.gameObject.SetActive(show);
        }

        private IEnumerator DismissLoadingScreenAfterDelay(float delaySeconds) {
            yield return new WaitForSeconds(delaySeconds);
            this.ShowLoadingScreenInternal(false, false);
        }


    }
}