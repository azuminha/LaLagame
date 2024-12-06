using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeck : MonoBehaviour
{
    public List<CardManager.CardBase> Deck = new List<CardManager.CardBase>();

    public void AddCard(CardManager.CardBase card)
    {
        Deck.Add(card);
    }
}
