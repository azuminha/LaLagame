using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestOpen : MonoBehaviour
{
    private Animator animator;
    public bool openned = false;
    public CardManager cardManager;

    private void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetTrigger("Close");
    }

    void ChestOpenning()
    {
        if(!openned)
        {
            animator.SetTrigger("Open");
            Debug.Log("ganhar card");
            StartCoroutine(waiter());
            openned = true;
        }
    }
    IEnumerator waiter()
    {

        yield return new WaitForSeconds(1.5F);
        GetNewCard();
    }
    void GetNewCard()
    {
        int randomNumber = Random.Range(2, 4);
        StartCoroutine(cardManager.ChooseCards(randomNumber));
    }
}
