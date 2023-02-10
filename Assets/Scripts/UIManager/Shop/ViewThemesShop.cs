using frame8.ScrollRectItemsAdapter.Classic;
using SPStudios.Tools;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ViewThemesShop : ClassicSRIA<ItemShopHolder>
{
    [SerializeField] RectTransform itemPrefab;
    [SerializeField] List<ThemeDataShop> Data;
    PlayerData m_playerData;
    List<string> m_themeNames;
    protected override void Awake()
    {
        base.Awake();
        m_themeNames = new List<string>();
        Data = new List<ThemeDataShop>();
    }
    protected override void Start()
    {
        base.Start();
        m_playerData = Singletons.Get<SaveManager>().GetData();
        ChangeModelsAndReset();
    }

    public void ChangeModelsAndReset()
    {
        m_themeNames.Clear();
        int newCount = ThemeDatabase.dictionnary.Count;
        foreach (KeyValuePair<string, ThemeData> pair in ThemeDatabase.dictionnary)
        {
            m_themeNames.Add(pair.Key);
        }
        Data.Clear();
        Data.Capacity = newCount;
        for (int i = 0; i < newCount; i++)
        {
            var model = CreateNewModel(i);
            Data.Add(model);
        }
        ResetItems(Data.Count);
    }
    ThemeDataShop CreateNewModel(int i)
    {
        ThemeDataShop theme = new ThemeDataShop();
        theme.name = m_themeNames[i];
        theme.icon = ThemeDatabase.GetThemeData(m_themeNames[i]).themeIcon;
        theme.priceCoin = ThemeDatabase.GetThemeData(m_themeNames[i]).cost;
        theme.pricePremium = ThemeDatabase.GetThemeData(m_themeNames[i]).premiumCost;
        return theme;
    }

    protected override ItemShopHolder CreateViewsHolder(int itemIndex)
    {
        var instance = new ItemShopHolder();
        instance.Init(itemPrefab, itemIndex);
        return instance;
    }

    protected override void UpdateViewsHolder(ItemShopHolder vh)
    {
        var model = Data[vh.ItemIndex];
        vh.name.SetText(model.name.ToString());
        vh.icon.sprite = model.icon;
        vh.priceCoin.SetText(model.priceCoin.ToString());
        vh.pricePremium.SetText(model.pricePremium.ToString());
        vh.usedAmount.gameObject.SetActive(false);
        if (m_playerData.themes.Contains(model.name))
        {
            vh.buyButton.onClick.RemoveListener(delegate { BuyItem(model); });
            vh.buyButton.gameObject.GetComponent<Image>().color = new Color(32, 32, 32, 125);
            vh.buyButton.gameObject.GetComponentInChildren<TextMeshProUGUI>().SetText("OWNER");
            vh.priceCoin.color = new Color(32, 32, 32);
            vh.pricePremium.color = new Color(32, 32, 32);
        }
        else
        {
            if (m_playerData.coins >= model.priceCoin && m_playerData.premium >= model.pricePremium)
            {
                vh.buyButton.onClick.AddListener(delegate { BuyItem(model); });
                vh.buyButton.gameObject.GetComponent<Image>().color = new Color(255, 255, 255, 255);
                vh.buyButton.gameObject.GetComponentInChildren<TextMeshProUGUI>().SetText("BUY");
                vh.priceCoin.color = new Color(32, 32, 32);
                vh.pricePremium.color = new Color(32, 32, 32);
            }
            else
            {
                vh.buyButton.onClick.RemoveListener(delegate { BuyItem(model); });
                vh.buyButton.gameObject.GetComponent<Image>().color = new Color(32, 32, 32, 125);
                vh.buyButton.gameObject.GetComponentInChildren<TextMeshProUGUI>().SetText("BUY");
                if (m_playerData.coins < model.priceCoin)
                    vh.priceCoin.color = new Color(255, 0, 0);
                if (m_playerData.premium < model.pricePremium)
                    vh.pricePremium.color = new Color(255, 0, 0);
            }
        }
    }
    void BuyItem(ThemeDataShop dataShop)
    {
        m_playerData.themes.Add(dataShop.name);
        m_playerData.coins -= dataShop.priceCoin;
        m_playerData.premium -= dataShop.pricePremium;
        ChangeModelsAndReset();
    }
}
public class ThemeDataShop
{
    public string name;
    public Sprite icon;
    public int priceCoin;
    public int pricePremium;
}
