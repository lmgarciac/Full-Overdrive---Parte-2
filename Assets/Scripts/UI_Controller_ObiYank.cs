using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Events;
using UnityEngine.UI;
using TMPro;

public class UI_Controller_ObiYank : MonoBehaviour
{
    [SerializeField] private GameObject go_dialoguebox;
    [SerializeField] private TextMeshProUGUI tx_NPCIname;
    [SerializeField] private TextMeshProUGUI tx_NPCIdialogue;
    [SerializeField] private Image im_NPCimage;

    [SerializeField] private Animator anim_dialogue;

    private static float dialogue_seconds = 0.03f;

    private WaitForSecondsRealtime waitforseconds = new WaitForSecondsRealtime(dialogue_seconds);
    private Queue<string> sentences;

    private bool isshop = false;

    void Start()
    {
        sentences = new Queue<string>();
        go_dialoguebox.SetActive(false);
    }
    void Update()
    {

    }

    private void OnEnable()
    {
        EventController.AddListener<DialogueEvent>(DialogueEvent);
        EventController.AddListener<BeforeSceneUnloadEvent>(BeforeSceneUnloadEvent);
        EventController.AddListener<AfterSceneLoadEvent>(AfterSceneLoadEvent);

    }
    private void OnDisable()
    {
        EventController.RemoveListener<DialogueEvent>(DialogueEvent);
        EventController.RemoveListener<BeforeSceneUnloadEvent>(BeforeSceneUnloadEvent);
        EventController.RemoveListener<AfterSceneLoadEvent>(AfterSceneLoadEvent);
    }

    private void DialogueEvent(DialogueEvent dialogue)
    {
        //NPCI
        if (!dialogue.isshop)
        {
            Debug.Log("Entro!!");
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
                tx_NPCIname.text = dialogue.dialogue.name.ToUpper();
                im_NPCimage.sprite = dialogue.dialogue.image;

                anim_dialogue.SetBool("Open", true);

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
        sentence = sentence.ToUpper();
        Debug.Log(sentence);
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
        //tx_NPCIdialogue.text = sentence;

    }

    public void EndDialogue()
    {
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

    private void BeforeSceneUnloadEvent(BeforeSceneUnloadEvent before)
    {

    }

    private void AfterSceneLoadEvent(AfterSceneLoadEvent after)
    {

    }

}