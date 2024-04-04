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
        DefaultFadeSceneSetup.TriggerFadeIn.Invoke(fadeInDuration, fadeInColor);
    }
    
    public void GoToNextScene()
    {
        DefaultFadeSceneSetup.TriggerFadeOut.Invoke(defaultFadeOutDuration, defaultFadeOutColor);
        if (GlobalManager.Instance.lastSceneName == null)
        {
            SceneTransitionManager.Instance.GoToSceneAsyncByIndexInXSeconds(1, defaultFadeOutDuration);
        }
        else
        {
            SceneTransitionManager.Instance.GoToSceneAsyncInXSeconds(GlobalManager.Instance.lastSceneName, defaultFadeOutDuration);
        }
    }
    
}
