using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DefaultFadeSceneSetup : MonoBehaviour
{
    [SerializeField] private float fadeInDuration = 3f;
    [SerializeField] private Color fadeInColor = new Color(0,0,0,1);
    
    [Space(height: 20)]
    [SerializeField] private float defaultFadeOutDuration = 5f;
    [SerializeField] private Color defaultFadeOutColor = new Color(0,0,0,0);
    
    public static Action<float, Color> TriggerFadeIn;
    public static Action<float, Color> TriggerFadeOut;

    public float DefaultFadeOutDuration => defaultFadeOutDuration;

    public Color DefaultFadeOutColor => defaultFadeOutColor;

    public float FadeInDuration => fadeInDuration;

    public Color FadeInColor => fadeInColor;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += FadeInAtSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= FadeInAtSceneLoaded;
    }
    
    private void FadeInAtSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        DefaultFadeSceneSetup.TriggerFadeIn.Invoke(FadeInDuration, FadeInColor);
    }
    
    public void GoToNextScene()
    {
        DefaultFadeSceneSetup.TriggerFadeOut.Invoke(DefaultFadeOutDuration, DefaultFadeOutColor);
        if (GlobalManager.Instance.LastSceneName == null)
        {
            SceneTransitionManager.Instance.GoToSceneAsyncByIndexInXSeconds(1, DefaultFadeOutDuration);
        }
        else
        {
            SceneTransitionManager.Instance.GoToSceneAsyncInXSeconds(GlobalManager.Instance.LastSceneName, DefaultFadeOutDuration);
        }
    }
    
}
