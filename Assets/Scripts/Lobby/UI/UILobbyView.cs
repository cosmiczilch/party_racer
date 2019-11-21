using Data;
using TimiShared.UI;
using UnityEngine;

namespace Lobby {

    public class UILobbyView : DialogViewBase {

        [SerializeField] private UICarStatsDisplay _carStatsDisplay = null;

        public class Config {
            public System.Action onRaceButtonCallback;
            public System.Action onNextCarClickedCallback;
            public System.Action onPrevCarClickedCallback;
        }
        private Config _config;

        public void Configure(Config config) {
            this._config = config;
        }

        public void UpdateForCurrentCar(CarDataModel carDataModel) {
            this._carStatsDisplay.Configure(carDataModel);
        }

        public void OnNextCarClicked() {
            if (this._config != null && this._config.onNextCarClickedCallback != null) {
                this._config.onNextCarClickedCallback.Invoke();
            }
        }

        public void OnPrevCarClicked() {
            if (this._config != null && this._config.onPrevCarClickedCallback != null) {
                this._config.onPrevCarClickedCallback.Invoke();
            }
        }

        public void OnRaceButtonClicked() {
            if (this._config != null && this._config.onRaceButtonCallback != null) {
                this._config.onRaceButtonCallback.Invoke();
            }
        }

    }
}