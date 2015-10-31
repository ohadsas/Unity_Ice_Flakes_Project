using UnityEngine;


/////appwarp + app42 \\\\
using com.shephertz.app42.gaming.multiplayer.client;
using com.shephertz.app42.gaming.multiplayer.client.events;
using com.shephertz.app42.gaming.multiplayer.client.listener;
using com.shephertz.app42.gaming.multiplayer.client.command;
using com.shephertz.app42.gaming.multiplayer.client.message;
using com.shephertz.app42.paas.sdk.csharp;
using com.shephertz.app42.paas.sdk.csharp.user;

using System;
using System.Collections.Generic;


public class SC_Listener_AppWarp : ConnectionRequestListener, LobbyRequestListener, ZoneRequestListener, RoomRequestListener, ChatRequestListener, UpdateRequestListener, NotifyListener, TurnBasedRoomListener
{
     # region GamyTech_events
    ////////////////GamyTech events\\\\\\\\\\\\\\\\\\\\
    public delegate void Success(ConnectEvent eventObj);
    public static event Success onConnectToAppWarp;
    public static event Success onDisconnectFromAppWarp;
    public delegate void roomsEvent(RoomEvent eventObj);
    public static event roomsEvent OnSubscribeToRoom;
    public static event roomsEvent OnUnSubscribeToRoom;
    public static event roomsEvent OnJoinToRoom;
    public static event roomsEvent OnLeaveFromRoom;
    public static event roomsEvent OnCreateRoomDone;
    public delegate void liveRoomInfo(LiveRoomInfoEvent eventObj);
    public static event liveRoomInfo onGetLiveRoomInfo;
    public static event liveRoomInfo OnSetCustomRoomDataDone;
    public static event liveRoomInfo OnUpdatePropertyDone;
    public static event liveRoomInfo OnGetLiveLobbyInfoDone;
    public delegate void sendSucceed(byte result);
    public static event sendSucceed OnSendPrivateUpdate;
    public static event sendSucceed OnSendUpdate;
    public static event sendSucceed OnSendChat;
    public static event sendSucceed OnSendPrivateChat;
    public static event sendSucceed OnSendMove;
    public static event sendSucceed OnStartGameDone;
    public static event sendSucceed OnStopGameDone;
    public static event sendSucceed OnLockPropertiesDone;
    public delegate void MatchedRooms(MatchedRoomsEvent eventObj);
    public static event MatchedRooms OnMatchedRooms;
    public delegate void roomData(RoomData eventObj);
    public static event roomData OnRoomCreated;
    public static event roomData OnRoomDestroyed;
    public delegate void UserNotifiersFromRoom(RoomData eventObj , string userName);
    public static event UserNotifiersFromRoom OnUserLeftRoom;
    public static event UserNotifiersFromRoom OnUserJoinRoom;
    public delegate void chatEvent(ChatEvent eventObj);
    public static event chatEvent OnChatReceived;
    public delegate void updateEvent(UpdateEvent eventObj);
    public static event updateEvent OnUpdatePeersReceived;
    public delegate void PrivateUpdateReceived(string sender, byte[] update, bool fromUdp);
    public static event PrivateUpdateReceived OnPrivateUpdateReceived;
    public delegate void PrivateChatReceived(string sender, string message);
    public static event PrivateChatReceived OnPrivateChatReceived;
    public delegate void MoveCompleted(MoveEvent move);
    public static event MoveCompleted OnMoveCompleted;
    public delegate void GameStarted(string sender, string roomId, string nextTurn);
    public static event GameStarted OnGameStarted;
    public delegate void GameStopped(string sender, string roomId);
    public static event GameStopped OnGameStopped;
    public delegate void MoveHistory(byte result, MoveEvent[] history);
    public static event MoveHistory OnGetMoveHistoryDone;
    public delegate void liveUserInfoEvent(LiveUserInfoEvent eventObj);
    public static event liveUserInfoEvent OnSetCustomUserDataDone;
    public delegate void LobyEventsData(LobbyData eventObj, string username);
    public static event LobyEventsData OnUserLeftLobby;
    public static event LobyEventsData OnUserJoinedLobby;
    public delegate void LobyEvents(LobbyEvent eventObj);
    public static event LobyEvents OnJoinLobbyDone;
    public static event LobyEvents OnLeaveLobbyDone;
    public static event LobyEvents OnSubscribeLobbyDone;
    public static event LobyEvents OnUnSubscribeLobbyDone;
    public delegate void RoomProperty(RoomData roomData, string sender, Dictionary<string, object> properties, Dictionary<string, string> lockedPropertiesTable);
    public static event RoomProperty OnUserChangeRoomProperty;
    public delegate void UserAction(string locid, bool isLobby, string username);
    public static event UserAction OnUserPaused;
    public static event UserAction OnUserResumed;
    public static event roomsEvent OnDeleteRoomDone;

