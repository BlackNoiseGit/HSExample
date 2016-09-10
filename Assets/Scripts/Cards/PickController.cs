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

#if UNITY_STANDALONE_WIN
        InputManager = new PCInputManager();
#endif
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

    private List<Vector3> RecalculatePosInDesk(int cardsCount)
    {
        List<Vector3> newDeskPositions = new List<Vector3>();

        if (MyCreatures.Count == 0)
        {
            newDeskPositions.Add(new Vector3(POSITION_CENTER_CREATURE_X, POSITION_CENTER_CREATURE_Y, POSITION_CENTER_CREATURE_Z));
            return newDeskPositions;
        }
        else
        {
            float cardsLen = cardsCount * POSITION_LEFT_STEP_CREATURE_X * 2;
            float step = 0;
            Vector3 leftBorederPos = Vector3.one;
            leftBorederPos = MyCreatures[0].transform.position;
            leftBorederPos.x = POSITION_CENTER_CREATURE_X - cardsLen / 2;
            step = POSITION_LEFT_STEP_CREATURE_X * 2;
            for (int i = 0; i <= cardsCount; i++)
                newDeskPositions.Add(leftBorederPos + Vector3.right * step * i);
        }

        return newDeskPositions;
    }

    private void MoveCreatures(List<Vector3> newDeskPositions, int creaturesCount)
    {
        if (MyCreatures.Count == 0)
            return;

        for (int i = 0; i <= creaturesCount; i++)
            MyCreatures[i].transform.DOMove(newDeskPositions[i], 0.2f);
    }

    private void RemoveCreature(GameObject creature)
    {
        MyCreatures.Remove(creature);
        List<Vector3> cardsPositions = RecalculatePosInDesk(MyCreatures.Count - 1);
        MoveCreatures(cardsPositions, cardsPositions.Count - 1);
    }

    public void MoveCardToBornPosition(GameObject card)
    {
        Vector3 pos = Vector3.one;
        List<Vector3> creaturesPos = RecalculatePosInDesk(MyCreatures.Count);
        pos = creaturesPos[creaturesPos.Count - 1];
        MoveCreatures(creaturesPos, creaturesPos.Count - 2);
        card.transform.DOMove(pos, 0.4f).SetEase(Ease.Linear).OnComplete(() => CreateCreature(card, pos));
        //card.transform.DOScale(Vector3.one, DURATION_TO_HAND_PATH).SetEase(ToHandCurve).OnComplete(() => MoveCardToHandList(card));
    }

    private void CreateCreature(GameObject card, Vector3 pos)
    {
        GameObject creature = card.GetComponent<BaseCard>().CreateCreatureAndPlaceOnDesk(pos);
        creature.transform.parent = MyCreaturesHolder.transform;
        MyCreatures.Add(creature);
        card.transform.parent = UsedCardsHolder.transform;
        card.SetActive(false);
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
                    MoveCardToBornPosition(PickedCardHolder);
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
