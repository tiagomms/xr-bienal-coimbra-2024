using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeUiImageViaScript : MonoBehaviour
{
    [SerializeField] private Image m_Image;

    public void ChangeImage(Sprite newImage)
    {
        if (m_Image != null)
            m_Image.sprite = newImage;
    }
}
