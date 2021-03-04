using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ModoDeDisparo
{
    SemiAuto,
    FullAuto
}

public class Gun : MonoBehaviour {
    protected Animator animator;
    protected AudioSource audioSource;
 
    public bool tiempoNoDisparo = false;
    public bool puedeDisparar = false;
    public bool recargando = false;
    public bool corriendo = false;
    
    [Header("Referencia de Objetos")]
    public ParticleSystem fuegoDeArma;
    public Transform puntoDeDisparo;
    public GameObject efectoDisparo;

    [Header("Referencia de Sonidos")]
    public AudioClip SonDisparo;
    public AudioClip SonRecargaConBalas;
    public AudioClip SonRecargaSinBalas;
    public AudioClip SonVacio;
    public AudioClip SonDesenfundar;

    [Header("Atributos de Arma")]
    public ModoDeDisparo modoDeDisparo = ModoDeDisparo.FullAuto;
    public float daño = 20f;
    public float ritmoDeDisparo = 0.3f;
    public int balasRestantes;
    public int balasEnCartucho;
    public int tamañoDeCartucho = 12;
    public int maximoDeBalas = 150;
    public Camera camaraPrincipal;
    public bool estaADS = false;
    public Vector3 disCadera;
    public Vector3 ADS;
    public float tiempoApuntar;
    public float zoom;
    public float normal;
    

    // Use this for initialization
    void Start () {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();

        balasEnCartucho = tamañoDeCartucho;
        balasRestantes = maximoDeBalas;

        Invoke("HabilitarArmar", 0.5F);

	}
	
	// Update is called once per frame
	void Update () {
		

        if (Input.GetButtonDown("Reload"))
        {
            RevisarRecargar();
        }
        if (Input.GetMouseButton(1))
        {
            transform.localPosition = Vector3.Slerp(transform.localPosition,ADS,tiempoApuntar*Time.deltaTime);
            estaADS = true;
            camaraPrincipal.fieldOfView= Mathf.Lerp(camaraPrincipal.fieldOfView, zoom, tiempoApuntar * Time.deltaTime);    
        }

        if (Input.GetMouseButtonUp(1))
        {
           estaADS = false;

        }

        if (estaADS == false)
        {
            transform.localPosition = Vector3.Slerp(transform.localPosition, disCadera, tiempoApuntar * Time.deltaTime);
            camaraPrincipal.fieldOfView = Mathf.Lerp(camaraPrincipal.fieldOfView, normal, tiempoApuntar * Time.deltaTime);
        }

        if (estaADS == true)
        {
            transform.localPosition = Vector3.Slerp(transform.localPosition, ADS, tiempoApuntar * Time.deltaTime);
            camaraPrincipal.fieldOfView = Mathf.Lerp(camaraPrincipal.fieldOfView, zoom, tiempoApuntar * Time.deltaTime);
        }


    }

    void HabilitarArmar()
    {
        puedeDisparar = true;
    }

    void RevisarDisparo()
    {
        if (!puedeDisparar) return;
        if (tiempoNoDisparo) return;
        if (recargando) return;
        if (balasEnCartucho > 0)
        {
            Debug.Log("RevisarDisparo");
            disparo();
        }
        else
        {
            SinBalas();
        }
    }

    void disparo()
    {
        Debug.Log("hola");
        audioSource.PlayOneShot(SonDisparo);
        tiempoNoDisparo = true;
        fuegoDeArma.Play();
        ReproducirAnimacionDisparo();
        balasEnCartucho--;
        StartCoroutine(ReiniciarTiempoNoDisparo());
        DisparoDirecto();
    }

    public void CrearEfectoDaño(Vector3 pos, Quaternion rot)
    {
        GameObject efectoDaño = Instantiate(efectoDisparo, pos, rot);
        Destroy(efectoDisparo, 1f);
    }

    void DisparoDirecto()
    {
        RaycastHit hit;
        if(Physics.Raycast(puntoDeDisparo.position, puntoDeDisparo.forward, out hit))
        {
            if (hit.transform.CompareTag("Enemigo"))
            {
                Vida vida = hit.transform.GetComponent<Vida>();
                if (vida == null)
                {
                    throw new System.Exception("No se encontro el enemigo");
                }
                else
                {
                    vida.RecibirDaño(daño);
                    CrearEfectoDaño(hit.point, hit.transform.rotation);
                }
            }
        }
    }

    public virtual void ReproducirAnimacionDisparo()
    {
        if(gameObject.name== "Glock19"){
            if(balasEnCartucho > 1)
            {
                animator.CrossFadeInFixedTime("disparo", 0.1f);
            }
            else
            {
                animator.CrossFadeInFixedTime("sinMunicion", 0.1f);
            }
        }
        else
        {
            animator.CrossFadeInFixedTime("disparo", 0.1f);
        }
        
    }

    void SinBalas()
    {
        audioSource.PlayOneShot(SonVacio);
        tiempoNoDisparo = true;
        StartCoroutine(ReiniciarTiempoNoDisparo());
    }

    IEnumerator ReiniciarTiempoNoDisparo()
    {
        yield return new WaitForSeconds(ritmoDeDisparo);
        tiempoNoDisparo = false;
    }

    public void RevisarRecargar()
    {
        if(balasRestantes > 0 && balasEnCartucho < tamañoDeCartucho)
        {
            recargarConMun();
        }
        else if(balasRestantes > 0 && balasEnCartucho == 0)
        {
            recargaSinMun();
        }
    }

    void recargarConMun()
    {
        if (recargando) return;
        recargando = true;
        animator.CrossFadeInFixedTime("recargarConMunicion", 0.1f);
        audioSource.PlayOneShot(SonRecargaConBalas);
        RecargarMuniciones();
        ReiniciarRecargar();
    }

    void recargaSinMun()
    {
        if (recargando) return;
        recargando = true;
        animator.CrossFadeInFixedTime("recargarSinMunicion", 0.1f);
        audioSource.PlayOneShot(SonRecargaSinBalas);
        RecargarMuniciones();
        ReiniciarRecargar();
    }

    void RecargarMuniciones()
    {
        int balasParaRecargar = tamañoDeCartucho - balasEnCartucho;
        int restarBalas = (balasRestantes >= balasParaRecargar) ? balasParaRecargar : balasRestantes;

        balasRestantes -= restarBalas;
        balasEnCartucho += balasParaRecargar;
    }

    public void sacarArma()
    {
        audioSource.PlayOneShot(SonDesenfundar);
    }

    public void VacioOn()
    {
        audioSource.PlayOneShot(SonVacio);
        Invoke("ReiniciarRecargar", 0.1f);
    }
    void ReiniciarRecargar()
    {
        recargando = false;
    }

    public void DispararArma()
    {
        if (modoDeDisparo == ModoDeDisparo.FullAuto)
        {
            RevisarDisparo();
        }
        else if (modoDeDisparo == ModoDeDisparo.SemiAuto)
        {
            RevisarDisparo();
        }
    }

    public void ApretarAds()
    {
        estaADS = true;
    }

    public void ApretarADS()
    {
        estaADS = false;
    }


}
