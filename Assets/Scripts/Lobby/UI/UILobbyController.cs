using Data;
using TimiShared.UI;

namespace Lobby {

    public class UILobbyController : DialogControllerBase<UILobbyView> {

        private const string kPrefabPath = "Prefabs/UI/UILobbyView";
        protected override string GetDialogViewPrefabPath() {
            return kPrefabPath;
        }

        private ILobbyMenuDelegate _lobbyMenuDelegate;
        private CarDataModel _currentCarDataModel;

        public UILobbyController(ILobbyMenuDelegate lobbyMenuDelegate, CarDataModel currentCarDataModel) {
            this._lobbyMenuDelegate = lobbyMenuDelegate;
            this._currentCarDataModel = currentCarDataModel;
        }

        public void UpdateForCurrrentCar(CarDataModel carDataModel) {
            this._currentCarDataModel = carDataModel;
            if (this.View != null) {
                this.View.UpdateForCurrentCar(this._currentCarDataModel);
            }
        }

        protected override void ConfigureView() {
            this.View.Configure(new UILobbyView.Config {
                onRaceButtonCallback = this.HandleRaceButtonCallback,
                onNextCarClickedCallback = this.HandleNextCarClickedCallback,
                onPrevCarClickedCallback = this.HandlePrevCarClickedCallback
            });
            this.View.UpdateForCurrentCar(this._currentCarDataModel);
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