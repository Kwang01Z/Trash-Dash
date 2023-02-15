using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SPStudios.Tools;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class SettingPopup : MonoBehaviour
{
    public AudioMixer mixer;
    [SerializeField] Slider m_masterSlider;
    [SerializeField] Slider m_musicSlider;
    [SerializeField] Slider m_sfxSlider;
    [SerializeField] GameController m_gameController;
    PlayerData m_playerData;
    protected float m_MasterVolume;
    protected float m_MusicVolume;
    protected float m_MasterSFXVolume;

    protected const float k_MinVolume = -80f;
    protected const string k_MasterVolumeFloatName = "MasterVolume";
    protected const string k_MusicVolumeFloatName = "MusicVolume";
    protected const string k_MasterSFXVolumeFloatName = "MasterSFXVolume";
    private void Start()
    {
        m_playerData = Singletons.Get<SaveManager>().GetData();
        Init();
        UpdateUI();
        m_masterSlider.onValueChanged.RemoveListener(delegate { OnSoundSlideValueChange(); });
        m_musicSlider.onValueChanged.RemoveListener(delegate { OnSoundSlideValueChange(); });
        m_sfxSlider.onValueChanged.RemoveListener(delegate { OnSoundSlideValueChange(); });
        m_masterSlider.onValueChanged.AddListener(delegate { OnSoundSlideValueChange(); });
        m_musicSlider.onValueChanged.AddListener(delegate { OnSoundSlideValueChange(); });
        m_sfxSlider.onValueChanged.AddListener(delegate { OnSoundSlideValueChange(); });
    }

    void OnSoundSlideValueChange()
    {
        m_MasterVolume = k_MinVolume * (1.0f - m_masterSlider.value);
        m_MusicVolume = k_MinVolume * (1.0f - m_musicSlider.value);
        m_MasterSFXVolume = k_MinVolume * (1.0f - m_sfxSlider.value);
        mixer.SetFloat(k_MasterVolumeFloatName, m_MasterVolume);
        mixer.SetFloat(k_MusicVolumeFloatName, m_MusicVolume); 
        mixer.SetFloat(k_MasterSFXVolumeFloatName, m_MasterSFXVolume);
        m_playerData.masterVolume = m_masterSlider.value;
        m_playerData.musicVolume = m_musicSlider.value;
        m_playerData.masterSFXVolume = m_sfxSlider.value;
    }
    public void SaveDataChange()
    {
        Singletons.Get<SaveManager>().Save();
    }
    public void DeleteData()
    {
        SaveManager.ClearAllSave();
        Singletons.Clear();
        m_gameController.LoadScence("Start");
    }
    void UpdateUI()
    {
        mixer.GetFloat(k_MasterVolumeFloatName, out m_MasterVolume);
        mixer.GetFloat(k_MusicVolumeFloatName, out m_MusicVolume);
        mixer.GetFloat(k_MasterSFXVolumeFloatName, out m_MasterSFXVolume);

        m_masterSlider.value = 1.0f - (m_MasterVolume / k_MinVolume);
        m_musicSlider.value = 1.0f - (m_MusicVolume / k_MinVolume);
        m_sfxSlider.value = 1.0f - (m_MasterSFXVolume / k_MinVolume);
    }
    public void Init()
    {
        mixer.SetFloat(k_MasterVolumeFloatName, (1.0f - m_playerData.masterVolume )* k_MinVolume);
        mixer.SetFloat(k_MusicVolumeFloatName, (1.0f - m_playerData.musicVolume )* k_MinVolume);
        mixer.SetFloat(k_MasterSFXVolumeFloatName, (1.0f - m_playerData.masterSFXVolume) * k_MinVolume);
    }
}
