using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEngine.AI;
using UnityEngine.UIElements;


// Mejor que añadir referencia al prefab en el otro, este script.

public class BloqueEditable : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteRenderer;
    public int type;

    public int GetTipo()
    {
        return type;
    }

    private void OnMouseDown()
    {
        if(type == -1) // Pasa el ladrillo a rompe 1 golpe
        {
            // cambiamos el tipo y el color correspondiente mediante se va editando para que el usuario pueda ver lo que hace.
            type++;
            spriteRenderer.color = HexToColor("#EFA09B");
        }
        else if (type == 0) // así siguiente
        {  
            type++;
            spriteRenderer.color = HexToColor("#E7C7B0");
        }
        else if (type == 1)
        {
            type++;
            spriteRenderer.color = HexToColor("#51A4CA");
        }
        else if (type == 2)
        {
            type++;
            spriteRenderer.color = HexToColor("#665054");
        }
        else if (type == 3)
        {
            // Así vuelve al ladrillo invisible.
            type = -1;
            spriteRenderer.color = HexToColor("#92B2A7");
        }
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
}
