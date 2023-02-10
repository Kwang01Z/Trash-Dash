using frame8.ScrollRectItemsAdapter.Classic;
using SPStudios.Tools;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ViewCharactersShop : ClassicSRIA<ItemShopHolder>
{
    [SerializeField] RectTransform itemPrefab;
    [SerializeField] List<CharacterDataShop> Data;
    PlayerData m_playerData;
    List<string> m_characterNames;
    protected override void Awake()
    {
        base.Awake();
        m_characterNames = new List<string>();
        Data = new List<CharacterDataShop>();
    }
    protected override void Start()
    {
        base.Start();
        m_playerData = Singletons.Get<SaveManager>().GetData();
        StartCoroutine(CharacterDatabase.LoadDatabase());
        ChangeModelsAndReset();
    }

    public void ChangeModelsAndReset()
    {
        m_characterNames.Clear();
        int newCount = CharacterDatabase.dictionary.Count;
        m_characterNames = CharacterDatabase.GetListCharName();
        Data.Clear();
        Data.Capacity = newCount;
        for (int i = 0; i < newCount; i++)
        {
            var model = CreateNewModel(i);
            Data.Add(model);
        }
        ResetItems(Data.Count);
    }
    CharacterDataShop CreateNewModel(int i)
    {
        CharacterDataShop dataShop = new CharacterDataShop();
        dataShop.name = m_characterNames[i];
        dataShop.icon = CharacterDatabase.GetCharacter(m_characterNames[i]).icon;
        dataShop.priceCoin = CharacterDatabase.GetCharacter(m_characterNames[i]).cost;
        dataShop.pricePremium = CharacterDatabase.GetCharacter(m_characterNames[i]).premiumCost;
        return dataShop;
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
        vh.name.SetText(model.name);
        vh.icon.sprite = model.icon;
        vh.priceCoin.SetText(model.priceCoin.ToString());
        vh.pricePremium.SetText(model.pricePremium.ToString());
        vh.usedAmount.gameObject.SetActive(false);
        if (m_playerData.characters.Contains(model.name))
        {
            vh.buyButton.onClick.RemoveListener(delegate { BuyItem(model.name, model.priceCoin, model.pricePremium); });
            vh.buyButton.gameObject.GetComponent<Image>().color = new Color(32, 32, 32, 125);
            vh.buyButton.gameObject.GetComponentInChildren<TextMeshProUGUI>().SetText("OWNER");
            vh.priceCoin.color = new Color(32, 32, 32);
            vh.pricePremium.color = new Color(32, 32, 32);
        }
        else
        {
            if (m_playerData.coins >= model.priceCoin && m_playerData.premium >= model.pricePremium)
            {
                vh.buyButton.onClick.AddListener(delegate { BuyItem(model.name, model.priceCoin, model.pricePremium); });
                vh.buyButton.gameObject.GetComponent<Image>().color = new Color(255, 255, 255, 255);
                vh.buyButton.gameObject.GetComponentInChildren<TextMeshProUGUI>().SetText("BUY");
                vh.priceCoin.color = new Color(32, 32, 32);
                vh.pricePremium.color = new Color(32, 32, 32);
            }
            else
            {
                vh.buyButton.onClick.RemoveListener(delegate { BuyItem(model.name, model.priceCoin, model.pricePremium); });
                vh.buyButton.gameObject.GetComponent<Image>().color = new Color(32, 32, 32, 125);
                vh.buyButton.gameObject.GetComponentInChildren<TextMeshProUGUI>().SetText("BUY");
                if (m_playerData.coins < model.priceCoin)
                    vh.priceCoin.color = new Color(255, 0, 0);
                if (m_playerData.premium < model.pricePremium)
                    vh.pricePremium.color = new Color(255, 0, 0);
            }
        }
        
    }
    void BuyItem(string charName, int coinPrice,int premiumPrice)
    {
        m_playerData.AddCharacter(charName);
        m_playerData.coins -= coinPrice;
        m_playerData.premium -= premiumPrice;
        ChangeModelsAndReset();
    }
}
public class CharacterDataShop
{
    public string name;
    public Sprite icon;
    public int priceCoin;
    public int pricePremium;
}
