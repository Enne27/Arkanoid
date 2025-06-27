using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEditor;
using UnityEngine;
using System;
using System.IO;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class Ball : MonoBehaviour
{
    //float hitCooldown;
    public float speed = 10f;
    public Rigidbody2D rbBola;
    private Vector2 velocity;
    private Vector3 initialPosition;
    GameObject jugador;
    private Vector2 jugadorInitialPosition;

    // El combo es solo si consigue darle a varios ladrillos de una, sin volver a tocar la bola el jugador.
    public int numCombo;
    public bool combo = false;

    private bool inputSpace;
    private Transform padre;

    float numBricksDestruidos;


        /*[SerializeField] private float maxXVelocity = 5;
        [SerializeField] private float maxXDistance = 1;*/

    [SerializeField] TMPro.TextMeshProUGUI pointsText;
    [SerializeField] Puntuacion puntos;

    [SerializeField] GameObject juego;

    [SerializeField] AudioClip[] clipsAudio;
    AudioSource audioSource;
    [SerializeField] GameObject particles;
    [SerializeField] Transform particlesParent;

    public void SetInitialPosition()
    {
        transform.position = initialPosition; // canviem la posici� pel vector que nosaltres hem fet.

        rbBola.velocity = Vector2.zero;

        jugador.transform.position = jugadorInitialPosition;

        // Que vuelva a tener el padre.
        transform.parent = padre;
    }
    public float getNumBricksDestruidos()
    {
        return numBricksDestruidos;
    }

    public void setNumBricksDestruidos()
    {
        numBricksDestruidos = 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        rbBola = GetComponent<Rigidbody2D>();
        jugador = GameObject.FindGameObjectWithTag("Player");

        jugadorInitialPosition = jugador.transform.position; // posici�n inicial de la pala
        initialPosition = transform.position; // posici�n inicial de la bola

        velocity = new Vector2(6, 6);

        padre = transform.parent;

        numBricksDestruidos = 0;

        numCombo = -5;

        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        Caida(); // Se llama al m�todo que comprueba si la pelota cae por debajo de la plataforma.
        MoveBall();
        inputSpace = false;
        //rbBola.velocity = velocity;
    }

    //private void FixedUpdate()
    //{
    //    hitCooldown -= Time.deltaTime;
    //}

    // Movimiento de la bola al inicio.

    /* SIGUE FUNCIONANDOME MAL EL MOVIMIENTO Y NO ME DEJA DARLE AL ESPACIO ANTES DE EMPEZAR A MOVER LA BOLA*/
    public void MoveBall()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            inputSpace = true;
        }


        //Debug.Log("Apriete la barra espaciadora para empezar.");
        if (transform.position.y == initialPosition.y && inputSpace == true )
        {
            Debug.Log("Se ha apretado la barra espaciadora.");
            /*velocity.x = Random.Range(-1, 1);
            velocity.y = 1 * 20;*/
            rbBola.velocity = velocity;
            //rbBola.AddForce(velocity * speed);
            inputSpace = false;
            transform.parent = juego.transform;
        }

        /* De la entrega anterior.
         * velocity = Vector2.down * speed; // La bola cae
           rbBola.velocity = velocity;
        */
    }
    

    private void Caida()
    {
        // Cuando cae por debajo de la plataforma
        if (gameObject.transform.position.y < jugador.transform.position.y - 3) // afegeixo tres per si li dona de costat
        {
            if (FindAnyObjectByType<GameManager>().LoseLive())
            {
                // Cada vez que cae, suena este audio del array.
                audioSource.clip = clipsAudio[0];
                audioSource.Play();

                transform.position = initialPosition; // canviem la posici� pel vector que nosaltres hem fet.

                rbBola.velocity = Vector2.zero; 

                jugador.transform.position = jugadorInitialPosition;

                // Que vuelva a tener el padre.
                transform.parent = padre;

                SetCombo();
            }
        }
    }

    
    // El movimiento no es el mejor posible.
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if (hitCooldown > 0) return;
        //hitCooldown = 0.05f;

        /*Vector2 normalColision = collision.contacts[0].normal; // direcci�n

        if (Mathf.Abs(normalColision.x) >= 0.5f)
        {
            velocity.x *= -1;
        }
        else
        {
            velocity.y *= -1;
        }

        // Compruebo si quiero rebotar contra ese cuerpo.
        // Si colisiona con algo que no sea la pala, porque tiene otro movimiento.
        if (collision.otherCollider && !collision.collider.GetComponent<Movement>())
        {
            // Para que la pelota no me haga las cosas raras de antes.
            /* Vector2 direction = transform.position - collision.transform.position;
             direction.Normalize();

             rbBola.velocity = direction * speed;
            //No me va bien

            // Cojo la referencia rigidbody y le invierto la velocidad.
            //velocity = Vector2.Reflect(velocity, directionBall);
            // Debug.Log(hitCooldown + " --> " + normalColision);

            //velocity = normalColision * velocity.magnitude; // multiplicamos la direcci�n por la magnitud para no perder velocidad.
        }*/


        /*if (collision.collider.GetComponent<Movement>())
        {
            // Si la bola colisiona contra la pala, el combo es falso. La primera vez el combo sigue cierto.
            combo = false;
            numCombo = -5;

            // Lo de abajo es lo que falla, me da 0.
            //Vector3 distanceToCenter = collision.otherCollider.transform.position - transform.position;
            // Arreglado
           /* Vector3 distanceToCenter = collision.transform.position - transform.position;

            // No se mueve en la dirección que debería.

            float xDist = distanceToCenter.x;

            velocity.x = Mathf.Lerp(0, maxXVelocity, xDist / maxXDistance);

            rbBola.velocity = velocity;

            Debug.Log(distanceToCenter);

            //rbBola.velocity = direction * speed;
        }*/

        
         if (collision.gameObject.CompareTag("Ladrillo"))
         {
            audioSource.clip = clipsAudio[3];
            audioSource.Play();
            numBricksDestruidos++;
            //Debug.Log(numBricksDestruidos + " destruido");
            Destroy(collision.gameObject);
            

            combo = true;
            if (combo)
            {
                numCombo += 5;
            }
            AddPoints(10 + numCombo); // Si hay un combo, suma a los puntos normalmente.
            Vector3 positionInstantiate = new Vector3(collision.transform.position.x, collision.transform.position.y, collision.transform.position.z);
            Instantiate(particles, positionInstantiate, Quaternion.identity, particlesParent);
            
        }

        if (collision.gameObject.CompareTag("Ladrillo2Vida"))
        {
            // Aclarar el color del ladrillo.
            Ladrillo vidas2 = collision.gameObject.GetComponent<Ladrillo>();
            vidas2.RestaVidas();
            SpriteRenderer renderer = collision.gameObject.GetComponent<SpriteRenderer>();
            renderer.color = HexToColor("#ECDCD1");

            // Poner sonido cuando golpea 1 vez y otro cuando destruye.
            audioSource.clip = clipsAudio[2];
            audioSource.Play();

            if (vidas2.GetVidas() == 0)
            {
                audioSource.clip = clipsAudio[3];
                audioSource.Play();
                Destroy(collision.gameObject); // Se tienen que destruir 
                numBricksDestruidos++;
                combo = true;
                if (combo)
                {
                    numCombo += 5;
                }
                AddPoints(10 + numCombo);
            }
        }

        if (collision.gameObject.CompareTag("Ladrillo3Vida"))
        {
            Ladrillo vidas2 = collision.gameObject.GetComponent<Ladrillo>();
            vidas2.RestaVidas();
            SpriteRenderer renderer = collision.gameObject.GetComponent<SpriteRenderer>();


            if (vidas2.GetVidas() == 2)
            {
                audioSource.clip = clipsAudio[1];
                audioSource.Play();
                renderer.color = HexToColor("#81C2E0");
            }
            else if (vidas2.GetVidas() == 1)
            {
                audioSource.clip = clipsAudio[2];
                audioSource.Play();
                renderer.color = HexToColor("#B9D7E5");
            }
            else
            {
                audioSource.clip = clipsAudio[3];
                audioSource.Play();
                //collision.gameObject.SetActive(false);
                Destroy(collision.gameObject);
                numBricksDestruidos++;
                combo = true;
                if (combo)
                {
                    numCombo += 5;
                }
                AddPoints(10 + numCombo);
            }
        }
    }

    public void AddPoints(int punts)
    {
        puntos.SetPoints(punts); // Sumar los puntos recibidos al total
        pointsText.text = puntos.GetPoints().ToString(); // Actualizar el texto de puntos
    }

    private Color HexToColor(string hex)
    {
        Color color = Color.black;
        if (ColorUtility.TryParseHtmlString(hex, out color))
        {
            return color;
        }
        return color;
    }

    public void SetCombo()
    {
        combo = false;
        numCombo = -5;
    }
   
}
