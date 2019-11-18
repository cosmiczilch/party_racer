using Data;
using TimiMultiPlayer;
using TimiShared.Debug;
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

        public CarController(CarDataModel carDataModel) {
            this.CarDataModel = carDataModel;
        }

        public void CreateView(Transform startingPositionTransform) {
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
        }

        public void HandleGasPedalDown() {
            DebugLog.LogColor("Gas Pedal down", LogColor.red);

        }

        public void HandleGasPedalUp() {
            DebugLog.LogColor("Gas Pedal up", LogColor.yellow);

        }

    }
}