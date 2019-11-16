using Photon.Pun;
using Photon.Realtime;
using TimiShared.Debug;
using TimiShared.Extensions;
using TimiShared.Init;
using TimiShared.Instance;
using UnityEngine;

namespace TimiMultiPlayer {

    public class MultiPlayerManager : MonoBehaviourPunCallbacks, IInstance, IInitializable {

        public static MultiPlayerManager Instance {
            get {
                return InstanceLocator.Instance<MultiPlayerManager>();
            }
        }

        // TODO: Get this from some config in the app
        #region Constants
        private const string kGameMultiPlayerVersion = "1.0";
        private const byte kMaxPlayersPerRoom = 2;
        #endregion

        #region IInitializable
        public void StartInitialize() {
            InstanceLocator.RegisterInstance<MultiPlayerManager>(this);

            // Do NOT automatically sync scene as that causes weird effects
            // when playing across devices with drastically different processing speeds
            // PhotonNetwork.AutomaticallySyncScene = true;

            PhotonNetwork.GameVersion = kGameMultiPlayerVersion;
            PhotonNetwork.NickName = SystemInfo.operatingSystem;

            this.CheckAndConnectToPhotonNetwork();
        }

        public bool IsFullyInitialized {
            get; private set;
        }

        public string GetName {
            get {
                return this.GetType().Name;
            }
        }
        #endregion

        private class PendingRoomJoinRequest {
            public System.Action successCallback;
            public System.Action failureCallback;
        }
        private PendingRoomJoinRequest _pendingRoomJoinRequest;

        #region Events
        public static System.Action OnOtherPlayerEnteredRoom = delegate {};
        public static System.Action OnOtherPlayerLeftRoom = delegate {};
        #endregion

        #region Public API
        public void CreateOrJoinRandomRoom(System.Action successCallback, System.Action failureCallback) {
            if (this._pendingRoomJoinRequest != null) {
                DebugLog.LogWarningColor("Another room join request is already in progress. Ignoring.", LogColor.brown);
                return;
            }

            this._pendingRoomJoinRequest = new PendingRoomJoinRequest {
                successCallback = successCallback,
                failureCallback = failureCallback
            };

            if (PhotonNetwork.IsConnected) {
                PhotonNetwork.JoinRandomRoom();
            } else {
                this.CheckAndConnectToPhotonNetwork();
            }
        }

        public void StartGameInRoom() {
            if (PhotonNetwork.IsConnected && PhotonNetwork.CurrentRoom != null) {
                DebugLog.LogColor("Marking room as closed", LogColor.blue);
                PhotonNetwork.CurrentRoom.IsOpen = false;
            }
        }

        public void LeaveRoom() {
            this._pendingRoomJoinRequest = null;
            if (PhotonNetwork.IsConnected && PhotonNetwork.CurrentRoom != null) {
                PhotonNetwork.LeaveRoom();
            }
        }

        public int NumPlayersInRoom {
            get {
                if (PhotonNetwork.IsConnected && PhotonNetwork.CurrentRoom != null) {
                    return (int) PhotonNetwork.CurrentRoom.PlayerCount;
                }
                return 0;
            }
        }

        public bool AreWePlayer1() {
            if (PhotonNetwork.IsConnected && PhotonNetwork.LocalPlayer != null &&
                PhotonNetwork.IsMasterClient) {
                return true;
            }
            return false;
        }

        public GameObject InstantiatePrefab(string prefabPath, Transform parent = null) {
            if (!PhotonNetwork.IsConnected) {
                DebugLog.LogErrorColor("Network instantiate attempted without connection", LogColor.yellow);
            }

            GameObject go = PhotonNetwork.Instantiate(prefabPath,
                                                      parent == null ? Vector3.zero : parent.transform.position,
                                                      parent == null ? Quaternion.identity : parent.transform.rotation);
            go.AssertNotNull("Instantiate game object");
            if (parent != null) {
                go.transform.SetParent(parent, worldPositionStays: true);
            }

            return go;
        }
        #endregion

        #region Photon callbacks
        public override void OnConnectedToMaster() {
            this.IsFullyInitialized = true;

            // If we have a pending room join request, process it now
            if (this._pendingRoomJoinRequest != null) {
                PhotonNetwork.JoinRandomRoom();
            }
        }

        public override void OnDisconnected(DisconnectCause cause) {
            if (this._pendingRoomJoinRequest != null) {
                if (this._pendingRoomJoinRequest.failureCallback != null) {
                    DebugLog.LogWarningColor("PhotonNetwork disconnected with pending room join request. Cause: " + cause.ToString(), LogColor.orange);
                    this._pendingRoomJoinRequest.failureCallback.Invoke();
                }
                this._pendingRoomJoinRequest = null;
            } else {
                DebugLog.LogColor("PhotonNetwork disconnected. Cause: " + cause.ToString(), LogColor.blue);
            }
        }

        public override void OnJoinRandomFailed(short returnCode, string message) {
            if (this._pendingRoomJoinRequest != null) {
                DebugLog.LogColor("Could not find random room to join. Join failure reason: " + returnCode.ToString(), LogColor.blue);
                DebugLog.LogColor("Attempting to create a new room", LogColor.blue);
                PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = kMaxPlayersPerRoom });
            }
        }

        public override void OnCreateRoomFailed(short returnCode, string message) {
            if (this._pendingRoomJoinRequest != null) {
                DebugLog.LogColor("Create room failed . Failure reason: " + returnCode.ToString(), LogColor.blue);
                if (this._pendingRoomJoinRequest.failureCallback != null) {
                    this._pendingRoomJoinRequest.failureCallback.Invoke();
                }
                this._pendingRoomJoinRequest = null;
            }
        }

        public override void OnJoinedRoom() {
            if (this._pendingRoomJoinRequest != null) {
                DebugLog.LogColor("Successfully joined room", LogColor.green);
                if (this._pendingRoomJoinRequest.successCallback != null) {
                    this._pendingRoomJoinRequest.successCallback.Invoke();
                }
                this._pendingRoomJoinRequest = null;
            }
        }

        // Called when other player entered room
        public override void OnPlayerEnteredRoom(Player newPlayer) {
            DebugLog.LogColor("Player entered room: " + newPlayer.ActorNumber.ToString(), LogColor.green);
            OnOtherPlayerEnteredRoom.Invoke();
        }

        // Called when other player disconnected from room
        public override void OnPlayerLeftRoom(Player otherPlayer) {
            DebugLog.LogColor("Player left room: " + otherPlayer.ActorNumber.ToString(), LogColor.green);
            OnOtherPlayerLeftRoom.Invoke();

            // We should leave the room as well to prevent new players searching for rooms from joining this room
            this.LeaveRoom();
        }
        #endregion

        private void CheckAndConnectToPhotonNetwork() {
            if (!PhotonNetwork.IsConnected) {
                PhotonNetwork.ConnectUsingSettings();
            }
        }
    }
}