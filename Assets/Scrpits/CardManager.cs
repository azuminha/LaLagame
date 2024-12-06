using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum EffectType
{
    None,
    Poison,
    Burn,
};

public class CardManager : MonoBehaviour
{
    [System.Serializable]
    public class CardBase
    {
        public string Name;
        public string Description;
        public int ManaCost;
        public int Attack;
        public int Defense;
        public EffectType Effect;
        public int QuestionDifficulty;
    }

    public CardBase[] Cards = new CardBase[]{}; // cards possiveis de se conseguir na roleta


    public int cardCount;

    [SerializeField] private GameObject card_prefab;
    [SerializeField] private GameObject cardHorizontalLayout;

    private void ButtonSet()
    {
        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(1).gameObject.SetActive(true);

        List<int> availableCards = new List<int>();
        for (int i = 0; i < Cards.Length; ++i)
        {
            availableCards.Add(i);
        }
        ShuffleList(availableCards);

        Debug.Log("CardCount: " + cardHorizontalLayout.transform.childCount + " " + cardCount);
        while (cardHorizontalLayout.transform.childCount < cardCount)
        {
            Instantiate(card_prefab, cardHorizontalLayout.transform);
        }
        Debug.Log("Card.len: " + Cards.Length);
        for(int i = 0; i < cardCount; ++i)
        {
            CardBase card = Cards[availableCards[i]];
            GameObject cardObject = cardHorizontalLayout.transform.GetChild(i).gameObject;
            Debug.Log($"cardObject name: {cardObject.name}");

            Button cardButton = cardObject.GetComponent<Button>();
            cardButton.onClick.AddListener(() => { CardChosen(card.Name); });

            TextMeshProUGUI cardTextName = cardObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI cardCost = cardObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI cardValue = cardObject.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI cardDescription = cardObject.transform.GetChild(3).GetComponent<TextMeshProUGUI>();
            
            cardTextName.text = card.Name;
            cardCost.text = card.ManaCost.ToString();
            cardValue.text = card.Attack.ToString() + "/" + card.Defense.ToString();
            cardDescription.text = card.Description;
        }
    }

    private void CardChosen(string cardName)
    {
        Debug.Log(cardName + " escolhido");
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(false);

    }

    private void ShuffleList(List<int> list)
    {
        for (int i = 0; i < list.Count; ++i)
        {
            int randomIndex = Random.Range(i, list.Count);
            int temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

//tirar isso
    private void Start()
    {
        ButtonSet();
    }
}
