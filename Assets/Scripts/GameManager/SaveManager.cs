using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
public class SaveManager : MonoBehaviour
{
    PlayerData m_playerData;
    public void Save()
    {
        Debug.Log("Saving!");
        FileStream file = new FileStream(Application.persistentDataPath + "/Player.dat",FileMode.OpenOrCreate);
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
    public void Load()
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
    public void InsertData(PlayerData data)
    {
        m_playerData = data;
        Save();
    }
    public void ClearSave()
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
}
