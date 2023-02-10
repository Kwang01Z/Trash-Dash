using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SPStudios.Tools;
using UnityEngine.UI;
using UnityEngine.Events;
public class ZoneSelectorBase : MonoBehaviour
{
    public Button m_buttonPre;
    public Button m_buttonNext;
    [HideInInspector] public PlayerData m_dataPlayer;
    private void Start()
    {
        m_dataPlayer = Singletons.Get<SaveManager>().GetData();
        LoadDatabase();
        ValidateData();
        m_buttonPre.onClick.AddListener(ValidateData);
        m_buttonNext.onClick.AddListener(ValidateData);
    }
    private void OnEnable()
    {
        m_buttonPre.onClick.AddListener(ButtonPrePressed);
        m_buttonNext.onClick.AddListener(ButtonNextPressed);
    }
    private void OnDisable()
    {
        m_buttonPre.onClick.RemoveListener(ButtonPrePressed);
        m_buttonNext.onClick.RemoveListener(ButtonNextPressed);
    }
    private void Update()
    {
        AutoHideButton();
        OtherUpdate();
    }
    public virtual void LoadDatabase() { }
    public virtual void ValidateData() { }
    public virtual void AutoHideButton() { }
    public virtual void ButtonPrePressed() { }
    public virtual void ButtonNextPressed() { }
    public virtual void OtherUpdate() { }
}
