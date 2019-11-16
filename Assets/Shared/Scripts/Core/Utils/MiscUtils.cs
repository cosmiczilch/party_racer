
using UnityEngine;

namespace TimiSharedUtils {

    public class Pair<U, V> {
        public U First  { get; set; }
        public V Second { get; set; }

        public Pair() {
        }

        public Pair(U u, V v) {
            this.First  = u;
            this.Second = v;
        }
    }

    public static class MiscUtils {
        public static int GetSecondsSinceEpoch() {
            System.DateTime epochStart = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
            return (int)(System.DateTime.UtcNow - epochStart).TotalSeconds;
        }

        public static bool IsFloatZero(float f, float epsilon = 0.0001f) {
            return Mathf.Abs(f) < epsilon;
        }
    }
}