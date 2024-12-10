using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using AC;
using UnityEngine.SceneManagement;

public class Battle1 : MonoBehaviour
{
    public GameObject BattleUI;
    public ActionList enterBattleCamera;
    public ActionList enterFirstPerson;
    public ActionList FalaAction;
    public Transform playerTransform;
    
    private const int UISize = 9;
    [SerializeField] private GameObject card_prefab;
    [SerializeField] private GameObject cardHorizontalLayout;
    [SerializeField] private GameObject resposta_prefab;
    [SerializeField] private GameObject respostaVerticalLayout;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip attackSound;
    [SerializeField] private AudioClip defstatSound;

    private Animator animator;
    
    public QuestionManager questionBase;

    private int HandSize = 4;
    public PlayerDeck Player;
    private List<CardManager.CardBase> HandCards = new List<CardManager.CardBase>();
    private List<CardManager.CardBase> Discard = new List<CardManager.CardBase>();
    private bool FinalizadoTurno = false;

    public int MaxEnemyLife = 30;
    private int EnemyLife = 30;
    private List<EffectType> EnemyStatus = new List<EffectType>();

    private int[] PerguntasRespondidas = {0, 0, 0};
    public int NPCBattleDif = 1;
    public int PontuacaoBase = 50;

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

        for(int i=0; i<EnemyStatus.Count; ++i)
        {
            if(EnemyStatus[i] == EffectType.Poison)
                EnemyStatusText.text += "Poison ";
            if(EnemyStatus[i] == EffectType.Burn)
                EnemyStatusText.text += "Burn ";
            if(EnemyStatus[i] == EffectType.Ice)
                EnemyStatusText.text += "Ice ";
            if(EnemyStatus[i] == EffectType.Heal)
                EnemyStatusText.text += "Heal ";
        }

        for(int i=0; i<Player.Status.Count; ++i)
        {
            if(Player.Status[i] == EffectType.Poison)
                StatusText.text += "Poison ";
            if(Player.Status[i] == EffectType.Burn)
                StatusText.text += "Burn ";
            if(Player.Status[i] == EffectType.Ice)
                StatusText.text += "Ice ";
            if(Player.Status[i] == EffectType.Heal)
                StatusText.text += "Heal ";
        }

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

    void EnableQuestionUI()
    {
        BattleUI.transform.GetChild(7).gameObject.SetActive(true);
        BattleUI.transform.GetChild(8).gameObject.SetActive(true);
    }

    void DisableQuestionUI()
    {
        BattleUI.transform.GetChild(7).gameObject.SetActive(false);
        BattleUI.transform.GetChild(8).gameObject.SetActive(false);
    }

    bool enter = false;
    bool answerCorret;
    void QuestionChosen(string ans, QuestionManager.QuestionBase quest)
    {
        //Debug.Log("quest.Resposta: " + quest.Resposta.Length.ToString() + " quest.InidiceResposta: " + quest.InidiceResposta.ToString());
        if(ans == quest.Resposta[quest.InidiceResposta])
        {
            answerCorret = true;
            PerguntasRespondidas[quest.Dificuldade-1]++;
        }else
        {
            answerCorret = false;
        }
        enter = false;
        DisableQuestionUI();
    }

    void ShowQuestions(int questDif)
    {   
        EnableQuestionUI();

        while (respostaVerticalLayout.transform.childCount < 4)
        {
            Instantiate(resposta_prefab, respostaVerticalLayout.transform);
        }
 
        // select all questions of difficult x
        List<QuestionManager.QuestionBase> ListaDeQuestoes = new List<QuestionManager.QuestionBase>();
        for(int i=0; i<questionBase.Questions.Length; ++i)
        {
            if(questionBase.Questions[i].Dificuldade == questDif)
            {
                ListaDeQuestoes.Add(questionBase.Questions[i]);
            }
        }

        // Seleciona uma pergunta aleatoria
        // Podemos mudar talvez besado na dificuldade nao sei ainda
        int randomIndex = Random.Range(0, ListaDeQuestoes.Count);
        //Debug.Log("questionBase.Questions.Length : " + ListaDeQuestoes.Count.ToString() + " RANDINDEX: " + randomIndex.ToString());
        QuestionManager.QuestionBase question = ListaDeQuestoes[randomIndex];
        
        //Coloca a pergunta na tela
        TextMeshProUGUI QuestionText = BattleUI.transform.GetChild(7).GetComponent<TextMeshProUGUI>();
        QuestionText.text = question.Pergunta;

        for(int i = 0; i < 4; ++i)
        {
            string questionText = question.Resposta[i];
            GameObject questionObject = respostaVerticalLayout.transform.GetChild(i).gameObject;
            
            UnityEngine.UI.Button respButton = questionObject.GetComponent<UnityEngine.UI.Button>();
            respButton.onClick.RemoveAllListeners();
            respButton.onClick.AddListener(() => { QuestionChosen(questionText, question); });

            TextMeshProUGUI RespText = questionObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            RespText.text = questionText;
        }
    }
    
