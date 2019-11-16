using TimiShared.UI;

namespace Lobby {

    public class UILobbyView : DialogViewBase {

        public class Config {
            public System.Action onRaceButtonCallback;
        }
        private Config _config;

        public void Configure(Config config) {
            this._config = config;
        }

    }
}