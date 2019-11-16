using System.Collections.Generic;
using UnityEngine;

namespace TimiShared.UI {

    public class DialogViewPool : MonoBehaviour {

        private class PoolItem {
            public string prefabPath;
            public DialogViewBase view;

            public PoolItem(string prefabPath, DialogViewBase view) {
                this.prefabPath = prefabPath;
                this.view = view;
            }
        }

        private List<PoolItem> _pool = new List<PoolItem>();

        private int GetMaxPrefabsInPool {
            get {
                // TODO: Make this an optional AppConstant
                return 20;
            }
        }

        public DialogViewBase TryGetDialogViewFromPool(string prefabPath, Transform parent) {

            int index = 0;
            var enumerator = this._pool.GetEnumerator();
            while (enumerator.MoveNext()) {
                PoolItem poolItem = enumerator.Current;
                if (poolItem.prefabPath == prefabPath) {
                    this._pool.RemoveAt(index);

                    DialogViewBase view = poolItem.view;
                    view.gameObject.SetActive(true);
                    view.transform.SetParent(parent);

                    return view;
                }

                ++index;
            }

            return null;
        }

        public void AddDialogViewToPool(string prefabPath, DialogViewBase view) {
            if (this.GetMaxPrefabsInPool == 0) {
                GameObject.Destroy(view.gameObject);
                return;
            }

            if (this._pool.Count >= this.GetMaxPrefabsInPool) {
                // Remove the item at the beginning of the pool
                PoolItem poolItem = this._pool[0];
                this._pool.RemoveAt(0);
                GameObject.Destroy(poolItem.view.gameObject);
            }

            view.gameObject.SetActive(false);
            view.transform.SetParent(this.transform);

            this._pool.Add(new PoolItem(prefabPath, view));
        }
    }
}