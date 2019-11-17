using System.Collections.Generic;
using System.Threading;
using ExitGames.Client.Photon;
using Game;
using Photon.Pun;
using Photon.Realtime;
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
            GameController.OnSceneViewsCreated += this.HandleSceneViewsCreated;
        }

        #region Parameters
        protected override string MultiplayerVersion {
            get { return "1.0"; }
        }

        protected override byte MinPlayersPerRoom {
            get { return 2; }
        }

        // TODO: Change this to 5 or something after testing
        protected override byte MaxPlayersPerRoom {
            get { return 2; }
        }

        protected override float WaitForMorePlayersTimeoutDurationSeconds {
            get { return 10.0f; }
        }
        #endregion

        #region Public API

        public void CreateAndStartGame(System.Action joinRoomSuccessCallback,
                                       System.Action readyToStartGameCallback,
                                       System.Action createdSceneViewsCallback,
                                       System.Action timedOutWaitingForPlayersCallback,
                                       System.Action failureCallback) {
            if (this._pendingStartGameRequest != null) {
                DebugLog.LogWarningColor("Another start game request is already in progress. Ignoring.", LogColor.brown);
                return;
            }
            this._pendingStartGameRequest = new PendingStartGameRequest {
                isWaitingToStartGame = false,
                isWaitingForAllSceneViewsCreated = false,

                onReadyToStartGameCallback = readyToStartGameCallback,
                onSceneViewsCreatedCallback = createdSceneViewsCallback,
                onTimedOutWaitingForPlayersCallback = timedOutWaitingForPlayersCallback,

                waitToStartGameStartTime = float.MaxValue,
                waitForSceneViewsStartTime = float.MaxValue
            };
            base.CreateOrJoinRandomRoom(
                successCallback: () => {
                    if (this._pendingStartGameRequest == null) {
                        DebugLog.LogWarningColor("Joined room without a start game request. Ignoring", LogColor.brown);
                    } else {
                        this._pendingStartGameRequest.isWaitingToStartGame = true;
                        this._pendingStartGameRequest.waitToStartGameStartTime = Time.time;
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
            public bool isWaitingForAllSceneViewsCreated;

            public float waitToStartGameStartTime;
            public float waitForSceneViewsStartTime;

            public System.Action onReadyToStartGameCallback;
            public System.Action onSceneViewsCreatedCallback;
            public System.Action onTimedOutWaitingForPlayersCallback;
        }
        private PendingStartGameRequest _pendingStartGameRequest;


        private void Update() {
            if (this._pendingStartGameRequest != null &&
                this._pendingStartGameRequest.isWaitingToStartGame) {

                if (this.IsReadyToStartGame()) {
                    this.CloseCurrentRoomForNewPlayers();
                    this._pendingStartGameRequest.isWaitingToStartGame = false;
                    this._pendingStartGameRequest.isWaitingForAllSceneViewsCreated = true;
                    this._pendingStartGameRequest.waitForSceneViewsStartTime = Time.time;

                    this._pendingStartGameRequest.onReadyToStartGameCallback.Invoke();

                } else {
                    if ((Time.time - this._pendingStartGameRequest.waitToStartGameStartTime) >=
                        WaitForMorePlayersTimeoutDurationSeconds) {
                        this.LeaveRoom();
                        this._pendingStartGameRequest.onTimedOutWaitingForPlayersCallback.Invoke();
                        this._pendingStartGameRequest = null;
                    }
                }
            }

            if (this._pendingStartGameRequest != null &&
                this._pendingStartGameRequest.isWaitingForAllSceneViewsCreated) {

                if (this.AreAllSceneViewsCreated()) {
                    this._pendingStartGameRequest.onSceneViewsCreatedCallback.Invoke();
                    this._pendingStartGameRequest = null;
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

            if ((Time.time - this._pendingStartGameRequest.waitToStartGameStartTime) >= WaitForMorePlayersTimeoutDurationSeconds) {
                return true;
            }

            return false;
        }

        private const string kSceneViewCreatedKeyPrefix = "svc:";
        private const float kWaitForSceneViewsCreatedTimeoutSeconds = 5.0f;

        private bool AreAllSceneViewsCreated() {
            if (!PhotonNetwork.IsConnected || PhotonNetwork.CurrentRoom == null) {
                return false;
            }

            if ((Time.time - this._pendingStartGameRequest.waitForSceneViewsStartTime) >=
                kWaitForSceneViewsCreatedTimeoutSeconds) {
                // Tired of waiting. Possibly one or more racers disconnected.
                // Just start the race.
                DebugLog.LogWarningColor("Timed out waiting for all players to create scene views", LogColor.orange);
                return true;
            }

            Dictionary<int, Player> players = PhotonNetwork.CurrentRoom.Players;
            foreach (KeyValuePair<int,Player> kvp in players) {
                string key = kSceneViewCreatedKeyPrefix + kvp.Value.ActorNumber;
                object isSceneViewCreatedForPlayer = false;
                if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue(key, out isSceneViewCreatedForPlayer)) {
                    if ((bool) isSceneViewCreatedForPlayer == false) {
                        return false;
                    }
                } else {
                    return false;
                }
            }

            return true;
        }

        private void HandleSceneViewsCreated() {
            if (!PhotonNetwork.IsConnected || PhotonNetwork.CurrentRoom == null) {
                return;
            }

            if (this._pendingStartGameRequest != null &&
               (this._pendingStartGameRequest.isWaitingToStartGame || this._pendingStartGameRequest.isWaitingForAllSceneViewsCreated)) {

                Hashtable hashtable = new Hashtable { { kSceneViewCreatedKeyPrefix + PhotonNetwork.LocalPlayer.ActorNumber, true } };
                PhotonNetwork.CurrentRoom.SetCustomProperties(hashtable);


            } else {
                // Not waiting to start game. Ignore
            }
        }


    }
}