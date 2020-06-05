using System.Collections.Generic;
using System.Linq;
using Data;
using TimiShared.Debug;
using TimiShared.Instance;

public class AppData : IInstance {

    public static AppData Instance {
        get {
            return InstanceLocator.Instance<AppData>();
        }
    }

    #region Data
    private List<CarDataModel> _carDataModels = new List<CarDataModel> {
        new CarDataModel {
            carId = 1,
            carName = "Camporano 666",
            garagePrefabPath = "Prefabs/Cars/Chev666_garage",
            racePrefabPath = "Prefabs/Cars/Chev666",
            mass = 2.5f,
            drag = 2.5f,
            engineForceMax = 2.8f,
            engineForceMin = 0.8f,
            engineForceRampUpTime = 250.0f,
            engineForceRampDownTime = 1000.0f,
            brakingForce = 0.05f,
            expectedSpeedRealUnits_display = 74.0f
        },
        new CarDataModel {
            carId = 2,
            carName = "Toresta 11",
            garagePrefabPath = "Prefabs/Cars/Tor11_garage",
            racePrefabPath = "Prefabs/Cars/Tor11",
            mass = 1.8f,
            drag = 1.4f,
            engineForceMax = 1.35f,
            engineForceMin = 0.6f,
            engineForceRampUpTime = 600.0f,
            engineForceRampDownTime = 3000.0f,
            brakingForce = 0.02f,
            expectedSpeedRealUnits_display = 79.9f
        },
    };
    private Dictionary<int, CarDataModel> _carDataModelsDict = new Dictionary<int, CarDataModel>();

    private CarStatDisplayDataModel _carStatDisplayData = new CarStatDisplayDataModel {
        carSpeedConversionFactor_game_to_real = 4.5f,
        carSpeedMax_game_units = 22.222f,
        carMassMax_game_units = 4.0f,
        carEngineForceMax_game_units = 4.0f,
        carHandlingMax_game_units = 1.8f,
    };
    #endregion

    #region Public API
    public CarStatDisplayDataModel CarStatDisplayDataModel {
        get { return this._carStatDisplayData; }
    }

    public CarDataModel GetCarDataModelByCarId(int carId) {
        CarDataModel res = null;
        if (!this._carDataModelsDict.TryGetValue(carId, out res)) {
            DebugLog.LogWarningColor("No such car with id: " + carId, LogColor.yellow);
        }
        return res;
    }

    public List<int> GetCarDataModelIds() {
        return this._carDataModelsDict.Keys.ToList();
    }
    #endregion

    public AppData() {
        this.ProcessData();
    }

    private void ProcessData() {
         if (this._carDataModels != null) {
            this._carDataModelsDict.Clear();
            var enumerator = this._carDataModels.GetEnumerator();
            while (enumerator.MoveNext()) {
                if (this._carDataModelsDict.ContainsKey(enumerator.Current.carId)) {
                    DebugLog.LogWarningColor("Duplicate car with id: " + enumerator.Current.carId, LogColor.orange);
                    continue;
                }
                this._carDataModelsDict.Add(enumerator.Current.carId, enumerator.Current);
            }
            enumerator.Dispose();
        }
    }



}
