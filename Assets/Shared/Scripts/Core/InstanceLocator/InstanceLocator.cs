using System;
using System.Collections.Generic;
using TimiShared.Debug;

namespace TimiShared.Instance {
    public static class InstanceLocator {
        private static Dictionary<Type, IInstance> _instances = new Dictionary<Type, IInstance>();

        public static void RegisterInstance<T>(IInstance instance) {
            if (instance.GetType() != typeof(T) &&
                !instance.GetType().IsSubclassOf(typeof(T))) {
                DebugLog.LogErrorColor("Type mismatch in registering instance of type " + typeof(T).Name, LogColor.red);
                return;
            }
            _instances[typeof(T)] = instance;
        }

        public static T Instance<T>() where T : class, new() {
            IInstance instance = null;
            if (_instances.TryGetValue(typeof(T), out instance)) {
                return instance as T;
            }
            DebugLog.LogWarningColor("No instance registered for type " + typeof(T).Name, LogColor.red);
            // TODO: currently this does not work for instances that are monobehaviours
            instance = new T() as IInstance;
            InstanceLocator.RegisterInstance<T>(instance);
            return instance as T;
        }
    }
}