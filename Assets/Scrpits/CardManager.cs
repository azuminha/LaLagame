using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        public string Name { get; set; }
        public string Description { get; set; }
        public int ManaCost { get; set; }
        public int Attack { get; set; }
        public int Defense { get; set; }
        public EffectType Effect { get; set; }
        public int QuestionDifficulty { get; set; }
    }

    public CardBase[] Cards = new CardBase[]
    {
        new CardBase { Name = "Ataque basico", Description = "Causa 1 de dano", ManaCost = 1, Attack = 1, Defense = 0, Effect = EffectType.None, QuestionDifficulty = 1 },
        new CardBase { Name = "Defesa basico", Description = "Aumenta 1 de armadura", ManaCost = 1, Attack = 0, Defense = 1, Effect = EffectType.None, QuestionDifficulty = 1 },
    };

    public int cardCount;

    [SerializeField] private GameObject card_prefab;
    [SerializeField] private GameObject cardHorizontalLayout;

    private void ButtonSet()
    {
        List<int> availableCards = new List<int>();
        for (int i = 0; i < Cards.Length; ++i)
        {
            availableCards.Add(i);
        }
        ShuffleList(availableCards);

        Debug.Log("CardCount: " + cardHorizontalLayout.transform.childCount + " " + cardCount);
        while (cardHorizontalLayout.transform.childCount < cardCount)
        {
            GameObject newCard = Instantiate(card_prefab, cardHorizontalLayout.transform);
            Debug.Log("Instanciou: " + newCard.name);
        }
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
