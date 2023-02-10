using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using SPStudios.Tools;
public class AccessoriesZone : ZoneSelectorBase
{
    [SerializeField] TextMeshProUGUI m_accessoryName;
    [SerializeField] Image m_accessoryIcon;
    Character m_character;
    public GameObject m_ObjChar;
    public List<int> m_OwnedAccesories = new List<int>();
    public override void LoadDatabase()
    {
        base.LoadDatabase();
        m_character = CharacterDatabase.GetCharacter(m_dataPlayer.characters[m_dataPlayer.usedCharacter]);
    }
    public override void ValidateData()
    {
        base.ValidateData();
        if (m_dataPlayer.accessories == null || m_character == null)
        {
            UpdateCharDisplay();
            gameObject.SetActive(false);
            return;
        }
        if (m_dataPlayer.usedAccessory <= -1)
        {
            m_accessoryName.SetText("");
            m_accessoryIcon.enabled = false;
        }
        else
        {
            m_accessoryIcon.enabled = true;
            m_accessoryName.SetText(m_character.accessories[m_dataPlayer.usedAccessory].accessoryName);
            m_accessoryIcon.sprite = m_character.accessories[m_dataPlayer.usedAccessory].accessoryIcon;
        }
        UpdateCharDisplay();
    }

    void UpdateCharDisplay()
    {
        if (m_ObjChar == null)
            return;
        m_ObjChar.GetComponent<Character>().SetupAccesory(m_dataPlayer.usedAccessory);
    }

    public override void AutoHideButton()
    {
        base.AutoHideButton();
        m_buttonNext.gameObject.SetActive(m_dataPlayer.usedAccessory < m_OwnedAccesories.Count - 1 && m_dataPlayer.accessories.Count > 0);
        m_buttonPre.gameObject.SetActive(m_dataPlayer.usedAccessory > -1);
    }
    public override void ButtonNextPressed()
    {
        m_dataPlayer.usedAccessory++;
        ValidateData();
    }

    public override void ButtonPrePressed()
    {
        m_dataPlayer.usedAccessory--;
        ValidateData();
    }
}
