using Data;
using UnityEngine;
using UnityEngine.UI;

namespace Lobby {

    public class UICarStatsDisplay : MonoBehaviour {

        private CarDataModel _carDataModel;

        [SerializeField] private Text _carNameText = null;
        [SerializeField] private Slider _speedSlider = null;
        [SerializeField] private Slider _weightSlider = null;
        [SerializeField] private Slider _torqueSlider = null;
        [SerializeField] private Slider _handlingSlider = null;

        public void Configure(CarDataModel carDataModel) {
            this._carDataModel = carDataModel;
            this.UpdateStatsDisplay();
        }

        private void UpdateStatsDisplay() {
            this._carNameText.text = this._carDataModel.carName;
            this._speedSlider.value = this._carDataModel.expectedSpeedRealUnits_display /
                                      (AppData.Instance.CarStatDisplayDataModel.carSpeedMax_game_units *
                                       AppData.Instance.CarStatDisplayDataModel.carSpeedConversionFactor_game_to_real);
            this._weightSlider.value = this._carDataModel.mass / AppData.Instance.CarStatDisplayDataModel.carMassMax_game_units;
            this._torqueSlider.value = this._carDataModel.engineForceMax / AppData.Instance.CarStatDisplayDataModel.carEngineForceMax_game_units;
            this._handlingSlider.value = (this._carDataModel.drag / this._carDataModel.mass) / AppData.Instance.CarStatDisplayDataModel.carHandlingMax_game_units;
        }

    }
}