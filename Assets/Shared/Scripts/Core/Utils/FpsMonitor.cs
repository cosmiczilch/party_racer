using System.Collections.Generic;
using TimiShared.Instance;
using UnityEngine;

namespace TimiShared.Utils {
    public class FpsMonitor : MonoBehaviour, IInstance {

        public static FpsMonitor Instance {
            get {
                return InstanceLocator.Instance<FpsMonitor>();
            }
        }

        #region Public API
        public float AverageFPS { get; private set; }
        #endregion

        [Range(1, 50)]
        [SerializeField] private int _numSamples = 30;

        // Example: if this is set to 5, we'll sample the fps every 5th frame
        // This allows us to get a smooth'ish average of the last few seconds
        // without having to store a very large number of samples.
        // To turn this off, set to 0
        [SerializeField] private float _skipSamples = 10;

        private Queue<float> _samples = new Queue<float>();
        private int _frameNumberOfLastSample = -1;


        // TODO: Change this to not be a MonoBehaviour; right now the only reason this is a MonoBehaviour is for this.StartCoroutine
        private void Start() {
            InstanceLocator.RegisterInstance<FpsMonitor>(this);

            if (this._numSamples <= 0) {
                this._numSamples = 1;
            }
            this.AverageFPS = 1;
        }

        private void Update() {
            if ((Time.frameCount - this._frameNumberOfLastSample) > this._skipSamples) {
                this._frameNumberOfLastSample = Time.frameCount;

                float fps = (1.0f / Time.deltaTime);
                this._samples.Enqueue(fps);

                // TODO: Add unit tests
                if (this._samples.Count == 1) {
                    this.AverageFPS = fps;
                } else {
                    this.AverageFPS = this.AverageFPS + (fps - this.AverageFPS) / (float)this._samples.Count;

                    if (this._samples.Count > this._numSamples) {
                        float removedFps = this._samples.Dequeue();
                        this.AverageFPS = this.AverageFPS - (removedFps - this.AverageFPS) / (float)this._samples.Count;
                    }
                }
            }
        }
    }
}
