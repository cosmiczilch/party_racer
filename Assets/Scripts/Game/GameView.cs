using System.Collections.Generic;
using TimiShared.Debug;
using UnityEngine;

namespace Game {

    public class GameView : MonoBehaviour {

        [SerializeField] private List<Transform> _playerCarAnchors = null;

        /// <summary>
        /// Get car anchor for player index
        /// </summary>
        /// <param name="playerIndex">Player index is 1 based</param>
        /// <returns></returns>
        public Transform GetPlayerCarAnchor(int playerIndex) {
            if (this._playerCarAnchors == null ||
                this._playerCarAnchors.Count < playerIndex ||
                playerIndex < 1) {
                DebugLog.LogWarningColor("No player car anchor set for player index: " + playerIndex, LogColor.orange);
                return null;
            }
            return this._playerCarAnchors[playerIndex - 1];
        }
    }
}