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
		UIObjects ["MainPanelSinglePlayerButton"].GetComponent<Button> ().onClick.AddListener (delegate { LoadingPanel();});
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
		UIObjects ["MultiPlayerPlayButton"].GetComponent<Button> ().onClick.AddListener (delegate { MainPanel();}); // redirect to the main menu for now
		Defined_Vars.Dollars = UIObjects ["MultiPlayerSlider"].GetComponent<Slider> ();
		Defined_Vars.Dollars.onValueChanged.AddListener(delegate { CountAndRound("MultiPlayerSliderText",Defined_Vars.Dollars.value);});
	}
	

	public void LoadingPanel(){
		Debug.Log ("LoadingPanel is on");
		MenuMod("LoadingPanel", Defined_Vars.ON);
		for (int i = 0; i < Defined_Vars.menus.Length; i++) {
			if (Defined_Vars.menus[i] != "LoadingPanel") MenuMod(Defined_Vars.menus[i],Defined_Vars.OFF);
		}
		UIObjects ["LoadingBackButton"].GetComponent<Button> ().onClick.AddListener (delegate { MainPanel();});

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
		Defined_Vars.MusicBar.onValueChanged.AddListener (delegate { CountAndRound("OptionPanelMusicHandleText",Defined_Vars.MusicBar.value);});
		Defined_Vars.SfxBar = UIObjects ["OptionPanelSfxSlider"].GetComponent<Slider> ();
		Defined_Vars.SfxBar.onValueChanged.AddListener (delegate { CountAndRound("OptionPanelSfxHandleText",Defined_Vars.SfxBar.value);});
	}

	public void CountAndRound(string str, float val){
		Text sliderText = UIObjects [str].GetComponent<Text> ();
		sliderText.text = Mathf.RoundToInt(val).ToString();	
	}

	public void LinkToSite(string link){
		System.Diagnostics.Process.Start(link);
	}

}
