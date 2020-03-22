using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Events;
using UnityEngine.UI;
using TMPro;

public class UI_Controller_Map : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI tx_collectables;
    [SerializeField] private TextMeshProUGUI tx_picks;
    [SerializeField] private TextMeshProUGUI tx_heals;
    [SerializeField] private TextMeshProUGUI tx_buffs;
    [SerializeField] private TextMeshProUGUI tx_money;

    [SerializeField] private GameObject go_dialoguebox;
    [SerializeField] private TextMeshProUGUI tx_NPCIname;
    [SerializeField] private TextMeshProUGUI tx_NPCIdialogue;
    [SerializeField] private Animator anim_dialogue;

    [SerializeField] private GameObject go_shopbox;
    [SerializeField] private TextMeshProUGUI tx_Shopname;
    [SerializeField] private TextMeshProUGUI tx_Shopdialogue;
    [SerializeField] private Animator anim_shop;

    [SerializeField] private TextMeshProUGUI tx_shopbuffs;
    [SerializeField] private TextMeshProUGUI tx_shopheals;
    [SerializeField] private TextMeshProUGUI tx_shopmoney;

    private static float dialogue_seconds = 0.03f;

    private WaitForSecondsRealtime waitforseconds = new WaitForSecondsRealtime(dialogue_seconds);

    private int collectables;
    private int picks;
    private int heals;
    private int buffs;
    private int money;

    private Queue<string> sentences;

    private bool isshop = false;

    void Start()
    {
        sentences = new Queue<string>();
        go_dialoguebox.SetActive(false);


        ////Por el momento para testear
        //tx_money.text = Player_Status.Money.ToString();
        //money = Player_Status.Money;
        ////Player_Status.Money = money = 1000;

    }
    void Update()
    {

    }

    private void OnEnable() {
        EventController.AddListener<CollectEvent>(CollectEvent);
        EventController.AddListener<DialogueEvent>(DialogueEvent);
        EventController.AddListener<BeforeSceneUnloadEvent>(BeforeSceneUnloadEvent);
        EventController.AddListener<AfterSceneLoadEvent>(AfterSceneLoadEvent);
        EventController.AddListener<BuyEvent>(BuyEvent);

    }
    private void OnDisable() {
        EventController.RemoveListener<CollectEvent>(CollectEvent);
        EventController.RemoveListener<DialogueEvent>(DialogueEvent);
        EventController.RemoveListener<BeforeSceneUnloadEvent>(BeforeSceneUnloadEvent);
        EventController.RemoveListener<AfterSceneLoadEvent>(AfterSceneLoadEvent);
        EventController.RemoveListener<BuyEvent>(BuyEvent);
    }

    private void CollectEvent(CollectEvent collect)
    {
        collectables++;
        tx_collectables.text = $"{collectables}";
    }

    private void DialogueEvent(DialogueEvent dialogue)
    {
        //NPCI
        if (!dialogue.isshop)
        {
            isshop = false;
            if (dialogue.talking)
            {
                DisplayNextSentence();
            }
            else
            {
                Debug.Log($"Starting conversation with {dialogue.dialogue.name}");
                sentences.Clear();
                go_dialoguebox.SetActive(true);
                tx_NPCIname.text = dialogue.dialogue.name;

                anim_dialogue.SetBool("Open", true);

                foreach (string sentence in dialogue.dialogue.sentences)
                {
                    sentences.Enqueue(sentence);
                }
                DisplayNextSentence();
            }
        }
        //SHOP
        else
        {
            isshop = true;

            tx_shopmoney.text = money.ToString();
            tx_shopbuffs.text = buffs.ToString();
            tx_heals.text = heals.ToString();


            if (dialogue.talking)
            {
                DisplayNextSentence();
            }
            else
            {
                Debug.Log($"Starting conversation with {dialogue.dialogue.name}");
                sentences.Clear();
                go_shopbox.SetActive(true);
                tx_Shopname.text = dialogue.dialogue.name;

                anim_shop.SetBool("Open", true);

                foreach (string sentence in dialogue.dialogue.sentences)
                {
                    sentences.Enqueue(sentence);
                }
                DisplayNextSentence();
            }
        }
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        Debug.Log(sentence);
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
        //tx_NPCIdialogue.text = sentence;

    }

    public void EndDialogue()
    {
        Debug.Log("End of conversation");
        if(isshop)
        {
            anim_shop.SetBool("Open", false);
        }
        else
        {
            anim_dialogue.SetBool("Open", false);
        }
    }

    IEnumerator TypeSentence(string sentence)
    {

        if (isshop)
        {
            tx_Shopdialogue.text = "";
            foreach (char letter in sentence.ToCharArray())
            {
                tx_Shopdialogue.text += letter;
                yield return waitforseconds;
            }
        }
        else
        {
            tx_NPCIdialogue.text = "";
            foreach (char letter in sentence.ToCharArray())
            {
                tx_NPCIdialogue.text += letter;
                yield return waitforseconds;
            }
        }

    }

    private void BeforeSceneUnloadEvent(BeforeSceneUnloadEvent before)
    {

    }

    private void AfterSceneLoadEvent(AfterSceneLoadEvent after)
    {
        tx_collectables.text = Player_Status.Collectables.ToString();
        collectables = Player_Status.Collectables;

        tx_picks.text = Player_Status.Picks.ToString();
        picks = Player_Status.Picks;

        tx_heals.text = Player_Status.Heals.ToString();
        heals = Player_Status.Heals;

        tx_buffs.text = Player_Status.Buffs.ToString();
        buffs = Player_Status.Buffs;

        tx_money.text = Player_Status.Money.ToString();
        money = Player_Status.Money;
    }

    private void BuyEvent(BuyEvent buy)
    {
        if(buy.isheal)
        {
            money -= buy.price;
            heals++;
            tx_heals.text = heals.ToString();
            tx_shopheals.text = heals.ToString();
            tx_money.text = money.ToString();
            tx_shopmoney.text = money.ToString();

        }
        else
        {
            money -= buy.price;
            buffs++;
            tx_buffs.text = buffs.ToString();
            tx_shopbuffs.text = buffs.ToString();
            tx_money.text = money.ToString();
            tx_shopmoney.text = money.ToString();
        }
    }
}