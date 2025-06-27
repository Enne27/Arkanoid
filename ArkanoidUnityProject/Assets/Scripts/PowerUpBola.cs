using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PowerUpBola : MonoBehaviour
{
    // No puede estar en el mismo script que lo otro del power up porque los prefab serializefield que no sea prefab.
    private Ball bola;
    Rigidbody2D rbPowerUp;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            bola.AddPoints(130);
            Destroy(gameObject);
        }
    }


    private void Start()
    {
        bola = FindObjectOfType<Ball>();
        rbPowerUp = GetComponent<Rigidbody2D>();
    }

   
}
