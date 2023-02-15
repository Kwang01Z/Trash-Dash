using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using SPStudios.Tools;
public class AccessoriesZone : ZoneSelectorBase
{
    [SerializeField] Image m_accessoryIcon;
    [SerializeField] GameObject m_border;
    public List<int> m_OwnedAccesories = new List<int>();
    public GameObject m_Character;
    bool hideNextButton;
    public override void ValidateData()
    {
        base.ValidateData();
        if (m_dataPlayer.usedAccessory > -1 && m_OwnedAccesories.Count > 0)
        {
            m_accessoryIcon.enabled = true;
            m_accessoryIcon.sprite = m_Character.GetComponent<Character>().accessories[m_dataPlayer.usedAccessory].accessoryIcon;
        }
        else
        {
            m_accessoryIcon.enabled = false;
        }
        UpdateCharDisplay();
    }

    void UpdateCharDisplay()
    {
        if (m_Character == null)
            return;
        m_Character.GetComponent<Character>().SetupAccesory(m_dataPlayer.usedAccessory);
    }

    public override void AutoHideButton()
    {
        base.AutoHideButton();
        m_buttonNext.gameObject.SetActive(m_dataPlayer.usedAccessory < m_Character.GetComponent<Character>().accessories.Length - 1 && m_OwnedAccesories.Count > 0 && !hideNextButton);
        m_buttonPre.gameObject.SetActive(m_dataPlayer.usedAccessory > -1);
        m_border.gameObject.SetActive(m_OwnedAccesories.Count > 0);
    }
    public override void ButtonNextPressed()
    {
        if (GetNextAccess(m_dataPlayer.usedAccessory) < m_Character.GetComponent<Character>().accessories.Length)
        {
            m_dataPlayer.usedAccessory = GetNextAccess(m_dataPlayer.usedAccessory);
        }
        if (GetNextAccess(m_dataPlayer.usedAccessory) >= m_Character.GetComponent<Character>().accessories.Length)
        {
            hideNextButton = true;
        }
        ValidateData();
    }
    int GetNextAccess(int c_access)
    {
        int currentAccess = c_access;
        c_access++;
        while (!ContainAccessory(c_access) && c_access < m_Character.GetComponent<Character>().accessories.Length)
        {
            c_access++;
        }
        if (c_access >= m_Character.GetComponent<Character>().accessories.Length)
        {
            c_access = currentAccess;
        }
        return c_access;
    }
    public override void ButtonPrePressed()
    {
        hideNextButton = false;
        m_dataPlayer.usedAccessory--;
        while (!ContainAccessory(m_dataPlayer.usedAccessory) && m_dataPlayer.usedAccessory > -1)
        {
            m_dataPlayer.usedAccessory--;
        }
        ValidateData();
    }
    bool ContainAccessory(int used)
    {
        foreach (int i in m_OwnedAccesories)
        {
            if (i == used) return true;
        }
        return false;
    }
}
