using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

public class Movement : MonoBehaviour
{
    public float movementForce; // Fuerza con la que se mueve. O se le pone valor aquí o en el inspector.
    private Rigidbody2D rb;

    private float inputs;
    private Vector2 movementVector;
    private float moveSpeed = 8;
    private float bounds = 5.7f;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }


// Update is called once per frame
    void Update()
    {
        inputs = Input.GetAxisRaw("Horizontal"); // A esto se referia con lo de Axis en el enunciado.

        // Hay que asegurarse en el inspector que la gravedad del Rigidbody esté desactivada y que movementForce sea mayor a 0.

        if (inputs == 1)   // Tecla D o flecha derecha
        {
            movementVector = Vector2.right;
        }

        else if (inputs == -1) // Tecla A o flecha izquierda
        {                     // Tiene que ser un elseif, sino, no se mueve a la derecha.
            movementVector = Vector2.left;
        }
        else // Si este else no se pone, la pala se mueve mientras no la tocamos
        {
            movementVector = Vector2.zero;
        }

        
        transform.position += new Vector3(inputs * moveSpeed * Time.deltaTime, 0f, 0f);

        Vector2 playerPosition = transform.position;
        // Limitar un valor entre otros dos.
        playerPosition.x = Mathf.Clamp(playerPosition.x + inputs * moveSpeed * Time.deltaTime, -bounds, bounds);
        transform.position = playerPosition;

        //rb.AddForce(100 * movementForce * Time.deltaTime * movementVector); // el deltaTime és petit, per això es multiplica per 100.

        // Después de esto, en el Rigidbody se tiene que activar en Constraints la posición y y la rotación z para que no rote ni se mueva para arriba.
        // Lo de que no se salga, ya se hace con esto.

    }
}
