using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class EditorLevel : MonoBehaviour
{
    // Este es el prefab de los ladrillos que hay en el editor.
    [SerializeField] GameObject EditorBlock;
    [SerializeField] Transform padreLadrillos;
    private string levelName;

    private void Start()
    {
        InstantiateBricks();
        levelName = Application.persistentDataPath + @"\Levels" + @"\LevelCustomized.json";
    }

    // Este método solo los instancia. Se modifican en el script de BloqueEditable.
    private void InstantiateBricks()
    {
        float x;
        float y;

        // Posiciones de los ladrillos y el tipo.
        for (y = 3; y > -1; y--)
        {
            for (x = -7f; x < 8f; x++)
            {
                Instantiate(EditorBlock, new Vector2(x, y), Quaternion.identity, padreLadrillos);
                x++;
            }
        }
    }

    public void Guardar()
    {
        LevelInfo levelInfoCustom = new LevelInfo();
        levelInfoCustom.bricks = new List<BrickData>();

        // Se cuentan los hijos del padre en el editor i se van guardando en el arrayList.
        for (int i = 0; i < padreLadrillos.childCount; i++)
        {
            BrickData block = new BrickData();
            // Cojemos el tipo de los ladrillos que había instanciados, para ello se necesita un método getTipo en el bloque editable.
            block.tipo = padreLadrillos.GetChild(i).gameObject.GetComponent<BloqueEditable>().GetTipo();
            block.position = padreLadrillos.GetChild(i).transform.position;
            if(block.tipo != -1)
            {
                levelInfoCustom.bricks.Add(block);
            }
            
        }

        // Se guarda desde aquí porque es mucho más complicado desde el LevelGenerator.
        string jsonDataCustom = JsonUtility.ToJson(levelInfoCustom);
        File.WriteAllText(levelName, jsonDataCustom);

        Debug.Log($"S'ha guardat la informació en {levelName}");
    }
}
