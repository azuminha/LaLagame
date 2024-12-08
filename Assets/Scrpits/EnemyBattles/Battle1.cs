using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using AC;
public class Battle1 : MonoBehaviour
{
    public GameObject BattleUI;
    public ActionList enterBattleCamera;
    public ActionList enterFirstPerson;
    
    private const int UISize = 7;
    [SerializeField] private GameObject card_prefab;
    [SerializeField] private GameObject cardHorizontalLayout;
    private Animator animator;
    

    private int HandSize = 4;
    private int cardTop = 0;
    public PlayerDeck Player;
    private List<CardManager.CardBase> HandCards = new List<CardManager.CardBase>();
    private List<CardManager.CardBase> Discard = new List<CardManager.CardBase>();
    private bool FinalizadoTurno = false;

    private int MaxEnemyLife = 10;
    private int EnemyLife = 10;

    void updateUI()
    {
        TextMeshProUGUI ManaText = BattleUI.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI LifeText = BattleUI.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI EnemyLifeText = BattleUI.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI StatusText = BattleUI.transform.GetChild(3).GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI EnemyStatusText = BattleUI.transform.GetChild(4).GetComponent<TextMeshProUGUI>();

        ManaText.text = "Mana: " + Player.Mana.ToString() + "/5";
        LifeText.text = "Vida: " + Player.Life.ToString() + "/" + Player.MaxLife.ToString();
        EnemyLifeText.text = "Vida Inimigo: " + EnemyLife.ToString() + "/" + MaxEnemyLife.ToString();

        // adicionar status dos personagens
        StatusText.text = "Status: ";
        EnemyStatusText.text = "Status: ";

        while (cardHorizontalLayout.transform.childCount < HandSize)
        {
            Instantiate(card_prefab, cardHorizontalLayout.transform);
        }
        
        for(int i = 0; i < HandSize; ++i)
        {
            CardManager.CardBase card = HandCards[i];
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
    }

    private void RestorePlayerDeck()
    {
        while(HandCards.Count > 0)
        {
            Player.Deck.Add(HandCards[HandCards.Count - 1]);
            HandCards.RemoveAt(HandCards.Count - 1);
        }

        while(Discard.Count > 0)
        {
            Player.Deck.Add(Discard[Discard.Count - 1]);
            Discard.RemoveAt(Discard.Count - 1);
        }
    }

    private void ShuffleList(List<CardManager.CardBase> list)
    {
        List<int> availableCards = new List<int>();
        for (int i = 0; i < Player.Deck.Count; ++i)
        {
            availableCards.Add(i);
        }

        for (int i = 0; i < list.Count; ++i)
        {
            int randomIndex = Random.Range(i, list.Count);
            int temp = availableCards[i];
            availableCards[i] = availableCards[randomIndex];
            availableCards[randomIndex] = temp;
        }

        List<CardManager.CardBase> tmp = new List<CardManager.CardBase>();
        //Debug.Log("OI");
        for(int i = 0; i < list.Count; ++i){
            tmp.Add(list[availableCards[i]]);
        }

        list.Clear();

        for(int i = 0; i < tmp.Count; ++i)
        {
            list.Add(tmp[i]);
        }
    }

    void InitHandCards()
    {
        // Embaralha Player.Deck
        Debug.Log("PLayer.Deck.Count: " + Player.Deck.Count.ToString());
        ShuffleList(Player.Deck);

        for(int i = 0; i < HandSize; ++i)
        {
            //CardManager.CardBase tmpCard;
            if(Player.Deck.Count > 0)
            {
                //tmpCard = Player.Deck[Player.Deck.Count - 1];
                HandCards.Add(Player.Deck[Player.Deck.Count - 1]);
                Player.Deck.RemoveAt(Player.Deck.Count - 1);
            }
        }
    }

    void BuyOneCard()
    {
        if(Player.Deck.Count > 0)
        {
            //tmpCard = Player.Deck[Player.Deck.Count - 1];
            HandCards.Add(Player.Deck[Player.Deck.Count - 1]);
            Player.Deck.RemoveAt(Player.Deck.Count - 1);
            HandSize++;
        }
    }

    void ShowCards()
    {
        while (cardHorizontalLayout.transform.childCount < HandSize)
        {
            Instantiate(card_prefab, cardHorizontalLayout.transform);
        }

        for(int i = 0; i < HandSize; ++i)
        {
            CardManager.CardBase card = HandCards[i];
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
    }

    bool enter = false;
    private void CardChosen(CardManager.CardBase card)
    {
        enter = true;

        Debug.Log(card.Name + " escolhido");
        if(Player.Mana >= card.ManaCost && FinalizadoTurno == false)
        {
            HandCards.Remove(card);
            Discard.Add(card);
            Destroy(cardHorizontalLayout.transform.GetChild(0).gameObject);

            //while(enter)
            //{
             //   yield return null;
            //}
            //process(Card);
            EnemyLife -= 2;
            Player.Mana -= 1;
            HandSize--;

            updateUI();
        }
    }

    void EnableBattleUI()
    {
        for(int i = 0; i < UISize; ++i)
            BattleUI.transform.GetChild(i).gameObject.SetActive(true);
        ShowCards();
    }

    void DisableBattleUI()
    {
        for(int i = 0; i < UISize; ++i)
            BattleUI.transform.GetChild(i).gameObject.SetActive(false);    
    }

    // IMPORTANTE:
    // ALTERAR ISSO PARA UM NOVO NPC
    void EnemyLogic()
    {
        Player.Life -= 1;
    }

    public IEnumerator StartBattle()
    {
        enterBattleCamera.Interact();

        InitHandCards();
        EnableBattleUI();
        updateUI();

        animator.SetTrigger("CombatPose");

        TextMeshProUGUI TurnoText = BattleUI.transform.GetChild(6).GetComponent<TextMeshProUGUI>();
      
        while(EnemyLife > 0 && Player.Life > 0)
        {
            // meu turno
            while(!FinalizadoTurno)
            {
                yield return null;
            }
            if(EnemyLife <= 0) break;

            // espera um tempo
            TurnoText.text = "TURNO INIMIGO";
            FinalizadoTurno = true;
            updateUI();
            yield return new WaitForSeconds(1f);
            //Turno Do inimigo
            animator.SetTrigger("MakeAtaque");
            EnemyLogic();
            yield return new WaitForSeconds(1f);
            
            // fase de compra e reset
            FinalizadoTurno = false;
            Player.ResetMana();
            TurnoText.text = "SEU TURNO";
            BuyOneCard();

            updateUI();
        }

        DisableBattleUI();
        RestorePlayerDeck();
        enterFirstPerson.Interact();
        Player.ResetLife();
        Player.ResetMana();

        if(EnemyLife <= 0)
        {
            Debug.Log("VC GANHOU");
            animator.SetTrigger("Morra");
        }else
        {
            Debug.Log("PERDEU");
        }
    }

    void Start()
    {
        DisableBattleUI();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("m") || EnemyLife <= 0 || Player.Life <= 0)
        {
            FinalizadoTurno = true;
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            Debug.Log("Enable Battle UI");
            //EnableBattleUI();
            StartBattle();
        }
        if (Input.GetKeyUp(KeyCode.F2))
        {
            Debug.Log("Disable Battle UI");
            DisableBattleUI();
        } 
    }
}
