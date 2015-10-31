//Ohad Sasson 301268819 the menu excercise was submitted on time
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Defined_Vars : MonoBehaviour {

	public enum player{
		playerOne,
		playerTwo
	};

	public static player Me;
	public static readonly bool ON = true;
	public static readonly bool OFF = false;
	public static Slider Dollars = null;
	public static Slider SfxBar = null;
	public static Slider MusicBar = null;
	public static string Link = "https://github.com/ohadsas/Unity_Ice_Flakes_Project";
	public static float scoreOne;
	public static float scoreTwo;
		
	public static bool isMyTurn = false;
	public static int totalMoveCount = 0;
	
	public static readonly string[] menus = {"MainPanel","SinglePlayerPanel","MultiPlayerPanel","StudentInfoPanel", "OptionsPanel"};
	
	//the current player pref vars values
	public static float volumeMenu;
	public static float volumeEffects;	
	//var for the first enter to the game to set the init volumes
	private static bool initVars;
	
	
	//getting the player prefs vals from the device
	void Start () 
	{
		// player prefrences - volume
		// init vars only at first entrance

			PlayerPrefs.SetString ("initVars", bool.TrueString);
			
			PlayerPrefs.SetFloat ("volumeMenu", 1f);
			volumeMenu = PlayerPrefs.GetFloat ("volumeMenu");
			
			PlayerPrefs.SetFloat ("volumeEffects", 1f);
			volumeEffects = PlayerPrefs.GetFloat ("volumeEffects");

	
		

		
		
	}


}
