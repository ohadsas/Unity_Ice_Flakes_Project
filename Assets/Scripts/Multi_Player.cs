using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using com.shephertz.app42.gaming.multiplayer.client.events;
using MiniJSON;

public class Multi_Player : MonoBehaviour {


	public Dictionary<string,GameObject> unityObjects;
	public GameObject cube;
	public bool isUserCreated;
	public bool isUserConnected;
	public string userName;
	private string roomId;
	private int index;
	private List<string> rooms;
	private int turnTime = 120;
	public string opponentName = "";
	public string secretKey = "2926389a84b4e0e4996943c8668de14e3ba0b4e77572f9485f3d2ee3453f49b8";
	public string apiKey = "82c08db9dfb2ae04a69f977b107e513b22ae1783fb29aa98bfb190f2399e7b7f";
	public float _yPos = 4.4f;

	static Multi_Player _instance;
	public static Multi_Player Instance
	{
		get
		{
			if (_instance == null)
				_instance = GameObject.Find("Multi_Player").GetComponent<Multi_Player>();
			
			return _instance;
		}
	}

	void OnEnable()
	{ //events listners
		SC_Listener_AppWarp.OnGameStarted += OnGameStarted;
		SC_Listener_AppWarp.OnGameStopped += OnGameStopped;
		SC_Listener_AppWarp.OnMoveCompleted += OnMoveCompleted;
		SC_Listener_App42.onCreatedUserApp42 += OnCreatedUserApp42;
		SC_Listener_App42.OnExceptionFromApp42 += OnExceptionFromApp42;
		SC_Listener_AppWarp.onConnectToAppWarp += onConnectToAppWarp;
		SC_Listener_AppWarp.onDisconnectFromAppWarp += onDisconnectFromAppWarp;
		SC_Listener_AppWarp.OnMatchedRooms += OnGetMatchedRoomsDone;
		SC_Listener_AppWarp.onGetLiveRoomInfo += OnGetLiveRoomInfo;
		SC_Listener_AppWarp.OnCreateRoomDone += OnCreateRoomDone;
		SC_Listener_AppWarp.OnJoinToRoom += OnJoinToRoom;
		SC_Listener_AppWarp.OnUserJoinRoom += OnUserJoinRoom;
		SC_Listener_AppWarp.OnUserLeftRoom += OnUserLeftRoom;

	}
	
	void OnDisable()
	{// event lisiners
		SC_Listener_AppWarp.OnGameStarted -= OnGameStarted;
		SC_Listener_AppWarp.OnGameStopped -= OnGameStopped;
		SC_Listener_AppWarp.OnMoveCompleted -= OnMoveCompleted;
		SC_Listener_App42.onCreatedUserApp42 -= OnCreatedUserApp42;
		SC_Listener_App42.OnExceptionFromApp42 -= OnExceptionFromApp42;
		SC_Listener_AppWarp.onConnectToAppWarp -= onConnectToAppWarp;
		SC_Listener_AppWarp.onDisconnectFromAppWarp -= onDisconnectFromAppWarp;
		SC_Listener_AppWarp.OnMatchedRooms -= OnGetMatchedRoomsDone;
		SC_Listener_AppWarp.onGetLiveRoomInfo -= OnGetLiveRoomInfo;
		SC_Listener_AppWarp.OnCreateRoomDone -= OnCreateRoomDone;
		SC_Listener_AppWarp.OnJoinToRoom -= OnJoinToRoom;
		SC_Listener_AppWarp.OnUserJoinRoom -= OnUserJoinRoom;
		SC_Listener_AppWarp.OnUserLeftRoom -= OnUserLeftRoom;

	}
	
	// Use this for initialization
	void Start () {
		Init();
	}
	// connecting to appwrap 
	void Init(){

		cube = GameObject.Find ("iceCube");

		userName = "Ohad" + ((int)(Time.time * 100000)).ToString ();//make random
		print (userName.ToString());
		SC_App42Kit.App42Init(apiKey,secretKey);
		//Init Server
		SC_AppWarpKit.WarpInit(apiKey,secretKey);
		
		try
		{
			Debug.Log ("Connecting to Appwrap");
			SC_AppWarpKit.connectToAppWarp(userName);
		}
		catch(Exception e)
		{
			Debug.Log (e.Data.ToString());
		}
	}
	
	public void OnCreatedUserApp42(object respond)
	{
		Debug.Log(respond);
		try{
			//unityObjects ["Loading_Text"].GetComponent<Text> ().text = "Connecting to Appwrap...";
			SC_AppWarpKit.connectToAppWarp(userName);
		}
		catch(Exception e){
			//BackToMenu_Btn();
		}
	}
	
	public void OnExceptionFromApp42(Exception error){
		Debug.Log("onConnectToApp42: " + error.Message);
	}
	
