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
    [SerializeField] private GameObject go_dialoguebox;
    [SerializeField] private TextMeshProUGUI tx_NPCIname;
    [SerializeField] private TextMeshProUGUI tx_NPCIdialogue;
    [SerializeField] private Animator anim_dialogue;

    private static float dialogue_seconds = 0.03f;

    private WaitForSecondsRealtime waitforseconds = new WaitForSecondsRealtime(dialogue_seconds);

    private int collectables;
    private Queue<string> sentences;



    void Start()
    {
        sentences = new Queue<string>();
        go_dialoguebox.SetActive(false);

    }
    void Update()
    {

    }

    private void OnEnable() {
        EventController.AddListener<CollectEvent>(CollectEvent);
        EventController.AddListener<DialogueEvent>(DialogueEvent);

    }
    private void OnDisable() {
        EventController.RemoveListener<CollectEvent>(CollectEvent);
        EventController.RemoveListener<DialogueEvent>(DialogueEvent);

    }

    private void CollectEvent(CollectEvent collect)
    {
        collectables++;
        tx_collectables.text = $"{collectables}";
    }

    private void DialogueEvent(DialogueEvent dialogue)
    {

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
        anim_dialogue.SetBool("Open", false);
    }

    IEnumerator TypeSentence(string sentence)
    {
        tx_NPCIdialogue.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            tx_NPCIdialogue.text += letter;
            yield return waitforseconds;
        }
    }
}