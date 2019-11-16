using TimiShared.Debug;
using UnityEngine;

namespace Utilities {

    public class CameraFlip : MonoBehaviour {

        [SerializeField] private Camera _camera;
        private Camera Camera {
            get {
                if (this._camera == null) {
                    this._camera = this.gameObject.GetComponent<Camera>();
                }
                return this._camera;
            }
        }

        public enum FlipDirection {
            Horizontal,
            Vertical
        }

        public void FlipCamera(FlipDirection flipDirection) {
            if (this.Camera == null) {
                DebugLog.LogWarningColor("No camera found", LogColor.yellow);
                return;
            }

            switch (flipDirection) {
                case FlipDirection.Horizontal:
                    this.Camera.projectionMatrix *= Matrix4x4.Scale(new Vector3(-1, 1, 1));
                    break;

                case FlipDirection.Vertical:
                    this.Camera.projectionMatrix *= Matrix4x4.Scale(new Vector3(1, -1, 1));
                    break;
            }
        }
    }
}