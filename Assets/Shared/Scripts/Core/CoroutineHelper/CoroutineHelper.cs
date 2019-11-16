using System.Collections;
using TimiShared.Extensions;
using UnityEngine;

public class CoroutineHelper : MonoBehaviour {

    private static CoroutineHelper _instance = null;
    public static CoroutineHelper Instance {
        get {
            if (_instance == null) {
                _instance = GameObject.FindObjectOfType<CoroutineHelper>();
                _instance.AssertNotNull("Coroutine Helper instance");
            }
            return _instance;
        }
    }

    #region public API
    public Coroutine RunCoroutine(IEnumerator coroutine) {
        return this.StartCoroutine(coroutine);
    }

    public void CancelCoroutine(Coroutine coroutine) {
        this.StopCoroutine(coroutine);
    }

    public void RunAfterDelay(float delaySeconds, System.Action callback) {
        this.RunCoroutine(RunAfterDelayCoroutine(delaySeconds, () => {
            if (callback != null) {
                callback.Invoke();
            }
        }));
    }
    #endregion

    private IEnumerator RunAfterDelayCoroutine(float delaySeconds, System.Action callback) {
        yield return new WaitForSeconds(delaySeconds);
        callback.Invoke();
    }

    private void OnDestroy() {
        this.StopAllCoroutines();
    }

}
