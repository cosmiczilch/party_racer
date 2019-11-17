using TimiShared.UI;

namespace Lobby {

    public class UICreatingMultiplayerRaceController : DialogControllerBase<UICreatingMultiplayerRaceView> {

        private const string kPrefabPath = "Prefabs/UI/UICreatingMultiplayerRaceView";
        protected override string GetDialogViewPrefabPath() {
            return kPrefabPath;
        }

        private UICreatingMultiplayerRaceView.State _state;

        public UICreatingMultiplayerRaceController(UICreatingMultiplayerRaceView.State state) {
            this._state = state;
        }

        public void SetState(UICreatingMultiplayerRaceView.State state) {
            this._state = state;
            this.ConfigureViewState();
        }

        protected override void ConfigureView() {
            this.ConfigureViewState();
        }

        private void ConfigureViewState() {
            this.View.Configure(this._state);
        }
    }
}