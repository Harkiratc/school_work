using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;

public class LoginBehind : Page
{    
    public void Login_server(object sender, EventArgs e)
	{
        if (username.Text != "" && password.Text != "")
        {
            System.Diagnostics.Debug.WriteLine("Your username is: " + username.Text + "  and your password is: " + password.Text);
        }
        else
        {
            System.Diagnostics.Debug.WriteLine("Shiet");
        }
	}
}