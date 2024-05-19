using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Connected to PervasiveAudioBetweenScenes, hardcoded in a hurry.
/// TODO: remove this once audio implementation between scenes is improved
/// </summary>
public class HandlePervasiveAudio : MonoBehaviour
{
    public void PlayMusic()
    {
        PervasiveAudioBetweenScenes.Instance.PlayMusic();
    }

    public void PauseMusic()
    {
        PervasiveAudioBetweenScenes.Instance.PauseMusic();
    }
    public void SetMinVolume()
    {
        PervasiveAudioBetweenScenes.Instance.SetMinVolume();
    }
    public void SetMaxVolume()
    {
        PervasiveAudioBetweenScenes.Instance.SetMaxVolume();
    }
    public void StopMusic()
    {
        PervasiveAudioBetweenScenes.Instance.StopMusic();
    }
    public void ChangeAudioClip(AudioClip audioClip)
    {
        PervasiveAudioBetweenScenes.Instance.ChangeAudioClip(audioClip);
    }

    public void ForceDestroy()
    {
        PervasiveAudioBetweenScenes.Instance.ForceDestroy();
    }

    public void ChangeTransformToHere()
    {
        PervasiveAudioBetweenScenes.Instance.ChangeTransform(transform);
    }
}
