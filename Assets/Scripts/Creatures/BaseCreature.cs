using UnityEngine;
using TMPro;
using System.Collections;

public class BaseCreature : MonoBehaviour 
{
    [SerializeField]
    private TextMeshPro AttackText;
    [SerializeField]
    private TextMeshPro HealthPointsText;

    private CreatureType CreatureType;
    private HeroType HeroType;

    private GameObject ParentCard;

	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Init(int attack, int health, CreatureType creatureType, HeroType heroType, GameObject parentCard)
    {
        AttackText.text = attack.ToString();
        HealthPointsText.text = health.ToString();
        CreatureType = creatureType;
        HeroType = heroType;
        ParentCard = parentCard;
    }
}
