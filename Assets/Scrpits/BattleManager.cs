using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance { get; set; }
    [SerializeField] private QuestionController questionController;

    private BattleData battleData;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StartBattle(List<Card> jogador, string enemyName, int enemyDamage, int enemyBoostDamage, int enemyDefence)
    {
        BattleData battleData = new BattleData();

        //montar


    }

    public void MostrarPergunta()
    {
        questionController.Mostrar("qualuqer cosa");
    }

}
