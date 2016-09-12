using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class PickController : MonoBehaviour 
{
    public System.Action<GameObject> CardPicked;

    #region Serialize Fields 
    [SerializeField]
    private List<GameObject> MyCreatures;
    [SerializeField]
    private List<GameObject> KilledCreatures;
    [SerializeField]
    private GameObject MyCreaturesHolder;
    [SerializeField]
    private GameObject KilledCreaturesHolder;
    [SerializeField]
    private GameObject UsedCardsHolder;

    [SerializeField]
    private Texture2D ScopeTex;
    #endregion

    #region Private Fields
    private PickState CurrentPickState;
    private GameObject PickedCardHolder = null;
    private GameObject CreatureHolder = null;

    private CreaturesLineMover LineMover;
    #endregion

    #region Const Fields
    private const int DELTA_TIME_AMOUNT = 50;
    private const float POSITION_CENTER_CREATURE_X = 0;
    private const float POSITION_CENTER_CREATURE_Y = -1.1f;
    private const float POSITION_CENTER_CREATURE_Z = -5;
    private const float POSITION_LEFT_STEP_CREATURE_X = 0.7f;
    #endregion

    private IInputCommands InputManager;

    enum PickState
    {
        Default,
        CardWasPicked,
        PickTarget
    }

	void Start () 
    {
        CurrentPickState = PickState.Default;

        MyCreatures = new List<GameObject>();
        KilledCreatures = new List<GameObject>();
        MyCreaturesHolder = new GameObject("MyCreaturesHolder");
        MyCreaturesHolder.transform.parent = gameObject.transform;
        KilledCreaturesHolder = new GameObject("KilledCreaturesHolder");
        KilledCreaturesHolder.transform.parent = gameObject.transform;
        UsedCardsHolder = new GameObject("UsedCardsHolder");
        UsedCardsHolder.transform.parent = gameObject.transform;

        LineMover = GetComponent<CreaturesLineMover>();
        if (LineMover == null)
            LineMover = gameObject.AddComponent<CreaturesLineMover>();

        Vector3 centerPos = new Vector3(POSITION_CENTER_CREATURE_X, POSITION_CENTER_CREATURE_Y, POSITION_CENTER_CREATURE_Z);
        LineMover.AnimationEnd += CreateCreature;
        LineMover.Init(MyCreatures, centerPos, POSITION_LEFT_STEP_CREATURE_X);

#if UNITY_STANDALONE_WIN
        InputManager = new PCInputManager();
#endif
	}

    void OnDestroy()
    {
        LineMover.AnimationEnd -= CreateCreature;
    }

	void Update () 
    {
        switch(CurrentPickState)
        {
            case PickState.Default :
                DefaultState();
                break;
            case PickState.CardWasPicked :
                CardWasPickedState();
                break;
            case PickState.PickTarget:
                PickTargetState();
                break;
        }
	
	}

    private void CreateCreature(GameObject card, Vector3 pos)
    {
        GameObject creature = card.GetComponent<BaseCard>().CreateCreatureAndPlaceOnDesk(pos);
        creature.transform.parent = MyCreaturesHolder.transform;
        MyCreatures.Add(creature);
        card.transform.parent = UsedCardsHolder.transform;
        card.SetActive(false);
        MainController.instance.EventsManager.RunEvent(BattleEvent.CreatureOnDesk);
    }

    private void DefaultState()
    {
        RaycastHit hitInfo;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hitInfo))
        {
            if (InputManager.CursorPressedDown())
            {
                if (hitInfo.transform.gameObject.tag == ConstTag.Card)
                {
                    PickedCardHolder = hitInfo.transform.gameObject;
                    if (CardPicked != null)
                        CardPicked(PickedCardHolder);

                    CurrentPickState = PickState.CardWasPicked;
                }
                else if (hitInfo.transform.gameObject.tag == ConstTag.Creature)
                {
                    Cursor.SetCursor(ScopeTex, new Vector2(ScopeTex.width / 2, ScopeTex.height/2), CursorMode.ForceSoftware);
                    CreatureHolder = hitInfo.transform.gameObject;
                    CurrentPickState = PickState.PickTarget;
                }
            }
        }

    }

    private void CardWasPickedState()
    {
        RaycastHit hitInfo;
        Vector3 cardPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        cardPos.z = -6;

        PickedCardHolder.transform.position = Vector3.Lerp(PickedCardHolder.transform.position, cardPos, Time.deltaTime * DELTA_TIME_AMOUNT);

        if (InputManager.CursorPressedUp())
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hitInfo))
            {
                if (hitInfo.transform.gameObject.tag == ConstTag.GameDesk)
                {
                    LineMover.MoveCardToBornPosition(PickedCardHolder);
                    CurrentPickState = PickState.Default;
                }
                else
                {
                    MainController.instance.DeckController.MoveCardToHand(PickedCardHolder);
                    CurrentPickState = PickState.Default;
                    PickedCardHolder = null;
                }
            }
       }
    }

    private void PickTargetState()
    {
       
    }
}
