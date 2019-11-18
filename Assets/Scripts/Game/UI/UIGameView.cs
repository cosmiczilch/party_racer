using TimiShared.UI;

namespace Game.UI {
    public class UIGameView : DialogViewBase {

        public class Config {
            public System.Action onLeaveRaceButtonCallback;
            public System.Action onGasPedalDownCallback;
            public System.Action onGasPedalUpCallback;
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
    }
}