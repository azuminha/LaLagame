using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TelaFinal : MonoBehaviour
{
    // Start is called before the first frame update
    public TextMeshProUGUI ScoreText;

    // Update is called once per frame
    void Start()
    {
        ScoreText.text = "SCORE: " + Score.score.ToString();
        WalkSoundScript.Castle = false;
        Score.resetScore();
    }
}
