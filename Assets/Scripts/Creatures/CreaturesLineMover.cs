using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class CreaturesLineMover : MonoBehaviour 
{
    public System.Action<GameObject, Vector3> AnimationEnd;

    private List<GameObject> OwnerDeck;
    private Vector3 CenterCreaturePosition;
    private float LeftStep;

    private const float DURATION_TO_DESK_MOVE = 0.4f;
    private const float DURATION_LEFT_CREATURE_MOVE = 0.4f;

    public void Init(List<GameObject> ownerDeck, Vector3 centerCreaturePosition, float leftStep)
    {
        OwnerDeck = ownerDeck;
        CenterCreaturePosition = centerCreaturePosition;
        LeftStep = leftStep;
    }

    private List<Vector3> RecalculatePosInDesk(int cardsCount)
    {
        List<Vector3> newDeskPositions = new List<Vector3>();

        if (OwnerDeck.Count == 0)
        {
            newDeskPositions.Add(CenterCreaturePosition);
            return newDeskPositions;
        }
        else
        {
            float cardsLen = cardsCount * LeftStep * 2;
            float step = 0;
            Vector3 leftBorederPos = Vector3.one;
            leftBorederPos = OwnerDeck[0].transform.position;
            leftBorederPos.x = CenterCreaturePosition.x - cardsLen / 2;
            step = LeftStep * 2;
            for (int i = 0; i <= cardsCount; i++)
                newDeskPositions.Add(leftBorederPos + Vector3.right * step * i);
        }

        return newDeskPositions;
    }

    private void MoveCreatures(List<Vector3> newDeskPositions, int creaturesCount)
    {
        if (OwnerDeck.Count == 0)
            return;

        for (int i = 0; i <= creaturesCount; i++)
            OwnerDeck[i].transform.DOMove(newDeskPositions[i], DURATION_LEFT_CREATURE_MOVE).SetEase(Ease.OutCubic);
    }

    private void RemoveCreature(GameObject creature)
    {
        OwnerDeck.Remove(creature);
        List<Vector3> cardsPositions = RecalculatePosInDesk(OwnerDeck.Count - 1);
        MoveCreatures(cardsPositions, cardsPositions.Count - 1);
    }

    public void MoveCardToBornPosition(GameObject card)
    {
        Vector3 pos = Vector3.one;
        List<Vector3> creaturesPos = RecalculatePosInDesk(OwnerDeck.Count);
        pos = creaturesPos[creaturesPos.Count - 1];
        MoveCreatures(creaturesPos, creaturesPos.Count - 2);
        card.transform.DOMove(pos, DURATION_TO_DESK_MOVE).SetEase(Ease.Linear).OnComplete(() => OnAnimationEnd(card, pos));
        //card.transform.DOScale(Vector3.one, DURATION_TO_HAND_PATH).SetEase(ToHandCurve).OnComplete(() => MoveCardToHandList(card));
    }

    public void OnAnimationEnd(GameObject card, Vector3 pos)
    {
        if (AnimationEnd != null)
            AnimationEnd(card, pos);
    }

}
