using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using AC;
public enum EffectType
{
    None,
    Poison,
    Burn,
    AddMana,
    Ice, // diminui dano tomado
    DrawCard,
    Heal,
};
public class CardManager : MonoBehaviour
{
    public ActionList enterBattleCamera;
    public ActionList enterFirstPerson;

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

    public PlayerDeck player;

    bool enter = false;

    private IEnumerator ButtonSet()
    {
        enter = true;

        enterBattleCamera.Interact ();

        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(1).gameObject.SetActive(true);

        List<int> availableCards = new List<int>();
        for (int i = 0; i < Cards.Length; ++i)
        {
            availableCards.Add(i);
        }
        ShuffleList(availableCards);

        //Debug.Log("CardCount: " + cardHorizontalLayout.transform.childCount + " " + cardCount);
        while (cardHorizontalLayout.transform.childCount < cardCount)
        {
            Instantiate(card_prefab, cardHorizontalLayout.transform);
        }
        //Debug.Log("Card.len: " + Cards.Length);
        
        for(int i = 0; i < cardCount; ++i)
        {
            CardBase card = Cards[availableCards[i]];
            GameObject cardObject = cardHorizontalLayout.transform.GetChild(i).gameObject;
            //Debug.Log($"cardObject name: {cardObject.name}");

            UnityEngine.UI.Button cardButton = cardObject.GetComponent<UnityEngine.UI.Button>();
            cardButton.onClick.RemoveAllListeners();
            cardButton.onClick.AddListener(() => { CardChosen(card); });

            TextMeshProUGUI cardTextName = cardObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI cardCost = cardObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI cardValue = cardObject.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI cardDescription = cardObject.transform.GetChild(3).GetComponent<TextMeshProUGUI>();
            
            cardTextName.text = card.Name;
            cardCost.text = card.ManaCost.ToString();
            cardValue.text = card.Attack.ToString() + "/" + card.Defense.ToString();
            cardDescription.text = card.Description;
        }
        
        yield return new WaitUntil(() => enter == false);
    }

    private void CardChosen(CardBase card)
    {
        Debug.Log(card.Name + " escolhido");

        player.AddCard(card);

        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(false);
        enterFirstPerson.Interact ();
        enter = false;
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
    public IEnumerator ChooseCards(int n)
    {
        // Loop to allow the player to choose 3 cards
        for (int i = 0; i < n; ++i)
        {
            yield return StartCoroutine(ButtonSet()); // Wait until ButtonSet completes before continuing
        }
    }

    private void Start()
    {
        StartCoroutine(ChooseCards(20));
    }

}
