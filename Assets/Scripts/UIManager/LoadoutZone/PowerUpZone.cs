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
    [SerializeField] GameObject m_border;
    bool hideNextButton;
    public override void LoadDatabase()
    {
        base.LoadDatabase();
        m_consumableDatabase.Load();
    }
    public override void ValidateData()
    {
        base.ValidateData();
        Consumable consumable = ConsumableDatabase.GetConsumbale(m_dataPlayer.usedConsumable);
        if (m_dataPlayer.consumables.Count > 0)
        {
            if (consumable != null && m_dataPlayer.usedConsumable != Consumable.ConsumableType.NONE)
            {
                m_icon.gameObject.SetActive(true);
                m_icon.sprite = consumable.icon;
                int amount = m_dataPlayer.GetConsumableAmount(m_dataPlayer.usedConsumable);
                m_amount.SetText("x" + amount);
            }
            else
            {
                m_icon.gameObject.SetActive(false);
                m_amount.SetText("");
            }
        }
    }
    public override void AutoHideButton()
    {
        base.AutoHideButton();
        m_buttonNext.gameObject.SetActive((int)m_dataPlayer.usedConsumable < (int)Consumable.ConsumableType.MAX_COUNT - 1 && m_dataPlayer.consumables.Count > 0 && !hideNextButton);
        m_buttonPre.gameObject.SetActive((int)m_dataPlayer.usedConsumable > 0);
        m_border.gameObject.SetActive(m_dataPlayer.consumables.Count > 0);
    }
    public override void ButtonPrePressed()
    {
        base.ButtonPrePressed();
        hideNextButton = false;
        m_dataPlayer.usedConsumable--;
        int a = 0;
        m_dataPlayer.consumables.TryGetValue(m_dataPlayer.usedConsumable, out a);
        while (a <= 0 && m_dataPlayer.usedConsumable > 0)
        {
            m_dataPlayer.usedConsumable--;
            m_dataPlayer.consumables.TryGetValue(m_dataPlayer.usedConsumable, out a);
        }
        ValidateData();
    }
    public override void ButtonNextPressed()
    {
        base.ButtonNextPressed();
        Consumable.ConsumableType currentType = m_dataPlayer.usedConsumable;
        if (GetNextConsumable(currentType) < Consumable.ConsumableType.MAX_COUNT)
        {
            m_dataPlayer.usedConsumable = GetNextConsumable(currentType);
        }
        if (GetNextConsumable(m_dataPlayer.usedConsumable) >= Consumable.ConsumableType.MAX_COUNT)
        {
            hideNextButton = true;
        }
        ValidateData();
    }
    Consumable.ConsumableType GetNextConsumable(Consumable.ConsumableType a_consumable)
    {
        a_consumable++;
        int count = 0;
        m_dataPlayer.consumables.TryGetValue(a_consumable, out count);
        while (count <= 0 && a_consumable < Consumable.ConsumableType.MAX_COUNT)
        {
            a_consumable++;
            m_dataPlayer.consumables.TryGetValue(a_consumable, out count);
        }
        return a_consumable;
    }
}
