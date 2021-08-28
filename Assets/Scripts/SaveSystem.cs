using System.IO;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveSystem : MonoBehaviour
{
    // Start is called before the first frame update

    public static string filepath = Application.persistentDataPath + "/dev09Patt.ern";

    public static void SaveGame(GameManager gm)
    {
        BinaryFormatter bf = new BinaryFormatter();
        string path = filepath;
        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerData data = new PlayerData(gm);

        bf.Serialize(stream, data);
        stream.Close();

    }
    public static PlayerData LoadGame()
    {
        string path = filepath;
        if (File.Exists(path))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            PlayerData data = bf.Deserialize(stream) as PlayerData;
            stream.Close();
            return data;

        }
        else
        {
            Debug.LogError("No File found at " + path);
            return null;
        }
    }
    //Streak
    public static int GetStreak()
    {
        PlayerData pd = LoadGame();

        if (pd != null)
        {
            return pd.gameStreak;
        }
        else
        {
            return 0;
        }

    }
}
