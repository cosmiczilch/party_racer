using Photon.Pun;
using UnityEngine;

namespace Game {
    public abstract class NetworkedObjectBase : MonoBehaviour {

        protected abstract PhotonView PhotonView { get; }

        protected virtual void Awake() {
            // In multiplayer games, don't simulate physics on objects that are not controlled by us
            if (GameController.Instance != null &&
                GameController.Instance.GameType == GameController.GameType_t.MultiPlayer &&
                !this.PhotonView.IsMine) {
                Rigidbody[] rigidbodies = this.gameObject.GetComponentsInChildren<Rigidbody>();
                if (rigidbodies != null) {
                    for (int i = 0; i < rigidbodies.Length; ++i) {
                        GameObject.Destroy(rigidbodies[i]);
                    }
                }
            }
        }

    }
}