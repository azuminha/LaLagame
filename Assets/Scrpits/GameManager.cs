using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    [SerializeField] List<Card> battleSuperNPCards;
    public void OnStartSuperBattle()
    {
        ScenesManager.LoadSceneAsync("Battle", () => {
            BattleManager.Instance.StartBattle(null, "", 0,0,0);
        });

    }
}
