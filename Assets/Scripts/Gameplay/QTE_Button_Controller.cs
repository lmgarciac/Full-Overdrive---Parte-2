using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QTE_Controller : MonoBehaviour
{
    private SpriteRenderer button_renderer;
    public Sprite defaultImage;
    public Sprite pressedImage;
    public KeyCode actionkey;
    void Start()
    {
        button_renderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if(Input.GetKeyDown(actionkey))
        {
            button_renderer.sprite = pressedImage;
        }
        if(Input.GetKeyUp(actionkey))
        {
            button_renderer.sprite = defaultImage;
        }
    }
}
