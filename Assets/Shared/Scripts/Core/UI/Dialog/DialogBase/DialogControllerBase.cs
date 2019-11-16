
using UnityEngine;

namespace TimiShared.UI {
    public abstract class DialogControllerBase<V> : IDialogController
    where V: DialogViewBase {

        private V _view;
        protected V View {
            get {
                return this._view;
            }
        }

        #region Public API
        public void PresentDialog(Transform parent = null, System.Action callback = null) {
            if (this._view != null) {
                return;
            }

            string prefabPath = this.GetDialogViewPrefabPath();
            UIRoot.Instance.DialogFactory.CreateDialog(prefabPath, (loadedDialogView) => {
                    this._view = loadedDialogView as V;
                    this.ConfigureView();
                    if (callback != null) {
                        callback.Invoke();
                    }
                }, parent);
        }

        public void RemoveDialog() {
            this.OnRemoveDialog();

            this._view.Hide();
            this._view = null;
        }

        public bool IsShowingDialog {
            get {
                return this._view != null;
            }

        }
        #endregion

        protected abstract string GetDialogViewPrefabPath();
        protected abstract void ConfigureView();
        protected virtual void OnRemoveDialog() {}
    }
}