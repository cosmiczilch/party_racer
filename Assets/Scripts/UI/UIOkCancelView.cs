using TimiShared.UI;
using UnityEngine;
using UnityEngine.UI;

namespace UI {

    public class UIOkCancelView : DialogViewBase {

        [SerializeField] private Text _messageText = null;
        [SerializeField] private Text _positiveButtonText = null;
        [SerializeField] private Text _negativeButtonText = null;

        private System.Action _onPositiveButtonClickedCallback;
        private System.Action _onNegativeButtonClickedCallback;

        public void Configure(string message, string positiveButtonText, string negativeButtonText,
                              System.Action onPositiveButtonClickedCallback,
                              System.Action onNegativeButtonClickedCallback) {
            this._messageText.text = message;
            this._positiveButtonText.text = positiveButtonText;
            this._negativeButtonText.text = negativeButtonText;
            this._onPositiveButtonClickedCallback = onPositiveButtonClickedCallback;
            this._onNegativeButtonClickedCallback = onNegativeButtonClickedCallback;
        }

        public void OnPositiveOkButtonClicked() {
            if (this._onPositiveButtonClickedCallback != null) {
                this._onPositiveButtonClickedCallback.Invoke();
            }
        }

        public void OnNegativeOkButtonClicked() {
            if (this._onNegativeButtonClickedCallback != null) {
                this._onNegativeButtonClickedCallback.Invoke();
            }
        }
    }
}