    private void CardChosen(CardManager.CardBase card)
    {
        StartCoroutine(HandleCardChosen(card));
    }
    
    void RemoveEnemyLife(int x)
    {
        int contador = 0;
        for(int i=0; i<EnemyStatus.Count; ++i)
        {
            if(EnemyStatus[i] == EffectType.Ice)
                contador++;
        }
        if(x - contador < 0)
            x = 0;
        else
            x = x - contador;
            
        EnemyLife -= x;
    }

    // processar card, MUDAR AQUI se adicionar uma mecanica
    void process(CardManager.CardBase card)
    {
        if(card.Attack > 0)
            Player.anim.SetTrigger("MakeAtaque");
        else
            audioSource.PlayOneShot(defstatSound);

        if(card.Effect == EffectType.None)
        {
            RemoveEnemyLife(card.Attack);
            Player.Life += card.Defense;
        }else if(card.Effect == EffectType.Poison) // Poison
        {
            if(card.Name == "Super Veneno")
            {
                for(int i=0; i<card.Attack; ++i)
                {
                    EnemyStatus.Add(EffectType.Poison);
                }
            }else
            {
                RemoveEnemyLife(card.Attack);
                EnemyStatus.Add(EffectType.Poison);
            }
        }else if(card.Effect == EffectType.Burn)
        {
            RemoveEnemyLife(card.Attack);
            Player.Life += card.Defense;
            EnemyStatus.Add(EffectType.Burn);

            if(card.Name == "Fogo com Dano")
            {
                EnemyStatus.Add(EffectType.Burn);
            }
        }else if(card.Effect == EffectType.AddMana)
        {
            RemoveEnemyLife(card.Attack);
            Player.Life += card.Defense;
            Player.Mana += 1;
        }else if(card.Effect == EffectType.Ice)
        {
            RemoveEnemyLife(card.Attack);
            Player.Life += card.Defense;
            Player.Status.Add(EffectType.Ice);
        }else if(card.Effect == EffectType.DrawCard)
        {
            RemoveEnemyLife(card.Attack);
            Player.Life += card.Defense;
            BuyOneCard();
            if(card.Name == "Super compra de cartas")
            {
                if(HandSize <= 3)
                {
                    BuyOneCard();
                    BuyOneCard();
                }
            }
        }else if(card.Effect == EffectType.Heal)
        {
            RemoveEnemyLife(card.Attack);
            Player.Life += card.Defense;
            Player.Status.Add(EffectType.Heal);
        }
    }

    private IEnumerator HandleCardChosen(CardManager.CardBase card)
    {

        Debug.Log(card.Name + " escolhido");
        if(Player.Mana >= card.ManaCost && FinalizadoTurno == false && enter == false)
        {
            HandCards.Remove(card);
            Discard.Add(card);
            Destroy(cardHorizontalLayout.transform.GetChild(0).gameObject);

            enter = true;
            answerCorret = false;
            ShowQuestions(card.QuestionDifficulty);
            yield return new WaitUntil(() => !enter);
            

            if(answerCorret)
            {
                //processar somente o dano e os status da carta
                process(card);
            }

            Player.Mana -= card.ManaCost;
            HandSize--;
            updateUI();
        }
    }

    void EnableBattleUI()
    {
        for(int i = 0; i < UISize; ++i)
            BattleUI.transform.GetChild(i).gameObject.SetActive(true);
        BattleUI.transform.GetChild(7).gameObject.SetActive(false);
        BattleUI.transform.GetChild(8).gameObject.SetActive(false);
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
        // Adicionar o gelo
        int gelo = 0;
        for(int i=0; i<Player.Status.Count; ++i)
            if(Player.Status[i] == EffectType.Ice)
                gelo++;
        //Math.Min(5, Math.Max(5 - gelo, 0));

        float probability = Random.Range(0f, 1f);
        if(probability <= 0.5)
            Player.Life -= Score.min(5, Score.max(5 - gelo, 0));
        else if(probability <= 0.8)
            for(int i = 0; i < 5; ++i) Player.Status.Add(EffectType.Poison);
        else if(probability <= 0.9) 
            EnemyLife += 5;
    }

