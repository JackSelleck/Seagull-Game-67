using System.IO;
using UnityEngine;

namespace Scripts.SaveSystem
{ 
    public class SaveSystem 
    {
        private readonly static string path = Application.persistentDataPath + "/savedata.json";
    
        public static void Save(GameState state)
        {
            string json = JsonUtility.ToJson(state, true);
            File.WriteAllText(path, json);
        }
    
        public static GameState Load()
        {
            if (!File.Exists(path))
            {
                return new GameState();
            }
    
            string json = File.ReadAllText(path);
            return JsonUtility.FromJson<GameState>(json);
        }    
    }
}