using UnityEngine;

namespace TimiShared.Init {

    public class AppInitBase : MonoBehaviour, IInitializable {

        public virtual void StartInitialize() {
            throw new System.NotImplementedException();
        }

        public virtual bool IsFullyInitialized {
            get {
                return true;
            }
        }

        public virtual string GetName {
            get {
                return this.GetType().Name;
            }
        }
    }
}