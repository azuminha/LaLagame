using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeck : MonoBehaviour
{
    public int MaxLife = 20;
    public List<CardManager.CardBase> Deck = new List<CardManager.CardBase>();
    public List<EffectType> Status = new List<EffectType>();
    public int Life = 10;
    public int Mana = 5;

    public Animator anim;

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
        Life = MaxLife;
    }

    public void AddLife(int x)
    {
        Life += x;
    }
}
