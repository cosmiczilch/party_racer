using TimiShared.Instance;

namespace TimiMultiPlayer {

    public class AppMultiPlayerManager : MultiPlayerManager, IInstance {

        public static AppMultiPlayerManager Instance {
            get {
                return InstanceLocator.Instance<AppMultiPlayerManager>();
            }
        }

        protected override void OnStartInitialize() {
            InstanceLocator.RegisterInstance<AppMultiPlayerManager>(this);
        }

        protected override string MultiplayerVersion {
            get { return "1.0"; }
        }

        protected override byte MinPlayersPerRoom {
            get { return 2; }
        }

        protected override byte MaxPlayersPerRoom {
            get { return 5; }
        }

        protected override float WaitForMorePlayersTimeoutDurationSeconds {
            get { return 5.0f; }
        }
    }
}