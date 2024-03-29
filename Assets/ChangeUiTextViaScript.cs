using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChangeUiTextViaScript : MonoBehaviour
{
    [SerializeField] private TMP_Text m_Text;
    [SerializeField] private bool m_ShouldChangeOnDebug;

    private void OnEnable()
    {
        
    }

    private void Awake()
    {
        m_Text = gameObject.GetComponentInChildren<TMP_Text>();
    }

    public void ChangeText(string newText)
    {
        m_Text.text = newText;
    }
}
