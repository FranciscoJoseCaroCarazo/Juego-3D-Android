using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Puntos : MonoBehaviour
{
    public float valor;
    public Puntos puntos;
    public Text texto;
    // Start is called before the first frame update
    void Start()
    {
        valor = 0;
    }

    // Update is called once per frame
    void Update()
    {
        texto.text = "Puntos: " + puntos.valor;
    }
}
