namespace Lobby {
    public interface ILobbyMenuDelegate {
        void HandleRaceButtonClicked();
        void HandleNextCarButtonClicked();
        void HandlePrevCarButtonClicked();
        void HandleSinglePlayerRaceSelected();
        void HandleMultiPlayerRaceSelected();
    }
}