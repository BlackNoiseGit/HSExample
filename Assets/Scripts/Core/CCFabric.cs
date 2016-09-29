using UnityEngine;
using System.Collections;
using SimpleJSON;
using UnityEngine.UI;

public class CCFabric
{
    private static ICCReader ConfigReader;

    static CCFabric()
    {
        ConfigReader = new ConfigReader();
    }

    public static GameObject CreateCard(string name, HeroType heroType)
    {
        MonoBehaviour.print(" ConfigReader " + ConfigReader);
        JSONClass ccinfo = ConfigReader.GetCCInfo(name, heroType);
        GameObject Card = MonoBehaviour.Instantiate(Resources.Load("Prefabs/BasePrefabs/BaseCard", typeof(GameObject))) as GameObject;
        Card.SetActive(false);
        
        string manaCost = ccinfo[ConstCCCfg.MANA_COST].Value;
        string attack = ccinfo[ConstCCCfg.ATTACK].Value;
        string hp = ccinfo[ConstCCCfg.HEALTH_POINTS].Value;
        Sprite portrait = Resources.Load<Sprite>(ccinfo[ConstCCCfg.TEXTURE_PATH].Value);
        CreatureType creatureType = (CreatureType)System.Enum.Parse(typeof(CreatureType), ccinfo[ConstCCCfg.CREATURE_TYPE].Value);

        BaseCard baseCard = Card.GetComponent<BaseCard>();
        baseCard.InitCard(manaCost, attack, hp, creatureType, portrait);

        return Card;
    }

}
