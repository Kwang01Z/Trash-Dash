using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SPStudios.Tools;
[System.Serializable]
public class HighscoreEntry : System.IComparable<HighscoreEntry>
{
    public string name;
    public int score;
    public int CompareTo(HighscoreEntry other)
    {
        return other.score.CompareTo(score);
    }
}
[System.Serializable]
public class PlayerData
{
    public int coins = 0;
    public int premium = 0;

    public bool isTutorialDone = false;

    public Dictionary<Consumable.ConsumableType, int> consumables = new Dictionary<Consumable.ConsumableType, int>();   // Inventory of owned consumables and quantity.
    public Consumable.ConsumableType usedConsumable = Consumable.ConsumableType.NONE;
    public List<string> accessories = new List<string>();  // List of owned accessories, in the form "charName:accessoryName".
    public int usedAccessory = -1;
    public List<string> characters = new List<string>();    // Inventory of characters owned.
    public int usedCharacter = 0;                               // Currently equipped character.
    public List<string> themes = new List<string>();                // Owned themes.
    public int usedTheme = 0;                                   // Currently theme

    public List<HighscoreEntry> highscores = new List<HighscoreEntry>();
    public List<MissionBase> missions = new List<MissionBase>();
    public Dictionary<MissionBase.MissionType, int> missionDones = new Dictionary<MissionBase.MissionType, int>(); 
    public float masterVolume = 1f, 
                 musicVolume = 1f, 
                 masterSFXVolume = 1f;
    public static PlayerData CreateNew()
    {
        PlayerData _newData = new PlayerData();
        _newData.characters.Add("Trash Cat");
        _newData.themes.Add("Day");
        _newData.CheckMissionCount();
        _newData.coins = 10000;
        _newData.premium = 10;
        _newData.consumables.Add(Consumable.ConsumableType.EXTRALIFE, 1);
        /*_newData.accessories.Add("TrashCat:");*/
        return _newData;
    }
    public void CheckMissionCount()
    {
        while (missions.Count < 2)
        {
            AddMission();
        }
    }
    void AddMission()
    {
        int val = Random.Range(0, (int)MissionBase.MissionType.MULTIPLIER);
        while (ContainMissionType((MissionBase.MissionType)val))
        {
            val = Random.Range(0, (int)MissionBase.MissionType.MULTIPLIER);
        }
        MissionBase newMission = new MissionBase();
        newMission = newMission.GetMissionBaseFromType((MissionBase.MissionType)val, 1 + GetLevelMissionFromMissionType((MissionBase.MissionType)val));
        missions.Add(newMission);
    }
    bool ContainMissionType(MissionBase.MissionType type)
    {
        foreach (MissionBase mission in missions)
        {
            if (mission.missionType == type)
                return true;
        }
        return false;
    }
    int GetLevelMissionFromMissionType(MissionBase.MissionType a_missionType)
    {
        foreach (KeyValuePair<MissionBase.MissionType , int> missionLevelDone in missionDones)
        {
            if (missionLevelDone.Key == a_missionType)
                return missionLevelDone.Value;
        }
        return 0;
    }
    void ClaimMission(MissionBase a_mission)
    {
        premium += a_mission.reward;
        missions.Remove(a_mission);
        if(missionDones.ContainsKey(a_mission.missionType))
        {
            missionDones.Remove(a_mission.missionType);
        }
        missionDones.Add(a_mission.missionType, a_mission.level);
        CheckMissionCount();
        Singletons.Get<SaveManager>().Save();
    }
    public int GetConsumableAmount(Consumable.ConsumableType type)
    {
        foreach (KeyValuePair<Consumable.ConsumableType, int> consumable in consumables)
        {
            if (consumable.Key == type)
                return consumable.Value;
        }
        return 0;
    }
    public void AddHighScore(string a_name, int a_score)
    {
        HighscoreEntry highscore = new HighscoreEntry();
        highscore.name = a_name;
        highscore.score = a_score;
        highscores.Add(highscore);
    }
    public void AddConsumable(Consumable.ConsumableType a_type, int a_amount)
    {
        if (consumables.ContainsKey(a_type))
        {
            int amount = consumables[a_type];
            amount += a_amount;
            consumables.Remove(a_type);
            consumables.Add(a_type, amount);
        }
        else
        {
            consumables.Add(a_type, a_amount);
        }
    }
    public void AddCharacter(string name)
    {
        if(!characters.Contains(name))
        characters.Add(name);
    }
    public void AddAccessories(string charName, string accessoryName)
    {
        if (!accessories.Contains(charName + ":" + accessoryName))
        {
            accessories.Add(charName + ":" + accessoryName);
        }
    }
    public List<CharacterAccessories> GetAccessoriesListByChar(string charName)
    {
        List<CharacterAccessories> accessoryList = new List<CharacterAccessories>();
        Character c = CharacterDatabase.GetCharacter(charName);
        if (c != null)
        {
            for (int i = 0; i < c.accessories.Length; ++i)
            {
                // Check which accessories we own.
                string compoundName = c.characterName + ":" + c.accessories[i].accessoryName;
                if (accessories.Contains(compoundName))
                {
                    accessoryList.Add(c.accessories[i]);
                }
            }
        }
        return accessoryList;
    }
    public void StartRunMissions(TrackManager manager)
    {
        for (int i = 0; i < missions.Count; ++i)
        {
            missions[i].RunStart(manager);
        }
    }
    public void Add(Consumable.ConsumableType type)
    {
        if (!consumables.ContainsKey(type))
        {
            consumables[type] = 0;
        }

        consumables[type] += 1;
    }
}
