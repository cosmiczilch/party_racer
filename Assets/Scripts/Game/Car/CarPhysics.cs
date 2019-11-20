using UnityEngine;

namespace Game.Car {

    public class CarPhysics : MonoBehaviour {

        public CarController Controller {
            get; private set;
        }

        public CarView View {
            get; private set;
        }

        private Rigidbody _carRigidBody = null;

        public void Initialize(CarController controller, CarView view) {
            this.Controller = controller;
            this.View = view;

            this._carRigidBody = this.View.gameObject.GetComponent<Rigidbody>();
            if (this._carRigidBody != null) {
                this._carRigidBody.mass = this.Controller.CarDataModel.mass;
                this._carRigidBody.drag = this.Controller.CarDataModel.drag;
            }
        }

        private void FixedUpdate() {
            if (this._carRigidBody == null) {
                return;
            }

            this.ApplyYaw();
            this.ApplyThrust();
        }

        private void ApplyThrust() {

            float engineThrust = this.Controller.CarDataModel.engineForce;
            if (this.Controller.IsGasPedalDown) {
                this._carRigidBody.AddForce(this.View.transform.forward * engineThrust, ForceMode.Impulse);
            }
        }

        private void ApplyYaw() {
            float direction = this.GetTurningDirection();

            this.View.transform.RotateAround(this.View.transform.position, this.View.transform.up, direction);
        }

        /// <summary>
        /// Get the turning direction from either keyboard (editor and standalone)
        /// or accelerometer (device)
        /// </summary>
        /// <returns>
        /// 0 implies no turn
        /// [-1, 0) values for left turn
        /// (0, +1] values for right turn
        /// In editor and standalone the value is an integer (-1, 0, or +1)
        /// </returns>
        private float GetTurningDirection() {
            float direction = 0;

#if UNITY_EDITOR || UNITY_STANDALONE
            if (Input.GetKey(KeyCode.A)) {
                direction = -1;
            }
            else if (Input.GetKey(KeyCode.D)) {
                direction = 1;
            }
#else
            direction = Input.acceleration.x * 1.5f;
#endif

            return direction;
        }
    }
}