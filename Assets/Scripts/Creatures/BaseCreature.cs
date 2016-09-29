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

    private bool AttackWasModified = false;
    private bool HealthWasModified = false;
    private int StartAttack = 0;
    private int StartHealth = 0;
    private int _ModifiedAttack = 0;
    private int _ModifiedHealth = 0;
    private int _CurrentAttack = 0;
    private int _CurrentHealth = 0;

    #region Public Fields
    public int ModifiedAttack
    {
        get { return _ModifiedAttack; }
        set
        {

            _ModifiedAttack = value;
            if (!AttackWasModified)
                _ModifiedAttack += StartAttack;
            
            AttackWasModified = true;
        }
    }

    public int ModifiedHealth
    {
        get { return _ModifiedHealth; }
        set
        {
            _ModifiedHealth = value;
            if (!HealthWasModified)
                _ModifiedHealth += StartHealth;
            HealthWasModified = true;
        }
    }


    public int CurrentAttack
    {
        get { return _CurrentAttack; }
        set
        { 
            _CurrentAttack = value;
            AttackText.text = _CurrentAttack.ToString();
            if (AttackWasModified)
            {
                if (_CurrentAttack > ModifiedAttack)
                    AttackText.color = Color.green;
                else if (_CurrentAttack == ModifiedAttack)
                    AttackText.color = Color.white;
            }
            if (_CurrentAttack > StartAttack)
                AttackText.color = Color.green;
        }
    }

    public int CurrentHealth
    {
        get { return _CurrentHealth; }
        set
        {
            if(HealthWasModified)
            {
                if (value > ModifiedHealth)
                    _CurrentHealth = ModifiedHealth;
                else
                    _CurrentHealth = value;

                if (_CurrentHealth == ModifiedHealth)
                    HealthPointsText.color = Color.green;
                else if (_CurrentHealth < ModifiedHealth)
                    HealthPointsText.color = Color.red;
            }
            else
            {
                if (value > StartHealth)
                    _CurrentHealth = StartHealth;
                else
                    _CurrentHealth = value;

                if (_CurrentHealth == StartHealth)
                    HealthPointsText.color = Color.white;
                else if (_CurrentHealth < StartHealth)
                    HealthPointsText.color = Color.red;
            }

            HealthPointsText.text = _CurrentHealth.ToString();
            if (_CurrentHealth <= 0)
                MakeDeath();
        }
    }
    #endregion


    // Use this for initialization
	void Start () 
    {
        //
	
	}
	
	// Update is called once per frame
	void Update () {

        if(Input.GetKeyDown(KeyCode.F1))
        {
            print(" ADD +3/+1");
            ModifiedAttack += 3;
            ModifiedHealth += 1;
            CurrentAttack += 3;
            CurrentHealth += 1;
        }

        if (Input.GetKeyDown(KeyCode.F2))
        {
            print(" -1 HP ");
            CurrentHealth--;
        }

        if (Input.GetKeyDown(KeyCode.F3))
        {
            print(" ADD +1/0 HP ");
            ModifiedHealth++;
            CurrentHealth++;
        }

        if (Input.GetKeyDown(KeyCode.F4))
        {
            print(" HEAL +1 HP ");
            CurrentHealth++;
        }
	
	}

    public void Init(int attack, int health, CreatureType creatureType, HeroType heroType, GameObject parentCard)
    {
        StartAttack = attack;
        StartHealth = health;
        CurrentAttack = attack;
        CurrentHealth = health;
        CreatureType = creatureType;
        HeroType = heroType;
        ParentCard = parentCard;
    }

    public void MakeDeath()
    {
        MainController.instance.EventsManager.RunEvent(BattleEvent.CreatureKilled);
        print(" UNIT DEATH =(");
    }
}
