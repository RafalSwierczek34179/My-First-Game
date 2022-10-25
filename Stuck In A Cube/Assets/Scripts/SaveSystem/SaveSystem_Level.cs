using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem_Level
{
    //This is all from Brackeys Youtube video called "SAVE & LOAD SYSTEM in Unity"

    public static void SaveLevel(LvlManager LevelManager)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/Level.something";
        FileStream stream = new FileStream(path, FileMode.Create);
        LevelData data = new LevelData(LevelManager);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static LevelData LoadLevel()
    {
        string path = Application.persistentDataPath + "/Level.something";
        if (File.Exists(path))
        {

            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            LevelData data = formatter.Deserialize(stream) as LevelData;
            stream.Close();

            return data;

        }
        else
        {
            return null;
        }
    }

}
