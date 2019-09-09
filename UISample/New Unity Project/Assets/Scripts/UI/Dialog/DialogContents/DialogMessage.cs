using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Toast.Rg
{

    /// <summary>
    /// メッセージを出した後オートで消えるダイアログ
    /// </summary>
    public class DialogMessage : DialogContents
    {
        [SerializeField] private Text _text;
        private int time = 0;
        private string _message;
        private int MaxTime = 60;

        public void Init(string message)
        {
            _message = message;
            _text.text = _message;
        }
        // Update is called once per frame
        void Update()
        {
            time++;
            if (MaxTime < time)
            {
                Dialog?.Close();
            }

        }
    }
}
