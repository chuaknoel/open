using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundUI : MonoBehaviour
{
    [SerializeField] private AudioMixer mainMixer;

    [Header("Sound Slider")]
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Slider voiceSlider;
    [SerializeField] private Slider ambientSlider;

    private void Start()
    {
        LoadVolumeSetting();
        SliderAddListner();
    }
    /// <summary>
    /// 슬라이더 값이 변경될 때마다 볼륨을 설정
    /// </summary>
    private void SliderAddListner()
    {
        masterSlider.onValueChanged.AddListener(SetMasterVolume);
        bgmSlider.onValueChanged.AddListener(SetBGMVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);
        voiceSlider.onValueChanged.AddListener(SetVoiceVolume);
        ambientSlider.onValueChanged.AddListener(SetAmbientVolume);
    }
    /// <summary>
    /// 슬라이더 값(0~1)을 오디오 믹서 볼륨 값(-80~0)으로 변환
    /// </summary>
    /// <param name="linear"></param>
    /// <returns></returns>
    private float ToMixerVolume(float linear)
    {
        return Mathf.Log10(Mathf.Clamp(linear, 0.01f, 1f)) * 20f;
    }
    /// <summary>
    /// 마스터 볼륨을 설정
    /// </summary>
    /// <param name="value"></param>
    private void SetMasterVolume(float value)
    {
        mainMixer.SetFloat("MasterVolume", ToMixerVolume(value));
        PlayerPrefs.SetFloat("MasterVolume", value);
    }
    /// <summary>
    /// BGM 볼륨을 설정
    /// </summary>
    /// <param name="value"></param>
    private void SetBGMVolume(float value)
    {
        mainMixer.SetFloat("BGMVolume", ToMixerVolume(value));
        PlayerPrefs.SetFloat("BGMVolume", value);
    }
    /// <summary>
    /// SFX 볼륨을 설정
    /// </summary>
    /// <param name="value"></param>
    private void SetSFXVolume(float value)
    {
        mainMixer.SetFloat("SFXVolume", ToMixerVolume(value));
        PlayerPrefs.SetFloat("SFXVolume", value);
    }
    /// <summary>
    /// 음성 볼륨을 설정
    /// </summary>
    /// <param name="value"></param>
    private void SetVoiceVolume(float value)
    {
        mainMixer.SetFloat("VoiceVolume", ToMixerVolume(value));
        PlayerPrefs.SetFloat("VoiceVolume", value);
    }
    /// <summary>
    /// 환경음 볼륨을 설정
    /// </summary>
    /// <param name="value"></param>
    private void SetAmbientVolume(float value)
    {
        mainMixer.SetFloat("AmbientVolume", ToMixerVolume(value));
        PlayerPrefs.SetFloat("AmbientVolume", value);
    }
    /// <summary>
    /// 저장된 볼륨 설정을 불러옴
    /// </summary>
    private void LoadVolumeSetting()
    {
        float masterVolume = PlayerPrefs.GetFloat("MasterVolume", 1f);
        float bgmVolume = PlayerPrefs.GetFloat("BGMVolume", 1f);
        float sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);
        float voiceVolume = PlayerPrefs.GetFloat("VoiceVolume", 1f);
        float ambientVolume = PlayerPrefs.GetFloat("AmbientVolume", 1f);

        masterSlider.value = masterVolume;
        bgmSlider.value = bgmVolume;
        sfxSlider.value = sfxVolume;
        voiceSlider.value = voiceVolume;
        ambientSlider.value = ambientVolume;

        mainMixer.SetFloat("MasterVolume", ToMixerVolume(masterVolume));
        mainMixer.SetFloat("BGMVolume", ToMixerVolume(bgmVolume));
        mainMixer.SetFloat("SFXVolume", ToMixerVolume(sfxVolume));
        mainMixer.SetFloat("VoiceVolume", ToMixerVolume(voiceVolume));
        mainMixer.SetFloat("AmbientVolume", ToMixerVolume(ambientVolume));
    }
}
