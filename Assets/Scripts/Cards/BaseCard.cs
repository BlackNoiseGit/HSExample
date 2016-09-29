using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public enum HeroType
{
    Hunter,
    Common
}

public enum CreatureType
{
    Common,
    Pirate,
    Beast,
    Dragon,
    Demon
}


public class BaseCard : MonoBehaviour 
{
    [SerializeField]
    private GameObject CreaturePrefab;

    [SerializeField]
    private Image Portrait;

    [SerializeField]
    private string CardName;
    
    [Header("Card UI properties")]
    [SerializeField]
    private int ManaCost;
    [SerializeField]
    private int Attack;
    [SerializeField]
    private int HealthPoints;

    [Header("Card and creature types")]
    [SerializeField]
    private HeroType HeroType;
    [SerializeField]
    private CreatureType CreatureType;



    [Header("Card UI properties refs")]
    [SerializeField]
    private TextMeshPro ManaCostText;
    [SerializeField]
    private TextMeshPro AttackText;
    [SerializeField]
    private TextMeshPro HealthPointsText;
    [SerializeField]
    private TextMeshPro CreatureTypeText;


	void Start () 
    {

	}

    public void InitCard(string manaCost, string attack, string healthPoints, CreatureType creatureType, Sprite portrait)
    {
        CreatureType = creatureType;
        ManaCostText.text = manaCost;
        AttackText.text = attack;
        HealthPointsText.text = healthPoints;
        CreatureTypeText.text = CreatureType.ToString();
        Portrait.sprite = portrait;
    }

    public GameObject CreateCreatureAndPlaceOnDesk(Vector3 pos)
    {
        GameObject creature = Instantiate(CreaturePrefab, pos, Quaternion.identity) as GameObject;
        creature.GetComponent<BaseCreature>().Init(Attack, HealthPoints, CreatureType, HeroType, gameObject);
        return creature;
    }

}
