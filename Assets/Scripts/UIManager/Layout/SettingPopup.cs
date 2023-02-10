using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SPStudios.Tools;
using UnityEngine.SceneManagement;
public class SettingPopup : MonoBehaviour
{
    [SerializeField] Slider m_masterSlider;
    [SerializeField] Slider m_musicSlider;
    [SerializeField] Slider m_sfxSlider;
    [SerializeField] GameController m_gameController;
    public PlayerData m_playerData;
    // Start is called before the first frame update
    void Start()
    {
        m_playerData = Singletons.Get<SaveManager>().GetData();
        m_masterSlider.value = m_playerData.masterVolume;
        m_musicSlider.value = m_playerData.musicVolume;
        m_sfxSlider.value = m_playerData.masterSFXVolume;
        m_masterSlider.onValueChanged.AddListener(delegate { OnSoundSlideValueChange(); });
        m_musicSlider.onValueChanged.AddListener(delegate { OnSoundSlideValueChange(); });
        m_sfxSlider.onValueChanged.AddListener(delegate { OnSoundSlideValueChange(); });
    }
    void OnSoundSlideValueChange()
    {
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
        SaveManager.ClearSave();
        Singletons.Clear();
        m_gameController.LoadScence("Start");
    }
}
