using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using System.Xml;
using Unity.Burst.Intrinsics;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.AI;


public class GameManager : MonoBehaviour
{
    public int lives;

    [SerializeField] GameObject menu; // Pillamos la referencia en el inspector
    [SerializeField] GameObject gameOver;
    [SerializeField] GameObject victory;
    [SerializeField] GameObject juego;
    [SerializeField] Ball bola;
    [SerializeField] GameObject ladrilloPadre;
    [SerializeField] GameObject selectorNiveles;

    
    [SerializeField] TMPro.TextMeshProUGUI pointsText;

    [SerializeField] Timer timer = new Timer();
    //TMPro.TextMeshProUGUI tiempo;

    [SerializeField] TextMeshProUGUI bestGame;
    [SerializeField] Puntuacion puntos;
    Puntuacion puntosGuardados = new Puntuacion(0);
    private Timer tiempoGuardado = new Timer();

    string SaveGame;
    //string pathSaveInfo;
    //string jsonArxiu;
    //string saveGameDataPath;
    //string saveGameFolderPath;

    XmlDocument doc, doc1;

    LevelGenerator levelGenerator;

    [SerializeField] AudioClip[] clipsAudioManager;
    AudioSource audioSource2;


    private void Start()
    {
        SaveGame = Application.persistentDataPath + "/SaveGame";
        //pathSaveInfo = SaveGame + "/InfoGuardada.json";
        //saveGameFolderPath = Application.persistentDataPath + @"\SaveGame";

        if (!Directory.Exists(SaveGame))
        {
            Directory.CreateDirectory(SaveGame);
        }
        else
        {
            Debug.Log("No s'ha creat el directori perquè ja existeix.");
        }

        levelGenerator = FindObjectOfType<LevelGenerator>();

        audioSource2 = GetComponent<AudioSource>();
    }

    public void SelectLevel()
    {
        gameOver.SetActive(false);
        //victory.SetActive(false);
        juego.SetActive(false);
        selectorNiveles.SetActive(true);
        menu.SetActive(false);
    }

    public void StartGame()
    {
        menu.SetActive(false);
        selectorNiveles.SetActive(false);
        juego.SetActive(true);
        //gameOver.SetActive(false);
        //victory.SetActive(false);
        //bola.MoveBall();
        

        for (int i = 0; i < ladrilloPadre.transform.childCount; i++)
        {
            ladrilloPadre.transform.GetChild(i).gameObject.SetActive(true);
        }

        lives = 5;
         
        ResetPoints(); // Cuando se inicie el juego, se pondr� a zero la puntuaci�n otra vez.
        timer.SetTime();
       // timePlaying();           Por alg�n motivo no funciona, me gustar�a hablarlo en clase, ya que ser�a mejor hacerlo por c�digo y no por interfaz.
        bola.setNumBricksDestruidos();
    }

    /*private void timePlaying()
    {
        //timer.setIsPlaying(true);
        tiempo.text = timer.getTime().ToString("F2");
    }*/

    public void ResetPoints()
    {
        puntos.RestartPoints();
        pointsText.text = puntos.GetPoints().ToString(); // Lo hago aqu� para el inicio del juego, pero tambi�n debe hacerse en cada frame.
        bola.SetCombo();
    }

    public bool LoseLive()
    {
        bool vivo = true;

        if (lives > 0)
        {
            lives--;
           
        }
        
        if(lives <= 0) // per si de cas, menor o igual
        {
            vivo = false;
            GameOver();
            audioSource2.clip = clipsAudioManager[0];
            audioSource2.Play();
        }

        return vivo;
    }


    private void GameOver()
    {
        menu.SetActive(true);
        gameOver.SetActive(true);
        timer.SetIsPlaying(false);
        juego.SetActive(false);
        //timer.SetTime();
        //guardar los datos de tiempo y puntos
        Guardar();
        levelGenerator.SetNumBricks(); // Tienen que setearse otra vez a 0, sino nunca se puede ganar después de la segunda vez.
    }

