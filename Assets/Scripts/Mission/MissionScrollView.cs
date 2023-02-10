using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using frame8.ScrollRectItemsAdapter.Classic;
using TMPro;
using SPStudios.Tools;
using System;

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
        vh.progress.SetText(model.progress + "/" + model.max);
        vh.reward.SetText(model.reward.ToString());
    }
}
public class MissionHolder : CAbstractViewsHolder
{
    public TextMeshProUGUI desc;
    public TextMeshProUGUI progress;
    public TextMeshProUGUI reward;
    public override void CollectViews()
    {
        base.CollectViews();
        desc = root.Find("Image").Find("Desc").GetComponent<TextMeshProUGUI>();
        progress = root.Find("Image").Find("Reward").Find("Progress").GetComponent<TextMeshProUGUI>();
        reward = root.Find("Image").Find("Reward").Find("RewardTxt").GetComponent<TextMeshProUGUI>();
    }
}
