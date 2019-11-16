using TimiShared.UI;

namespace Lobby {

    public class UILobbyController : DialogControllerBase<UILobbyView> {

        private const string kPrefabPath = "Prefabs/UI/UILobbyView";
        protected override string GetDialogViewPrefabPath() {
            return kPrefabPath;
        }

        protected override void ConfigureView() {
            this.View.Configure(new UILobbyView.Config {
                onRaceButtonCallback = this.HandleRaceButtonCallback
            });
        }

        private void HandleRaceButtonCallback() {
        }
    }
}