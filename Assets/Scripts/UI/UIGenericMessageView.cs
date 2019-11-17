using TimiShared.UI;
using UnityEngine;
using UnityEngine.UI;

namespace UI {

    public class UIGenericMessageView : DialogViewBase {

        [SerializeField] private Text _messageText = null;

        private System.Action _onOkButtonCLickedCallback;

        public void Configure(string message, System.Action onOkButtonClickedCallback) {
            this._messageText.text = message;
            this._onOkButtonCLickedCallback = onOkButtonClickedCallback;
        }

        public void OnOkButtonClicked() {
            if (this._onOkButtonCLickedCallback != null) {
                this._onOkButtonCLickedCallback.Invoke();
            }
        }

    }
}