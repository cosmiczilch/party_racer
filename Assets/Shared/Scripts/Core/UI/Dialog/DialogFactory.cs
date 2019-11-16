using TimiShared.Debug;
using TimiShared.Loading;
using UnityEngine;

namespace TimiShared.UI {
    public class DialogFactory : MonoBehaviour {

        public void CreateDialog(string prefabPath, System.Action<DialogViewBase> callback, Transform parent = null) {

            if (parent == null) {
                parent = UIRoot.Instance.MainCanvas.transform;
            }

            DialogViewBase dialogViewBase = UIRoot.Instance.DialogViewPool.TryGetDialogViewFromPool(prefabPath, parent);
            if (dialogViewBase != null) {
                callback(dialogViewBase);
                return;
            }

            PrefabLoader.Instance.InstantiateAsynchronous(prefabPath,
                parent,
                (loadedGO) => {
                    dialogViewBase = loadedGO.GetComponent<DialogViewBase>();
                    if (dialogViewBase != null) {
                        dialogViewBase.Init(prefabPath);
                        callback(dialogViewBase);
                    } else {
                        DebugLog.LogWarningColor("Prefab at " + prefabPath + " is not a dialog", LogColor.orange);
                    }
                });

        }
    }
}