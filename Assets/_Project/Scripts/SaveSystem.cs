using UnityEngine;
using System.IO;
using Managers;

public class SaveSystem
{
    static SaveData _saveData;

    public struct SaveData
    {
        public ScoreData ScoreData;
    }

    static string SaveFileName()
    {
        var saveFile = Application.persistentDataPath + "/save" + ".save";
        return saveFile;
    }

    public static void SaveGame()
    {
        HandleSaveData();

        File.WriteAllText(SaveFileName(), JsonUtility.ToJson(_saveData, true));
    }

    public static void HandleSaveData()
    {
        ScoreManager.Instance.Save(ref _saveData.ScoreData);
    }

    public static void LoadGame()
    {
        if (!File.Exists(SaveFileName())) return;
        var saveString = File.ReadAllText(SaveFileName());
        _saveData = JsonUtility.FromJson<SaveData>(saveString);

        HandleLoadData();
    }

    static void HandleLoadData()
    {
        ScoreManager.Instance.Load(_saveData.ScoreData);
    }
}