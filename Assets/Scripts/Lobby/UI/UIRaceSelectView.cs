using TimiShared.UI;

namespace Lobby {
    public class UIRaceSelectView : DialogViewBase {

        public class Config {
            public System.Action onCloseCallback;
            public System.Action onSinglePlayerCallback;
            public System.Action onMultiPlayerCallback;
        }
        private Config _config;

        public void Configure(Config config) {
            this._config = config;
        }

        public void OnCloseButtonClicked() {
            if (this._config != null && this._config.onCloseCallback != null) {
                this._config.onCloseCallback.Invoke();
            }
        }

        public void OnSinglePlayerButtonClicked() {
            if (this._config != null && this._config.onSinglePlayerCallback != null) {
                this._config.onSinglePlayerCallback.Invoke();
            }
        }

        public void OnMultiPlayerButtonClicked() {
            if (this._config != null && this._config.onMultiPlayerCallback != null) {
                this._config.onMultiPlayerCallback.Invoke();
            }
        }

    }
}