using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Events;

public class FadeScreen : MonoBehaviour
{
    [SerializeField] private float _defaultFadeDuration = 3;
    [SerializeField] private Color _fadeColor = new Color(0,0,0, 1);

    private bool initialFadeInTriggered = false;

    private Renderer _renderer;
    private static readonly int SHADER_COLOR = Shader.PropertyToID("_BaseColor");
    


    public Color FadeColor
    {
        get => _fadeColor;
        set => _fadeColor = value;
    }

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
    }

    private void OnEnable()
    {
        AbstractPortalToSomeNewPlace.OnPortalEnter += FadeOutFromPortal;
        AbstractPortalToSomeNewPlace.OnPortalThrough += FadeInFromPortal;
        DefaultFadeSceneSetup.TriggerFadeIn += StartFadeIn;
        DefaultFadeSceneSetup.TriggerFadeOut += EndingFadeOut;
    }
    
    private void OnDisable()
    {
        AbstractPortalToSomeNewPlace.OnPortalEnter -= FadeOutFromPortal;
        AbstractPortalToSomeNewPlace.OnPortalThrough -= FadeInFromPortal;
        DefaultFadeSceneSetup.TriggerFadeIn -= StartFadeIn;
        DefaultFadeSceneSetup.TriggerFadeOut -= EndingFadeOut;

    }

    private void EndingFadeOut(float duration, Color newColor)
    {
        _fadeColor = newColor;
        FadeOut(duration);
    }

    private void Start()
    {
        _renderer.material.SetColor(SHADER_COLOR, FadeColor);
        StartCoroutine(InitialFadeInIfNotInvoked());
    }

    private IEnumerator InitialFadeInIfNotInvoked()
    {
        yield return new WaitForSeconds(1f);
        StartFadeIn(_defaultFadeDuration, _fadeColor);
    }

    private void StartFadeIn(float duration, Color newColor)
    {
        if (initialFadeInTriggered) return;
        _fadeColor = newColor;
        initialFadeInTriggered = true;
        FadeIn(duration);
    }

    private void FadeOutFromPortal(PortalSettings portalSettings)
    {
        FadeOut(portalSettings.enterPortalAnimDuration);
    }    
    
    private void FadeInFromPortal(PortalSettings portalSettings)
    {
        FadeIn(portalSettings.leavePortalAnimDuration);
    }

    public void FadeIn(float duration = 0f)
    {
        Fade(1, 0, duration);
    }
    
    public void FadeOut(float duration = 0f)
    {
        Fade(0, 1, duration);
    }

    public void Fade(float alphaIn, float alphaOut, float duration)
    {
        float fadeDuration = Math.Abs(duration - (0f)) < 0.01 ? _defaultFadeDuration : duration;
        StartCoroutine(FadeRoutine(alphaIn, alphaOut, fadeDuration));
    }

    private IEnumerator FadeRoutine(float alphaIn, float alphaOut, float duration)
    {
        float timer = 0;
        Color newColor = FadeColor;
        newColor.a = alphaIn;

        while (timer <= duration)
        {
            newColor.a = Mathf.Lerp(alphaIn, alphaOut, timer / duration);

            _renderer.material.SetColor(SHADER_COLOR, newColor);
            timer += Time.deltaTime;
            yield return null;
        }

        newColor.a = alphaOut;
        _renderer.material.SetColor(SHADER_COLOR, newColor);
    }
}
