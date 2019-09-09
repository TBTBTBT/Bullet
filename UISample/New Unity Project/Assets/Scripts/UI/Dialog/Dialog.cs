using System.Collections;
using System.Collections.Generic;
using Toast.Rg;
using UnityEditor;
using UnityEngine;

namespace Toast
{
    public class Dialog : MonoBehaviour
    {
        public class InputData
        {
            //public Vector2 Size = new Vector2(640,320);
            public Vector2 Position = new Vector2(640, 320);
            public DialogContents Contents;
            public System.Action<DialogContents> ContentsInit;
        }

        public enum State
        {
            Init,
            Opening,
            Stable,
            Closing,
            End
        }

        private Dictionary<State, string> AnimationTrigger = new Dictionary<State, string>()
        {
            {State.Opening, "Opening"},
            {State.Stable, "Stable"},
            {State.Closing, "Closing"},
            {State.End, "End"},
        };
        /// <summary>
        /// 終了まで待機するステート名
        /// </summary>
        private string AnimationEndOpeningStateName = "Stable";
        private string AnimationEndClosingStateName = "End";
        [SerializeField] private Animator _animator;
        [SerializeField] private RectTransform _dialogRoot;
        [SerializeField] private Transform _contentsBase;
        private DialogContents _dialogContentsInstance;
        private Statemachine<State> _statemachine = new Statemachine<State>();

        private InputData _inputCache;
        // Start is called before the first frame update
        void Awake()
        {
           
            
        }

        public void Init(InputData input)
        {
            _statemachine.Init(this);
            _inputCache = input;
            _statemachine.Next(State.Opening);
            Debug.Log($"[ Dialog ]{ _statemachine.GetCurrentState()}");
        }

        public void Close()
        {
            if (_statemachine.GetCurrentState() == State.Stable)
            { 
                _statemachine.Next(State.Closing);
            }
        }
        public IEnumerator WaitForClose()
        {
            while ( _statemachine.GetCurrentState() != State.End)
            {
                yield return null;
            }
        }
        IEnumerator Opening()
        {
            Debug.Log("[ Dialog ]Opening");
            SetAnimator(State.Opening);
            ResolveInput(_inputCache);
            while (!_animator.GetCurrentAnimatorStateInfo(0).IsName(AnimationEndOpeningStateName))
            {
                yield return null;
            }
            _statemachine.Next(State.Stable);
            yield return null;
        }

        IEnumerator Stable()
        {
            while (true)
            {
                if (_dialogContentsInstance != null)
                {
                    _dialogContentsInstance.UpdateInStable();
                }
                yield return null;
            }
            yield return null;
        }

        IEnumerator Closing()
        {
            Debug.Log("[ Dialog ]Closing");
            SetAnimator(State.Closing);
            while (!_animator.GetCurrentAnimatorStateInfo(0).IsName(AnimationEndClosingStateName))
            {
                yield return null;
            }
            _statemachine.Next(State.End);
            yield return null;
        }

        IEnumerator End()
        {
            Debug.Log("[ Dialog ]End");
            yield return null;
        }

        void SetAnimator(State s)
        {
            if (AnimationTrigger.ContainsKey(s))
            {
                _animator.SetTrigger(AnimationTrigger[s]);
            }
        }

        #region 初期化

        private void ResolveInput(InputData input)
        {
            SetSize(input);
            InitContents(input);
        }

        private void SetSize(InputData input)
        {
            //_dialogRoot.sizeDelta = input.Size;
            _dialogRoot.anchoredPosition = input.Position;
        }
        private void InitContents(InputData input)
        {
            if (input.Contents == null)
            {
                return;
            }
            var ins = Instantiate(input.Contents, _contentsBase);
            input.ContentsInit?.Invoke(ins);
            //ins.GetComponent<RectTransform>().sizeDelta = input.Size;
            ins.Dialog = this;
            _dialogContentsInstance = ins;
            //ins.GetComponent<RectTransform>().anchoredPosition = ;
        }

        #endregion
        // Update is called once per frame
        void Update()
        {
            _statemachine.Update();
        }
    }
}