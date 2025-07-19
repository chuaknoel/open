using UnityEngine;
using Enums;
using System.Net;
using System;
/// <summary>
/// 오디오 클립을 관리하는 클래스
/// </summary>
[Serializable]
public class Sound
{
    public AudioClip Clip { get; private set; }
    public SoundType Type { get; private set; }

    public Sound(AudioClip clip, string address)
    {
        this.Clip = clip;
        this.Type = SoundManager.Instance.GetSoundType(address);
    }
}