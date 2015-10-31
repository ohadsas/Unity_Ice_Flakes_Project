//Ohad Sasson 301268819 the menu excercise was submitted on time
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Cube_Logic: MonoBehaviour {
	public Defined_Vars.player currentPlayer;
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
	
	
	// Starts this instance
	void Start() {
		Init();
		StartCoroutine(MoveCubeLeft());
	}
	
	// Updates this instance
	void Update() {
		DistanceBoard();
		if (currentPlayer == Defined_Vars.player.playerOne) {
			MovementLogic();
		}
	}

	
	// CheckLocation() is on FixedUpdate because we want to check  for the same frame sequnce
	void FixedUpdate() {
		if (count == 4 || count == 5) {
			CheckLocation();
		}
	}
	
	// Init instance
	void Init() {
		//Debug.Log ("init() was called");
		rb = GetComponent < Rigidbody > ();
		GameObj = new Dictionary < string, GameObject > ();
		GameObject[] objects = GameObject.FindGameObjectsWithTag("GameObject");
		foreach(GameObject g in objects) {
			GameObj.Add(g.name, g);
		}
		GameObj["CanvasWinLose"].SetActive(false);
		GameObj["RematchButton"].
		GetComponent < Button > ().onClick.AddListener(delegate {Rematch();});
		GameObj["MenuButton"].GetComponent < Button > ().onClick.AddListener(delegate {Menu();});
		obsticlesReorganize ();
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
	void OnCollisionEnter(Collision hit) {
		var opposite = -rb.velocity;
		if (hit.transform.gameObject.name == "wallFront") {
			rb.drag = +1;
			rb.AddForce(opposite * Time.deltaTime);
		}
	}
	
	// Checks the cube location and if it stopped moving and then passing the turn
	void CheckLocation() {
		if (rb != null) {
			if (rb.velocity.magnitude == 0) {
				var lastPos = transform.position;
				if (lastPos == transform.position) {
					if (count == 4) {
						obsticlesReorganize ();
						CleanAndInit();
						RandomCpMovment();
						currentPlayer = Defined_Vars.player.playerTwo;
					} else {
						CalculateWin();
					}
					count = count + 1;
				}
			}
		}
	}
	
	// Move cube left and call moveRight at the end position
	IEnumerator MoveCubeLeft() {
		//Debug.Log ("moveCubeLeft");
		while (!LeftMax && count == 0) {
			transform.position = new Vector3(x, transform.position.y, transform.position.z);
			x = x - speed;
			if (x <= -40) {
				LeftMax = true;
			}
			yield
				return null;
			StartCoroutine(MoveCubeRight());
		}
	}
	
	// Move cube right and call moveLeft at the end position
	IEnumerator MoveCubeRight() {
		while (LeftMax && count == 0) {
			transform.position = new Vector3(x, transform.position.y, transform.position.z);
			x = x + speed;
			if (x >= 40) {
				LeftMax = false;
			}
			yield
				return null;
			StartCoroutine(MoveCubeLeft());
		}
	}
	
	//Logic Movement of the cube rigidBody.
	void MovementLogic() {
		if (currentPlayer == Defined_Vars.player.playerOne) {
			if (Input.GetKeyDown(KeyCode.Space) && count < 3) {
				count++;
			} else if (count == 1) {
				StartCoroutine(PowerSync());
			} else if (count == 2) {
				GameObj["SoundEffect1"].GetComponent<AudioSource>().Play();
				rb.AddForce(transform.forward * power * thrust, ForceMode.Impulse);
				count = count + 1;
				StartCoroutine(NextTurn());
			}
		}
	}
	
	// Distances from the cube to the board.
	// It also writes the distance to the current player scoreboard.
	void DistanceBoard() {
		GameObject iceCube = GameObject.Find("iceCube");
		GameObject wallFront = GameObject.Find("wallFront");
		
		int distance = (int) Vector3.Distance(iceCube.transform.position, wallFront.transform.position) - 60; // substructing 23 = width of the cube+ wall
		GameObj["CanvasDistanceText"].GetComponent < Text > ().text = "D : " + distance;
		if (currentPlayer == Defined_Vars.player.playerOne) {
			Defined_Vars.scoreOne = distance;
			GameObj["CanvasPlayerOneText"].GetComponent < Text > ().text = "Me : " + distance.ToString();
		} else {
			Defined_Vars.scoreTwo = distance;
			GameObj["CanvasPlayerTwoText"].GetComponent < Text > ().text = "CPU : " + distance.ToString();
		}
	}
	
	// I added this sync IEnumarator to sync between the up and the down,
	// Becasue im using it on the update section and i cant call just for one of them.
	IEnumerator PowerSync() {
		if (powerMax == false) yield
			return StartCoroutine(PowerUp());
		else yield
			return StartCoroutine(PowerDown());
	}
	
	// Powers up with time delay
	IEnumerator PowerUp() {
		while (!powerMax && count == 1) {
			power = power + 1 * Time.deltaTime;
			GameObj["CanvasText"].GetComponent < Text > ().text = "Power : " + power;
			if (power >= 10) {
				powerMax = true;
			}
			yield
				return StartCoroutine(PowerDown());
		}
	}
	
	// Powers down with time delay
	IEnumerator PowerDown() {
		while (powerMax && count == 1) {
			power = power - 1 * Time.deltaTime;
			GameObj["CanvasText"].GetComponent < Text > ().text = "Power : " + power;
			if (power <= 0) {
				powerMax = false;
			}
			yield
				return StartCoroutine(PowerUp());
		}
	}
	
	// Randoms the cp movment
	void RandomCpMovment() {
		float x = Random.Range(-40, 40);
		transform.position = new Vector3(x, transform.position.y, transform.position.z);
		x = x - speed;
		float power = Random.Range(0, 10);
		GameObj["SoundEffect1"].GetComponent<AudioSource>().Play();
		rb.AddForce(transform.forward * power * 1100, ForceMode.Impulse);
		GameObj["CanvasText"].GetComponent < Text > ().text = "Power : " + power;
	}
	
	// Nexts the turn
	IEnumerator NextTurn() {
		yield return new WaitForSeconds(3);
		count++;
	}
	
	// Cleans the and initialize the cube in the starting position
	void CleanAndInit() {
		transform.position = new Vector3(0f, 4.4f, -140f);
	}
	
	// Calculates the winner
	void CalculateWin() {
		GameObj["CanvasWinLose"].SetActive(true);
		if (Defined_Vars.scoreOne < Defined_Vars.scoreTwo) {
			Debug.Log("you win!!!");
			GameObj["SoundEffect2"].GetComponent<AudioSource>().Play();
			GameObj["CanvasWinLoseText"].GetComponent < Text > ().text = "Winner!";
		} else {
			Debug.Log("loser!!!");
			GameObj["SoundEffect3"].GetComponent<AudioSource>().Play();
			GameObj["CanvasWinLoseText"].GetComponent < Text > ().text = "Loser!";
		}
	}
	
	// Rematch
	void Rematch() {
		GameObj["CanvasWinLose"].SetActive(false);
		count = 0;
		CleanAndInit();
		LeftMax = false;
		currentPlayer = Defined_Vars.player.playerOne;
		obsticlesReorganize ();
		StartCoroutine(MoveCubeLeft());
	}
	//returing to menu button
	void Menu(){
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