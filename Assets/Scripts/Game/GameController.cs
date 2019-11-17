using TimiShared.Instance;

namespace Game {

    public class GameController : IInstance {

        public static GameController Instance {
            get {
                return InstanceLocator.Instance<GameController>();
            }
        }

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

        // Dummy, for IInstance
        public GameController() {
        }

        public GameController(Config_t config) {
            this.Config = config;

            InstanceLocator.RegisterInstance<GameController>(this);
        }

    }
}