    # endregion

    /////////////////////////////// ConnectionRequestListener \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
    public void onConnectDone(ConnectEvent eventObj)
    {
        if (onConnectToAppWarp != null)
            onConnectToAppWarp(eventObj);//send event 
    }

    public void onInitUDPDone(byte res)
    {
    }

    public void onDisconnectDone(ConnectEvent eventObj)
    {
        if (onDisconnectFromAppWarp != null)
            onDisconnectFromAppWarp(eventObj);
    }

    /////////////////////////////// RoomRequestListener \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
    public void onSubscribeRoomDone(RoomEvent eventObj)
    {
        if (OnSubscribeToRoom != null)
            OnSubscribeToRoom(eventObj);
        
    }

    public void onUnSubscribeRoomDone(RoomEvent eventObj)
    {
        if (OnUnSubscribeToRoom != null)
            OnUnSubscribeToRoom(eventObj);
    }

    public void onJoinRoomDone(RoomEvent eventObj)
    {
        if (OnJoinToRoom != null)
            OnJoinToRoom(eventObj);
    }

    public void onLeaveRoomDone(RoomEvent eventObj)
    {
        if (OnLeaveFromRoom != null)
            OnLeaveFromRoom(eventObj);
    }

    //invoke when GetLiveroomInfo is called
    public void onGetLiveRoomInfoDone(LiveRoomInfoEvent eventObj)
    {
        if (onGetLiveRoomInfo != null)
                onGetLiveRoomInfo(eventObj);
    }

    public void onSetCustomRoomDataDone(LiveRoomInfoEvent eventObj)
    {
        if (OnSetCustomRoomDataDone != null)
            OnSetCustomRoomDataDone(eventObj);
    }

    public void onUpdatePropertyDone(LiveRoomInfoEvent eventObj)
    {
        if (OnUpdatePropertyDone != null)
            OnUpdatePropertyDone(eventObj);
    }

    public void onLockPropertiesDone(byte result)
    {
        if (OnLockPropertiesDone != null)
            OnLockPropertiesDone(result);
    }

    public void onSendPrivateUpdateDone(byte result)
    {
        if (OnSendPrivateUpdate != null)
            OnSendPrivateUpdate(result);
    }

    public void onUnlockPropertiesDone(byte result)
    {
        throw new NotImplementedException();
    }

    ////////////////////////////////////// Zone Request Listener \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\

    public void onGetAllRoomsDone(AllRoomsEvent eventObj)
    {
        throw new NotImplementedException();
    }

    public void onCreateRoomDone(RoomEvent eventObj)
    {
        if (OnCreateRoomDone != null)
            OnCreateRoomDone(eventObj);
    }

    public void onGetOnlineUsersDone(AllUsersEvent eventObj)
    {
        throw new NotImplementedException();
    }

    public void onGetLiveUserInfoDone(LiveUserInfoEvent eventObj)
    {
        throw new NotImplementedException();
    }

    public void onSetCustomUserDataDone(LiveUserInfoEvent eventObj)
    {
        if (OnSetCustomUserDataDone != null)
            OnSetCustomUserDataDone(eventObj);
    }

    //invoke when GetRoomInRange is called
    public void onGetMatchedRoomsDone(MatchedRoomsEvent eventObj)
    {
        if (OnMatchedRooms != null)
            OnMatchedRooms(eventObj);
    }
    ////////////////////////////////////////Lobby Request Listener\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\

    public void onJoinLobbyDone(LobbyEvent eventObj)
    {
        if (OnJoinLobbyDone != null)
            onJoinLobbyDone(eventObj);
    }

    public void onLeaveLobbyDone(LobbyEvent eventObj)
    {
        if (OnLeaveLobbyDone != null)
            OnLeaveLobbyDone(eventObj);
    }

    public void onSubscribeLobbyDone(LobbyEvent eventObj)
    {
        if (OnSubscribeLobbyDone != null)
            OnSubscribeLobbyDone(eventObj);
    }

    public void onUnSubscribeLobbyDone(LobbyEvent eventObj)
    {
        if (OnJoinLobbyDone != null)
            onJoinLobbyDone(eventObj);
    }

    public void onGetLiveLobbyInfoDone(LiveRoomInfoEvent eventObj)
    {
        if (OnGetLiveLobbyInfoDone != null)
            OnGetLiveLobbyInfoDone(eventObj);
    }

    ///////////////////////////////////////////////////////Notify Listener\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
    public void onRoomCreated(RoomData eventObj)
    {
       if(OnRoomCreated != null)
          OnRoomCreated(eventObj);
        
    }

