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

    #region Car Data
    private List<CarDataModel> _carDataModels = new List<CarDataModel> {
        new CarDataModel {
            carId = 1,
            carName = "Chevrolet Camaro",
            prefabPath = "Prefabs/Cars/Chev666",
        },
        new CarDataModel {
            carId = 2,
            carName = "Toresta 11",
            prefabPath = "Prefabs/Cars/Tor11",
        },
    };
    private Dictionary<int, CarDataModel> _carDataModelsDict = new Dictionary<int, CarDataModel>();
    #endregion

    #region Public API
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
