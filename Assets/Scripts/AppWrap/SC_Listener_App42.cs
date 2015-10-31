using UnityEngine;


/////app42 SDK\\\\

using com.shephertz.app42.paas.sdk.csharp;
using com.shephertz.app42.paas.sdk.csharp.user;
using System;
using System.Collections.Generic;
	

public class SC_Listener_App42:App42CallBack
{

    # region App42Events
    public delegate void Success(object respond);
    public static event Success onCreatedUserApp42;
    public delegate void App42Connect(Exception error);
    public static event App42Connect OnExceptionFromApp42;
#endregion

    public void OnSuccess(object respond)
    {
        if (onCreatedUserApp42 != null)
            onCreatedUserApp42(respond);
	  
	 }//end of function onSuccess
	
     public void OnException(Exception e)
     {
       if (onCreatedUserApp42 != null)
           OnExceptionFromApp42(e);
	 }

    }//end of class listener
