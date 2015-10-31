using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Menu_Logic : MonoBehaviour {

	public Dictionary<string,GameObject> UIObjects;
	
	void Start (){
		Debug.Log ("start() was called");
		Init ();
		MainPanel ();
	}

	void  Init(){
		Debug.Log ("init() was called");
		UIObjects = new Dictionary<string, GameObject>();
		GameObject[] objects = GameObject.FindGameObjectsWithTag("UIObjects");
		foreach (GameObject g in objects) {
			UIObjects.Add (g.name, g);
		}

		UIObjects ["Music"].GetComponent<AudioSource> ().volume = PlayerPrefs.GetFloat ("volumeMenu");
		UIObjects ["OptionPanelMusicSlider"].GetComponent<Slider> ().value =  PlayerPrefs.GetFloat ("volumeMenu")*10;
		UIObjects ["OptionPanelSfxSlider"].GetComponent<Slider> ().value  = PlayerPrefs.GetFloat ("volumeEffects")*10;
		UIObjects ["OptionPanelSfxHandleText"].GetComponent<Text>().text = ((int)(PlayerPrefs.GetFloat ("volumeEffects")*10)).ToString();
		UIObjects ["OptionPanelMusicHandleText"].GetComponent<Text>().text = ((int)(PlayerPrefs.GetFloat ("volumeMenu")*10)).ToString();
		
	}	

//	void Update(){
//	
//	}

	void MainPanel(){		
		Debug.Log ("MainPanel is on");
		MenuMod("MainPanel", Defined_Vars.ON);
		for (int i = 0; i < Defined_Vars.menus.Length; i++) {
						if (Defined_Vars.menus[i] != "MainPanel") MenuMod(Defined_Vars.menus[i],Defined_Vars.OFF);
		}
		UIObjects ["MainPanelSinglePlayerButton"].GetComponent<Button> ().onClick.AddListener (delegate { SinglePlayerPanel();});
		UIObjects ["MainPanelMultiPlayerButton"].GetComponent<Button> ().onClick.AddListener (delegate { MultiPlayerPanel(); });
		UIObjects ["MainPanelStudentInfoButton"].GetComponent<Button> ().onClick.AddListener (delegate { StudentInfoPanel();});
		UIObjects ["MainPanelOptionsButton"].GetComponent<Button> ().onClick.AddListener (delegate { OptionsPanel();});
	}

	public void MenuMod(string menuName , bool mod){
		UIObjects [menuName].SetActive(mod);
	}

	public void MultiPlayerPanel(){
		Debug.Log ("MultiPlayer is on");
		MenuMod("MultiPlayerPanel", Defined_Vars.ON);
		for (int i = 0; i < Defined_Vars.menus.Length; i++) {
			if (Defined_Vars.menus[i] != "MultiPlayerPanel") MenuMod(Defined_Vars.menus[i],Defined_Vars.OFF);
		}
		UIObjects ["MultiPlayerBackButton"].GetComponent<Button> ().onClick.AddListener (delegate { MainPanel();});
		UIObjects ["MultiPlayerOptionsButton"].GetComponent<Button> ().onClick.AddListener (delegate { OptionsPanel(); });
		UIObjects ["MultiPlayerPlayButton"].GetComponent<Button> ().onClick.AddListener (delegate { goToMulti();}); // redirect to the main menu for now
		Defined_Vars.Dollars = UIObjects ["MultiPlayerSlider"].GetComponent<Slider> ();
		Defined_Vars.Dollars.onValueChanged.AddListener(delegate { CountAndRound("MultiPlayerSliderText",Defined_Vars.Dollars.value);});
	}
	

	public void SinglePlayerPanel(){
		Debug.Log ("SinglePlayerPanel is on");
//		MenuMod("SinglePlayerPanel", Defined_Vars.ON);
//		for (int i = 0; i < Defined_Vars.menus.Length; i++) {
//			if (Defined_Vars.menus[i] != "SinglePlayerPanel") MenuMod(Defined_Vars.menus[i],Defined_Vars.OFF);
//		}
//		UIObjects ["SinglePlayerPanelBackButton"].GetComponent<Button> ().onClick.AddListener (delegate { MainPanel();});
		Application.LoadLevel("SC_gameCube");

	}
	public void goToMulti(){
		Application.LoadLevel ("SC_multiPlayer");
	}

	public void	StudentInfoPanel(){
		Debug.Log ("StudnetInfo is on");
		MenuMod ("StudentInfoPanel", Defined_Vars.ON);
		for (int i = 0; i < Defined_Vars.menus.Length; i++) {
			if (Defined_Vars.menus[i] != "StudentInfoPanel") MenuMod(Defined_Vars.menus[i],Defined_Vars.OFF);
		}
		UIObjects ["StudentInfoBackButton"].GetComponent<Button> ().onClick.AddListener (delegate { MainPanel();});
		UIObjects ["StudentInfoOptionsButton"].GetComponent<Button> ().onClick.AddListener (delegate { OptionsPanel();});
		UIObjects["LinkToSiteButton"].GetComponent<Button> ().onClick.AddListener (delegate { LinkToSite(Defined_Vars.Link); });	
	}

	public void	OptionsPanel(){
		Debug.Log ("OptionsPanel is on");
		MenuMod ("OptionsPanel", Defined_Vars.ON);
		for (int i = 0; i < Defined_Vars.menus.Length; i++) {
			if (Defined_Vars.menus[i] != "OptionsPanel") MenuMod(Defined_Vars.menus[i],Defined_Vars.OFF);
		}

		UIObjects ["OptionsBackButton"].GetComponent<Button> ().onClick.AddListener (delegate { MainPanel();});
		Defined_Vars.MusicBar = UIObjects ["OptionPanelMusicSlider"].GetComponent<Slider> ();

		Defined_Vars.MusicBar.onValueChanged.AddListener (
			delegate { CountAndRound("OptionPanelMusicHandleText",Defined_Vars.MusicBar.value);});

		Defined_Vars.MusicBar.onValueChanged.AddListener (
			delegate { saveVolumeValues();});

		Defined_Vars.SfxBar = UIObjects ["OptionPanelSfxSlider"].GetComponent<Slider> ();

		Defined_Vars.SfxBar.onValueChanged.AddListener (
			delegate { CountAndRound("OptionPanelSfxHandleText",Defined_Vars.SfxBar.value);});

		Defined_Vars.SfxBar.onValueChanged.AddListener (
			delegate { saveVolumeValues();});

	}

	public void CountAndRound(string str, float val){
		Text sliderText = UIObjects [str].GetComponent<Text> ();
		sliderText.text = Mathf.RoundToInt(val).ToString();	

	}

	//saving the current slider vals to the needed vars for the sounds to be match to the player wanted volume
	void saveVolumeValues(){

		UIObjects ["Music"].GetComponent<AudioSource> ().volume =  UIObjects ["OptionPanelMusicSlider"].GetComponent<Slider> ().value/10;

		Defined_Vars.volumeMenu = UIObjects ["OptionPanelMusicSlider"].GetComponent<Slider> ().value / 10;
	
		Defined_Vars.volumeEffects  =  	UIObjects ["OptionPanelSfxSlider"].GetComponent<Slider> ().value/10;

		PlayerPrefs.SetFloat ("volumeMenu",Defined_Vars.volumeMenu);
		PlayerPrefs.SetFloat ("volumeEffects",Defined_Vars.volumeEffects);


	}

	public void LinkToSite(string link){
		System.Diagnostics.Process.Start(link);
	}

}
