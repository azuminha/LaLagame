using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Card", order = 1)]
public class Card : ScriptableObject
{
    [SerializeField] public string nome;
    [SerializeField] public string descricao;

    [SerializeField] public string pergunta1;
    [SerializeField] public List<string> respostas1;
    [SerializeField] public int indexCorreta1;

    [SerializeField] public string pergunta2;
    [SerializeField] public List<string> respostas2;
    [SerializeField] public int indexCorreta2;

    [SerializeField] public string pergunta3;
    [SerializeField] public List<string> respostas3;
    [SerializeField] public int indexCorreta3;

    [SerializeField] public int dano;
    [SerializeField] public int defesa;
    [SerializeField] public int ampliarDano;
}
