using System;
using System.Collections;
using System.Collections.Generic;
using Toast.Rg;
using UnityEngine;
using UnityEngine.UI;

namespace Toast.Rg
{


    public class DialogMessageAndButton : DialogContents
    {
        public enum State
        {
            Wait,
            ShowAnim,
            ShowAll,
            Next,
            End
        }

        [SerializeField] private TMPro.TMP_Text _text;
        [SerializeField] private Button _nextButton;
        [SerializeField] private DialogButton _buttonPrefab;
        [SerializeField] private RectTransform _buttonRoot;
        private Statemachine<State> _statemachine = new Statemachine<State>();
        private List<MessageSet> _cache;
        private int _pointer = 0;
        private int _msgSpeed = 1;
        private bool button = false;
        
        void Awake()
        {
            _text.text = "";
            _statemachine.Init(this);
            if (_nextButton != null)
            {
                _nextButton.onClick.AddListener(DoNext);
            }
        }

        public override void UpdateInStable()
        {
            _statemachine.Update();
        }
        public void Init(List<MessageSet> cache)
        {
            _cache = cache;
            _pointer = 0;
            _statemachine.Next(State.ShowAnim);
        }

        public void Init(MessageSet cache)
        {
            _cache = new List<MessageSet>(){cache};
            _pointer = 0;
            _statemachine.Next(State.ShowAnim);
        }

        public void DoNext()
        {
            button = true;
        }

        public void ForceNext()
        {
            _statemachine.Next(State.Next);
        }
        
        IEnumerator ShowAnim()
        {
           
            if (_cache[_pointer].AsyncAction != null)
            {
                yield return _cache[_pointer].AsyncAction;
            }
            _cache[_pointer].Action?.Invoke();
            string disp = "";
            var strCount = 0;
            while (disp.Length != _cache[_pointer].Message.Length)
            {
             
                disp += _cache[_pointer].Message.Substring(strCount, 1);
                _text.text = disp;
                var wait = 0;
                while (_msgSpeed > wait)
                {
                    wait++;
                    if (button)
                    {
                        button = false;
                        _statemachine.Next(State.ShowAll);
                    }
                    yield return null;
                }

                strCount++;
            }
            _statemachine.Next(State.ShowAll);
            yield return null;
        }
        IEnumerator ShowAll()
        {
            var isCreate = CheckButtonCreate(_cache[_pointer]);
            string disp = _cache[_pointer].Message;
            _text.text = disp;
            while (!button)
            {
                yield return null;
            }

            button = false;
            if (isCreate)
            {
                foreach (Transform child in _buttonRoot.transform)
                {
                    Destroy(child.gameObject);
                }
            }
            _statemachine.Next(State.Next);
            yield return null;
        }

        IEnumerator Next()
        {
            _pointer++;
            if (_cache.Count > _pointer)
            {
                _statemachine.Next(State.ShowAnim);
            }
            else
            {
                _statemachine.Next(State.End);
            }

            yield return null;
        }
        IEnumerator End()
        {
            Dialog.Close();
            yield return null;
        }

        bool CheckButtonCreate(MessageSet cache)
        {
            if(cache.Button == null)
                return false;
            foreach (var keyValuePair in cache.Button)
            {
                ButtonCreate(keyValuePair.Key,keyValuePair.Value);
            }

            return cache.Button.Count > 0;
        }
        void ButtonCreate(string label, Func<bool> action)
        {
            var b = Instantiate(_buttonPrefab, _buttonRoot);
            b.Button.onClick.AddListener(() =>
            {
                if (action != null && action.Invoke())
                {
                    Dialog.Close();
                }
            });
            b.Text.text = label;
        }

    }
    [Serializable]
    public class MessageSet
    {
        public string Name;
        public string Message;
        public Dictionary<string, Func<bool>> Button = null; //nullじゃなければボタン出す 戻り値はダイアログ閉じるか
        public IEnumerator AsyncAction;
        public Action Action;
    }

    
}