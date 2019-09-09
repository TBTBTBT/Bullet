using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toast
{
    public class DialogSingleton : SingletonMonoBehaviour<DialogSingleton>
    {
        [SerializeField] private Dialog _dialogPrefab;
        [SerializeField] private Transform _dialogRoot;

        private List<Dialog> _dialogStack = new List<Dialog>();
        public static IEnumerator OpenModal(Dialog.InputData input)
        {
            yield return Instance.Open(input);
        }
        public static void OpenModeless(Dialog.InputData input)
        {
            Instance.StartCoroutine(Instance.Open(input));
        }

        public static void Close()
        {
            if (Instance._dialogStack.Count > 0)
            {
                Instance._dialogStack[Instance._dialogStack.Count - 1 ].Close();
            }
        }

        public static void SetDialogRoot(RectTransform root)
        {
            Instance._dialogRoot = root;
        }
        public static void SetPrefab(Dialog dialog)
        {
            Instance._dialogPrefab = dialog;
        }
        IEnumerator Open(Dialog.InputData input)
        {
            var ins = Instantiate(_dialogPrefab, _dialogRoot);
            ins.Init(input);
            _dialogStack.Add(ins);
            Debug.Log("[ DialogSingleton ]WaitForClose");
            yield return ins.WaitForClose();
            Debug.Log("[ DialogSingleton ]Close");
            _dialogStack.Remove(ins);
            Destroy(ins.gameObject);
        }

    }
}