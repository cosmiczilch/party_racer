using TimiShared.UI;

namespace Lobby {

    public class UILobbyController : DialogControllerBase<UILobbyView> {

        private const string kPrefabPath = "Prefabs/UI/UILobbyView";
        protected override string GetDialogViewPrefabPath() {
            return kPrefabPath;
        }

        private ILobbyMenuDelegate _lobbyMenuDelegate;

        public UILobbyController(ILobbyMenuDelegate lobbyMenuDelegate) {
            this._lobbyMenuDelegate = lobbyMenuDelegate;
        }

        protected override void ConfigureView() {
            this.View.Configure(new UILobbyView.Config {
                onRaceButtonCallback = this.HandleRaceButtonCallback,
                onNextCarClickedCallback = this.HandleNextCarClickedCallback,
                onPrevCarClickedCallback = this.HandlePrevCarClickedCallback
            });
        }

        private void HandleNextCarClickedCallback() {
            if (this._lobbyMenuDelegate != null) {
                this._lobbyMenuDelegate.HandleNextCarButtonClicked();
            }
        }

        private void HandlePrevCarClickedCallback() {
            if (this._lobbyMenuDelegate != null) {
                this._lobbyMenuDelegate.HandlePrevCarButtonClicked();
            }
        }

        private void HandleRaceButtonCallback() {
            if (this._lobbyMenuDelegate != null) {
                this._lobbyMenuDelegate.HandleRaceButtonClicked();
            }
        }
    }
}