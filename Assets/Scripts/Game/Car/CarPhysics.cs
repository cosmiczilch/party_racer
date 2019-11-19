using UnityEngine;

namespace Game.Car {

    public class CarPhysics : MonoBehaviour {

        [SerializeField] private AnimationCurve _brakingCurve = null;

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
        }

        private void FixedUpdate() {
            if (this._carRigidBody == null) {
                return;
            }

            this.ApplyYaw();
            this.ApplyThrust();
        }

        private void ApplyThrust() {

            float engineThrust = 2.5f;
            if (this.Controller.IsGasPedalDown) {
                this._carRigidBody.AddForce(this.View.transform.forward * engineThrust, ForceMode.Impulse);
            }
        }

        private void ApplyYaw() {
            int direction = 0;
            if (Input.GetKey(KeyCode.A)) {
                direction = -1;
            }
            else if (Input.GetKey(KeyCode.D)) {
                direction = 1;
            }

            if (direction == 0) {
                return;
            }

            this.View.transform.RotateAround(this.View.transform.position, this.View.transform.up, direction * 0.4f);
        }
    }
}