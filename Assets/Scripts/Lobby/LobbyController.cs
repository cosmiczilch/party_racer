using System.Collections.Generic;
using Data;
using ExitGames.Client.Photon;
using Game;
using TimiMultiPlayer;
using TimiShared.Debug;
using TimiShared.Extensions;
using TimiShared.Loading;
using UI;
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
            AppSceneManager.Instance.LoadGameScene(new GameController.Config_t {
                    gameType = GameController.GameType_t.SinglePlayer,
                    carDataModel = this.CurrentCar,
                    playerIndex = 1
                },
                callback: () => {
                    this.UIView.RemoveDialog();
                });
        }


        public void HandleMultiPlayerRaceSelected() {
            this.ShowCreatingMultiplayerRaceDialog(UICreatingMultiplayerRaceView.State.Connecting);

            AppMultiPlayerManager.Instance.CreateAndStartGame(
                joinRoomSuccessCallback: () => {
                    this.ShowCreatingMultiplayerRaceDialog(UICreatingMultiplayerRaceView.State.WaitingForOpponents);
                },
                readyToStartGameCallback: () => {
                    this.ShowCreatingMultiplayerRaceDialog(UICreatingMultiplayerRaceView.State.StartingRace);
                    AppSceneManager.Instance.LoadGameScene(new GameController.Config_t {
                        gameType = GameController.GameType_t.MultiPlayer,
                        carDataModel = this.CurrentCar,
                        playerIndex = AppMultiPlayerManager.Instance.LocalPlayerIndex()
                    });
                },
                createdSceneViewsCallback: () => {
                    DebugLog.LogColor("All scene views created. Ready to start race", LogColor.green);
                    this.RemoveCreatingMultiplayerRaceDialog();
                    this.UIView.RemoveDialog();
                    if (GameController.Instance != null) {
                        GameController.Instance.ReadyToStartGame = true;
                    }
                },
                timedOutWaitingForPlayersCallback: () => {
                    this.RemoveCreatingMultiplayerRaceDialog();
                    UIGenericMessageController genericMessageController = new UIGenericMessageController("Timed out trying to find other players at this time. Please try again later.");
                    genericMessageController.PresentDialog();
                },
                failureCallback: () => {
                    this.RemoveCreatingMultiplayerRaceDialog();
                    UIGenericMessageController genericMessageController = new UIGenericMessageController("Failed to connect to MultiPlayer services at this time. Please try again later.");
                    genericMessageController.PresentDialog();
                });

        }
        #endregion

        #region Creating multiplayer race ui helpers
        private UICreatingMultiplayerRaceController _creatingMultiplayerRaceController;

        private void ShowCreatingMultiplayerRaceDialog(UICreatingMultiplayerRaceView.State state) {
            if (this._creatingMultiplayerRaceController == null) {
                this._creatingMultiplayerRaceController = new UICreatingMultiplayerRaceController(state);
                this._creatingMultiplayerRaceController.PresentDialog();
            } else {
                this._creatingMultiplayerRaceController.SetState(state);
            }
        }

        private void RemoveCreatingMultiplayerRaceDialog() {
            if (this._creatingMultiplayerRaceController != null) {
                this._creatingMultiplayerRaceController.RemoveDialog();
                this._creatingMultiplayerRaceController = null;
            }
        }
        #endregion
    }
}