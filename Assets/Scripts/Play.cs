using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.UI;

public class Play : MonoBehaviour 
{
	[SerializeField]
	Transform Jugador;
    public GameObject suelo;
    public Text score;
    string[] confText = new string[12];
    public AudioSource audioSFX, deadBeepSFX, gwSFX;
    float volume;
    int interval = 1, 
        scoreNum = 0;
    float nextTime = 0;
    bool pause = false;

    void Start() {
        DontDestroyOnLoad(audioSFX);
		Screen.sleepTimeout = SleepTimeout.NeverSleep;

        if (File.Exists(Application.persistentDataPath + "/config.ini"))
        {
            confText = File.ReadAllLines(Application.persistentDataPath + "/config.ini");
        }
        else
        {
            confText[0] = "Language";
            confText[1] = "en"; //LENGUAJE
            confText[2] = "ColourBlind";
            confText[3] = "0"; //DALTONISMO
            confText[4] = "audio";
            confText[5] = "100"; //AUDIO
            confText[6] = "SFX";
            confText[7] = "1"; //SFX habilitado
            confText[8] = "size";
            confText[9] = "1";
            confText[10] = "extra";
            confText[11] = "locked";
            File.WriteAllLines(Application.persistentDataPath + "/config.ini", confText);
            
        }
        //DALTONISMO
        if (confText[3] == "1")
        {
            suelo.GetComponent<Renderer>().material.color = Color.blue;
        }
        else
        {
            suelo.GetComponent<Renderer>().material.color = Color.green;
        }
        //AUDIO
        volume = float.Parse(confText[5]) / 100;
        setAudio(volume);

        //SFX
        if (confText[7] == "1")
        {
            audioSFX.enabled = true;
            deadBeepSFX.enabled = true;
            gwSFX.enabled = true;
        }
        else
        {
            audioSFX.enabled = false;
            deadBeepSFX.enabled = false;
            gwSFX.enabled = false;
        }

        scoreNum = 0;

    }

    Vector3 prevPos, prevPosJugador;
    void Update()
    {
        if (!pause) {
            
            transform.position = new Vector3(Jugador.position.x + 6, transform.position.y, transform.position.z);
            if (prevPos.x > transform.position.x && scoreNum > 20)
            {
                reproduceDeadBeep();
                scoreNum = 0;
            }

            if(prevPosJugador.z!=Jugador.transform.position.z && scoreNum > 10)
            {
                gwSFX.Play();
            }

            prevPos = transform.position;
            prevPosJugador = Jugador.transform.position;
            if (Time.time >= nextTime)
            {
                nextTime += interval;
                scoreNum += 10;
                score.text = score.text = "Score: " + scoreNum;
            }
        }

    }
    void reproduceDeadBeep()
    {
        deadBeepSFX.Play();
    }

    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void setAudio(float c)
    {
        //audio.volume = c;
        audioSFX.volume = c;
        deadBeepSFX.volume = c;
        gwSFX.volume = c;
    }

    public void reproduceAudioSFX()
    {
        audioSFX.Play();
    }

    public void makePause()
    {
        pause = !pause;
    }


}
