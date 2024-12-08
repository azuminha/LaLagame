using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeck : MonoBehaviour
{
    public int MaxLife = 50;
    public List<CardManager.CardBase> Deck = new List<CardManager.CardBase>();
    public int Life = 10;
    public int Mana = 5;

    public void AddCard(CardManager.CardBase card)
    {
        Deck.Add(card);
    }

    public void ResetMana()
    {
        Mana = 5;
    }

    public void ResetLife()
    {
        Life = 10;
    }

    public void AddLife(int x)
    {
        Life += x;
        if(Life > MaxLife)
            Life = MaxLife;
    }
}
