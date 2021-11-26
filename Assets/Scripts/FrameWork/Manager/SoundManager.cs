using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public enum SoundType
    {
        Default,
        BackGround,
        Other,
    }

    public Dictionary<string, AudioClip> m_AudioClipDict = new Dictionary<string, AudioClip>();

    public AudioSource m_DefaultAudioSource = null;
    public AudioSource m_BackGroundAudioSource = null;
    public AudioSource m_OtherAudioSource = null;

    private static SoundManager m_Instance;
    public static SoundManager Instance
    {
        get { return HotfixFrameWork.Util.GetInstance(ref m_Instance, "_SoundMgr"); }
    }

    private void Awake()
    {
        m_DefaultAudioSource = gameObject.AddComponent<AudioSource>();
        m_BackGroundAudioSource = gameObject.AddComponent<AudioSource>();
        m_OtherAudioSource = gameObject.AddComponent<AudioSource>();
        m_DefaultAudioSource.playOnAwake = false;
        m_BackGroundAudioSource.playOnAwake = false;
        m_BackGroundAudioSource.loop = false;
        m_OtherAudioSource.playOnAwake = false;
    }


    public void PlayMusic(string musicName, SoundType soundType = SoundType.Default)
    {
        if (!m_AudioClipDict.ContainsKey(musicName))
        {
            AudioClip audioClip = Resources.Load<AudioClip>("Sound/" + musicName);
            m_AudioClipDict.Add(musicName, audioClip);
        }
        if (soundType == SoundType.Default)
        {
            if (m_DefaultAudioSource.isPlaying == true)
            {
                m_DefaultAudioSource.Stop();
            }
            m_DefaultAudioSource.clip = m_AudioClipDict[musicName];
            m_DefaultAudioSource.Play();
        }
        else if (soundType == SoundType.BackGround)
        {
            if (m_BackGroundAudioSource.isPlaying == true)
            {
                m_BackGroundAudioSource.Stop();
            }
            m_BackGroundAudioSource.clip = m_AudioClipDict[musicName];
            m_BackGroundAudioSource.Play();
        }
        else if (soundType == SoundType.Other)
        {
            if (m_OtherAudioSource.isPlaying == true)
            {
                m_OtherAudioSource.Stop();
            }
            m_OtherAudioSource.clip = m_AudioClipDict[musicName];
            m_OtherAudioSource.Play();
        }
    }

    public void SetMusicVolume(float volume)
    {
        m_BackGroundAudioSource.volume = volume;
    }

    public void SetSoundVolume(float volume)
    {
        m_DefaultAudioSource.volume = volume;
        m_OtherAudioSource.volume = volume;
    }
}
