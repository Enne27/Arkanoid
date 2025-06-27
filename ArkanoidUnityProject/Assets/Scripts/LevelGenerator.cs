using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


// Es diferente al script de bloques porque ese tiene otra info.
[System.Serializable]
public class BrickData
{
    public int tipo;
    public Vector2 position;
}

// Otra clase para TODA la informaci�n del nivel (referente a los ladrillos).
[System.Serializable]
public class LevelInfo
{
    public List<BrickData> bricks;
    
}

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] GameObject[] brickPrefabs;
    [SerializeField] Transform ladrillosPadre; // Se generan a partir de la posici�n del padre, no la de Unity.
    private string levelPath;
    private string[] levelName;

    private float numBricks = 0;
    private float numBricksIndestructibles = 0;

    public string level;

    public float getNumBricks()
    {
        return numBricks-numBricksIndestructibles;
    }

    public void SetNumBricks()
    {
        numBricks = 0;
    }

    public float GetNumBircksIndestructibles()
    {
        return numBricksIndestructibles;
    }
    public void SetNumBircksIndestructibles()
    {
       numBricksIndestructibles++;
    }


    // Start is called before the first frame update
    void Start()
    {
        levelName = new string[4];
        levelPath = Application.persistentDataPath + @"\Levels";

        level = "SaveData.xml";

        levelName[0] = levelPath + @"\Level_1.json";
        levelName[1] = levelPath + @"\Level_2.json";
        levelName[2] = levelPath + @"\Level_3.json";
        levelName[3] = levelPath + @"\LevelCustomized.json";




        if (!Directory.Exists(levelPath))
        {
            Directory.CreateDirectory(levelPath);
            GenerateLevelInfo();
        }
        
    }

    void GenerateLevelInfo()
    {
        // Para las posiciones.
        float x;
        float y;
        int i;
        for(i = 0; i < levelName.Length; i++)
        {
            switch (i)
            {
                case 0: 
                     LevelInfo levelInfo = new LevelInfo();
                     levelInfo.bricks = new List<BrickData>();
        
                    // Posiciones de los ladrillos y el tipo.
                    for (y = 3; y > -1; y--)
                    {
                        for (x = -7f; x < 8f; x++)
                        {
                             BrickData brickData = new BrickData();
                             brickData.tipo = 0; // Tipo de bloque (0: Rompe con 1 golpe, 1: Rompe con 2 golpes, 2: rompe con 3 golpes, 3: irrompible)
                             brickData.position = new Vector2(x, y); 
                             levelInfo.bricks.Add(brickData);
                             x++;
                        }
                    }
                    // Generar json
                    string jsonData = JsonUtility.ToJson(levelInfo);
                    File.WriteAllText(levelName[0], jsonData);

                   
                    Debug.Log(level);
                    break;

                case 1:
                    LevelInfo levelInfo2 = new LevelInfo();
                    levelInfo2.bricks = new List<BrickData>();

                    // Posiciones de los ladrillos y el tipo.
                    for (y = 3; y > -1; y--)
                    {
                        for (x = -7f; x < 8f; x++)
                        {
                            BrickData brickData = new BrickData();
                            if ((x == 3 && y == 3) || (x == -5 && y == 0) || (x == 5 && y == 2) || (x == -3 && y == 2) || (x == 7 && y == 3))
                            {
                                brickData.tipo = 1; // Tipo de bloque (0: Rompe con 1 golpe, 1: Rompe con 2 golpes, 4: Rompe con 3 golpes 3: Indestructible)
                            }
                            else if ((x == -7 && y == 3) || (x == 3 && y == 1) || (x == -5 && y == 1) || (x == 7 && y == 0) || (x == -5 && y == 2))
                            {
                                brickData.tipo = 2;
                            }
                            else
                            {
                                brickData.tipo = 0;
                            }
                            brickData.position = new Vector2(x, y);
                            levelInfo2.bricks.Add(brickData);
                            x++;
                        }
                    }

                    // Generar json
                    string jsonData2 = JsonUtility.ToJson(levelInfo2);
                    File.WriteAllText(levelName[1], jsonData2);

                   
                    break;

                case 2:
                    LevelInfo levelInfo3 = new LevelInfo();
                    levelInfo3.bricks = new List<BrickData>();

                    // Posiciones de los ladrillos y el tipo.
                    for (y = 3; y > -1; y--)
                    {
                        for (x = -7f; x < 8f; x++)
                        {
                            BrickData brickData = new BrickData();
                            if ((x == 3 && y == 3) || (x == -5 && y == 0) || (x == 5 && y == 2) || (x == -3 && y == 2) || (x == 7 && y == 3))
                            {
                                brickData.tipo = 1; // Tipo de bloque (0: Rompe con 1 golpe, 1: Rompe con 2 golpes, 4: Rompe con 3 golpes 3: Indestructible)
                            }
                            else if ((x == -7 && y == 3) || (x == 3 && y == 1) || (x == -5 && y == 1) || (x == 7 && y == 0) || (x == -5 && y == 2))
                            {
                                brickData.tipo = 2;
                            }
                            else if ((x == 5 && y == 3) || (x == -7 && y == 2) || (x == 7 && y == 1) || (x == -3 && y == 1))
                            {
                                brickData.tipo = 3;
                                SetNumBircksIndestructibles();
                            }
                            else
                            {
                                brickData.tipo = 0;
                            }
                            brickData.position = new Vector2(x, y);
                            levelInfo3.bricks.Add(brickData);
                            x++;

                            
                        }
                    }

                    // Generar json
                    string jsonData3 = JsonUtility.ToJson(levelInfo3);
                    File.WriteAllText(levelName[2], jsonData3);
                    break;
            }
        }
    }

    // Funci�n del nivel 1
    public void Level1()
    {
        string jsonData = File.ReadAllText(levelName[0]);

        LevelInfo levelInfo = JsonUtility.FromJson<LevelInfo>(jsonData);

        foreach (BrickData brickData in levelInfo.bricks)
        {
            // Posici�n de cada ladrillo.
            Vector3 brickPosition = new Vector3(brickData.position.x, brickData.position.y, 0f);
            GameObject prefabLadrillo = brickPrefabs[brickData.tipo]; // seg�n el tipo

            GameObject brick = Instantiate(prefabLadrillo, brickPosition, Quaternion.identity, ladrillosPadre);
            brick.GetComponent<SpriteRenderer>().color = new Color(0.9372549019607843f, 0.6274509803f, 0.60784313725f); //Para cambiar el color. Solo acepta decimales.
            
            numBricks++;
        }

        level = "SaveDataLevel1.xml";
        Debug.Log(level);
    }

    public void Level2()
    {
        string jsonData = File.ReadAllText(levelName[1]);

        LevelInfo levelInfo = JsonUtility.FromJson<LevelInfo>(jsonData);

        foreach (BrickData brickData in levelInfo.bricks)
        {
            // Posici�n de cada ladrillo.
            Vector3 brickPosition = new Vector3(brickData.position.x, brickData.position.y, 0f);
            GameObject prefabLadrillo = brickPrefabs[brickData.tipo]; // seg�n el tipo

            GameObject brick = Instantiate(prefabLadrillo, brickPosition, Quaternion.identity, ladrillosPadre);

            numBricks++;
        }

        level = "SaveDataLevel2.xml";

        Debug.Log(level);
    }

    public void Level3()
    {
        string jsonData = File.ReadAllText(levelName[2]);

        LevelInfo levelInfo = JsonUtility.FromJson<LevelInfo>(jsonData);

        foreach (BrickData brickData in levelInfo.bricks)
        {
            // Posici�n de cada ladrillo.
            Vector3 brickPosition = new Vector3(brickData.position.x, brickData.position.y, 0f);
            GameObject prefabLadrillo = brickPrefabs[brickData.tipo]; // seg�n el tipo

            GameObject brick = Instantiate(prefabLadrillo, brickPosition, Quaternion.identity, ladrillosPadre);
            
            if (brickData.tipo == 3)
            {
                SetNumBircksIndestructibles();
            }
             numBricks++;
        }

        level = "SaveDataLevel3.xml";
        Debug.Log(level);
    }

    public void CustomizedLevel()
    {
        string jsonData = File.ReadAllText(levelName[3]);

        LevelInfo levelInfo = JsonUtility.FromJson<LevelInfo>(jsonData);

        foreach (BrickData brickData in levelInfo.bricks)
        {
            // Posici�n de cada ladrillo.
            Vector3 brickPosition = new Vector3(brickData.position.x, brickData.position.y, 0f);
            GameObject prefabLadrillo = brickPrefabs[brickData.tipo]; // seg�n el tipo;
            GameObject brick = Instantiate(prefabLadrillo, brickPosition, Quaternion.identity, ladrillosPadre);
            
            if(numBricks == 3)
            {
                SetNumBircksIndestructibles();
               
            }
            numBricks++;
        }

        level = "SaveDataLevelCustomized.xml";

        Debug.Log(level);
    }

    public string GetLevelFile()
    {
        return level;
    }


}