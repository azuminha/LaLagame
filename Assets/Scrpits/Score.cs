using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Score : MonoBehaviour
{
    static public int score = 0;
    public TextMeshProUGUI scoreText;

    public static void addScore(int x)
    {
        score += x;
    }

    public static void resetScore()
    {
        score = 0;
    }
    // Update is called once per frame
    void Update()
    {
        string m = "Score: " + score.ToString();
        scoreText.text = m;
    }
}
