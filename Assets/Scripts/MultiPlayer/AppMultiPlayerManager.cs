using TimiShared.Debug;
using TimiShared.Extensions;
using TimiShared.Instance;
using UnityEngine;

namespace TimiMultiPlayer {

    public class AppMultiPlayerManager : MultiPlayerManager, IInstance {

        public static AppMultiPlayerManager Instance {
            get {
                return InstanceLocator.Instance<AppMultiPlayerManager>();
            }
        }

        protected override void OnStartInitialize() {
            InstanceLocator.RegisterInstance<AppMultiPlayerManager>(this);
        }

        #region Parameters
        protected override string MultiplayerVersion {
            get { return "1.0"; }
        }

        protected override byte MinPlayersPerRoom {
            get { return 2; }
        }

        protected override byte MaxPlayersPerRoom {
            get { return 5; }
        }

        protected override float WaitForMorePlayersTimeoutDurationSeconds {
            get { return 10.0f; }
        }
        #endregion

        #region Public API

        public void CreateAndStartGame(System.Action joinRoomSuccessCallback,
                                       System.Action readyToStartGameCallback,
                                       System.Action timedOutWaitingForPlayersCallback,
                                       System.Action failureCallback) {
            if (this._pendingStartGameRequest != null) {
                DebugLog.LogWarningColor("Another start game request is already in progress. Ignoring.", LogColor.brown);
                return;
            }
            this._pendingStartGameRequest = new PendingStartGameRequest {
                isWaitingToStartGame = false,
                onReadyToStartGameCallback = readyToStartGameCallback,
                onTimedOutWaitingForPlayersCallback = timedOutWaitingForPlayersCallback,
                waitStartTime = float.MaxValue
            };
            base.CreateOrJoinRandomRoom(
                successCallback: () => {
                    if (this._pendingStartGameRequest == null) {
                        DebugLog.LogWarningColor("Joined room without a start game request. Ignoring", LogColor.brown);
                    } else {
                        this._pendingStartGameRequest.isWaitingToStartGame = true;
                        this._pendingStartGameRequest.waitStartTime = Time.time;
                        if (joinRoomSuccessCallback != null) {
                            joinRoomSuccessCallback.Invoke();
                        }
                    }
                },
                failureCallback: () => {
                    _pendingStartGameRequest = null;
                    if (failureCallback != null) {
                        failureCallback.Invoke();
                    }
                });
        }
        #endregion

        private class PendingStartGameRequest {
            public bool isWaitingToStartGame;
            public float waitStartTime;
            public System.Action onReadyToStartGameCallback;
            public System.Action onTimedOutWaitingForPlayersCallback;
        }
        private PendingStartGameRequest _pendingStartGameRequest;


        private void Update() {
            if (this._pendingStartGameRequest != null &&
                this._pendingStartGameRequest.isWaitingToStartGame) {

                if (this.IsReadyToStartGame()) {
                    this._pendingStartGameRequest.onReadyToStartGameCallback.Invoke();
                    this._pendingStartGameRequest = null;

                } else {
                    if ((Time.time - this._pendingStartGameRequest.waitStartTime) >=
                        WaitForMorePlayersTimeoutDurationSeconds) {
                        this._pendingStartGameRequest.onTimedOutWaitingForPlayersCallback.Invoke();
                        this._pendingStartGameRequest = null;
                    }
                }
            }
        }

        private bool IsReadyToStartGame() {
            if (this._pendingStartGameRequest == null || !this._pendingStartGameRequest.isWaitingToStartGame) {
                return false;
            }

            if (this.NumPlayersInRoom < MinPlayersPerRoom) {
                return false;
            }

            if (this.NumPlayersInRoom >= MaxPlayersPerRoom) {
                return true;
            }

            if ((Time.time - this._pendingStartGameRequest.waitStartTime) >= WaitForMorePlayersTimeoutDurationSeconds) {
                return true;
            }

            return false;
        }

    }
}