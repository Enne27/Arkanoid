using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField] GameObject bolaJugadorPrefab;
    [SerializeField] Ball bolaJugadorScript;
    [SerializeField] Transform juego;
    [SerializeField] GameObject powerUp;

    private bool toInstance;

    // Si hace un combo de 7, consigue un powerUp.
    private void InstantiatePowerUp()
    {
        if (bolaJugadorScript.numCombo == 35 && toInstance) // 35 en lugar de 7 porque el combo lo voy poniendo de 5 en 5. El equivalente a darle 7 veces es 35.
        {   
            toInstance = false;
            Vector3 positionInstantiate = new Vector3(bolaJugadorPrefab.transform.position.x, bolaJugadorPrefab.transform.position.y, bolaJugadorPrefab.transform.position.z);
            Instantiate(powerUp, positionInstantiate, Quaternion.identity, juego);

            //rbPowerUp.velocity = Vector2.down * 1.5f * Time.deltaTime;
        }
    }
    
    private void Start()
    {
        toInstance = true;
    }
    private void Update()
    {
        InstantiatePowerUp();
    }
}
