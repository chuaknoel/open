using System;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 오디오 타입을 정의하는 열거형
/// </summary>
public enum AudioType
{
    Static,
    Dynamic
}
/// <summary>
/// 실재 재생을 담당하는 클래스
/// </summary>
public class AudioPlayer : MonoBehaviour
{
    [SerializeField] private AudioType audioType;

    [Header("Audio Source")]
    private Dictionary<SoundType, AudioSource> audioSources;

    private void Start()
    {
        audioSources = new Dictionary<SoundType, AudioSource>();
        MakeAudioSource();
    }
    /// <summary>
    /// 오디오 소스를 생성하는 메서드
    /// </summary>
    private void MakeAudioSource()
    {
        if (audioType == AudioType.Dynamic)
        {
            CreateAudioSource(SoundType.SFX, is3D: true);
            CreateAudioSource(SoundType.Ambient, is3D: true, isLoop: true);
        }
        else
        {
            CreateAudioSource(SoundType.BGM);
            CreateAudioSource(SoundType.Voice);
        }
    }
    /// <summary>
    /// SoundType과 bool 값에 따라 생성될 오디오 소스를 설정
    /// </summary>
    /// <param name="type"></param>
    /// <param name="is3D"></param>
    private void CreateAudioSource(SoundType type, bool is3D = false, bool isLoop = false)
    {
        if (audioSources.ContainsKey(type)) return;

        GameObject audioObject = new GameObject($"{type}AudioSource");
        audioObject.transform.SetParent(this.transform);
        audioObject.transform.localPosition = Vector3.zero;

        AudioSource source = audioObject.AddComponent<AudioSource>();
        source.outputAudioMixerGroup = SoundManager.Instance.GetGroup(type);

        if (is3D)
        {
            source.spatialBlend = 1f;
            source.minDistance = 1f;
            source.maxDistance = 15f;
            source.rolloffMode = AudioRolloffMode.Logarithmic;
            if (isLoop)
            {
                source.loop = true;
            }
        }

            audioSources[type] = source;
    }
    /// <summary>
    /// 사운드 재생
    /// </summary>
    /// <param name="clip"></param>
    /// <param name="type"></param>
    public void Play(Sound sound)
    {
        if (sound == null || sound.Clip == null) return;
        if (audioSources.TryGetValue(sound.Type, out var source))
        {
            if (sound.Type == SoundType.SFX)
                source.PlayOneShot(sound.Clip);
            else
            {
                source.clip = sound.Clip;
                source.Play();
            }
        }
    }
}