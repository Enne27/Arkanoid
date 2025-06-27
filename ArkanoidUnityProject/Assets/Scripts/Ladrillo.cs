using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.AI;


public class Ladrillo : MonoBehaviour
{
    /* [SerializeField] TMPro.TextMeshProUGUI pointsText;
     [SerializeField] Puntuacion puntos;
     public Ball bola; // Cojo una referencia a la bola, para el combo.
                       // int points;
     public GameObject bolaG;
     //int vidas = 2;
     Material mat;

     private void Start(){
         bola = bolaG.GetComponent<Ball>();
         mat = GetComponent<Renderer>().material;
     }

     private void OnCollisionEnter2D(Collision2D collision)
     {
         if (collision.gameObject.CompareTag("Ball"))
         {
             //vidas--;

             /*if (vidas <= 0)
             {
                 gameObject.SetActive(false);
            // }

             bola.combo = true;
             if(bola.combo){
                 bola.numCombo += 5;
             }
             AddPoints(10 + bola.numCombo); // Si hay un combo, suma a los puntos normalmente.
             Color newColor = mat.color;
             newColor.a -= 0.5f;
             mat.color = newColor;
         }
     }
     private void AddPoints(int punts)
     {
         puntos.SetPoints(punts); // Sumar los puntos recibidos al total
         pointsText.text = puntos.GetPoints().ToString(); // Actualizar el texto de puntos
     }*/


    // Hay que hacer que cambie el color. Más transparente en chocar.
    // cubo1.GetComponent<Renderer>().material.color.a = 0.5f;

    public int vidas;
    SpriteRenderer renderer;

    private void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
    }

    public int GetVidas()
    {
        return vidas;
    }

    public void RestaVidas()
    {
        this.vidas --;
    }

    
    
}
