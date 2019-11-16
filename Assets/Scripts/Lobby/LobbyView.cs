using UnityEngine;

namespace Lobby {

    public class LobbyView : MonoBehaviour {

        [SerializeField] private Transform _carAnchor = null;
        public Transform CarAnchor {
            get { return this._carAnchor; }
        }

    }
}