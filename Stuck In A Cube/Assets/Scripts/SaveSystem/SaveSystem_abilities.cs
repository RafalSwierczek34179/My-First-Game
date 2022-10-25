using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem_abilities
{

    //This is all from Brackeys Youtube video called "SAVE & LOAD SYSTEM in Unity"

    public static void SaveAbility(SA_Menu_Buttons _ability)
    {

        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/Ability.selected";
        FileStream stream = new FileStream(path, FileMode.Create);
        AbilityData data = new AbilityData(_ability);

        formatter.Serialize(stream, data);
        stream.Close();

    }

    public static AbilityData LoadAbility()
    {

        string path = Application.persistentDataPath + "/Ability.selected";
        if (File.Exists(path))
        {

            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            AbilityData data = formatter.Deserialize(stream) as AbilityData;
            stream.Close();

            return data;

        }
        else
        {
            return null;
        }

    }

}
