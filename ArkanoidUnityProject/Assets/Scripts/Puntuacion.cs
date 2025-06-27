using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Puntuacion : MonoBehaviour
{

    public int points;

    public Puntuacion(int points)
    {
        this.points = points;
    }

    public void RestartPoints()
    {
        this.points = 0;
    }

    public void SetPoints(int puntos)
    {
        this.points += puntos;
    }

    public int GetPoints()
    {
        return this.points;
    }
    
}
