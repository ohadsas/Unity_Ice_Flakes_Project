using UnityEngine;
using System.Collections;
using System;
using com.shephertz.app42.gaming.multiplayer.client.events;
using com.shephertz.app42.gaming.multiplayer.client.command;
using System.Collections.Generic;
using MiniJSON;

public class SC_Logic : MonoBehaviour {

    private string apiKey = "d8babde27e9310a6f141e850003e1b61bc68f7ddb02b90f71e79b7512d08be4c";
    private string secretKey = "37b40d5fc19109edd30a651ac6d424a2fcf4e088707953e5d1792efa43f4826c";
	private string email = "@gmail.com";
	private string userName = "";
    private TextMesh guiText;
	private string roomId = "";
	private List<string> rooms;
	private string opponentName = "";

	private bool isMyTurn = false;


	void OnEnable()
	{
			SC_Listener_App42.onCreatedUserApp42 += onCreatedUserApp42;
			SC_Listener_App42.OnExceptionFromApp42 += OnExceptionFromApp42;
		
			SC_Listener_AppWarp.onConnectToAppWarp += onConnectToAppWarp;
			SC_Listener_AppWarp.onDisconnectFromAppWarp += onDisconnectFromAppWarp;
			SC_Listener_AppWarp.OnMatchedRooms += OnGetMatchedRoomsDone;
			SC_Listener_AppWarp.OnSubscribeToRoom += onSubscribeToRoom;
			SC_Listener_AppWarp.OnUnSubscribeToRoom += onUnSubscribeToRoom;
			SC_Listener_AppWarp.OnJoinToRoom += OnJoinToRoom;
			SC_Listener_AppWarp.OnLeaveFromRoom += OnLeaveFromRoom;
			SC_Listener_AppWarp.OnCreateRoomDone += OnCreateRoomDone;
			SC_Listener_AppWarp.onGetLiveRoomInfo += OnGetLiveRoomInfo;
			SC_Listener_AppWarp.OnSendPrivateUpdate += OnSendPrivateUpdate;
			SC_Listener_AppWarp.OnSendPrivateChat += OnSendPrivateChat;
			SC_Listener_AppWarp.OnStartGameDone += OnStartGameDone;
			SC_Listener_AppWarp.OnStopGameDone += OnStopGameDone;
			SC_Listener_AppWarp.OnRoomCreated += OnRoomCreated;
			SC_Listener_AppWarp.OnUserJoinRoom += OnUserJoinRoom;
			SC_Listener_AppWarp.OnUserLeftRoom += OnUserLeftRoom;
			SC_Listener_AppWarp.OnPrivateUpdateReceived += OnPrivateUpdateReceived;
			SC_Listener_AppWarp.OnPrivateChatReceived += OnPrivateChatReceived;
			SC_Listener_AppWarp.OnGameStarted += OnGameStarted;
			SC_Listener_AppWarp.OnGameStopped += OnGameStopped;
			SC_Listener_AppWarp.OnSendMove += OnSendMove;
			SC_Listener_AppWarp.OnMoveCompleted += OnMoveCompleted;
	}
		
	void OnDisable()
	{
		SC_Listener_App42.onCreatedUserApp42 -= onCreatedUserApp42;
		SC_Listener_App42.OnExceptionFromApp42 -= OnExceptionFromApp42;
		
		SC_Listener_App42.OnExceptionFromApp42 -= OnExceptionFromApp42;
		SC_Listener_AppWarp.onConnectToAppWarp -= onConnectToAppWarp;
		SC_Listener_AppWarp.onDisconnectFromAppWarp -= onDisconnectFromAppWarp;
		SC_Listener_AppWarp.OnMatchedRooms -= OnGetMatchedRoomsDone;
		SC_Listener_AppWarp.OnSubscribeToRoom -= onSubscribeToRoom;
		SC_Listener_AppWarp.OnUnSubscribeToRoom -= onUnSubscribeToRoom;
		SC_Listener_AppWarp.OnJoinToRoom -= OnJoinToRoom;
		SC_Listener_AppWarp.OnLeaveFromRoom -= OnLeaveFromRoom;
		SC_Listener_AppWarp.OnCreateRoomDone -= OnCreateRoomDone;
		SC_Listener_AppWarp.onGetLiveRoomInfo -= OnGetLiveRoomInfo;
		SC_Listener_AppWarp.OnSendPrivateUpdate -= OnSendPrivateUpdate;
		SC_Listener_AppWarp.OnSendPrivateChat -= OnSendPrivateChat;
		SC_Listener_AppWarp.OnStartGameDone -= OnStartGameDone;
		SC_Listener_AppWarp.OnStopGameDone -= OnStopGameDone;
		SC_Listener_AppWarp.OnRoomCreated -= OnRoomCreated;
		SC_Listener_AppWarp.OnUserJoinRoom -= OnUserJoinRoom;
		SC_Listener_AppWarp.OnUserLeftRoom -= OnUserLeftRoom;
		SC_Listener_AppWarp.OnPrivateUpdateReceived -= OnPrivateUpdateReceived;
		SC_Listener_AppWarp.OnPrivateChatReceived -= OnPrivateChatReceived;
		SC_Listener_AppWarp.OnGameStarted -= OnGameStarted;
		SC_Listener_AppWarp.OnGameStopped -= OnGameStopped;
		SC_Listener_AppWarp.OnSendMove -= OnSendMove;
		SC_Listener_AppWarp.OnMoveCompleted -= OnMoveCompleted;

	}
	