    // Mudar aqui se adicionar um novo status
    void processEnemyStatus()
    {
        int addLife = 0;
        for(int i = 0; i < EnemyStatus.Count; ++i)
        {
            if(EnemyStatus[i] == EffectType.Poison)
            {
                addLife -= 1;
            }else if(EnemyStatus[i] == EffectType.Burn)
            {
                addLife -= 1;
            }else if(EnemyStatus[i] == EffectType.Heal)
            {
                addLife += 1;
            }
        }
        EnemyLife += addLife;
    }

    // Mudar aqui para como funciona o update de status (Agora vai ser tudo baseado na probabilidade) pode mudar de inimigo para inimigo
    void updateEnemyStatus()
    {
        for(int i = 0; i < EnemyStatus.Count; ++i)
        {
            float probability = Random.Range(0f, 1f);
            if(probability < 0.2f)
            {
                EnemyStatus.RemoveAt(i);
                i--;
            }
        }
    }

    void processPlayerStatus()// podia passar o endereco de memoria para nao repetir codigo, mas to nem ai nao aguento mais
    {
        int addLife = 0;
        for(int i = 0; i < Player.Status.Count; ++i)
        {
            if(Player.Status[i] == EffectType.Poison)
            {
                addLife -= 1;
            }else if(Player.Status[i] == EffectType.Burn)
            {
                addLife -= 1;
            }else if(Player.Status[i] == EffectType.Heal)
            {
                addLife += 1;
            }
        }
        Player.Life += addLife;
    }

    void updatePlayerStatus() // mesma coisa aqui AHSUHSAUHSAUASH
    {
        for(int i = 0; i < Player.Status.Count; ++i)
        {
            float probability = Random.Range(0f, 1f);
            if(probability < 0.2f)
            {
                Player.Status.RemoveAt(i);
                i--;
            }
        }
    }

    public IEnumerator StartBattle()
    {
        if(Score.EnemiesDefeated < 7)
        {
            FalaAction.Interact();
        }else{
        EnemyLife = MaxEnemyLife;
        transform.LookAt(new Vector3(playerTransform.position.x, transform.position.y, playerTransform.position.z));

        enterBattleCamera.Interact();

        InitHandCards();
        EnableBattleUI();
        updateUI();

        animator.SetTrigger("CombatPose");
        Player.anim.SetTrigger("CombatPose");

        TextMeshProUGUI TurnoText = BattleUI.transform.GetChild(6).GetComponent<TextMeshProUGUI>();
      
        while(EnemyLife > 0 && Player.Life > 0)
        {
            // meu turno
            while(!FinalizadoTurno)
            {
                yield return null;
            }
            if(EnemyLife <= 0) break;

            //Turno Do inimigo
            TurnoText.text = "TURNO INIMIGO";
            FinalizadoTurno = true;

            processEnemyStatus();
            updateEnemyStatus();

            updateUI();
            yield return new WaitForSeconds(1f);
            
            animator.SetTrigger("MakeAtaque");
           
            EnemyLogic();
            yield return new WaitForSeconds(1f);
            
            // fase de compra e reset
            FinalizadoTurno = false;
            Player.ResetMana();
            TurnoText.text = "SEU TURNO";
            BuyOneCard();

            processPlayerStatus();
            updatePlayerStatus();

            updateUI();
        }

        DisableBattleUI();
        RestorePlayerDeck();
        enterFirstPerson.Interact();
        Player.ResetLife();
        Player.ResetMana();
        Player.anim.SetTrigger("IdlePose");

        if(EnemyLife <= 0)
        {
            //Debug.Log("VC GANHOU");
            animator.SetTrigger("Morra");
            Debug.Log("dif 1 " + PerguntasRespondidas[0].ToString() + " dif 2 " + PerguntasRespondidas[1].ToString() + " dif 3" + PerguntasRespondidas[2].ToString());
            int Pontuacao = PontuacaoBase * NPCBattleDif;
            for(int i=0; i<2; ++i)
            {
                Pontuacao += (i+1)*5*NPCBattleDif*PerguntasRespondidas[i];
            }
            Score.addScore(Pontuacao);
            Player.MaxLife += 5;
            Player.ResetLife();
            Player.ResetStatus();

            WalkSoundScript.Castle = true;
            playerTransform.position = new Vector3(391+56, 54+10, 45+49);
            Score.KilledEnemy();
        }else
        {
            Debug.Log("PERDEU");
            UnityEngine.SceneManagement.SceneManager.LoadScene("TelaFinal");
        }
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
