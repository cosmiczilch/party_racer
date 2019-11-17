using TimiShared.UI;

namespace UI {
    public class UIOkCancelController : DialogControllerBase<UIOkCancelView> {

        private const string kPrefabPath = "Prefabs/UI/UIOkCancelView";
        protected override string GetDialogViewPrefabPath() {
            return kPrefabPath;
        }

        private string _message = "";
        private string _positiveButtonText = "";
        private string _negativeButtonText = "";

        private System.Action _onPositiveCallback;
        private System.Action _onNegativeCallback;

        public UIOkCancelController(string message, string positiveButtonText, string negativeButtonText,
            System.Action onPositiveCallback, System.Action onNegativeCallback) {
            this._message = message;
            this._positiveButtonText = positiveButtonText;
            this._negativeButtonText = negativeButtonText;

            this._onPositiveCallback = onPositiveCallback;
            this._onNegativeCallback = onNegativeCallback;
        }
        protected override void ConfigureView() {
            this.View.Configure(this._message, this._positiveButtonText, this._negativeButtonText, this.HandlePositiveButtonClickedCallback, this.HandleNegativeButtonClickedCallback);
        }

        private void HandlePositiveButtonClickedCallback() {
            if (this._onPositiveCallback != null) {
                this._onPositiveCallback.Invoke();
            }
            this.RemoveDialog();
        }

        private void HandleNegativeButtonClickedCallback() {
            if (this._onNegativeCallback != null) {
                this._onNegativeCallback.Invoke();
            }
            this.RemoveDialog();
        }
    }
}