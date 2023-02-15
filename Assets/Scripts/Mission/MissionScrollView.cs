using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using frame8.ScrollRectItemsAdapter.Classic;
using TMPro;
using SPStudios.Tools;
using System;
using UnityEngine.UI;

public class MissionScrollView : ClassicSRIA<MissionHolder>
{
    [SerializeField] RectTransform itemPrefab;
    [SerializeField] List<MissionBase> Data;
    PlayerData m_playerData;
    protected override void Awake()
    {
        base.Awake();

        Data = new List<MissionBase>();
    }
    protected override void Start()
    {
        base.Start();
        m_playerData = Singletons.Get<SaveManager>().GetData();
        ChangeModelsAndReset(m_playerData.missions.Count);
    }

    private void ChangeModelsAndReset(int count)
    {
        Data.Clear();
        Data.Capacity = count;
        for (int i = 0; i < count; i++)
        {
            MissionBase model = CreateNewModel(i);
            Data.Add(model);
        }
        ResetItems(Data.Count);
    }

    private MissionBase CreateNewModel(int i)
    {
        MissionBase mission = m_playerData.missions[i];
        return mission;
    }

    protected override MissionHolder CreateViewsHolder(int itemIndex)
    {
        var instance = new MissionHolder();
        instance.Init(itemPrefab, itemIndex);

        return instance;
    }

    protected override void UpdateViewsHolder(MissionHolder vh)
    {
        var model = Data[vh.ItemIndex];
        vh.desc.SetText(model.desc);
        vh.progress.SetText(Math.Floor(model.progress) + "/" + model.max);
        vh.reward.SetText(model.reward.ToString());
        if (model.progress >= model.max)
        {
            vh.claimButton.onClick.AddListener(delegate { ClaimReward(model); });
            vh.claimButton.image.color = new Color(1, 1, 1, 1);
            
        }
        else
        {
            vh.claimButton.onClick.RemoveListener(delegate { ClaimReward(model); });
            vh.claimButton.image.color = new Color(0.5f,0.5f,0.5f,0.5f);
        }
        
    }
    void ClaimReward(MissionBase mission)
    {
        m_playerData.ClaimMission(mission);
        m_playerData.CheckMissionCount();
        ChangeModelsAndReset(m_playerData.missions.Count);
    }
}
public class MissionHolder : CAbstractViewsHolder
{
    public TextMeshProUGUI desc;
    public TextMeshProUGUI progress;
    public TextMeshProUGUI reward;
    public Button claimButton;
    public override void CollectViews()
    {
        base.CollectViews();
        desc = root.Find("Image").Find("Desc").GetComponent<TextMeshProUGUI>();
        progress = root.Find("Image").Find("Reward").Find("Progress").GetComponent<TextMeshProUGUI>();
        reward = root.Find("Image").Find("Reward").Find("RewardTxt").GetComponent<TextMeshProUGUI>();
        claimButton = root.Find("Image").Find("ClaimButton").GetComponent<Button>();
    }
}
