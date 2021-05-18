using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


public static class GameManager
{
    public static void SaveGameState(SaveDataGameState playerData)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/data.save";
        FileStream stream = new FileStream(path, FileMode.Create);

        SaveDataGameState data = new SaveDataGameState(playerData.currentLevel,
        playerData.powerUpArrows, playerData.powerUpBombs, playerData.powerUpMultiBombs);

        formatter.Serialize(stream, data);
        stream.Close();
    }
    public static SaveDataGameState LoadData()
    {
        string path = Application.persistentDataPath + "/data.save";
        if(File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            SaveDataGameState data = formatter.Deserialize(stream) as SaveDataGameState;
            stream.Close();

            return data;
        }
        else{
            Debug.Log("Save file not found in " + path);
            SaveDataGameState data = new SaveDataGameState(0, 0, 0, 0);
            SaveGameState(data);
            return data;
        }
    }
}
