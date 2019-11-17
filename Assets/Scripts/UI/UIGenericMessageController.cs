using TimiShared.UI;

namespace UI {

    public class UIGenericMessageController : DialogControllerBase<UIGenericMessageView> {

        private const string kPrefabPath = "Prefabs/UI/UIGenericMessageView";
        protected override string GetDialogViewPrefabPath() {
            return kPrefabPath;
        }

        private string _message = "";

        public UIGenericMessageController(string message) {
            this._message = message;
        }

        protected override void ConfigureView() {
            this.View.Configure(this._message, this.HandleOkButtonClickedCallback);
        }

        private void HandleOkButtonClickedCallback() {
            this.RemoveDialog();
        }
    }
}