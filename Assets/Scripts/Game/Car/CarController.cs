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

        public CarController(CarDataModel carDataModel) {
            this.CarDataModel = carDataModel;
        }

        private const string kCarPhysicsPrefabPath = "Prefabs/Game/CarPhysics";

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

            GameObject carPhysicsGO = PrefabLoader.Instance.InstantiateSynchronous(kCarPhysicsPrefabPath, null);
            carPhysicsGO.AssertNotNull("Car Physics game object");

            this.CarPhysics = carPhysicsGO.GetComponent<CarPhysics>();
            this.CarPhysics.AssertNotNull("Car Physics component");
            this.CarPhysics.Initialize(this, this.View);
        }

        public bool IsGasPedalDown {
            get; private set;
        }

        public void HandleGasPedalDown() {
            this.IsGasPedalDown = true;
        }

        public void HandleGasPedalUp() {
            this.IsGasPedalDown = false;
        }

    }
}