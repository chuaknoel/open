using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Audio;
using UnityEngine.ResourceManagement.AsyncOperations;
using Enums;
/// <summary>
/// SoundManager는 게임 내 사운드를 관리하는 싱글톤 클래스입니다.
/// </summary>
public class SoundManager : MonoBehaviour
{
    [Header("Audio Mixer Groups")]
    [SerializeField] private AudioMixerGroup BgmMixerGroup;
    [SerializeField] private AudioMixerGroup SFXMixerGroup;
    [SerializeField] private AudioMixerGroup VoiceMixerGroup;
    [SerializeField] private AudioMixerGroup AmbientMixerGroup;

    private List<string> soundData;
    private Dictionary<string, Sound> soundClips;

    public static SoundManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        soundClips = new Dictionary<string, Sound>();
    }
    /// <summary>
    /// 클립 불러오기
    /// </summary>
    public Sound GetClip(string clipName)
    {
        return soundClips[clipName];
    }
    /// <summary>
    /// 오디오 믹서 그룹 지정
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public AudioMixerGroup GetGroup(SoundType type)
    {
        return type switch
        {
            SoundType.BGM => BgmMixerGroup,
            SoundType.SFX => SFXMixerGroup,
            SoundType.Voice => VoiceMixerGroup,
            SoundType.Ambient => AmbientMixerGroup,
            _ => null
        };
    }
    /// <summary>
    /// 사운드 타입을 그룹 지정
    /// </summary>
    /// <param name="groupName"></param>
    /// <returns></returns>
    public SoundType GetSoundType(string address)
    {
        if (string.IsNullOrEmpty(address)) return SoundType.None;
        string group = address.Contains("/") ? address.Split('/')[0].ToLower() : null;

        return group switch
        {
            "bgm" => SoundType.BGM,
            "sfx" => SoundType.SFX,
            "voice" => SoundType.Voice,
            "ambient" => SoundType.Ambient,
            _ => SoundType.None
        };
    }

    public bool TryGetClip(string clipName, out Sound sound)
    {
        return soundClips.TryGetValue(clipName, out sound);
    }
}