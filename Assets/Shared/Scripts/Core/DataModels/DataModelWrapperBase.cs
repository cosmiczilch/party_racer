
public abstract class DataModelWrapperBase<T> 
where T: DataModelBase {

    private T _model;
    public T Model {
        get { return _model; }
    }

    public DataModelWrapperBase(T model) {
        this._model = model;
        this.ProcessModel();
    }

    protected abstract void ProcessModel();
}
