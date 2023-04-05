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

        public void Load()
        {
            string rawData = Utility.IO.ReadString(path);
            Tool[] data = Utility.JSON.FromJson<Tool>(rawData);
            foreach(Tool tool in data)
            {
                tools.Add(tool.Name, tool);
            }
        }

        public void Save()
        {
            List<Tool> data = tools.Values.ToList();
            string rawData = Utility.JSON.ToJson<Tool>(data.ToArray(), true);
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
    }

    [Serializable]
    public class Tool
    {
        public string Name;
        public bool Unlocked;
        public int ChargeCount;
    }
}