	void Start () 
	{
		guiText = GameObject.Find ("GUIText").GetComponent<TextMesh> ();
		SC_App42Kit.App42Init(apiKey,secretKey);
		SC_AppWarpKit.WarpInit(apiKey,secretKey);
	}
	
	void Update () 
	{
		//Init user once so we will have a user on App42
		if(Input.GetKeyDown(KeyCode.A))
		{
			Debug.Log("Creating User...");
			guiText.text += "Creating User..."+ System.Environment.NewLine;
			userName = "Moshe" + ((int)(Time.time * 100000)).ToString();
			SC_App42Kit.InitUser(userName,"1234",userName + email);
		}
		
		if(Input.GetKeyDown(KeyCode.Q))
		{
			Debug.Log("Connecting To Server...");
			guiText.text += "Connecting To Server..."+ System.Environment.NewLine;
			SC_AppWarpKit.connectToAppWarp(userName);
		}
		
		if(Input.GetKeyDown(KeyCode.W))
		{
			Debug.Log("Disconnecting from Server...");
			//guiText.text += "Disconnecting from Server..."+ System.Environment.NewLine;
			//SC_AppWarpKit.DisconnectFromAppWarp();
		}

		if(Input.GetKeyDown(KeyCode.R))
		{
			Debug.Log("Room Created !!");
			guiText.text += "Room Created"+ System.Environment.NewLine;

			SC_AppWarpKit.CreateTurnBaseRoom("Stam1",userName,2,null,15);
		}

		if(Input.GetKeyDown(KeyCode.T))
		{
			Debug.Log("Get Rooms !!");
			guiText.text += "Get Rooms"+ System.Environment.NewLine;
			
			SC_AppWarpKit.GetRoomsInRange(1,2);
		}

		if(Input.GetKeyDown(KeyCode.M))
		{
			Debug.Log("Sent Message !!");
			guiText.text += "Sent Message " + userName + " " + Time.time + System.Environment.NewLine;

			SC_AppWarpKit.sendPrivateChat(opponentName, userName + " " + Time.time);
		}

		if(Input.GetKeyDown(KeyCode.O) && isMyTurn)
		{
			Debug.Log("Move turn !!");
			guiText.text += "Move turn " + System.Environment.NewLine;
			
			SC_AppWarpKit.sendMove(userName + Time.time);
		}


		if(Input.GetKeyDown(KeyCode.Z))
		{
			guiText.text =  "";
		}
	}

	
	public void onCreatedUserApp42(object respond)
	{
		Debug.Log(respond );
		guiText.text += "User Created..."+ System.Environment.NewLine;
	}
	
	public void OnExceptionFromApp42(Exception error)
	{
		Debug.Log("onConnectToApp42: " + error.Message);
		guiText.text += error.Message + System.Environment.NewLine;
	}
	
	public void onConnectToAppWarp(ConnectEvent eventObj)
	{
		Debug.Log("onConnectToAppWarp " + eventObj.getResult());
		guiText.text += "Connected To AppWrap" + System.Environment.NewLine;
	}
	
	public void onDisconnectFromAppWarp(ConnectEvent eventObj)
	{
		Debug.Log("onDisconnectFromAppWarp " + eventObj.getResult());
		guiText.text += "Disconnected from AppWrap" + System.Environment.NewLine;
	}
	
	public void OnCreateRoomDone(RoomEvent eventObj)
	{
		Debug.Log("OnCreateRoomDone " + eventObj.getResult() + " room Owner " + eventObj.getData().getRoomOwner() + " " + eventObj.getData().getRoomOwner());
		if(eventObj.getResult() == 	WarpResponseResultCode.SUCCESS)
		{
			roomId = eventObj.getData ().getId ();
			guiText.text += "Room created! " +  eventObj.getData ().getId () + System.Environment.NewLine;
			SC_AppWarpKit.JoinToRoom(eventObj.getData().getId());
		}
	}
	
	//only room creator will get the notification
	public void OnDeleteRoomDone(RoomEvent eventObj)
	{
		Debug.Log("OnDeleteRoomDone " + eventObj.getResult());
	}
	
