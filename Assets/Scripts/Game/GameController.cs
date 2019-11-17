using Lobby;
using TimiShared.Extensions;
using TimiShared.Instance;
using TimiShared.Loading;

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

        // Dummy, for IInstance
        public GameController() {
        }

        public GameController(Config_t config) {
            InstanceLocator.RegisterInstance<GameController>(this);

            this.Config = config;

            this.CreateSceneView();
        }

        private const string kGameViewPrefabPath = "Prefabs/Game/RootGameView";

        private void CreateSceneView() {
            PrefabLoader.Instance.InstantiateAsynchronous(kGameViewPrefabPath, null, g => {
                g.AssertNotNull("Game View game object");
                this.View = g.GetComponent<GameView>();
                this.View.AssertNotNull("Game View component");

                OnSceneViewsCreated.Invoke();
            });
        }

    }
}