using frame8.ScrollRectItemsAdapter.Classic;
using SPStudios.Tools;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ViewItemsShop : ClassicSRIA<ItemShopHolder>
{
    [SerializeField] RectTransform itemPrefab;
    [SerializeField] List<ConsumableDataShop> Data;
    [SerializeField] ConsumableDatabase consumableDatabase;
    PlayerData m_playerData;
    Consumable.ConsumableType[] s_ConsumablesTypes = System.Enum.GetValues(typeof(Consumable.ConsumableType)) as Consumable.ConsumableType[];
    protected override void Awake()
    {
        base.Awake();

        Data = new List<ConsumableDataShop>();
    }
    protected override void Start()
    {
        base.Start();
        m_playerData = Singletons.Get<SaveManager>().GetData();
        consumableDatabase.Load();
        ChangeModelsAndReset();
    }

    public void ChangeModelsAndReset()
    {
        Data.Clear();
        int newCount = s_ConsumablesTypes.Length - 1;
        Data.Capacity = newCount;
        for (int i = 1; i < newCount; i++)
        {
            var model = CreateNewModel(i);
            Data.Add(model);
        }
        ResetItems(Data.Count);
    }
    ConsumableDataShop CreateNewModel(int i)
    {
        ConsumableDataShop consumable = new ConsumableDataShop();
        consumable.name = s_ConsumablesTypes[i];
        consumable.icon = ConsumableDatabase.GetConsumbale(s_ConsumablesTypes[i]).icon;
        consumable.priceCoin = ConsumableDatabase.GetConsumbale(s_ConsumablesTypes[i]).GetPrice();
        consumable.pricePremium = ConsumableDatabase.GetConsumbale(s_ConsumablesTypes[i]).GetPremiumCost();
        consumable.usedAmount = m_playerData.GetConsumableAmount(s_ConsumablesTypes[i]);
        return consumable;
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
        if (model.usedAmount != 0)
        {
            vh.usedAmount.gameObject.SetActive(true);
            vh.usedAmount.SetText("x" + model.usedAmount.ToString());
        }
        else
        {
            vh.usedAmount.gameObject.SetActive(false);
        }
        if (m_playerData.coins >= model.priceCoin && m_playerData.premium >= model.pricePremium)
        {
            vh.buyButton.onClick.AddListener(delegate { BuyItem(model.name,model.priceCoin,model.pricePremium); });
            vh.buyButton.gameObject.GetComponent<Image>().color = new Color(255, 255, 255, 255);

            vh.priceCoin.color = new Color(32, 32, 32);
            vh.pricePremium.color = new Color(32, 32, 32);
        }
        else
        {
            vh.buyButton.onClick.RemoveListener(delegate { BuyItem(model.name,model.priceCoin,model.pricePremium); });
            vh.buyButton.gameObject.GetComponent<Image>().color = new Color(32, 32, 32, 125);

            if(m_playerData.coins < model.priceCoin)
                vh.priceCoin.color = new Color(255, 0, 0);
            if(m_playerData.premium < model.pricePremium)
                vh.pricePremium.color = new Color(255, 0, 0);
        }
    }
    void BuyItem(Consumable.ConsumableType type, int coinPrice, int premiumPrice)
    {
        m_playerData.AddConsumable(type, 1);
        m_playerData.coins -= coinPrice;
        m_playerData.premium -= premiumPrice;
        ChangeModelsAndReset();
    }
}
public class ConsumableDataShop
{
    public Consumable.ConsumableType name;
    public Sprite icon;
    public int priceCoin;
    public int pricePremium;
    public int usedAmount;
}
public class ItemShopHolder : CAbstractViewsHolder
{
    public TextMeshProUGUI name;
    public Image icon;
    public TextMeshProUGUI priceCoin;
    public TextMeshProUGUI pricePremium;
    public TextMeshProUGUI usedAmount;
    public Button buyButton;
    public TextMeshProUGUI buyButtonTxt;
    public override void CollectViews()
    {
        base.CollectViews();
        name = root.Find("Info").Find("Name").GetComponent<TextMeshProUGUI>();
        icon = root.Find("Icon").GetComponent<Image>();
        priceCoin = root.Find("Info").Find("PriceButtonZone").Find("PriceZone").Find("PriceCoin").Find("Amount").GetComponent<TextMeshProUGUI>();
        pricePremium = root.Find("Info").Find("PriceButtonZone").Find("PriceZone").Find("PricePremium").Find("Amount").GetComponent<TextMeshProUGUI>();
        usedAmount = root.Find("Icon").Find("Amount").GetComponent<TextMeshProUGUI>();
        buyButton = root.Find("Info").Find("PriceButtonZone").Find("BuyButton").GetComponent<Button>();
        buyButtonTxt = root.Find("Info").Find("PriceButtonZone").Find("BuyButton").Find("Text").GetComponent<TextMeshProUGUI>();
    }
}
