using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class ChangeUiVideoViaScript : MonoBehaviour
{
    [SerializeField] public VideoPlayer m_VideoPlayer;
    [SerializeField] private AudioSource m_AudioSource;

    private void Start()
    {
        m_VideoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;
        m_VideoPlayer.SetTargetAudioSource(0, m_AudioSource);
    }

    public void ChangeVideo(VideoClip newClip, float volume, bool playNow = true)
    {
        if (m_VideoPlayer != null)
        {
            m_VideoPlayer.Stop();
            m_VideoPlayer.clip = newClip;
        }

        if (m_AudioSource != null)
        {
            m_AudioSource.volume = volume;
        }

        if (playNow)
        {
            m_VideoPlayer.Play();
        }
    }
}
