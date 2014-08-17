using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Common;

public class HelpFunctions : System.Web.UI.Page
{
    public void HelpLogOut(Client.ServerServicesClient client,int sessionId)
    {
        client.Logout(sessionId);
    }
}
