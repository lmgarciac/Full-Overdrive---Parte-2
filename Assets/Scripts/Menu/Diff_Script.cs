using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Diff_Script : MonoBehaviour
{
    private TextMeshProUGUI tx_diff;
    private Slider diff_slider;

    void Start()
    {
        tx_diff = this.GetComponent<TextMeshProUGUI>();
        diff_slider = this.GetComponentInParent<Slider>();
        PlayerOptions.Difficulty = (int)diff_slider.value;

        ChangeDifficulty();
    }

    public void ChangeDifficulty()
    {
        if(diff_slider.value == 0)
        {
            tx_diff.text = $"EASY";
        }
        if (diff_slider.value == 1)
        {
            tx_diff.text = $"MEDIUM";
        }
        if (diff_slider.value == 2)
        {
            tx_diff.text = $"HARD";
        }
        if (diff_slider.value == 3)
        {
            tx_diff.text = $"BRUTAL";
        }

        PlayerOptions.Difficulty = (int)diff_slider.value;
    }

}
