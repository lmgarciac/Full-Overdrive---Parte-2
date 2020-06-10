using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pigeon_Controller : MonoBehaviour
{
    private Animator animator;
    private float time;
    // Start is called before the first frame update
    void Start()
    {
        animator = this.GetComponent<Animator>();
        animator.speed = Random.Range(0.7f, 1.8f);
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (time > 2.0f)
        {
            animator.speed = Random.Range(0.7f, 1.8f);
            time = 0f;
        }
    }
}
