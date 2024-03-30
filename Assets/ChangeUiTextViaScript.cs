using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeUiTextViaScript : MonoBehaviour
{
    [SerializeField] private Text m_Text;

    public void ChangeText(string newText)
    {
        if (m_Text != null)
            m_Text.text = newText;
    }
}