    public void onRoomDestroyed(RoomData eventObj)
    {
        if(OnRoomDestroyed != null)
            OnRoomDestroyed(eventObj);
    }

    public void onUserLeftRoom(RoomData eventObj, string username)
    {
        if (OnUserLeftRoom != null)
            OnUserLeftRoom(eventObj, username);
    }

    //only host will recieve the notification 
    public void onUserJoinedRoom(RoomData eventObj, string _UserName)
    {
        if (OnUserJoinRoom != null)
            OnUserJoinRoom(eventObj, _UserName);
   }

    public void onUserLeftLobby(LobbyData eventObj, string username)
    {
        if (OnUserLeftLobby != null)
            OnUserLeftLobby(eventObj, username);
    }

    public void onUserJoinedLobby(LobbyData eventObj, string username)
    {
        if(OnUserJoinedLobby != null)
            OnUserJoinedLobby(eventObj, username);
    }

    public void onChatReceived(ChatEvent eventObj)
    {
        if (OnChatReceived != null)
            OnChatReceived(eventObj);
    }

    //invoked when SendUpdatePeer pressed, the updates get to everybody in the room
    public void onUpdatePeersReceived(UpdateEvent eventObj)
    {
        if (OnUpdatePeersReceived != null)
            OnUpdatePeersReceived(eventObj);   
    }

    void onUserChangeRoomProperty(RoomData roomData, string sender, Dictionary<string, object> properties, Dictionary<string, string> lockedPropertiesTable)
    {
        if (OnUserChangeRoomProperty != null)
            OnUserChangeRoomProperty(roomData, sender, properties, lockedPropertiesTable);
    }

    public void onPrivateUpdateReceived(string sender, byte[] update, bool fromUdp)
    {
        if (OnPrivateUpdateReceived != null)
            OnPrivateUpdateReceived(sender, update, fromUdp);
        
    }
    
    public void onPrivateChatReceived(string sender, string message)
    {
        if (OnPrivateChatReceived != null)
            OnPrivateChatReceived(sender, message);
    }

    //other players in the room get the last move
    public void onMoveCompleted(MoveEvent move)
    {
        if (OnMoveCompleted != null)
            OnMoveCompleted(move);
        
    }

    // Invoked when a user start game in a turn based room(both players get the logic)
    public void onGameStarted(string sender, string roomId, string nextTurn)
    {
       if (OnGameStarted != null)
            OnGameStarted(sender, roomId, nextTurn);

    }

    // Invoked when a user stop game in a turn based room(both players get the logic)
    public void onGameStopped(string sender, string roomId)
    {
        if (OnGameStopped != null)
            OnGameStopped(sender, roomId);
    }

    public void onUserPaused(string locid, bool isLobby, string username)
    {
        if (OnUserPaused != null)
            OnUserPaused(locid, isLobby, username);
    }

    public void onUserResumed(string locid, bool isLobby, string username)
    {
        if (OnUserResumed != null)
            OnUserResumed(locid, isLobby, username);
    }
    //////////////////////////////////////////////////Update Request Listener\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
    public void onSendUpdateDone(byte result)
    {
      if (OnSendUpdate != null)
        OnSendUpdate(result);
    }

    //////////////////////////////////////////////////Chat Request Listener\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\

    public void onSendChatDone(byte result)
    {
        if (OnSendChat != null)
            OnSendChat(result);
    }

    public void onSendPrivateChatDone(byte result)
    {
        if(OnSendPrivateChat != null)
            OnSendPrivateChat(result);
    }

    //////////////////////////////////////////////////Turn Based Room Listener\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\

    public void onSendMoveDone(byte result)  // Invoked when a response for a sendMove request is received.  
    {
        if (OnSendMove != null)
            OnSendMove(result);
    }

    //Invoked when a response for a startGame request is received.(only the player that start the game will recieve it)  
    public void onStartGameDone(byte result)
    {
        if (OnStartGameDone != null)
            OnStartGameDone(result);
    }

    ///Invoked when a response for a stopGame request is received.  

    public void onStopGameDone(byte result)
    {
        if (OnStopGameDone != null)
            OnStopGameDone(result);
    }

    ///Invoked when a response for a getMoveHistory request is received.  
    public void onGetMoveHistoryDone(byte result, MoveEvent[] history)
    {
        if (OnGetMoveHistoryDone != null)
            OnGetMoveHistoryDone(result, history);
    }

    public void onDeleteRoomDone(RoomEvent eventObj)
    {
        if (OnDeleteRoomDone != null)
            OnDeleteRoomDone(eventObj);
    }

    void NotifyListener.onUserChangeRoomProperty(RoomData roomData, string sender, Dictionary<string, object> properties, Dictionary<string, string> lockedPropertiesTable)
    {
        throw new NotImplementedException();
    }
}
		