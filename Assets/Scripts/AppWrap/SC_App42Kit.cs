using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

/////////////Unity usage\\\\\\\\\\\\\
using UnityEngine;

///////app42 SDK\\\\\\\\
using com.shephertz.app42.paas.sdk.csharp;
using com.shephertz.app42.paas.sdk.csharp.user;
using com.shephertz.app42.paas.sdk.csharp.session;


public class SC_App42Kit:MonoBehaviour
{
    /////////varibles for app42\\\\\\
    private static SC_Listener_App42 sc_Listener_App42;
    private static UserService userService;
    //private static SessionService sessionService;
    private static ServiceAPI api;
    public static void App42Init(string apiKey , string secretKey)
    {
        sc_Listener_App42 = new SC_Listener_App42(); //initiate Listener with current GameLogic Script
        api = new ServiceAPI(apiKey, secretKey);
        userService = api.BuildUserService();
        Debug.Log("apiKey " + apiKey + Environment.NewLine + "secretKey " + secretKey);
    //    sessionService = api.BuildSessionService();
    //    Debug.Log(sessionService.GetSessionId());
    }
    
    //invoke when Login is onvoked
    public static void InitUser(string UserName, string UserPWD, string UserEmail)
    {
      userService.CreateUser(UserName, UserPWD, UserEmail, sc_Listener_App42);
     }

    //invoke when Login is LogOut
    public void DeleteUser(string UserName)
    {
        Dictionary<string, string> otherMetaHeaders = new Dictionary<string, string>();  
        otherMetaHeaders.Add("deletePermanent", "true");  
        userService.SetOtherMetaHeaders(otherMetaHeaders);  
        userService.DeleteUser(UserName, sc_Listener_App42);
       
    }

    //public void AuthenticateUser(string UserName, string password)
    //{
    //    userService.Authenticate(UserName, password, sc_Listener_App42);
    //}

    //public void GetSession(string UserName)
    //{
    //   sessionService.GetSession(UserName, sc_Listener_App42);
    //}

    //public void Logout(string userName)
    //{
    //    userService.Logout(sessionService.GetSession(userName, true).ToString(), sc_Listener_App42);
    //}
    
    
    }//end of class: SC_Connection_App42

