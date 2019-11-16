using System.Collections.Generic;
using System.Reflection;
using TimiShared.Debug;
using UnityEngine;

namespace TimiShared.Extensions {
    public static class Extensions {

        /*
         * TODO: Make sure that this is actually a good strategy
         */
		public static void AssertNotNull(this UnityEngine.Object obj, string varName = "") {
			if (obj == null) {
                DebugLog.LogErrorColor("Uninitialized variable" +
                    (string.IsNullOrEmpty(varName) ? "" : ": " + varName), LogColor.red);
                throw new UninitializedVariableException(varName);
			}
		}

        /*
         * TODO: Make sure that this is actually a good strategy
         */
        public static void AssertNotNullOrEmpty(this string s, string varName = "") {
            if (string.IsNullOrEmpty(s)) {
                DebugLog.LogErrorColor("Uninitialized string" +
                    (string.IsNullOrEmpty(varName) ? "" : ": " + varName), LogColor.red);
                throw new UninitializedVariableException(varName);
            }
        }

        public static T GetOrAddComponent<T>(this GameObject gameObject) where T : UnityEngine.Component {
            T component = gameObject.GetComponent<T>();
            if (component == null) {
                component = gameObject.AddComponent<T>();
            }
            return component;
        }

        public static T GetRequiredComponent<T>(this GameObject gameObject) where T : UnityEngine.Component {
            T component = gameObject.GetComponent<T>();
            if (component == null) {
                DebugLog.LogWarningColor("Cannot find component of type " + typeof(T).Name, LogColor.grey);
            }

            return component;
        }

        public static bool CopyObjectFieldsFrom(this object destination, object source) {
            if (source.GetType() != destination.GetType()) {
                DebugLog.LogErrorColor("Data type mismatch between source: " + source.GetType().Name + " and destination: " + destination.GetType().Name, LogColor.red);
                return false;
            }

            var sourceFields = source.GetType().GetFields();
            for (int i = 0; i < sourceFields.Length; ++i) {
                FieldInfo sourceField = sourceFields[i];
                if (sourceField.IsPublic && !sourceField.IsStatic && sourceField.FieldType != typeof(System.Action)) {
                    sourceField.SetValue(destination, sourceField.GetValue(source));
                }
            }

            return true;
        }

        public static void Randomize<T>(this List<T> list, int iterations, System.Random RNG) {
            if (list == null || list.Count == 0) {
                return;
            }

            int listCount = list.Count;
            for (int i = 0; i < iterations; ++i) {
                int a = RNG.Next(0, listCount);
                int b = RNG.Next(0, listCount);
                list.Swap(a, b);
            }
        }

        public static void Swap<T>(this List<T> list, int index_a, int index_b) {
            if (list == null || index_a >= list.Count || index_b >= list.Count) {
                return;
            }

            T temp = list[index_a];
            list[index_a] = list[index_b];
            list[index_b] = temp;
        }
    }
}
