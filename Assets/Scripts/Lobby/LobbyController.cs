namespace Lobby {

    public class LobbyController {

        public UILobbyController View {
            get; private set;
        }

        public LobbyController() {
            this.CreateView();
        }

        private void CreateView() {
            this.View = new UILobbyController();
            this.View.PresentDialog();
        }

    }
}