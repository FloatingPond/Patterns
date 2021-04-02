using System.IO;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveSystem : MonoBehaviour
{
    // Start is called before the first frame update
    public static void SaveGame(GameManager gm)
    {
        BinaryFormatter bf = new BinaryFormatter();
        string path = Application.persistentDataPath + "/dev03Patt.ern";
        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerData data = new PlayerData(gm);

        bf.Serialize(stream, data);
        stream.Close();

    }
    public static PlayerData LoadGame()
    {
        string path = Application.persistentDataPath + "/dev03Patt.ern";
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
}
