using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionController : MonoBehaviour
{
    [SerializeField] TMPro.TextMeshProUGUI questionText;

    private void Start()
    {
        gameObject.SetActive(false);
    }
    public void Mostrar(string pergunta)
    {
        gameObject.SetActive(true);
        questionText.text = pergunta;
    }
}
