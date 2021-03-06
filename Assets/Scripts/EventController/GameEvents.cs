﻿using System.Collections.Generic;
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

    public class QteMissEvent : GameEvent
    {
        public bool enableinput;
        public int color; //0 Blue, 1 Red, 2 Yellow, 3 Green
    }

    public class AnimEvent : GameEvent
    {
        public bool playerturn;
        public bool camshake;
        public int animation;
        public bool dontshowUI;
        public bool animstate;
        public bool choosestate;
        public Vector3 modelposition;
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

    public class QuitGameEvent : GameEvent
    {
    }

    public class PayMoneyEvent : GameEvent
    {
        public int moneypaid;
    }

    //PONG EVENTS

    public class ScoreEvent : GameEvent
    {
        public int playerId;
        public int scoreData;
        public Vector3 playerPosition;
    }
    public class HitEvent : GameEvent
    {
        public int playerId;
    }
    public class WaitEvent : GameEvent
    {
        public bool waiting;
        public KeyCode startKey;
    }
    public class GameStartEvent : GameEvent
    {
        public KeyCode startKey;
        public int highscore;
    }
    public class GameOverPongEvent : GameEvent
    {
        public int playerId;
        public KeyCode reloadkey;
        public int p1score;
        public int p2score;
    }

    //Inventory Events

    public class UseItemEvent : GameEvent
    {
        public Item item;
    }

    public class ObtainItemEvent : GameEvent
    {
        public Item item;
        public GameObject rotatingItem;
        public bool showMessage;
    }

    public class LoadEnemyEvent : GameEvent
    {
        public Enemy_Template enemyTemplate;
        public GameObject enemyGameObject;
    }
}
