using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
using System.Xml;

public class MainMenu : MonoBehaviour {

    public Text scriptTextPlay, scriptTextOptions, scriptTextHIscore, scriptTextAbout, scriptExtraText;
    int langind = 0;
    string[] options = new string[2] { "OPTIONS", "OPCIONES" },
             play = new string[2] { "PLAY", "JUGAR" },
             about = new string[2] { "About", "Acerca de" };
    string currlang;
    string[] confText = new string[12];
    string[] hiscore = new string[6] {"1. Player", "10000", "2. Player2", "5000", "3. Player3", "100"};
    public AudioSource audioSFX;
    public Image background;
    static string dataStr = "";
    static string level = "locked";

    // Use this for initialization
    void Start()
    {
        //File.Delete(Application.persistentDataPath + "/config.ini");
        DontDestroyOnLoad(audioSFX);
        float volume;

        if (File.Exists(Application.persistentDataPath + "/config.ini"))
        {
            confText = File.ReadAllLines(Application.persistentDataPath + "/config.ini");
        }
        else
        {
            if (Application.systemLanguage.ToString() == "Spanish")
            {
                confText[1] = "es"; //LENGUAJE
            } else if(Application.systemLanguage.ToString() == "English")
            {
                confText[1] = "en";
            }
            else
            {
                confText[1] = "en";
            }

            confText[0] = "Language";
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
            //MessageBox.Show("Bienvenido", "Para jugar debes utilizar el móvil en posición vertical y ladearlo para esquivar los obstáculos. En opciones...", "Ok");
            new MobileNativeMessage("Bienvenido a MoveBall!", "Este es el menú principal donde podrás dirigirte al menú Opciones para cambiar la configuración o empezar a jugar sobre el botón Jugar ", "OK");
          
        }

        //MULTILENGUAJE
        currlang = confText[1];
        //DALTONISMO
        if (confText[3] == "1")
        {
            background.color = Color.green + Color.red + Color.blue;
        }
        else
        {
            background.color = Color.green;
        }
        //AUDIO
        volume = float.Parse(confText[5]) / 100;
        setAudio(volume);

        //SFX
        if (confText[7] == "1")
        {
            audioSFX.enabled = true;
        }
        else
        {
            audioSFX.enabled = false;
        }
        ChangeLang(currlang);

        setSize();


        //EasyCodeScanner.Initialize();
        //EasyCodeScanner.OnScannerMessage += onScannerMessage;
        //EasyCodeScanner.OnScannerEvent += onScannerEvent;
        //EasyCodeScanner.OnDecoderMessage += onDecoderMessage;
        level = confText[11];
        if (level == "unlock")
            scriptExtraText.text = "Extra";

        getGeographicalCoordinates();
    }

    public void ChangeLang(string aux)
    {
        if (aux == "en")
            langind = 0;
        else if (aux == "es")
            langind = 1;
        scriptTextPlay.text = play[langind];
        scriptTextOptions.text = options[langind];
        scriptTextAbout.text = about[langind];

    }
    
    public void aboutThis()
    {
        new MobileNativeMessage("About", "Version 2.1\nCreado por: \n - Alejandro Simón Sánchez \n - Luis Mendoza Montero");
    }

    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void setAudio(float c)
    {
        //audio.volume = c;
        audioSFX.volume = c;
    }

    public void reproduceAudioSFX()
    {
        audioSFX.Play();
    }

    public void showHIscore()
    {
        new MobileNativeMessage("HIScores", hiscore[0] + "\n" + hiscore[1] + " " + playerPrefsKey + "\n" + hiscore[2] + "\n" + hiscore[3] + " " + playerPrefsKey + "\n" + hiscore[4] + "\n" + hiscore[5] + " " + playerPrefsKey, "Ok");
    }

    void setSize()
    {
        if (confText[9] == "1") {
            scriptTextPlay.fontSize=80;
            scriptTextOptions.fontSize=80;
            scriptTextHIscore.fontSize = 70;
            scriptTextAbout.fontSize = 70;
        }
        else
        {
            scriptTextPlay.fontSize = 70;
            scriptTextOptions.fontSize = 70;
            scriptTextHIscore.fontSize = 60;
            scriptTextAbout.fontSize = 60;
        }
    }

    void Update()
    {
    }

    public string playerPrefsKey = "Country";
    public void getGeographicalCoordinates()
    {
        if (Input.location.isEnabledByUser)
            StartCoroutine(getGeographicalCoordinatesCoroutine());
    }
    private IEnumerator getGeographicalCoordinatesCoroutine()
    {
        Input.location.Start();
        int maximumWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maximumWait > 0)
        {
            yield return new WaitForSeconds(1);
            maximumWait--;
        }
        if (maximumWait < 1 || Input.location.status == LocationServiceStatus.Failed)
        {
            Input.location.Stop();
            yield break;
        }
        float latitude = Input.location.lastData.latitude;
        float longitude = Input.location.lastData.longitude;
        //      Asakusa.
        //      float latitude = 35.71477f;
        //      float longitude = 139.79256f;
        Input.location.Stop();
        WWW www = new WWW("https://maps.googleapis.com/maps/api/geocode/xml?latlng=" + latitude + "," + longitude + "&sensor=true");
        yield return www;
        if (www.error != null) yield break;
        XmlDocument reverseGeocodeResult = new XmlDocument();
        reverseGeocodeResult.LoadXml(www.text);
        if (reverseGeocodeResult.GetElementsByTagName("status").Item(0).ChildNodes.Item(0).Value != "OK") yield break;
        string countryCode = null;
        bool countryFound = false;
        foreach (XmlNode eachAdressComponent in reverseGeocodeResult.GetElementsByTagName("result").Item(0).ChildNodes)
        {
            if (eachAdressComponent.Name == "address_component")
            {
                foreach (XmlNode eachAddressAttribute in eachAdressComponent.ChildNodes)
                {
                    if (eachAddressAttribute.Name == "short_name") countryCode = eachAddressAttribute.FirstChild.Value;
                    if (eachAddressAttribute.Name == "type" && eachAddressAttribute.FirstChild.Value == "country")
                        countryFound = true;
                }
                if (countryFound) break;
            }
        }

        if (countryFound && countryCode != null)
            PlayerPrefs.SetString(playerPrefsKey, countryCode);
        playerPrefsKey = countryCode;
    }

}