	public void onSubscribeToRoom(RoomEvent eventObj)
	{
		Debug.Log("onSubscribeToRoom " + eventObj.getResult());
		guiText.text += "SubscribeToRoom ! " +  eventObj.getData ().getId () + System.Environment.NewLine;
		//if (eventObj.getResult() == WarpResponseResultCode.SUCCESS)
		//	Debug.Log("onSubscribeRoomDone : " + eventObj.getResult());
	}
	
	public void onUnSubscribeToRoom(RoomEvent eventObj)
	{
		Debug.Log("onUnSubscribeToRoom " + eventObj.getResult());
	}
	
	public void OnJoinToRoom(RoomEvent eventObj)
	{
		if (eventObj.getResult() == WarpResponseResultCode.SUCCESS)
		{
			Debug.Log("OnJoinToRoom " + eventObj.getResult());
			opponentName = eventObj.getData().getRoomOwner();
			Debug.Log("roomId: " + roomId + ", OpponentName: " + opponentName);
			guiText.text += "Joined Room! " +  eventObj.getData ().getId () + System.Environment.NewLine;
			SC_AppWarpKit.RegisterToRoom(roomId);
		}
		else
		{
			SC_AppWarpKit.JoinToRoom(roomId);
		}
	}
	public void OnLeaveFromRoom(RoomEvent eventObj)
	{
		Debug.Log("OnLeaveFromRoom " + eventObj.getResult());
	}
	
	public void OnGetLiveRoomInfo(LiveRoomInfoEvent eventObj)
	{
		// Debug.Log("OnGetLiveRoomInfo " + eventObj.getResult() + " " + eventObj.getData().getId() + " " + eventObj.getJoinedUsers().Length);
	}
	
	public void OnSendPrivateUpdate(byte result)
	{
	}
	
	public void OnSendPrivateChat(byte result)
	{
		Debug.Log("onSendPrivateChatDone : " + result);

	}
	
	//onky room creator will get that
	public void OnStartGameDone(byte result)
	{
		Debug.Log("OnStartGameDone : " + result);
	}

	public void OnStopGameDone(byte result)
	{
		Debug.Log("OnStopGameDone : " + result);
	}
	
	public void OnGetMatchedRoomsDone(MatchedRoomsEvent eventObj)
	{
		Debug.Log("OnGetMatchedRoomsDone : " + eventObj.getResult());
		rooms = new List<string>();
		foreach (var roomData in eventObj.getRoomsData())
		{
		    Debug.Log("Room ID:" + roomData.getId() + ", " + roomData.getRoomOwner());
			guiText.text += "Room ID:" + roomData.getId() + ", " + roomData.getRoomOwner() + System.Environment.NewLine;
			rooms.Add(roomData.getId()); // add to the list of rooms id
		}
		
		Debug.Log("Rooms Amount: " + rooms.Count);
		if(rooms.Count > 0)
		{
			roomId = rooms[0];
			SC_AppWarpKit.JoinToRoom (rooms[0]);
		}
	}
	
	public void OnRoomCreated(RoomData eventObj)
	{
		Debug.Log("OnRoomCreated : " + eventObj.getName());
	}
	
	//both player get the notification
	public void OnUserLeftRoom(RoomData eventObj, string nameOfUser)
	{
		Debug.Log("OnUserLeftRoom : " + eventObj.getName());
	}
	
	//only host recieve it
	public void OnUserJoinRoom(RoomData eventObj, string userName)
	{
		Debug.Log("OnUserJoinRoom" + " " + eventObj.getRoomOwner() + " User connected" + userName);
		opponentName = userName;
		SC_AppWarpKit.StartGame();
	}
	
	public void OnPrivateUpdateReceived(string sender, byte[] eventObj, bool fromUdp)
	{
		//Debug.Log("OnPrivateUpdateReceived" + " " + messageReceived);
	}
	
	public void OnPrivateChatReceived(string sender, string message)
	{
//		Debug.Log("OnPrivateChatReceived (" + sender + ") " + message + " " + sc_GuiManager.currentUser.CurrentUserState + " " + haveStart + " " + haveStartApproved);
	
		if(sender != userName)
		{
			Debug.Log ("Message Recived, (" + sender + ") " + message);
			guiText.text += "Message Recived, (" + sender + ") " + message;
		}
	}
	
	public void OnGameStarted(string sender, string roomId, string nextTurn)
	{
//		Debug.Log("OnGameStarted" + " " + sender + " " + roomId + " " + nextTurn + " " + sc_GuiManager.currentUser.CurrentUserState);
		isMyTurn = true;
	}
	
	public void OnGameStopped(string sender, string roomId)
	{
		Debug.Log("OnGameStopped" + " " + sender + " " + roomId);
	}

	public void OnSendMove(byte result)
	{
		Debug.Log("OnSendMove : " + result);
	}

	public void OnMoveCompleted(MoveEvent move)
	{     
		Debug.Log("OnMoveCompleted" + " " + move.getNextTurn() + " " + move.getMoveData());
		if (move.getNextTurn () == userName)
			isMyTurn = true;
		else isMyTurn = false;
	}
}