	public void onConnectToAppWarp(ConnectEvent eventObj){

		Debug.Log("onConnectToAppWarp " + eventObj.getResult());
		if (eventObj.getResult () == 0){
			isUserConnected = true;
			SC_AppWarpKit.GetRoomsInRange(1,2);
		}
		else {
			isUserConnected = false;
		}
	}
	
	public void onDisconnectFromAppWarp(ConnectEvent eventObj)
	{
		isUserConnected = false;
		Debug.Log("onDisconnectFromAppWarp " + eventObj.getResult());
	}
	
	public void OnGetMatchedRoomsDone(MatchedRoomsEvent eventObj)
	{
		Debug.Log("OnGetMatchedRoomsDone : " + eventObj.getResult());
		if (isUserConnected) {
			rooms = new List<string> ();
			
			foreach (var roomData in eventObj.getRoomsData()) {
				Debug.Log ("Room ID:" + roomData.getId () + ", " + roomData.getRoomOwner ());
				rooms.Add (roomData.getId ()); // add to the list of rooms id
			}
			
			index = 0;
			if (index < rooms.Count) {
				SC_AppWarpKit.GetLiveRoomInfo (rooms [index]);		
			} else {
				Debug.Log ("No Rooms");
				SC_AppWarpKit.CreateTurnBaseRoom ("ROOM" + Time.time, userName, 2, null, turnTime);
			}
		}
	}
	
	public void OnGetLiveRoomInfo(LiveRoomInfoEvent eventObj)
	{
		if (isUserConnected) {
			Debug.Log ("OnGetLiveRoomInfo " + eventObj.getResult () + " " + eventObj.getData ().getId () + " " + eventObj.getJoinedUsers ().Length);
			if (eventObj.getResult () == 0 && eventObj.getJoinedUsers ().Length == 1) 
			{
				Debug.Log("Joined room " + eventObj.getData ().getId ());
				roomId = eventObj.getData ().getId ();
				SC_AppWarpKit.JoinToRoom (eventObj.getData ().getId ());
				SC_AppWarpKit.RegisterToRoom (eventObj.getData ().getId ());
			} 
			else
			{
				Debug.Log("Still Looking");
				index++;
				if (index < rooms.Count)
					SC_AppWarpKit.GetLiveRoomInfo (rooms [index]);
				else {
					Debug.Log ("No More Rooms");
					SC_AppWarpKit.CreateTurnBaseRoom ("ROOM" + Time.time, userName, 2, null, turnTime);
				}
			}
		}
	}
	
	public void OnCreateRoomDone(RoomEvent eventObj)
	{
		if (isUserConnected)
		{
			Debug.Log ("OnCreateRoomDone " + eventObj.getResult () + eventObj.getData ().getId () + " " + eventObj.getData ().getRoomOwner ());
			if (eventObj.getResult () == 0) {
				roomId = eventObj.getData ().getId ();
				SC_AppWarpKit.JoinToRoom (eventObj.getData ().getId ());
				SC_AppWarpKit.RegisterToRoom (eventObj.getData ().getId ());
			}
		}
	}
	
	public void OnUserJoinRoom(RoomData eventObj, string _UserName)
	{
		if(isUserConnected)
		{
			Debug.Log ("OnUserJoinRoom" + " " + eventObj.getRoomOwner () + " User connected" + userName);
			if (_UserName != eventObj.getRoomOwner () && userName == eventObj.getRoomOwner ())
			{
				opponentName = _UserName;
				SC_AppWarpKit.StartGame();
				Debug.Log("Start Game");
			}
			
			if(_UserName != eventObj.getRoomOwner ()){
				opponentName = eventObj.getRoomOwner ();
			}
		}
	}
	
	public void OnUserLeftRoom(RoomData eventObj, string _UserName)
	{
		Debug.Log("OnUserLeftRoom : " + eventObj.getName());
		Cube_Logic_Multiplayer.Instance.GameObj["CanvasWinLose"].SetActive(true);
		Cube_Logic_Multiplayer.Instance.GameObj["DissconectText"].SetActive(true);
		Cube_Logic_Multiplayer.Instance.GameObj["CanvasWinLosePanel"].SetActive(false);
		Cube_Logic_Multiplayer.Instance.GameObj["RematchButton"].SetActive(false);
		Cube_Logic_Multiplayer.Instance.GameObj["CanvasPower"].SetActive(false);
		Cube_Logic_Multiplayer.Instance.GameObj["CanvasDistance"].SetActive(false);
		Cube_Logic_Multiplayer.Instance.GameObj["CanvasPlayerOne"].SetActive(false);
		Cube_Logic_Multiplayer.Instance.GameObj["CanvasPlayerTwo"].SetActive(false);
	
	}
	
