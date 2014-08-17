using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;

public partial class Home : System.Web.UI.Page
{
    Client.ServerServicesClient client = new Client.ServerServicesClient();
    public void Login_server(object sender, EventArgs e)
    {
        System.Diagnostics.Debug.WriteLine("entered login server");
        if (Request.Cookies["daisySession"] != null )
        {
            int sessionIdC = Convert.ToInt32(Server.HtmlEncode(Request.Cookies["daisySession"]["session"]));
            int accessLevelC = Convert.ToInt32(Server.HtmlEncode(Request.Cookies["daisySession"]["level"]));
            System.Diagnostics.Debug.WriteLine("Session id in login_server: " + sessionIdC);
            String sessionNameC = Request.Cookies["daisySession"]["username"];
            if (sessionIdC != -1 || sessionNameC != "loggedout")
            {
                if (accessLevelC == 0)
                {
                    System.Diagnostics.Debug.WriteLine("entered mainstudent");
                    Response.Redirect("MainStudent.aspx", true);
                }
                else if (accessLevelC == 1)
                    Response.Redirect("MainLadok.aspx", true);  
            }
        }
        else
        {
            if (username.Text != "" && password.Text != "")
            {
                System.Diagnostics.Debug.WriteLine("Username: " + username.Text + ", password: " + password.Text );
                int sessionId = client.Login(username.Text, password.Text);

                if (sessionId != -1)
                {
                    User user = client.GetUser(sessionId);
                    HttpCookie session = new HttpCookie("daisySession");
                    session.Values["session"] = sessionId.ToString();
                    session.Values["username"] = username.Text;
                    session.Values["level"] = user.Accesslevel.ToString();
                    session.Expires = DateTime.Now.AddDays(1);
                    Response.Cookies.Add(session);
                    if (client.GetAccessLevel(sessionId) == 0)
                    {
                        Response.Redirect("MainStudent.aspx", true);
                    }
                    else if (client.GetAccessLevel(sessionId) == 1)
                    {
                        Response.Redirect("MainLadok.aspx", true);
                    }
                }
                else
                {
                    response.Text = "Username / Password combinaiton failed";
                }
            }
            else if (username.Text == "" && password.Text == ""){
            }
            else
            {
                response.Text = "Enter both username and password";
            }
        } 
    }
}