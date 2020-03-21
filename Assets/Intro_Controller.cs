using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intro_Controller : MonoBehaviour
{
    [SerializeField] private Animator anim1;
    [SerializeField] private Animator anim2;
    [SerializeField] private Animator anim3;
    [SerializeField] private Animator anim4;

    [SerializeField] private Animator comic;

    //[SerializeField] private float timeScale = 1f;


    private WaitForSecondsRealtime waitforseconds = new WaitForSecondsRealtime(4f);
    private SceneController sceneController;

    void Start()
    {
        sceneController = FindObjectOfType<SceneController>();

        StartCoroutine(InitComic());
    }

    void Update()
    {
        //Time.timeScale = timeScale;
    }

    IEnumerator InitComic()
    {

        yield return waitforseconds;
        anim1.SetBool("Show", true);

        yield return waitforseconds;
        anim2.SetBool("Show", true);

        yield return waitforseconds;
        anim3.SetBool("Show", true);

        yield return waitforseconds;
        anim4.SetBool("Show", true);

        yield return waitforseconds;
        comic.SetBool("ZoomIn", true);

        yield return waitforseconds;
        yield return waitforseconds;
        yield return waitforseconds;


        sceneController.FadeAndLoadScene("_Test_Navigation");
    }

}
