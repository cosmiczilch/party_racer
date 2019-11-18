using Lobby;
using TimiShared.UI;
using UI;

namespace Game.UI {
    public class UIGameController : DialogControllerBase<UIGameView> {

        private const string kPrefabPath = "Prefabs/UI/UIGameView";
        protected override string GetDialogViewPrefabPath() {
            return kPrefabPath;
        }

        public class Config {
            public System.Action onGasPedalDownCallback;
            public System.Action onGasPedalUpCallback;
        }
        private Config _config;

        public UIGameController(Config config) {
            this._config = config;
        }

        protected override void ConfigureView() {
            this.View.Configure(new UIGameView.Config {
                onLeaveRaceButtonCallback = this.HandleLeaveRaceButtonCallback,
                onGasPedalDownCallback = this.HandleGasPedalDownCallback,
                onGasPedalUpCallback = this.HandleGasPedalUpCallback
            });
        }

        private void HandleGasPedalUpCallback() {
            if (this._config != null && this._config.onGasPedalUpCallback != null) {
                this._config.onGasPedalUpCallback.Invoke();
            }
        }

        private void HandleGasPedalDownCallback() {
            if (this._config != null && this._config.onGasPedalDownCallback != null) {
                this._config.onGasPedalDownCallback.Invoke();
            }
        }

        private void HandleLeaveRaceButtonCallback() {
            UIOkCancelController okCancelController = new UIOkCancelController("Do you really want to leave this race?",
                "Yes", "No",
                onPositiveCallback: () => {
                    GameController.Instance.LeaveGame();
                },
                onNegativeCallback: null);
            okCancelController.PresentDialog();
        }
    }
}