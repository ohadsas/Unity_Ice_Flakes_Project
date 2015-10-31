using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/////////////Unity usage\\\\\\\\\\\\\
using UnityEngine;
using AssemblyCSharp;

///////AppWarp + app42 references\\\\\\\\
using com.shephertz.app42.paas.sdk.csharp.user;
using com.shephertz.app42.paas.sdk.csharp;
using com.shephertz.app42.gaming.multiplayer.client;
using System.Collections;
using com.shephertz.app42.gaming.multiplayer.client.command;


public class SC_AppWarpKit : MonoBehaviour
{
    /////////varibles for Appwarp\\\\\\
    private static SC_Listener_AppWarp sc_Listener_AppWarp;
    
    //invoke when sc_GameEngine is created after clicked "Guest log" or Facebook User
    public static void WarpInit(string apiKey, string secretKey) //add object repsond
    {
            sc_Listener_AppWarp = new SC_Listener_AppWarp(); //initiate Listener with current GameLogic Script
            WarpClient.initialize(apiKey, secretKey);
            WarpClient.GetInstance().AddConnectionRequestListener(sc_Listener_AppWarp);
            WarpClient.GetInstance().AddChatRequestListener(sc_Listener_AppWarp);
            WarpClient.GetInstance().AddLobbyRequestListener(sc_Listener_AppWarp);
            WarpClient.GetInstance().AddNotificationListener(sc_Listener_AppWarp);
            WarpClient.GetInstance().AddUpdateRequestListener(sc_Listener_AppWarp);
            WarpClient.GetInstance().AddZoneRequestListener(sc_Listener_AppWarp);
            WarpClient.GetInstance().AddRoomRequestListener(sc_Listener_AppWarp);
            WarpClient.GetInstance().AddTurnBasedRoomRequestListener(sc_Listener_AppWarp);
     }

    //invoke when want to connect to platform
	public static void connectToAppWarp(string userNAme)
    {
        WarpClient.GetInstance().Connect(userNAme);
    }
    
    //Disconnect
	public static void DisconnectFromAppWarp()
    {
        WarpClient.GetInstance().Disconnect();
    }

	public static void GetRoomWithProperties(Dictionary<string, object> properties)
    {
        WarpClient.GetInstance().GetRoomWithProperties(properties);
    }

    //start the game
	public static void StartGame()
    {
        WarpClient.GetInstance().startGame();
    }
    //stop the game
	public static void StopGame()
    {
        WarpClient.GetInstance().stopGame();
    }

    //create a turnBase room
	public static void CreateTurnBaseRoom(string roomName,string roomOwner, int maxUsersInRoom, Dictionary<string,object> tableProperties, int TurnTime )
    {
        WarpClient.GetInstance().CreateTurnRoom(roomName, roomOwner, maxUsersInRoom, tableProperties, TurnTime);
    }

    //function find a proper room on a given range
	public static void GetRoomsInRange(int minUsersInRoom, int maxUsersInRoom)
    {
        WarpClient.GetInstance().GetRoomsInRange(minUsersInRoom, maxUsersInRoom); // connect to a room with maximum 2 players
    }
	public static void JoinToRoom(string roomID)
    {
        WarpClient.GetInstance().JoinRoom(roomID); 
    }

	public static void LeaveFromRoom(string roomID)
    {
        WarpClient.GetInstance().LeaveRoom(roomID);
    }

    //register to room in order to get messages,updates and more
	public static void RegisterToRoom(string roomID)
    {
        WarpClient.GetInstance().SubscribeRoom(roomID);
    }

    //Unregister from room
	public static void UnRegisterFromRoom(string roomID)
    {
        WarpClient.GetInstance().UnsubscribeRoom(roomID);
    }

    //get live updates from current room 
	public static void GetLiveRoomInfo(string roomID)
    {
        WarpClient.GetInstance().GetLiveRoomInfo(roomID);
    }

    //send move
	public static void sendMove(string move)
    {
        WarpClient.GetInstance().sendMove(move);
    }

    //send chat over appWarp
	public static void sendChat(string message)
    {
        WarpClient.GetInstance().SendChat(message);
    }

    //send update to users in the room over appWarp
	public static void SendUpdateToUsersInRoom(byte[] updateToSend)
    {
        WarpClient.GetInstance().SendUpdatePeers(updateToSend);
    }
    
    //send private update to user over appWarp
	public static void sendPrivateUpdate(string toUserName , byte[] message)
    {
        WarpClient.GetInstance().sendPrivateUpdate(toUserName, message);
    }

    //send private update to user over appWarp
	public static void sendPrivateChat(string toUserName, string message)
    {
        WarpClient.GetInstance().sendPrivateChat(toUserName, message);
    }

	public static void DeleteRoom(string roomId)
    {
        WarpClient.GetInstance().DeleteRoom(roomId);
    }
    
}//end of class: SC_FourInALine_AppWarp

