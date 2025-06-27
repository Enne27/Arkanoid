using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{

    public float timer1 = 0f;

    public bool isPlaying;
    [SerializeField] TMPro.TextMeshProUGUI tiempo;

    public Timer() // Constructor
    {
    }
    public void SetIsPlaying(bool play)
    {
        this.isPlaying = play;
    }

    public float GetTime()
    {
        return this.timer1;
    }

    public void SetTime()
    {
        this.timer1 = 0f;
    }

    public void SetTiempoGuardado(float time)
    {
        this.timer1 = time;
    }

    // Start is called before the first frame update
    void Start()
    {
        this.isPlaying = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (this.isPlaying == true)
        {
            timer1 += Time.deltaTime;
            tiempo.text = timer1.ToString("F2");
        }
    }
}
