using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveProgress
{
    public static void SaveGameProgress(DataStorage dataStorage)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Path.GetFullPath("./") + "/saveProggess.save1";
        FileStream stream = new FileStream(path, FileMode.Create);

        ProgressData data = new ProgressData(dataStorage);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static ProgressData LoadProgress()
    {
        string path = Path.GetFullPath("./") + "/saveProggess.save1";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            ProgressData data = formatter.Deserialize(stream) as ProgressData;
            stream.Close();

            return data;
        }
        else
        {
            Debug.LogError("Not found save file");
            return null;
        }
    }

}
