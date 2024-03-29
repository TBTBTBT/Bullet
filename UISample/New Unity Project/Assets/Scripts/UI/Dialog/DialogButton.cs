﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Toast.Rg
{


    public class DialogButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private TMPro.TMP_Text _text;
        public Button Button => _button;
        public TMPro.TMP_Text Text => _text;
    }

}