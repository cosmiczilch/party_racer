using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;

public class TimiSharedSerializer {

    // TODO: Redo these functions.
    // TODO: Error checks on stream and object type (serializable?).
    // TODO: Should we really call get bytes or memory streams?

    #region Serialize
    public static void Serialize(Stream stream, object obj) {
        string json = TimiSharedSerializer.Serialize(obj);
        byte[] bytes = Encoding.UTF8.GetBytes(json);
        if (stream.CanWrite) {
            stream.Write(bytes, 0, bytes.Length);
        }
    }

    public static string Serialize(object obj) {
        if (UnityEngine.Debug.isDebugBuild) {
            return JsonConvert.SerializeObject(obj, Formatting.Indented);
        }
        return JsonConvert.SerializeObject(obj);
    }
    #endregion

    #region Deserialize
    public static void DeserializeIntoObject(string json, object obj) {
        JsonConvert.PopulateObject(json, obj);
    }

    public static void DeserializeIntoObject(Stream stream, object obj) {
        StreamReader sr = new StreamReader(stream, Encoding.UTF8);
        string json = sr.ReadToEnd();
        TimiSharedSerializer.DeserializeIntoObject(json, obj);
    }

    public static object DeserializeNonGeneric(Type type, Stream stream) {
        StreamReader sr = new StreamReader(stream, Encoding.UTF8);
        string json = sr.ReadToEnd();
        return TimiSharedSerializer.DeserializeNonGeneric(type, json);
    }

    public static object DeserializeNonGeneric(Type type, string json) {
        return JsonConvert.DeserializeObject(json, type);
    }

    public static T Deserialize<T>(Stream stream) {
        StreamReader sr = new StreamReader(stream, Encoding.UTF8);
        string json = sr.ReadToEnd();
        return TimiSharedSerializer.Deserialize<T>(json);
    }

    public static T Deserialize<T>(string json) {
        return JsonConvert.DeserializeObject<T>(json);
    }
    #endregion
}
