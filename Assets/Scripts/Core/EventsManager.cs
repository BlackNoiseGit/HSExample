using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public enum BattleEvent
{
    StartTurn,
    EndTurn,
    CreatureOnDesk,
    CreatureEndTurn,
    CratureReceivedDamage,
    CreatureWasHealed
}

public class EventsManager 
{
    private Dictionary<BattleEvent, Action> BattleEvents;
    private Action EStart = null;
    private Action EEnd = null;
    private Action ECreatureOnDesk = null;
    private Action ECreatureEndTurn = null;
    private Action ECratureReceivedDamage = null;
    private Action ECreatureWasHealed = null;

    private Action EndEvent = null;

    public EventsManager()
    {
        BattleEvents = new Dictionary<BattleEvent, Action>();
        BattleEvents.Add(BattleEvent.StartTurn, EStart);
        BattleEvents.Add(BattleEvent.EndTurn, EEnd);
        BattleEvents.Add(BattleEvent.CreatureOnDesk, ECreatureOnDesk);
        BattleEvents.Add(BattleEvent.CreatureEndTurn, ECreatureEndTurn);
        BattleEvents.Add(BattleEvent.CratureReceivedDamage, ECratureReceivedDamage);
        BattleEvents.Add(BattleEvent.CreatureWasHealed, ECreatureWasHealed);
    }

    public void Subscribe(BattleEvent batteEvent, Action callback)
    {
        BattleEvents[batteEvent] += callback;
    }

    public void RunEvent(BattleEvent bEvent)
    {
        Action battleEvent = BattleEvents[bEvent];
        if (battleEvent != null)
            battleEvent();
    }

}
