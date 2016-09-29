using UnityEngine;
using System.Collections;
using SimpleJSON;
using System.IO;

public class ConfigReader : ICCReader 
{
    private string CreaturesConfigPath = Application.dataPath + "/Configs/CreaturesConfig.json";

    private string GetCreatureConfigAsStr()
    {
        return File.ReadAllText(CreaturesConfigPath);
    }

    public JSONClass GetCCInfo(string name, HeroType heroType)
    {
        JSONNode config = JSON.Parse(GetCreatureConfigAsStr());
        JSONArray heroCCInfo = config[heroType.ToString()].AsArray;

        if (heroCCInfo == null)
            return null;

        for (int i = 0; i <= heroCCInfo.Count - 1; i++)
        {
            if (heroCCInfo[i]["Name"].Value == name)
                return heroCCInfo[i].AsObject;
        }

        return null;
    }
}