    private void Victory()
    {
        // Sonidos de victoria
        audioSource2.clip = clipsAudioManager[1];
        audioSource2.Play();

        victory.SetActive(true);
        menu.SetActive(true);
        timer.SetIsPlaying(false);
        juego.SetActive(false);
        //timer.SetTime();
        Guardar();
        bola.setNumBricksDestruidos();
        bola.SetInitialPosition();
        levelGenerator.SetNumBricks();
    }

    public void CheckLadrillosActivos()
    {
        Debug.Log("hijosLadrillo: " + ladrilloPadre.transform.childCount + "\nnumBricks: " + levelGenerator.getNumBricks());
        if (bola.getNumBricksDestruidos() == levelGenerator.getNumBricks() && bola.getNumBricksDestruidos() != 0)
        {
            Victory();
            Debug.Log("Victoria");
        }
    }

    public void Guardar()
    {
        string saveGameFolderPath = SaveGame;
        string saveGameDataPath = saveGameFolderPath + @"\" + levelGenerator.GetLevelFile();

        DataSave gameData = new DataSave();
        gameData.points = puntos.GetPoints();
        gameData.time = timer.GetTime();

        // Per veure el temps guardat s'ha de carregar el del fitxer.
        /*if (File.Exists(saveGameDataPath))
        {
            doc1 = new XmlDocument();
            doc1.Load(saveGameDataPath);
            XmlNodeList lista = doc1.GetElementsByTagName("BestGame");

            foreach (XmlNode xmlNode in lista) // vidasa cada nodo en lista
            {
                float time = 0f;

                foreach (XmlNode child in xmlNode.ChildNodes)
                {
                    if (child.Name == "time")
                    {
                        time = float.vidase(child.InnerText);
                        tiempoGuardado.SetTiempoGuardado(time);
                    }
                }
            }
        }*/

        if (puntos.GetPoints() > puntosGuardados.GetPoints() /*|| gameData.time < tiempoGuardado.GetTime()*/) // Faltaria si el temps �s menor i els punts iguals.
        {
            doc = new XmlDocument();
            XmlElement root = doc.CreateElement("BestGame");
            doc.AppendChild(root);

            XmlElement points = doc.CreateElement("points");
            points.InnerText = gameData.points.ToString();
            root.AppendChild(points);

            XmlElement time = doc.CreateElement("time");
            time.InnerText = gameData.time.ToString();
            root.AppendChild(time);

            doc.Save(saveGameDataPath);

            Debug.Log("La informació s'ha guardat correctament.");
        }
        
    }

    public void Cargar()
    {
        string saveGameFolderPath = SaveGame;

        string ruta = levelGenerator.GetLevelFile();
        Debug.Log("La ruta es: " + ruta);
        string saveGameDataPath = saveGameFolderPath + @"\" + ruta;

       // doc = new XmlDocument();

        if (File.Exists(saveGameDataPath))
        {
            Debug.Log("Informació carregada de: " + saveGameDataPath);
            XmlDocument xmlDoc = new XmlDocument();

            xmlDoc.Load(saveGameDataPath);

            XmlNodeList lista = xmlDoc.GetElementsByTagName("BestGame"); 

            
            foreach (XmlNode xmlNode in lista) // para cada nodo en lista
            {
                int puntos = 0;
                float time= 0f;

                foreach (XmlNode child in xmlNode.ChildNodes)
                {
                    if (child.Name == "points")
                    {
                        puntos = int.Parse(child.InnerText);
                    }
                    else if(child.Name == "time")
                    {
                        time = float.Parse(child.InnerText);
                        tiempoGuardado.SetTiempoGuardado(time);
                    }
                }
                
                bestGame.text = "Points: " + puntos.ToString() + "\nTime: " + time.ToString("F2");

                /*jsonArxiu = File.ReadAllText(pathSaveInfo);
                DataSave gameData = JsonUtility.FromJson<DataSave>(jsonArxiu);
                bestGame.text = "Points: " + gameData.points.ToString() + "\n" + "Time: " + gameData.time.ToString("F2");*/
            }
        }
        else
        {
            Debug.Log("L'arxiu XML no existeix.");
        }
    }

    public void Exit()
    {
        Application.Quit();
    }

    private void Update()
    {
        CheckLadrillosActivos();
    }

    public void ChangeScene()
    {
        SceneManager.LoadScene("EditorLevel");
    }
}
