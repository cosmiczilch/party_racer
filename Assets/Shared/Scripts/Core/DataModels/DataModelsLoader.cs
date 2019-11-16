using System.Collections.Generic;
using TimiShared.Debug;
using TimiShared.Init;
using UnityEngine;

public class DataModelsLoader : MonoBehaviour, IInitializable {

    private Dictionary<string /* type name */, DataModelBase> _dataModels;

    #region IInitializable
    public void StartInitialize() {
        this.LoadDataModelsFromDisk();
        this.IsFullyInitialized = true;
    }

    public string GetName {
        get {
            return this.GetType().Name;
        }
    }

    public bool IsFullyInitialized {
        get; private set;
    }
    #endregion

    private void Awake() {
        this.FindDataModels();
    }


    private void FindDataModels() {
        this._dataModels = new Dictionary<string, DataModelBase>();
        DataModelsContainer[] dataModelsContainers = this.gameObject.GetComponentsInChildren<DataModelsContainer>();
        for (int i = 0; i < dataModelsContainers.Length; ++i) {
            List<DataModelBase> dataModels = dataModelsContainers[i].GetDataModels();
            var enumerator = dataModels.GetEnumerator();
            while (enumerator.MoveNext()) {
                this.RegisterDataModel(enumerator.Current);
            }
        }
    }

    private void RegisterDataModel(DataModelBase dataModel) {
        string dataModelTypeName = dataModel.GetType().Name;
        if (this._dataModels.ContainsKey(dataModelTypeName)) {
            DebugLog.LogErrorColor("Already registered datamodel for type " + dataModelTypeName, LogColor.orange);
            return;
        }
        this._dataModels.Add(dataModel.GetType().Name, dataModel);
    }

    private void LoadDataModelsFromDisk() {
        var enumerator = this._dataModels.GetEnumerator();
        while (enumerator.MoveNext()) {
            DataModelBase dataModel = enumerator.Current.Value;
            dataModel.LoadDataModelFromDisk();
        }
    }

}
