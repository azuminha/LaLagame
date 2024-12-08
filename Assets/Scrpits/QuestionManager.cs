using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionManager : MonoBehaviour
{
    [System.Serializable]
    public class QuestionBase
    {
        public string Pergunta;
        public string[] Resposta;
        public int InidiceResposta;
        public int Dificuldade;
    }
    
    public QuestionBase[] Questions = new QuestionBase[]{};
    
    void Start()
    {
        
    }

    
    void Update()
    {
        
    }
}
