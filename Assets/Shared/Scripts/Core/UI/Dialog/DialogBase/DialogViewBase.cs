using UnityEngine;

namespace TimiShared.UI {
    public class DialogViewBase : MonoBehaviour {

        private string _prefabPath;

        protected virtual bool FreeToPoolOnHide {
            get {
                return true;
            }
        }

        public void Init(string prefabPath) {
            this._prefabPath = prefabPath;
        }

        public virtual void Hide() {
            if (this.FreeToPoolOnHide) {
                UIRoot.Instance.DialogViewPool.AddDialogViewToPool(this._prefabPath, this);
            }
            else {
                GameObject.Destroy(this.gameObject);
            }
        }

    }
}