using Data;
using TimiShared.Extensions;
using TimiShared.Loading;
using UnityEngine;

namespace Lobby {

    public class LobbyView : MonoBehaviour {

        [SerializeField] private Transform _carAnchor = null;

        private Transform _loadedCar;

        public void SetCurrentCar(CarDataModel carDataModel) {
            this.UnloadCurrentCar();
            GameObject go = PrefabLoader.Instance.InstantiateSynchronous(carDataModel.prefabPath, this._carAnchor);
            go.AssertNotNull("Car game object for car id: " + carDataModel.carId);
            this._loadedCar = go.transform;
        }

        private void UnloadCurrentCar() {
            if (this._loadedCar != null) {
                GameObject.Destroy(this._loadedCar.gameObject);
                this._loadedCar = null;
            }
        }

    }
}