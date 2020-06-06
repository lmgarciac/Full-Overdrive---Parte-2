using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Events;

public class QTE_Main_Controller : MonoBehaviour
{
    public bool licktriggered;
    private int qteleavecounter;
    private readonly QtePlayEvent ev_qteplay = new QtePlayEvent();
    private readonly QteLeaveEvent ev_qteleave = new QteLeaveEvent();
    public Qte_Template QteSo;
    public string manualQte = null;

    private int noteindex;
    private float noteDelay;
    private float notePosition;
    private float arrowsPosition;

    private int noteAmount;

    private static GameObject guitarPrefab;
    private static GameObject noteUp;
    private static GameObject noteDown;
    private static GameObject noteLeft;
    private static GameObject noteRight;
    private GameObject[] notesArray;
    private Vector3 noteinitialPosition;
    private Vector3 guitarinitialPosition;


    void Start()
    {
        //Para poder testear el QTE por si solo.
        if (manualQte != "")
        {
            InitializeQte(manualQte);
        }
    }

    void Update()
    {
        if (noteAmount != 0)
        {
            for (noteindex = 0; noteindex < noteAmount; noteindex++)
            {
                notesArray[noteindex].transform.localPosition -= new Vector3(0f, QteSo.speed * Time.deltaTime, 0f);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag.StartsWith("Arrow") && !licktriggered)
        {
            //Debug.Log("Entro!");
            ev_qteplay.noteamount = noteAmount;
            EventController.TriggerEvent(ev_qteplay);//Debería controlarse que sea el primer Arrow y el resto no        
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag.StartsWith("Arrow") && !licktriggered)
        {
            qteleavecounter++;
            //Debug.Log($"{qteleavecounter} / {noteAmount}");
            ev_qteleave.qtenoteleave++;
            EventController.TriggerEvent(ev_qteleave);//Debería controlarse que sea el primer Arrow y el resto no        
        }
    }

    public void InitializeQte(string qtename)
    {
        //if(noteAmount!= 0)
        //{
        //    return;
        //}

        guitarPrefab = (GameObject)Resources.Load("Models/Electric_Guitar_Model");
        noteUp = (GameObject)Resources.Load("Prefabs/Arrow_Up_Prefab");
        noteDown = (GameObject)Resources.Load("Prefabs/Arrow_Down_Prefab");
        noteLeft = (GameObject)Resources.Load("Prefabs/Arrow_Left_Prefab");
        noteRight = (GameObject)Resources.Load("Prefabs/Arrow_Right_Prefab");
        //Debug.Log(qtename);
        QteSo = (Qte_Template)Resources.Load<Qte_Template>($"so_QTEs/{qtename}");

        licktriggered = false;
        noteAmount = QteSo.noteData.Length;
        //Debug.Log($"Noteamount: {noteAmount}");
        noteindex = 0;
        arrowsPosition = QteSo.arrowsHeight;

        //Generate Guitar
//        guitarinitialPosition = new Vector3(-2f, 0.3f, 1f) + new Vector3(QteSo.QTEPosition.x, QteSo.QTEPosition.y, QteSo.QTEPosition.z);
        guitarinitialPosition = new Vector3(-2f, 0.3f, 1f);

        GameObject newGuitar = Instantiate(guitarPrefab, guitarinitialPosition, guitarPrefab.transform.rotation, this.transform) as GameObject;
        //newGuitar.transform.parent = this.transform;
        newGuitar.transform.position = guitarinitialPosition;
        newGuitar.transform.rotation = Quaternion.identity;

        //Generate Notes
        notesArray = new GameObject[noteAmount];

        for (noteindex = 0; noteindex < noteAmount; noteindex++)
        {
            noteDelay = QteSo.noteData[noteindex].noteDelay + arrowsPosition;
            notePosition = QteSo.noteData[noteindex].notePosition;

            if (notePosition == 0) //Left
            {
                noteinitialPosition = new Vector3(-1.9f, noteDelay, 0f);
                //noteinitialPosition = new Vector3(-61.9f, noteDelay, 0f);
                GameObject clone = (GameObject)Instantiate(noteLeft, noteinitialPosition,noteLeft.transform.rotation, this.transform);
                //noteinitialPosition = new Vector3(-1f, noteDelay, 0f) + new Vector3(QteSo.QTEPosition.x, QteSo.QTEPosition.y, QteSo.QTEPosition.z);
                clone.transform.position = noteinitialPosition;
                //clone.transform.parent = this.transform;

                notesArray[noteindex] = clone;
            }
            if (notePosition == 1) //Up
            {
                noteinitialPosition = new Vector3(-0.69f, noteDelay, 0f);
                //noteinitialPosition = new Vector3(-60.69f, noteDelay, 0f);

                GameObject clone = (GameObject)Instantiate(noteUp,noteinitialPosition,noteUp.transform.rotation,this.transform);
                //noteinitialPosition = new Vector3(-0.5f, noteDelay, 0f) + new Vector3(QteSo.QTEPosition.x, QteSo.QTEPosition.y, QteSo.QTEPosition.z);
                clone.transform.position = noteinitialPosition;
                //clone.transform.parent = this.transform;

                notesArray[noteindex] = clone;
            }
            if (notePosition == 2) //Down
            {
                noteinitialPosition = new Vector3(0.86f, noteDelay, 0f);
                //noteinitialPosition = new Vector3(-59.14f, noteDelay, 0f);

                GameObject clone = (GameObject)Instantiate(noteDown,noteinitialPosition,noteDown.transform.rotation,this.transform);
                //noteinitialPosition = new Vector3(0.5f, noteDelay, 0f) + new Vector3(QteSo.QTEPosition.x, QteSo.QTEPosition.y, QteSo.QTEPosition.z);
                clone.transform.position = noteinitialPosition;
                //clone.transform.parent = this.transform;

                notesArray[noteindex] = clone;
            }
            if (notePosition == 3) //Right
            {
                noteinitialPosition = new Vector3(2.04f, noteDelay, 0f);
                //noteinitialPosition = new Vector3(-57.96f, noteDelay, 0f);

                GameObject clone = (GameObject)Instantiate(noteRight,noteinitialPosition,noteRight.transform.rotation,this.transform);
                //noteinitialPosition = new Vector3(1f, noteDelay, 0f) + new Vector3(QteSo.QTEPosition.x, QteSo.QTEPosition.y, QteSo.QTEPosition.z);
                clone.transform.position = noteinitialPosition;
                //clone.transform.parent = this.transform;

                notesArray[noteindex] = clone;
            }
        }
    }

    private void OnDestroy()
    {
        noteAmount = 0;
    }
}