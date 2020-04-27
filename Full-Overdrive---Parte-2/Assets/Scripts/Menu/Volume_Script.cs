using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Volume_Script : MonoBehaviour
{

    private Slider sliderVolume;
    // Start is called before the first frame update
    void Start()
    {
        sliderVolume = this.GetComponent<Slider>();
        sliderVolume.value = AudioListener.volume;
        PlayerOptions.Volume = sliderVolume.value;
    }

    // Update is called once per frame
    void Update()
    {
        if (AudioListener.volume != sliderVolume.value || PlayerOptions.Volume != sliderVolume.value)
        {
            AudioListener.volume = sliderVolume.value;
            PlayerOptions.Volume = sliderVolume.value;
        }
    }

}
