using UnityEngine;

namespace TimiShared.Loading {
    public abstract class AsyncRequest : CustomYieldInstruction {
        public abstract void StartRequest();
    }
}