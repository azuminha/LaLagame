using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Score : MonoBehaviour
{
    static public int score = 0;
    static public int EnemiesDefeated = 0;
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

    public static void KilledEnemy()
    {
        EnemiesDefeated++;
    }

    public static int min(int a, int b)
    {
        return (a < b) ? a : b;
    }

    public static int max(int a, int b)
    {
        return (a > b) ? a : b;
    }
}
