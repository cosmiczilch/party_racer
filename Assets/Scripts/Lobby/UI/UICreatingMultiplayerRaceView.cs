using TimiShared.UI;
using UnityEngine;

namespace Lobby {

    public class UICreatingMultiplayerRaceView : DialogViewBase {

        public enum State {
            Connecting,
            WaitingForOpponents,
            StartingRace
        }

        [SerializeField] private Transform _connectingTransform = null;
        [SerializeField] private Transform _waitingForOpponentsTransform = null;
        [SerializeField] private Transform _startingRaceTransform = null;

        public void Configure(State state) {
            this.ConfigureState(state);
        }

        private void ConfigureState(State state) {
            this._connectingTransform.gameObject.SetActive(state == State.Connecting);
            this._waitingForOpponentsTransform.gameObject.SetActive(state == State.WaitingForOpponents);
            this._startingRaceTransform.gameObject.SetActive(state == State.StartingRace);
        }

    }
}