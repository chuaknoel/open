using UnityEngine;

/// <summary>
/// 오디오 클립을 관리하는 클래스
/// </summary>
public class Sound
{
    public AudioClip Clip { get; private set; }
    public SoundType Type { get; private set; }

    public Sound(AudioClip clip, SoundType type)
    {
        this.Clip = clip;
        this.Type = type;
    }
}