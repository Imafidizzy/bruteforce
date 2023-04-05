using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Utility
{
    public class PlayerData
    {
        private string path = "./Assets/Resources/tmp/PlayerData.json";
        private Dictionary<string, Tool> tools = new Dictionary<String, Tool>();
        private Dictionary<int, Level> levels = new Dictionary<int, Level>();

        public void Load()
        {
            string rawData = Utility.IO.ReadString(path);
            PlayerDataModel data = JsonUtility.FromJson<PlayerDataModel>(rawData);
            foreach(Tool tool in data.Tools)
            {
                tools.Add(tool.Name, tool);
            }
            foreach(Level level in data.Levels)
            {
                levels.Add(level.Id, level);
            }
        }

        public void Save()
        {
            PlayerDataModel data = new PlayerDataModel(
                tools.Values.ToArray(),
                levels.Values.ToArray()
            );
            string rawData = JsonUtility.ToJson(data, true);
            Utility.IO.WriteString(path, rawData);
        }

        public Tool GetTool(string name)
        {
            return tools[name];
        }

        public void SetTool(string name, Tool data)
        {
            tools[name] = data;
            Save();
        }

        public int GetCompletedLevelCount()
        {
            int result = levels.Values.Sum(x => x.Completed ? 1 : 0);
            return result;
        }

        public void CompleteLevel(int id)
        {
            Level level = levels[id];
            level.Completed = true;
            levels[id] = level;
            Save();
        }
    }

    [Serializable]
    public struct Tool
    {
        public string Name;
        public bool Unlocked;
        public int ChargeCount;
    }

    [Serializable]
    public struct Level
    {
        public int Id;
        public bool Completed;
    }

    [Serializable]
    public struct PlayerDataModel
    {
        public Tool[] Tools;
        public Level[] Levels;

        public PlayerDataModel(Tool[] tools, Level[] levels) {
            this.Tools = tools;
            this.Levels = levels;
        }
    }
}
