using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LogicaJugador : MonoBehaviour {
    public Vida vida;
    public bool Vida0 = false;
    [SerializeField] private Animator animadorPerder;
    public Puntos puntos;
    public AudioSource[] sounds;
    public GameObject mapa;

    // Use this for initialization
    void Start () {
        vida = GetComponent<Vida>();
        puntos.valor = 0;
        sounds = GetComponents<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {
        RevisarVida();
        //pararSonido();
        if (vida.valor <= 0 && Input.GetMouseButtonDown(0))
        {
            Invoke("ReiniciarJuego", 0f);
        }


    }

    void pararSonido()
    {
        if (PauseMenú.GamePause)
        {
            mapa.GetComponent<AudioSource>().Pause();
        }
        else if(Input.GetKey(KeyCode.Escape))
        {
            mapa.GetComponent<AudioSource>().UnPause();
        }
    }

    void RevisarVida()
    {
        if (Vida0) return;
        if(vida.valor <= 0)
        {
            AudioListener.volume = 0f;
            animadorPerder.SetTrigger("mostrar");
            Vida0 = true;
        }
    }

    void ReiniciarJuego()
    {
        SceneManager.LoadScene(0);
        puntos.valor = 0;
        AudioListener.volume = 1f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemigo")
        {
            gameObject.GetComponent<AudioSource>().volume = 0f;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Enemigo")
        {
            gameObject.GetComponent<AudioSource>().volume = 1f;
        }
    }


}
