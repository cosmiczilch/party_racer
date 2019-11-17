using Data;
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
            GameObject carGO = PrefabLoader.Instance.InstantiateSynchronous(this.CarDataModel.prefabPath, null);
            carGO.AssertNotNull("Car view game object");

            this.View = carGO.GetComponent<CarView>();
            this.View.AssertNotNull("Car view component");

            this.View.transform.SetPositionAndRotation(startingPositionTransform.position, startingPositionTransform.rotation);
        }

    }
}