using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class PowerUpZone : ZoneSelectorBase
{
    [SerializeField] Image m_icon;
    [SerializeField] TextMeshProUGUI m_amount;
    [SerializeField] ConsumableDatabase m_consumableDatabase;
    public override void LoadDatabase()
    {
        base.LoadDatabase();
        m_consumableDatabase.Load();
    }
    public override void ValidateData()
    {
        base.ValidateData();
        Consumable consumable = ConsumableDatabase.GetConsumbale(m_dataPlayer.usedConsumable);
        if (m_dataPlayer.consumables != null)
        {
            if (consumable != null)
            {
                m_icon.enabled = true;
                m_icon.sprite = consumable.icon;
                int amount = m_dataPlayer.GetConsumableAmount(m_dataPlayer.usedConsumable);
                m_amount.SetText("x" + amount);
            }
            else
            {
                m_icon.enabled = false;
                m_amount.SetText("");
            }
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
    public override void AutoHideButton()
    {
        base.AutoHideButton();
        m_buttonNext.gameObject.SetActive((int)m_dataPlayer.usedConsumable < (int)Consumable.ConsumableType.MAX_COUNT - 1 && m_dataPlayer.consumables.Count > 0);
        m_buttonPre.gameObject.SetActive((int)m_dataPlayer.usedConsumable > 0);
    }
    public override void ButtonPrePressed()
    {
        base.ButtonPrePressed();
        m_dataPlayer.usedConsumable--;
        int a = 0;
        m_dataPlayer.consumables.TryGetValue(m_dataPlayer.usedConsumable, out a);
        while (a == 0 && m_dataPlayer.usedConsumable > 0)
        {
            m_dataPlayer.usedConsumable--;
            m_dataPlayer.consumables.TryGetValue(m_dataPlayer.usedConsumable, out a);
        }
        ValidateData();
    }
    public override void ButtonNextPressed()
    {
        base.ButtonNextPressed();
        m_dataPlayer.usedConsumable++;
        int b = 0;
        m_dataPlayer.consumables.TryGetValue(m_dataPlayer.usedConsumable, out b);
        while (b == 0 && (int)m_dataPlayer.usedConsumable < (int)Consumable.ConsumableType.MAX_COUNT - 1)
        {
            m_dataPlayer.usedConsumable++;
            m_dataPlayer.consumables.TryGetValue(m_dataPlayer.usedConsumable, out b);
        }
        ValidateData();
    }

}
