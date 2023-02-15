using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using SPStudios.Tools;
using UnityEditor;
public class SaveManager : ReRegisterMonoSingleton
{
    [SerializeField] PlayerData m_playerData = new PlayerData();

    protected override void OnInitOrAwake()
    {
        base.OnInitOrAwake();
        LoadOrCreateNewData();
        StartCoroutine(ThemeDatabase.LoadDatabase());
        StartCoroutine(CharacterDatabase.LoadDatabase());
    }
    public void Save()
    {
        Debug.Log("Saving!");
        FileStream file = new FileStream(Application.persistentDataPath + "/Player.dat", FileMode.OpenOrCreate);
        try
        {
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(file, m_playerData);
        }
        catch (SerializationException e)
        {
            Debug.LogError("Saving error: " + e);
        }
        finally
        {
            file.Close();
        }
    }
    public void LoadOrCreateNewData()
    {
        if (!File.Exists(Application.persistentDataPath + "/Player.dat"))
        {
            Debug.LogWarning("Do not Exits file save Player.dat");
            Debug.Log("Begin create new data");
            m_playerData = PlayerData.CreateNew();
        }
        else
        {
            Debug.Log("Loading data!");
            FileStream file = new FileStream(Application.persistentDataPath + "/Player.dat", FileMode.Open);
            try
            {
                BinaryFormatter formatter = new BinaryFormatter();
                m_playerData = (PlayerData)formatter.Deserialize(file);
            }
            catch (SerializationException e)
            {
                Debug.LogError("Loading data error: " + e);
            }
            finally
            {
                file.Close();
            }
        }  
    }
#if UNITY_EDITOR
    [MenuItem("Trash Dash Data Editor/Clear Save")]
    public static void ClearSave()
    {
        Debug.Log("Clear Save!");
        try
        {
            File.Delete(Application.persistentDataPath + "/Player.dat");
        }
        catch (SerializationException e)
        {
            Debug.LogError("Clear Save error: " + e);
        }
    }
#endif
    public static void ClearAllSave()
    {
        Debug.Log("Clear Save!");
        try
        {
            File.Delete(Application.persistentDataPath + "/Player.dat");
        }
        catch (SerializationException e)
        {
            Debug.LogError("Clear Save error: " + e);
        }
    }
    public PlayerData GetData()
    {
        return m_playerData;
    }
}
