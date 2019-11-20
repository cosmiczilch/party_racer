using TimiShared.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI {
    public class UIGameView : DialogViewBase {

        [SerializeField] private Transform _speedometerNeedle = null;
        [SerializeField] private Text _speedometerReadingText = null;

        public class Config {
            public System.Action onLeaveRaceButtonCallback;
            public System.Action onGasPedalDownCallback;
            public System.Action onGasPedalUpCallback;
            public System.Action onBrakePedalDownCallback;
            public System.Action onBrakePedalUpCallback;
        }
        private Config _config;

        public void Configure(Config config) {
            this._config = config;
        }

        public void OnLeaveRaceButtonClicked() {
            if (this._config != null && this._config.onLeaveRaceButtonCallback != null) {
                this._config.onLeaveRaceButtonCallback.Invoke();
            }
        }

        public void OnGasPedalDown() {
            if (this._config != null && this._config.onGasPedalDownCallback != null) {
                this._config.onGasPedalDownCallback.Invoke();
            }
        }

        public void OnGasPedalUp() {
            if (this._config != null && this._config.onGasPedalUpCallback != null) {
                this._config.onGasPedalUpCallback.Invoke();
            }
        }

        public void OnBrakePedalDown() {
            if (this._config != null && this._config.onBrakePedalDownCallback != null) {
                this._config.onBrakePedalDownCallback.Invoke();
            }
        }

        public void OnBrakePedalUp() {
            if (this._config != null && this._config.onBrakePedalUpCallback != null) {
                this._config.onBrakePedalUpCallback.Invoke();
            }
        }

        private void UpdateSpeedometer() {
            if (GameController.Instance != null &&
                GameController.Instance.OurCar != null) {

                float speedInGame = GameController.Instance.OurCar.CurrentSpeed;
                float normalizedSpeed = speedInGame / AppData.Instance.CarStatDisplayDataModel.carSpeedMax_game_units;
                float needleAngle = Mathf.Lerp(-90, 90, normalizedSpeed);

                this._speedometerNeedle.transform.rotation = Quaternion.AngleAxis(needleAngle, -this._speedometerNeedle.transform.forward);

                float realSpeed = speedInGame * AppData.Instance.CarStatDisplayDataModel.carSpeedConversionFactor_game_to_real;
                this._speedometerReadingText.text = ((int)realSpeed).ToString() + " MPH";
            }
        }

        private void Update() {
            this.UpdateSpeedometer();
        }
    }
}