using TimiShared.UI;
using UI;

namespace Game.UI {
    public class UIGameController : DialogControllerBase<UIGameView> {

        private const string kPrefabPath = "Prefabs/UI/UIGameView";
        protected override string GetDialogViewPrefabPath() {
            return kPrefabPath;
        }

        protected override void ConfigureView() {
            this.View.Configure(new UIGameView.Config {
                onLeaveRaceButtonCallback = this.HandleLeaveRaceButtonCallback
            });
        }

        private void HandleLeaveRaceButtonCallback() {
            UIOkCancelController okCancelController = new UIOkCancelController("Do you really want to leave this race?",
                "Yes", "No",
                onPositiveCallback: () => {
                    GameController.Instance.LeaveGame();
                },
                onNegativeCallback: null);
            okCancelController.PresentDialog();
        }
    }
}