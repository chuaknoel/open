using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Audio;
using UnityEngine.ResourceManagement.AsyncOperations;

public enum SoundType
{
    BGM,
    SFX,
    Voice,
    Ambient,
    None
}
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

    private static SoundManager _instance;
    public static SoundManager Instance => _instance;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            soundClips = new Dictionary<string, Sound>();
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }
    /// <summary>
    /// 클립 불러오기
    /// </summary>
    public Sound GetClip(string clipName)
    {
        return soundClips[clipName];
    }
    /// <summary>
    /// 클립을 주소를 통해 로드
    /// </summary>
    /// <param name="clipName"></param>
    public void LoadClip(string clipName)
    {
        Addressables.LoadResourceLocationsAsync(clipName).Completed += locationHandle =>
        {
            if (locationHandle.Status == AsyncOperationStatus.Succeeded && locationHandle.Result.Count > 0)
            {
                var location = locationHandle.Result[0];

                // 그룹 이름으로 SoundType 결정
                string groupName = location.PrimaryKey.Contains("/") ? location.PrimaryKey.Split('/')[0] : null;
                SoundType type = GetSoundType(groupName);

                Addressables.LoadAssetAsync<AudioClip>(location).Completed += clipHandle =>
                {
                    if (clipHandle.Status == AsyncOperationStatus.Succeeded)
                    {
                        AudioClip clip = clipHandle.Result;
                        soundClips[clipName] = new Sound(clip, type);
                    }
                    else
                    {
                        Logger.Log($"Failed to load audio clip: {clipName}");
                    }
                };
                // 클립 로드(JSon으로 로드할거임)
            }
            else
            {
                Logger.Log($"No audio clip found for: {clipName}");
            }
        };
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
    public SoundType GetSoundType(string groupName)
    {
        return groupName.ToLower() switch
        {
            "bgm" => SoundType.BGM,
            "sfx" => SoundType.SFX,
            "voice" => SoundType.Voice,
            "ambient" => SoundType.Ambient,
            _ => SoundType.None
        };
    }
}