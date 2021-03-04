using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InterfazBalas : MonoBehaviour
{
    public Text texto;
    public Gun armas;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        texto.text = armas.balasEnCartucho + " / " + armas.balasRestantes;
        
    }
}
