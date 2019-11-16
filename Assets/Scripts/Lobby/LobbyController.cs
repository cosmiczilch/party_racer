using TimiShared.Extensions;
using TimiShared.Loading;
using UnityEngine;

namespace Lobby {

    public class LobbyController {

        public LobbyView View {
            get; private set;
        }

        public UILobbyController UIView {
            get; private set;
        }

        private const string kLobbyViewPath = "Prefabs/Lobby/LobbyView";

        public LobbyController() {
            this.CreateView();
        }

        private void CreateView() {
            GameObject lobbyViewGO = PrefabLoader.Instance.InstantiateSynchronous(kLobbyViewPath, null);
            lobbyViewGO.AssertNotNull("Lobby View game object");
            this.View = lobbyViewGO.GetComponent<LobbyView>();
            this.View.AssertNotNull("Lobby View component");

            this.UIView = new UILobbyController();
            this.UIView.PresentDialog();
        }

    }
}