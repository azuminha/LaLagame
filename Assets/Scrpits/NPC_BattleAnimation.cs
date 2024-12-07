using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_BattleAnimation : MonoBehaviour
{
    public Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void RunNpcAnim()
    {
        anim.Play("CombatAnim");
    }
}
