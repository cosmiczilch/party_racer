using System.Collections;
using Data;
using Game.Car;
using Game.UI;
using Lobby;
using TimiMultiPlayer;
using TimiShared.Extensions;
using TimiShared.Instance;
using TimiShared.Loading;
using UnityEngine;

namespace Game {

    public class GameController : IInstance {

        public static GameController Instance {
            get {
                return InstanceLocator.Instance<GameController>();
            }
        }

        #region Events
        public static System.Action OnSceneViewsCreated = delegate {};
        #endregion

        public enum GameType_t {
            SinglePlayer,
            MultiPlayer
        }

        public class Config_t {
            public GameType_t gameType;
            public CarDataModel carDataModel;
            public int playerIndex;
        }

        public Config_t Config {
            get; private set;
        }

        public GameType_t GameType {
            get {
                return this.Config.gameType;
            }
        }

        public GameView View {
            get; private set;
        }

        public UIGameController UIView {
            get; private set;
        }

        public CarController OurCar {
            get; private set;
        }

        // Dummy, for IInstance
        public GameController() {
        }

        public GameController(Config_t config) {
            InstanceLocator.RegisterInstance<GameController>(this);

            this.Config = config;
            this.OurCar = new CarController(this.Config.carDataModel);

            this.CreateSceneView();

            CoroutineHelper.Instance.StartCoroutine(this.CheckAndCreateGameViews());
        }

        public void LeaveGame() {
            if (this.UIView != null) {
                this.UIView.RemoveDialog();
            }
            if (this.GameType == GameType_t.MultiPlayer) {
                AppMultiPlayerManager.Instance.LeaveRoom();
            }
            AppSceneManager.Instance.LoadLobbyScene();
        }

        private const string kGameViewPrefabPath = "Prefabs/Game/RootGameView";
        private void CreateSceneView() {
            GameObject gameViewGO = PrefabLoader.Instance.InstantiateSynchronous(kGameViewPrefabPath, null);
            gameViewGO.AssertNotNull("Game View game object");

            this.View = gameViewGO.GetComponent<GameView>();
            this.View.AssertNotNull("Game View component");

            OnSceneViewsCreated.Invoke();
        }

        public bool ReadyToStartGame { get; set; }

        private IEnumerator CheckAndCreateGameViews() {
            if (this.GameType == GameType_t.MultiPlayer) {
                while (!this.ReadyToStartGame) {
                    yield return null;
                }
            }

            this.CreateCarView();
            this.CreateUIView();
            // Add more view creations here
        }

        private void CreateCarView() {
            this.OurCar.CreateViewAndPhysicsController(this.View.GetPlayerCarAnchor(this.Config.playerIndex));
        }

        private void CreateUIView() {
            this.UIView = new UIGameController(new UIGameController.Config {
                onGasPedalUpCallback = this.HandleGasPedalUpCallback,
                onGasPedalDownCallback = this.HandleGasPedalDownCallback,
                onBrakePedalUpCallback = this.HandleBrakePedalUpCallback,
                onBrakePedalDownCallback = this.HandleBrakePedalDownCallback
            });
            this.UIView.PresentDialog();
        }

        private void HandleGasPedalDownCallback() {
            if (this.OurCar != null) {
                this.OurCar.HandleGasPedalDown();
            }
        }

        private void HandleGasPedalUpCallback() {
            if (this.OurCar != null) {
                this.OurCar.HandleGasPedalUp();
            }
        }

        private void HandleBrakePedalDownCallback() {
            if (this.OurCar != null) {
                this.OurCar.HandleBrakePedalDown();
            }
        }

        private void HandleBrakePedalUpCallback() {
            if (this.OurCar != null) {
                this.OurCar.HandleBrakePedalUp();
            }
        }
    }
}