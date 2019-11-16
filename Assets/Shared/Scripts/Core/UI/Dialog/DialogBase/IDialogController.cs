using UnityEngine;

namespace TimiShared.UI {

    public interface IDialogController {

        void PresentDialog(Transform parent = null, System.Action callback = null);
        void RemoveDialog();
        bool IsShowingDialog { get; }

    }
}