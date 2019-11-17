using TimiShared.UI;

namespace Game.UI {
    public class UIGameView : DialogViewBase {

        public class Config {
            public System.Action onLeaveRaceButtonCallback;
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
    }
}