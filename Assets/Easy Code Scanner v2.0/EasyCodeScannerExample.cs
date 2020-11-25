using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System;
using UnityEngine.SceneManagement;
using System.IO;

public class EasyCodeScannerExample : MonoBehaviour {
	
	static string dataStr;
    static string expected;
	public Renderer PlaneRender;
    string[] confText = new string[12];
    int langind = 0;
    void Start () {
		dataStr = "";
		// Initialize EasyCodeScanner
		EasyCodeScanner.Initialize();
		
		//Register on Actions
		EasyCodeScanner.OnScannerMessage += onScannerMessage;
		EasyCodeScanner.OnScannerEvent += onScannerEvent;
		EasyCodeScanner.OnDecoderMessage += onDecoderMessage;

        Screen.orientation = ScreenOrientation.Portrait;
        confText = File.ReadAllLines(Application.persistentDataPath + "/config.ini");

        if (confText[1] == "en")
            langind = 0;
        else if (confText[1] == "es")
            langind = 1;
    }
	
	void OnDestroy() {
		
		//Unregister
		EasyCodeScanner.OnScannerMessage -= onScannerMessage;
		EasyCodeScanner.OnScannerEvent -= onScannerEvent;
		EasyCodeScanner.OnDecoderMessage -= onDecoderMessage;
	}
	
	public void Update() {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("MainMenu");
        }

    }

    public void launchScanner()
    {
        if (confText[11] == "locked")
        {
            string[] aux = new string[2] { "Visit moveballgame.wordpress.com and scan the QR code", "Visita moveballgame.wordpress.com y escanea el QR" };
            EasyCodeScanner.launchScanner(true, aux[langind], -1, true);
        }
        else
        {
            SceneManager.LoadScene("Play3");
        }
    }
	
	//Callback when returns from the scanner
	void onScannerMessage(string data){
		Debug.Log("EasyCodeScannerExample - onScannerMessage data=:"+data);
		dataStr = data;

        if (data == "unlock" || dataStr == "unlock")
        {
            confText[11] = "unlock";
            File.WriteAllLines(Application.persistentDataPath + "/config.ini", confText);

        }
        SceneManager.LoadScene("MainMenu");

        //Just to show case : get the image and display it on a Plane
        //Texture2D tex = EasyCodeScanner.getScannerImage(200, 200);
        //PlaneRender.material.mainTexture = tex;

        //Just to show case : decode a texture/image - refer to code list
        //EasyCodeScanner.decodeImage(-1, tex);
    }
	
	//Callback which notifies an event
	//param : "EVENT_OPENED", "EVENT_CLOSED"
	void onScannerEvent(string eventStr){
		Debug.Log("EasyCodeScannerExample - onScannerEvent:"+eventStr);

        if (eventStr == "unlock" || dataStr == "unlock")
        {
            confText[11] = "unlock";
            File.WriteAllLines(Application.persistentDataPath + "/config.ini", confText);

        }
        SceneManager.LoadScene("MainMenu");
    }
	
	//Callback when decodeImage has decoded the image/texture 
	void onDecoderMessage(string data){
		Debug.Log("EasyCodeScannerExample - onDecoderMessage data:"+data);
        dataStr = data;

        if (data == "unlock" || dataStr == "unlock")
        {
            confText[11] = "unlock";
            File.WriteAllLines(Application.persistentDataPath + "/config.ini", confText);

        }
        SceneManager.LoadScene("MainMenu");
	}
}