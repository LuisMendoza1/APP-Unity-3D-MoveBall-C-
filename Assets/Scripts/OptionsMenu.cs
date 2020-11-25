using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OptionsMenu : MonoBehaviour {
    
    public Text scriptTextOptions, scriptTextColourBlind, scriptTextSound, scriptTextSFXSound, scriptTextSize;
    int langind = 0;
    string[] options = new string[2] { "OPTIONS", "OPCIONES" },
             colourblind = new string[2] { "Colour-Blind", "Daltonismo" },
             sound = new string[2] { "Sound", "Sonido" },
             sfxsound = new string[2] {"SFX Effects", "Efectos SFX"},
             size = new string[2] {"Extra Size", "Escala Extra"};
                

    string currlang = "en";
    public Toggle toggleColour, toogleSFX, toogleSize;
    public Slider sliderVolume;
    string[] confText = new string[12];
    public Image background;
    public AudioSource audioSFX;


    // Use this for initialization
    void Start()
    {
        DontDestroyOnLoad(audioSFX);
        if (File.Exists(Application.persistentDataPath + "/config.ini")) { 
            confText = File.ReadAllLines(Application.persistentDataPath + "/config.ini");
        } else
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
        
        if (confText[3] == "1")
        {
            toggleColour.isOn = true;
            background.color = Color.green + Color.red + Color.blue;
        }
        //DALTONISMO
        else
        {
            toggleColour.isOn = false;
            background.color = Color.green;
        }
        //AUDIO
        sliderVolume.value = float.Parse(confText[5]) / 100;
        setAudioGlobal();

        //SFX
        if (confText[7] == "1")
        {
            toogleSFX.isOn = true;
        }
        else
        {
            toogleSFX.isOn = false;
        }
        endisableSFX();

        //MULTILENGUAJE
        currlang = confText[1];
        ChangeLang(currlang);

        // tamaño
        if (confText[9] == "1")
        {
            toogleSize.isOn = true;
        } else
        {
            toogleSize.isOn = false;
        }
        setSize();

    }

    public void ChangeLang(string aux)
    {
        if (aux == "en")
            langind = 0;
        else if (aux == "es")
            langind = 1;
        
        scriptTextOptions.text = options[langind];
        scriptTextColourBlind.text = colourblind[langind];
        scriptTextSound.text = sound[langind];
        scriptTextSFXSound.text = sfxsound[langind];
        scriptTextSize.text = size[langind];
        confText[1] = aux;
        File.WriteAllLines(Application.persistentDataPath + "/config.ini", confText);
    }

    

    public void setAudioGlobal()
    {
        string aux = "";
        confText[5]= aux+(sliderVolume.value*100);
        File.WriteAllLines(Application.persistentDataPath + "/config.ini", confText);
        setAudio(sliderVolume.value);
    }

    public void setAudio(float c)
    {
        //audio.volume = c;
        audioSFX.volume = c;
    }

    public void endisableSFX()
    {
        if(toogleSFX.isOn)
        {
            confText[7] = "1";
            audioSFX.enabled = true;
        }
        else
        {
            confText[7] = "0";
            audioSFX.enabled = false;
        }
        File.WriteAllLines(Application.persistentDataPath + "/config.ini", confText);
    }

    public void reproduceAudioSFX()
    {
        audioSFX.Play();
    }
    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);

    }

    public void setColourBlind()
    {

        if (toggleColour.isOn)
        {
            background.color = Color.green + Color.red + Color.blue;
            confText[3] = "1";
        }
        else
        {
            confText[3] = "0";
            background.color = Color.green;
        }
        File.WriteAllLines(Application.persistentDataPath + "/config.ini", confText);
    }

    public void setSize()
    {
        int original = 60, 
            less = 50;
        if (toogleSize.isOn)
        {
            confText[9] = "1";
            scriptTextColourBlind.fontSize = original;
            scriptTextSound.fontSize = original;
            scriptTextSFXSound.fontSize = original;
            scriptTextSize.fontSize = original;
        }
        else
        {
            confText[9] = "0";
            scriptTextColourBlind.fontSize = less;
            scriptTextSound.fontSize = less;
            scriptTextSFXSound.fontSize = less;
            scriptTextSize.fontSize = less;
        }
        File.WriteAllLines(Application.persistentDataPath + "/config.ini", confText);
    }
    
}
