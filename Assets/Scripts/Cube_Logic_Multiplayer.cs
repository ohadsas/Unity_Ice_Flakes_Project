//Ohad Sasson 301268819 the menu excercise was submitted on time
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Cube_Logic_Multiplayer: MonoBehaviour 
{
	//nultiplayer vars
	public static Rigidbody rb = null;
	public Dictionary < string, GameObject > GameObj;
	public Dictionary < string, GameObject >  obsticles;
	public static float x = 0f;
	public static float speed = 3.0f;
	public static float power = 0f;
	public static int count = 0;
	public static int thrust = 1150;
	public bool powerMax = false;
	public bool LeftMax = false;
	public bool turnEnd = false;
	public float x_position, z_position, init_force, final_position_z; 
	int distance;

	//singlton instance
	static Cube_Logic_Multiplayer _instance;
	public static Cube_Logic_Multiplayer Instance
	{
		get
		{
			if (_instance == null)
				_instance = GameObject.Find("iceCube").GetComponent<Cube_Logic_Multiplayer>();
			
			return _instance;
		}
	}

	// Starts this instance
	void Start() 
	{
		Init();
	}

	//when two players are online start the cube movement left and right
	public void GameStarted(){
		StartCoroutine(MoveCubeLeft());
	}

	// Updates this instance
	void Update()
	{
		DistanceBoard();
		MovementLogic ();
	}
		
	// CheckLocation() is on FixedUpdate because we want to check  for the same frame sequnce
	void FixedUpdate()
	{
		if (count == 4 || count == 5) 
		{
			CheckLocation();
		}
	}
	
	// Init instance
	void Init() {

		obsticlesReorganize ();

		GameObj = new Dictionary < string, GameObject > ();
		GameObject[] objects = GameObject.FindGameObjectsWithTag("GameObject");
		foreach(GameObject g in objects) {
			GameObj.Add(g.name, g);
		}

		rb = GetComponent < Rigidbody > ();

		GameObj ["CanvasDistanceText"].SetActive (false);
		GameObj["CanvasText"].SetActive(false);
		GameObj["CanvasWinLose"].SetActive(false);
		GameObj["RematchButton"].GetComponent < Button > ().onClick.AddListener(delegate {Rematch();});
		GameObj["MenuButton"].GetComponent < Button > ().onClick.AddListener(delegate {Menu();});

		GameObj ["SoundEffect1"].GetComponent<AudioSource> ().volume = Defined_Vars.volumeEffects;
		GameObj ["SoundEffect2"].GetComponent<AudioSource> ().volume = Defined_Vars.volumeEffects;
		GameObj ["SoundEffect3"].GetComponent<AudioSource> ().volume = Defined_Vars.volumeEffects;

	}
	
	// Raises the trigger enter event with the flames, add drag + 1  to the cube on each encounter.
	void OnTriggerEnter(Collider other) {
		rb.drag = +1;
		Debug.Log ("OUCH!!");
		string name = other.gameObject.name;
		GameObject flame = GameObject.Find (name);
		flame.transform.position= new Vector3(flame.transform.position.x,-150f,flame.transform.position.z);
	}
	
	// Raises the collision enter event with the front wall.
	void OnCollisionEnter(Collision hit) 
	{
		var opposite = -rb.velocity;
		if (hit.transform.gameObject.name == "wallFront")
		{
			rb.drag = +1;
			rb.AddForce(opposite * Time.deltaTime);
		}
	}
	
	// the main login function that counts the moves
	// case 0 : the player who starts the game first will enter case one when the cube is fully stoped on the board, then the data of the move will be sent to the second player
	// case 1 : called on the second player after the initiation of the first player move fully stoped, then board will be cleaned and the second player can do his turn
	// case 2 : called on the second player when the cube is fully stoped on the board, then the data of the move will be sent to the first player, then calculating whos wins on the second player side
	// case 3: called on the first player after the second player move animation fully finished and them calculate who wins on the first player side
	void CheckLocation(){

		if (rb != null){

			if (rb.velocity.magnitude == 0){

				var lastPos = transform.position;
				final_position_z = lastPos.z;

				if (lastPos == transform.position) {

					if (count == 4){

						switch(Defined_Vars.totalMoveCount){

							case 0:
								// used for first player only
								print ("Defined_Vars.totalMoveCount == 0");					
								Defined_Vars.scoreOne = distance;
								GameObj["CanvasPlayerOneText"].GetComponent < Text > ().text = "Me: " + distance.ToString();				
								CleanAndInit();
								count = 0; //  resetting cube movment ability 
								Defined_Vars.totalMoveCount++;
								//send my move to opponent
								Multi_Player.Instance.DoLogic(x_position, z_position, init_force, distance);
								GameObj["CurrentTurnText"].GetComponent<Text>().text = "Rival Turn";
								Cube_Logic_Multiplayer.Instance.GameObj ["CanvasDistanceText"].SetActive (false);
								Cube_Logic_Multiplayer.Instance.GameObj["CanvasText"].SetActive(false);
								LeftMax = false;
								StartCoroutine(MoveCubeLeft());
								obsticlesReorganize();
								break;

							case 1:
								// used for second player
								print ("Defined_Vars.totalMoveCount == 1");
								GameObj["CanvasPlayerTwoText"].GetComponent < Text > ().text = "RIVAL: " + Defined_Vars.scoreTwo.ToString();	
								CleanAndInit();
								Defined_Vars.totalMoveCount++;

								count = 0;				
								GameObj["CurrentTurnText"].GetComponent<Text>().text = "Your Turn";
								Cube_Logic_Multiplayer.Instance.GameObj ["CanvasDistanceText"].SetActive (true);
								Cube_Logic_Multiplayer.Instance.GameObj["CanvasText"].SetActive(true);
								LeftMax = false;
								StartCoroutine(MoveCubeLeft());
								obsticlesReorganize();
								break;

							case 2:
								// like case 0 to the second player
								print ("Defined_Vars.totalMoveCount == 2");
								Defined_Vars.scoreOne = distance;

								GameObj["CanvasPlayerOneText"].GetComponent < Text > ().text = "Me: " + distance.ToString();			
								CleanAndInit();
								Defined_Vars.totalMoveCount++;
								Defined_Vars.isMyTurn = true;
								// send Json 
								Multi_Player.Instance.DoLogic(x_position, z_position, init_force, distance); 
								GameObj ["CanvasPower"].SetActive (false);
								CalculateWin();

								break;

							case 3:
								// used for first player only caclculate the win in first player 
								print ("Defined_Vars.totalMoveCount == 3");
								GameObj["CanvasPlayerTwoText"].GetComponent < Text > ().text = "Rival: " + Defined_Vars.scoreTwo.ToString();
								CalculateWin();
								Defined_Vars.isMyTurn = true;
								Defined_Vars.totalMoveCount++;
								count = 0;
								GameObj ["CanvasPower"].SetActive (false);

								break;
							
						}
					}
				}
			}
		}
	}
	
	// Move cube left and call moveRight at the end position
	IEnumerator MoveCubeLeft() 
	{
		//Debug.Log ("moveCubeLeft");
		while (!LeftMax && count == 0)
		{
			transform.position = new Vector3(x, transform.position.y, transform.position.z);
			x = x - speed;
			if (x <= -40) 
			{
				LeftMax = true;
			}
			yield return null;
			StartCoroutine(MoveCubeRight());
		}
	}
	
	// Move cube right and call moveLeft at the end position
	IEnumerator MoveCubeRight() 
	{
		while (LeftMax && count == 0)
		{
			transform.position = new Vector3(x, transform.position.y, transform.position.z);
			x = x + speed;
			if (x >= 40)
			{
				LeftMax = false;
			}
			yield return null;
			StartCoroutine(MoveCubeLeft());
		}
	}
	
	//Logic Movement of the cube rigidBody.
	void MovementLogic() {

			if (Input.GetKeyDown(KeyCode.Space) && count < 3 && Defined_Vars.isMyTurn)
			{
				count++;
			}
			else if (count == 1) 
			{
				StartCoroutine(PowerSync());
			} 
			else if (count == 2) 
			{

				GameObj["SoundEffect1"].GetComponent<AudioSource>().Play();
			 	rb.AddForce(transform.forward * power * thrust, ForceMode.Impulse);
				count = count + 1;
				//setting position and force;
				init_force = (transform.forward.z * power * thrust); 
				x_position = transform.position.x;
				z_position = transform.position.z;
				
				// count set to 4 with courntine give it a delay of a second
				StartCoroutine(NextTurn());
			}

	}
	
	// Distances from the cube to the board.
	// It also writes the distance to the current player scoreboard.
	void DistanceBoard() 
	{
		GameObject iceCube = GameObject.Find("iceCube");
		GameObject wallFront = GameObject.Find("wallFront");
		
		distance = (int) Vector3.Distance(iceCube.transform.position, wallFront.transform.position) - 60; // substructing 23 = width of the cube+ wall
		GameObj["CanvasDistanceText"].GetComponent < Text > ().text = "D : " + distance;

	}
	
	// I added this sync IEnumarator to sync between the up and the down,
	// Becasue im using it on the update section and i cant call just for one of them.
	IEnumerator PowerSync()
	{
		if (powerMax == false) yield return StartCoroutine(PowerUp());
		else yield return StartCoroutine(PowerDown());
	}
	
	// Powers up with time delay
	IEnumerator PowerUp() 
	{
		while (!powerMax && count == 1)
		{
			power = power + 1 * Time.deltaTime;
			GameObj["CanvasText"].GetComponent < Text > ().text = "Power : " + power;
			if (power >= 10)
			{
				powerMax = true;
			}
			yield return StartCoroutine(PowerDown());
		}
	}
	
	// Powers down with time delay
	IEnumerator PowerDown() 
	{
		while (powerMax && count == 1) 
		{
			power = power - 1 * Time.deltaTime;
			GameObj["CanvasText"].GetComponent < Text > ().text = "Power : " + power;
			if (power <= 0) 
			{
				powerMax = false;
			}
			yield return StartCoroutine(PowerUp());
		}
	}
	

	// Nexts the turn
	IEnumerator NextTurn() 
	{
		yield return new WaitForSeconds(3);
		count++;
	}
	
	// Cleans the and initialize the cube in the starting position
	void CleanAndInit() 
	{
		transform.position = new Vector3(0f, 4.4f, -140f);
	}
	
	// Calculates the winner
	void CalculateWin()
	{
		GameObj["CanvasWinLose"].SetActive(true);

		if (Defined_Vars.scoreOne < Defined_Vars.scoreTwo){
			Debug.Log("you win!!!");
			GameObj["SoundEffect2"].GetComponent<AudioSource>().Play();
			GameObj["DissconectText"].SetActive(false);
			
			GameObj["CanvasWinLoseText"].GetComponent < Text > ().text = "Winner!";
		}
		else 
		{
			Debug.Log("loser!!!");
			GameObj["DissconectText"].SetActive(false);
			GameObj["SoundEffect3"].GetComponent<AudioSource>().Play();
			GameObj["CanvasWinLoseText"].GetComponent < Text > ().text = "Loser!";
		}
	}
	
	// Rematch, restarting all the components to the init positions
	// button in canvasWinLose 
	void Rematch() 
	{	
		Defined_Vars.totalMoveCount=0; // for the main switch 
		//init scores and power canvas
		GameObj["CanvasWinLose"].SetActive(false);
		GameObj["CanvasPlayerTwoText"].GetComponent < Text > ().text = "Rival: ";
		GameObj["CanvasPlayerOneText"].GetComponent < Text > ().text = "Me: ";
		GameObj ["CanvasPower"].SetActive (true);
		GameObj["CanvasText"].GetComponent < Text > ().text = "Power : ";
		//check turns again
		if (Defined_Vars.Me == Defined_Vars.player.playerOne) {
			GameObj ["CurrentTurnText"].GetComponent<Text> ().text = "Your Turn";
			Defined_Vars.isMyTurn = true;	
			GameObj ["CanvasDistanceText"].SetActive (true);
			GameObj["CanvasText"].SetActive(true);

		} else {
			Defined_Vars.isMyTurn = false;
			GameObj["CurrentTurnText"].GetComponent<Text>().text = "Rival Turn";
			GameObj ["CanvasDistanceText"].SetActive (false);
			GameObj["CanvasText"].SetActive(false);
		}

		count = 0;
		CleanAndInit();
		LeftMax = false;

		StartCoroutine(MoveCubeLeft());
		obsticlesReorganize();
	}

	//return to menu button
	void Menu(){
		SC_AppWarpKit.DisconnectFromAppWarp();
		Application.LoadLevel ("UI_Menu_scene");
	}

	//drawing the obsticles randomly on the board
	public void obsticlesReorganize(){
		GameObject[] objects  = GameObject.FindGameObjectsWithTag("ObsticalsObj");
		int i = -120;
		foreach (GameObject g in objects) {;
			g.transform.position = new Vector3 (Random.Range (-38, 38), 10, i);
			i += 25;
		}
	}
}