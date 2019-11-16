using Newtonsoft.Json;
using TimiShared.Persister;

[System.Serializable]
public abstract class DataModelBase : IPersistable {

    [JsonIgnore]
    private Persister _persister;
    [JsonIgnore]
    public Persister Persister {
        get {
            return this._persister;
        }
    }

    [JsonIgnore]
    public System.Action OnDataModelUpdated = delegate {};

    #region IPersistable
    public virtual string GetBaseFolderName() {
        return "AppData/DataModels";
    }

    public virtual string GetFileName() {
        return this.GetType().Name + ".json";
    }
    #endregion

    public DataModelBase() {
        this._persister = new Persister(this);
    }

    public void LoadDataModelFromDisk() {
        if (this._persister.Restore()) {
            this.OnDataModelUpdated.Invoke();
            // TODO: Set IsInitialized?
        }
    }

    public void ApplyChanges() {
        this._persister.Save();
    }
}
