using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Events;

public class NPC_Controller : MonoBehaviour
{
    public List<Transform> waypoints = new List<Transform>();
    private Transform targetWaypoint;
    private int targetWaypointIndex = 0;
    private int lastWaypointIndex;
    private float minDistance = 0.1f;
    public float movementSpeed = 5.0f;
    public float rotationSpeed = 2.0f;
    private GameObject miloGO;
    private bool miloCanMove;

    void Start()
    {
        if(this.gameObject.tag == "Milo")
        {
            Debug.Log("Milo Position: " + Map_Status.MiloPosition);
        }

        lastWaypointIndex = waypoints.Count - 1;
        targetWaypoint = waypoints[targetWaypointIndex];

        if (this.gameObject.tag == "Milo") //Only for Milo
        {
            miloCanMove = false;
            miloGO = this.transform.Find("Perro").gameObject;
            miloGO.GetComponent<Animator>().SetBool("Run", false);
        }
    }

    private void OnEnable()
    {
        EventController.AddListener<BeforeSceneUnloadEvent>(BeforeSceneUnloadEvent);
        EventController.AddListener<AfterSceneLoadEvent>(AfterSceneLoadEvent);
        EventController.AddListener<QuitGameEvent>(QuitGameEvent);

    }

    private void OnDisable()
    {
        EventController.RemoveListener<BeforeSceneUnloadEvent>(BeforeSceneUnloadEvent);
        EventController.RemoveListener<AfterSceneLoadEvent>(AfterSceneLoadEvent);
        EventController.RemoveListener<QuitGameEvent>(QuitGameEvent);

    }

    void Update()
    {
        float movementStep = movementSpeed * Time.deltaTime;
        float rotationStep = rotationSpeed * Time.deltaTime;

        Vector3 directionToTarget = targetWaypoint.position - transform.position;

        Quaternion rotationToTarget = new Quaternion();

        if (directionToTarget != Vector3.zero)
        {
            rotationToTarget = Quaternion.LookRotation(directionToTarget);
        }

        float distance = Vector3.Distance(transform.position, targetWaypoint.position);


        if (this.gameObject.tag == "Milo")
        {
            Quests miloQuest = Player_Status.FindQuest("MiloQuest");

            if(miloQuest != null && miloQuest.queststatus == 2)
            {
                miloGO.GetComponent<Animator>().SetBool("Run", true);

                CheckDistanceToWaypoint(distance);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotationToTarget, rotationStep);
                transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, movementStep);
            }
        }
        else
        {
            CheckDistanceToWaypoint(distance);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotationToTarget, rotationStep);
            transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, movementStep);
        }

    }

    void CheckDistanceToWaypoint(float currentDistance)
    {
        if (currentDistance <= minDistance)
        {
            targetWaypointIndex++;
            UpdateTargetWaypoint();
        }
    }

    void UpdateTargetWaypoint()
    {

        if (targetWaypointIndex > lastWaypointIndex)
        {
            if (this.gameObject.tag != "Milo")
            {
                targetWaypointIndex = 0;
            }
            else
            {
                targetWaypointIndex = lastWaypointIndex;
                miloGO.GetComponent<Animator>().SetBool("Run", false);
            }
        }
            targetWaypoint = waypoints[targetWaypointIndex];        
    }


    private void BeforeSceneUnloadEvent(BeforeSceneUnloadEvent before)
    {
        if (this.gameObject.tag == "Milo")
        {
            ////Milo position
            Map_Status.MiloRotation = this.transform.rotation;
            Map_Status.MiloPosition = this.transform.position;
        }
    }

    private void AfterSceneLoadEvent(AfterSceneLoadEvent after)
    {
        //Restore positions
        if (!PlayerOptions.NewGame && this.gameObject.tag == "Milo")
        {
            this.transform.position = Map_Status.MiloPosition;
            this.transform.rotation = Map_Status.MiloRotation;
        }
    }

    private void QuitGameEvent(QuitGameEvent quitgame)
    {
        if (this.gameObject.tag == "Milo")
        {
            ////Milo position
            Map_Status.MiloRotation = this.transform.rotation;
            Map_Status.MiloPosition = this.transform.position;
        }
    }

}