	public void OnJoinToRoom(RoomEvent eventObj)
	{
		if(isUserConnected)
		{
			Debug.Log("OnJoinToRoom " + eventObj.getResult());
			if (eventObj.getResult() == 0)
			{		
				Debug.Log("Joined Room! " +  eventObj.getData ().getId ());
			}
			else
			{
				SC_AppWarpKit.JoinToRoom(roomId);
				SC_AppWarpKit.RegisterToRoom(roomId);
			}
		}
	}
	
	void OnApplicationQuit() 
	{
		SC_AppWarpKit.DisconnectFromAppWarp ();
	}






	
	// the player who starts after the 2 players are in the same room
	public void OnGameStarted(string sender, string roomId, string nextTurn){

		Cube_Logic_Multiplayer.Instance.GameStarted ();
		if (nextTurn == userName) 
		{
			Defined_Vars.isMyTurn = true;
			Defined_Vars.Me = Defined_Vars.player.playerOne;
			Cube_Logic_Multiplayer.Instance.GameObj["CurrentTurnText"].GetComponent<Text>().text = "Your Turn";
			Cube_Logic_Multiplayer.Instance.GameObj ["CanvasDistanceText"].SetActive (true);
			Cube_Logic_Multiplayer.Instance.GameObj["CanvasText"].SetActive(true);
			Debug.Log ("OnGameStarted ELSE - isMyTurn : " + Defined_Vars.isMyTurn + " player : " + Defined_Vars.Me.ToString ());

		}
		// waits not your turn
		else {

			Defined_Vars.isMyTurn = false;
			Defined_Vars.Me = Defined_Vars.player.playerTwo;
			Cube_Logic_Multiplayer.Instance.GameObj["CurrentTurnText"].GetComponent<Text>().text = "Rival Turn";
			Debug.Log ("OnGameStarted IF - isMyTurn : " + Defined_Vars.isMyTurn + " player : " + Defined_Vars.Me.ToString ());
		}

	}
	
	public void OnGameStopped(string sender, string roomId)
	{
		Debug.Log("OnGameStopped" + ", sender: " + sender + ", roomId: " + roomId + ", userName: " );//+ SC_MenuTicTacToe.Instance.userName);

	}

	//CALLED WHEN SEND MOVE WAS SENT
	public void OnMoveCompleted(MoveEvent move)
	{     Debug.Log ("OnMoveComplete : " + move.getMoveData ());
		//not my turn
		if (move.getNextTurn () == userName) {
			
			Dictionary<string,object> _recData = Json.Deserialize (move.getMoveData ()) as Dictionary<string,object>; 

			float _xPos = float.Parse (_recData ["x position"].ToString ());
			float _zPos = float.Parse (_recData ["z position"].ToString ());
			float _fPos = float.Parse (_recData ["final position"].ToString ());
			float _fforce = float.Parse (_recData ["init force"].ToString ());
			int _totalMoveCount = int.Parse (_recData ["totalMoveCount"].ToString ());

			OpponentLogic (_xPos, _zPos,_fPos,_fforce,_totalMoveCount);

			Defined_Vars.isMyTurn = true;
		}
	}

	public void DoLogic(float x_position,float z_position, float init_force, float final_position_z)
	{
		//Serialize the data to Json and we are sending to the opponent 
		Dictionary<string,object> _sendData = new Dictionary<string, object>();
		//sending location and speed
		_sendData.Add("x position",x_position);
		_sendData.Add("z position",z_position);
		_sendData.Add("final position",final_position_z);
		_sendData.Add("init force",init_force);	
		_sendData.Add("totalMoveCount",Defined_Vars.totalMoveCount);
		string _sendDataText = Json.Serialize(_sendData);
		
		//Send move  calling to moveComplete - Change turn to next player and sending the last move
		SC_AppWarpKit.sendMove(_sendDataText);
		Defined_Vars.isMyTurn = false;
			
	}
	
	public void OpponentLogic(float _xPos,float _zPos, float _fPos, float _fforce,int _totalMoveCount)
	{
		Debug.Log("ImplementOpponentLogic");
		Cube_Logic_Multiplayer.Instance.GameObj["SoundEffect1"].GetComponent<AudioSource>().Play();
		Defined_Vars.totalMoveCount = _totalMoveCount;

		Cube_Logic_Multiplayer.count = 3;

		cube.GetComponent<Rigidbody> ().isKinematic = true;
		cube.transform.position = new Vector3 (_xPos, _yPos, _zPos);
		cube.GetComponent<Rigidbody> ().isKinematic = false;

		Vector3 v = new Vector3 (0f, 0f, _fforce);
		cube.GetComponent<Rigidbody> ().AddForce (v, ForceMode.Impulse);

		StartCoroutine(countToFour());
		Defined_Vars.isMyTurn = true;
		Defined_Vars.scoreTwo = _fPos;

	}

	IEnumerator countToFour() 
	{
		yield return new WaitForSeconds(1);
		Cube_Logic_Multiplayer.count = 4;
	}

}