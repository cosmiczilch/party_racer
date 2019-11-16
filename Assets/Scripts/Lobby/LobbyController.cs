using System.Collections.Generic;
using Data;
using ExitGames.Client.Photon;
using TimiShared.Debug;
using TimiShared.Extensions;
using TimiShared.Loading;
using UnityEngine;

namespace Lobby {

    public class LobbyController : ILobbyMenuDelegate {

        public LobbyView View {
            get; private set;
        }

        public UILobbyController UIView {
            get; private set;
        }

        public CarDataModel CurrentCar {
            get {
                return AppData.Instance.GetCarDataModelByCarId(this._allCarIds[this._currentCarIndex]);
            }
        }
        private List<int> _allCarIds;
        private int _currentCarIndex;


        private const string kLobbyViewPath = "Prefabs/Lobby/LobbyView";

        public LobbyController() {
            this._allCarIds = AppData.Instance.GetCarDataModelIds();
            this._currentCarIndex = 0;

            this.CreateView();
        }

        private void CreateView() {
            GameObject lobbyViewGO = PrefabLoader.Instance.InstantiateSynchronous(kLobbyViewPath, null);
            lobbyViewGO.AssertNotNull("Lobby View game object");
            this.View = lobbyViewGO.GetComponent<LobbyView>();
            this.View.AssertNotNull("Lobby View component");

            this.RefreshCurrentCarView();

            this.UIView = new UILobbyController(this);
            this.UIView.PresentDialog();
        }

        private void RefreshCurrentCarView() {
            this.View.SetCurrentCar(this.CurrentCar);
        }

        #region ILobbyMenuDelegate
        public void HandleRaceButtonClicked() {
            UIRaceSelectController raceSelectController = new UIRaceSelectController(this);
            raceSelectController.PresentDialog();
        }

        public void HandleNextCarButtonClicked() {
            this._currentCarIndex = (this._currentCarIndex + 1) % this._allCarIds.Count;
            this.RefreshCurrentCarView();
        }

        public void HandlePrevCarButtonClicked() {
            this._currentCarIndex = (this._currentCarIndex + this._allCarIds.Count - 1) % this._allCarIds.Count;
            this.RefreshCurrentCarView();
        }

        public void HandleSinglePlayerRaceSelected() {
            DebugLog.LogColor("Single player race", LogColor.green);
        }

        public void HandleMultiPlayerRaceSelected() {
            DebugLog.LogColor("Multi player race", LogColor.green);
        }
        #endregion
    }
}