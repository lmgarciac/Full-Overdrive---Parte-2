using System.Collections.Generic;
using UnityEngine;

namespace Events
{
    public class GameEvent { }

    public enum countertype
    {
        inittimer = 0,
        sessiontimer = 1,
    }
    public enum characterid
    {
        player = 0,
        enemy = 1,
    }

    public enum animation
    {
        none = 0,
        idle = 1,
        play = 2,
        special = 3,
    }


    public class StartTimerEvent : GameEvent { 
        public float totalseconds;
        public int countertype;
   }
   public class UpdateTimerEvent : GameEvent { 
        public float currentseconds;
        public int countertype;
    }
   public class FinishTimerEvent : GameEvent { 
        public int countertype;
    }
   public class CounterStatusEvent : GameEvent { 
        public int counterstatus;
    }
    public class BattleStartedEvent : GameEvent
    {
        public int playerhp;
        public int playersp;
        public int playerinitsp;
        public int playerinithp;
        public int enemyhp;
        public int enemysp;
        public int enemyinitsp;
        public int enemyinithp;
        public int buffqty;
        public int healqty;
        public int specialcost;
    }

    public class EnableTurnEvent : GameEvent {
       public int characterid;
       public int turnstate;
    }

    public class ActionEvent : GameEvent
    {
        public int action; //0 attack
                           //1 defend
                           //2 special
                           //3 item
                           //4 buff
                           //5 heal
        public int damage;
        public int item; //0 buff
                         //1 heal 
        public float buff;
        public int characterid;
    }

    public class SelectActionEvent : GameEvent
    {
        public int characterid;
        public int action; //0 attack
                           //1 defend
                           //2 special
                           //3 item
    }

    public class GameOverEvent : GameEvent {
        public bool playerwin;
     }
    public class QteHitEvent : GameEvent {
        public bool success;
        public int color; //0 Blue, 1 Red, 2 Yellow, 3 Green
     }
    public class QteLeaveEvent : GameEvent
    {
        public int qtenoteleave;
    }
    public class QtePrizeEvent : GameEvent {
        public int prizeSP;
        public float prizeMultiplier;
        public int prizeHP;
        public int prizeDamage;
        public bool playerturn;
        public float effic;
     }
    public class QtePlayEvent : GameEvent {
        public int noteamount;
     }

    public class AnimEvent : GameEvent
    {
        public bool playerturn;
        public bool camshake;
        public int animation;
        public bool dontshowUI;
    }


    ///////////////WORLD MAP EVENTS//////////////////
    public class CollectEvent : GameEvent
    {

    }

    public class DialogueEvent : GameEvent
    {
        public bool talking;
        public Dialogue dialogue;
        public bool isshop;
    }

    public class DialogueStatusEvent : GameEvent
    {
        public bool dialogueactive;
    }

    public class BuyEvent : GameEvent
    {
        public bool isheal;
        public int price;
    }

    public class BeforeSceneUnloadEvent : GameEvent
    {

    }

    public class AfterSceneLoadEvent : GameEvent
    {

    }

    public class ExpandBoundariesEvent : GameEvent
    {
        public int currentarea;
    }
}
