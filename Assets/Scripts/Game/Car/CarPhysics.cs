using TimiShared.Debug;
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
        }

        private void FixedUpdate() {
            if (this.Controller.IsGasPedalDown) {
                this._carRigidBody.AddForce(this.View.transform.forward * 0.5f, ForceMode.Impulse);
            }
        }
    }
}