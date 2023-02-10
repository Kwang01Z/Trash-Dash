using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using frame8.ScrollRectItemsAdapter.Classic;
using TMPro;
using SPStudios.Tools;
public class LeaderBoardScrollView : ClassicSRIA<HightScoreHolder>
{
    [SerializeField] RectTransform itemPrefab;
    [SerializeField] List<HightScore> Data;
    PlayerData m_playerData;
    protected override void Awake()
    {
        base.Awake();

        Data = new List<HightScore>();
    }
    protected override void Start()
    {
        base.Start();
        m_playerData = Singletons.Get<SaveManager>().GetData();
        ChangeModelsAndReset(m_playerData.highscores.Count);
    }

    private void ChangeModelsAndReset(int newCount)
    {
        Data.Clear();
        Data.Capacity = newCount;
        for (int i = 0; i < newCount; i++)
        {
            var model = CreateNewModel(i);
            Data.Add(model);
        }
        ResetItems(Data.Count);
    }
    HightScore CreateNewModel(int i)
    {
        HightScore hightScore = new HightScore();
        hightScore.index = i + 1;
        hightScore.name = m_playerData.highscores[i].name;
        hightScore.score = m_playerData.highscores[i].score;
        return hightScore;
    }

    protected override HightScoreHolder CreateViewsHolder(int itemIndex)
    {
        var instance = new HightScoreHolder();
        instance.Init(itemPrefab, itemIndex);

        return instance;
    }

    protected override void UpdateViewsHolder(HightScoreHolder vh)
    {
        var model = Data[vh.ItemIndex];
        vh.index.SetText(model.index.ToString());
        vh.name.SetText(model.name);
        vh.score.SetText(model.score.ToString());
    }
}
public class HightScore
{
    public int index;
    public string name;
    public int score;
}
public class HightScoreHolder : CAbstractViewsHolder
{
    public TextMeshProUGUI index;
    public TextMeshProUGUI name;
    public TextMeshProUGUI score;

    public override void CollectViews()
    {
        base.CollectViews();
        index = root.Find("Number").GetComponent<TextMeshProUGUI>();
        name = root.Find("Name").GetComponent<TextMeshProUGUI>();
        score = root.Find("Score").GetComponent<TextMeshProUGUI>();
    }
}
