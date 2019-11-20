using Data;
using TimiMultiPlayer;
using TimiShared.Extensions;
using TimiShared.Loading;
using UnityEngine;

namespace Game.Car {

    public class CarController {

        public CarDataModel CarDataModel {
            get; private set;
        }

        public CarView View {
            get; private set;
        }

        public CarPhysics CarPhysics {
            get; private set;
        }

        public float CurrentSpeed {
            get {
                if (this.CarPhysics != null) {
                    return this.CarPhysics.CurrentSpeed;
                }
                return 0.0f;
            }
        }

        public CarController(CarDataModel carDataModel) {
            this.CarDataModel = carDataModel;
        }

        private const string kCarPhysicsPrefabPath = "Prefabs/Game/CarPhysics";
        private const string kCarCameraHolderPrefabPath = "Prefabs/Game/CarCameraHolder";

        public void CreateViewAndPhysicsController(Transform startingPositionTransform) {
            GameObject carGO;
            if (GameController.Instance.GameType == GameController.GameType_t.SinglePlayer) {
                carGO = PrefabLoader.Instance.InstantiateSynchronous(this.CarDataModel.racePrefabPath, null);
            } else {
                carGO = AppMultiPlayerManager.Instance.InstantiatePrefab(this.CarDataModel.racePrefabPath, null);
            }
            carGO.AssertNotNull("Car view game object");

            this.View = carGO.GetComponent<CarView>();
            this.View.AssertNotNull("Car view component");

            this.View.transform.SetPositionAndRotation(startingPositionTransform.position, startingPositionTransform.rotation);

            PrefabLoader.Instance.InstantiateSynchronous(kCarCameraHolderPrefabPath, this.View.transform);

            GameObject carPhysicsGO = PrefabLoader.Instance.InstantiateSynchronous(kCarPhysicsPrefabPath, null);
            carPhysicsGO.AssertNotNull("Car Physics game object");

            this.CarPhysics = carPhysicsGO.GetComponent<CarPhysics>();
            this.CarPhysics.AssertNotNull("Car Physics component");
            this.CarPhysics.Initialize(this, this.View);
        }

        public bool IsGasPedalDown {
            get; private set;
        }

        private float _gasPedalUpTime = float.MinValue;
        private float _gasPedalDownTime = float.MaxValue;

        public float TimeSinceGasPedalDown {
            get {
                return Mathf.Max(Time.time - this._gasPedalDownTime, 0.0f);
            }
        }

        public float TimeSinceGasPedalUp {
            get {
                return Mathf.Max(Time.time - this._gasPedalUpTime, 0.0f);
            }
        }

        public void HandleGasPedalDown() {
            this._gasPedalDownTime = Time.time;
            this.IsGasPedalDown = true;
        }

        public void HandleGasPedalUp() {
            this._gasPedalUpTime = Time.time;
            this.IsGasPedalDown = false;
        }

        public bool IsBrakePedalDown {
            get; private set;
        }

        public void HandleBrakePedalDown() {
            this.IsBrakePedalDown = true;
        }

        public void HandleBrakePedalUp() {
            this.IsBrakePedalDown = false;
        }
    }
}