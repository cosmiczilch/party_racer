using TimiShared.UI;

namespace Lobby {
    public class UIRaceSelectController : DialogControllerBase<UIRaceSelectView> {

        private const string kPrefabPath = "Prefabs/UI/UIRaceSelectView";
        protected override string GetDialogViewPrefabPath() {
            return kPrefabPath;
        }

        private ILobbyMenuDelegate _lobbyMenuDelegate;

        public UIRaceSelectController(ILobbyMenuDelegate lobbyMenuDelegate) {
            this._lobbyMenuDelegate = lobbyMenuDelegate;
        }

        protected override void ConfigureView() {
            this.View.Configure(new UIRaceSelectView.Config {
                onCloseCallback = this.HandleCloseCallback,
                onSinglePlayerCallback = this.HandleSinglePlayerCallback,
                onMultiPlayerCallback = this.HandleMultiPlayerCallback
            });
        }

        private void HandleMultiPlayerCallback() {
            if (this._lobbyMenuDelegate != null) {
                this._lobbyMenuDelegate.HandleMultiPlayerRaceSelected();
            }
            this.RemoveDialog();
        }

        private void HandleSinglePlayerCallback() {
            if (this._lobbyMenuDelegate != null) {
                this._lobbyMenuDelegate.HandleSinglePlayerRaceSelected();
            }
            this.RemoveDialog();
        }

        private void HandleCloseCallback() {
            this.RemoveDialog();
        }
    }
}