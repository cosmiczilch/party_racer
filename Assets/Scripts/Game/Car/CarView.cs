using Photon.Pun;
using UnityEngine;

namespace Game.Car {

    public class CarView : NetworkedObjectBase {

        [SerializeField] private PhotonView _photonView = null;
        protected override PhotonView PhotonView {
            get { return this._photonView; }
        }

    }
}