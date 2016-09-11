using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class DeckController : MonoBehaviour 
{
    [SerializeField]
    List<GameObject> MyDeck;
    [SerializeField]
    List<GameObject> MyHand;
    [SerializeField]
    List<GameObject> MyDesk;

    [SerializeField]
    private GameObject MyDeckHolder;
    [SerializeField]
    private GameObject MyHandHolder;
    [SerializeField]
    private GameObject MyDeskHolder;



    [SerializeField]
    GameObject CardPrefab;

    [SerializeField]
    AnimationCurve ToHandCurve;

    [SerializeField]
    private float HandBorderX = 3;

    private const float BIG_CARD_SCALE_COEFF = 1.8f;

    private const float DURATION_TO_HAND_PATH = 0.45f;
    private const float DURATION_CARDS_STRIFE = 0.3f;

    private const float POSITION_CENTER_CARD_X = 0;
    private const float POSITION_CENTER_CARD_Y = -3.5f;
    private const float POSITION_CENTER_CARD_Z = -5;
    private const float POSITION_LEFT_STEP_CARD_X = 0.7f;

    void Start()
    {
        GenerateDeck(10);
        MainController.instance.PickController.CardPicked += RemoveCardFromHand;
    }

    [ContextMenu("MOVE")]
    private void Move()
    {
        TakeCardFromDeck();

    }

    public void TakeCardFromDeck()
    {
        GameObject card = MyDeck[0];

        card.transform.DOMoveX(card.transform.position.x - 3.5f, 1);
        MainController.instance.DelayService.DelayCallMethod(1.4f, () => MoveCardToHand(card));

    }

    public void MoveCardToHand(GameObject card)
    {
        Vector3 pos = Vector3.one;
        List<Vector3> cardsPositions = RecalculatePosInHand(MyHand.Count);
        pos = cardsPositions[cardsPositions.Count - 1];
        MoveCardToNewPosInHand(cardsPositions, cardsPositions.Count - 2);
        card.transform.DOMove(pos, DURATION_TO_HAND_PATH).SetEase(ToHandCurve);
        card.transform.DOScale(Vector3.one, DURATION_TO_HAND_PATH).SetEase(ToHandCurve).OnComplete(() => MoveCardToHandList(card));
        
        //CalculateNewPosInHand(ref pos);
        //card.transform.DOMove(pos, DURATION_TO_HAND_PATH).SetEase(ToHandCurve);
        //card.transform.DOScale(Vector3.one, DURATION_TO_HAND_PATH).SetEase(ToHandCurve).OnComplete(() => MoveCardToHandList(card));
    }



    private void RemoveCardFromHand(GameObject card)
    {
        MyHand.Remove(card);
        List<Vector3> cardsPositions = RecalculatePosInHand(MyHand.Count-1);
        MoveCardToNewPosInHand(cardsPositions, cardsPositions.Count - 1);
    }

      

    private void MoveCardToHandList(GameObject card)
    {
        MyHand.Add(card);
        if (MyDeck.Contains(card))
            MyDeck.Remove(card);
        card.transform.parent = MyHandHolder.transform;
    }

    private void GenerateDeck(int count)
    {
        for(int i = 0; i <= count; i++)
        {
            var height = Camera.main.orthographicSize;
            var width = height * Screen.width / Screen.height;
            Vector3 pos = new Vector3((float)width *1.2f, (float)height/2, -5);
            //Vector3 posSc = new Vector3(1.1f,0.75f, 0);
            //Vector3 pos = Camera.main.ViewportToWorldPoint(posSc);
            pos.z = -5;
            GameObject card = Instantiate(CardPrefab, pos, Quaternion.identity) as GameObject;
            card.name = "Card " + i;
            card.tag = ConstTag.Card;
            card.transform.parent = MyDeckHolder.transform;
            card.transform.localScale = BIG_CARD_SCALE_COEFF * Vector3.one;
            MyDeck.Add(card);
        }
    }

    private List<Vector3> RecalculatePosInHand(int cardsCount)
    {
        List<Vector3> newHandPositions = new List<Vector3>();

        if (MyHand.Count == 0)
        {
            newHandPositions.Add(new Vector3(POSITION_CENTER_CARD_X, POSITION_CENTER_CARD_Y, POSITION_CENTER_CARD_Z));
            return newHandPositions;
        }
        else
        {
            float cardsLen = cardsCount * POSITION_LEFT_STEP_CARD_X * 2;
            float step = 0;
            Vector3 leftBorderPos = Vector3.one;

            if (cardsLen < HandBorderX * 2)
            {
                leftBorderPos = MyHand[0].transform.position;
                leftBorderPos.x = POSITION_CENTER_CARD_X - cardsLen / 2;
                step = POSITION_LEFT_STEP_CARD_X * 2;
                for (int i = 0; i <= cardsCount; i++)
                    newHandPositions.Add(leftBorderPos + Vector3.right * step * i);
            }
            else
            {
                leftBorderPos = MyHand[0].transform.position;
                leftBorderPos.x = -HandBorderX;
                step = Mathf.Abs(leftBorderPos.x * 2 / cardsCount);
                for (int i = 0; i <= cardsCount; i++)
                    newHandPositions.Add(leftBorderPos + Vector3.right * step * i - Vector3.forward * 0.05f * i);
            }
        }

        return newHandPositions;
    }

    private void MoveCardToNewPosInHand(List<Vector3> newHandPositions)
    {
        for(int i = 0; i <= newHandPositions.Count - 2; i++)
        {
            MyHand[i].transform.DOMove(newHandPositions[i], DURATION_CARDS_STRIFE);
        }

    }

    private void MoveCardToNewPosInHand(List<Vector3> newHandPositions, int cardsCount)
    {
        if (MyHand.Count == 0)
            return;

        for (int i = 0; i <= cardsCount; i++)
            MyHand[i].transform.DOMove(newHandPositions[i], DURATION_CARDS_STRIFE);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            //TakeCardFromDeck();
        {
            int r = Random.Range(0, MyHand.Count - 1);
            MyHand[r].SetActive(false);
           // MyHand.Remove(MyHand[r]);
            RemoveCardFromHand(MyHand[r]);
        }

    }
}
