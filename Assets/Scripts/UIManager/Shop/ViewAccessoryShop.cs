using frame8.ScrollRectItemsAdapter.Classic;
using SPStudios.Tools;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ViewAccessoryShop : ClassicSRIA<ItemShopHolder>
{
    [SerializeField] RectTransform itemPrefab;
    [SerializeField] List<AccessoryDataShop> Data;
    PlayerData m_playerData;
    List<string> m_characterNames;
    protected override void Awake()
    {
        base.Awake();
        m_characterNames = new List<string>();
        Data = new List<AccessoryDataShop>();
    }
    protected override void Start()
    {
        base.Start();
        m_playerData = Singletons.Get<SaveManager>().GetData();
        ChangeModelsAndReset();
    }

    public void ChangeModelsAndReset()
    {
        m_characterNames.Clear();
        foreach (KeyValuePair<string, Character> pair in CharacterDatabase.dictionary)
        {
            m_characterNames.Add(pair.Key);
        }
        
        int dataCapacity = 0;
        foreach (string charName in m_characterNames)
        {
            dataCapacity += CharacterDatabase.GetCharacter(charName).accessories.Length;
        }
        Data.Clear();
        Data.Capacity = dataCapacity;

        foreach (string charName in m_characterNames)
        {
            for (int i = 0; i < CharacterDatabase.GetCharacter(charName).accessories.Length; i++)
            {
                var model = CreateNewModel(charName,i);
                Data.Add(model);
            }
        }
        
        ResetItems(Data.Count);
    }
    AccessoryDataShop CreateNewModel(string charName,int i)
    {
        AccessoryDataShop dataShop = new AccessoryDataShop();
        dataShop.charName = charName;
        dataShop.name = CharacterDatabase.GetCharacter(charName).accessories[i].accessoryName;
        dataShop.icon = CharacterDatabase.GetCharacter(charName).accessories[i].accessoryIcon;
        dataShop.priceCoin = CharacterDatabase.GetCharacter(charName).accessories[i].cost;
        dataShop.pricePremium = CharacterDatabase.GetCharacter(charName).accessories[i].premiumCost;
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
        vh.name.SetText(model.charName+"-"+model.name);
        vh.icon.sprite = model.icon;
        vh.priceCoin.SetText(model.priceCoin.ToString());
        vh.pricePremium.SetText(model.pricePremium.ToString());
        vh.usedAmount.gameObject.SetActive(false);
        if (m_playerData.accessories.Contains(model.charName + ":" + model.name))
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
    void BuyItem(AccessoryDataShop item)
    {
        m_playerData.AddAccessories(item.charName,item.name);
        m_playerData.coins -= item.priceCoin;
        m_playerData.premium -= item.pricePremium;
        ChangeModelsAndReset();
    }
}
public class AccessoryDataShop
{
    public string charName;
    public string name;
    public Sprite icon;
    public int priceCoin;
    public int pricePremium;
}
