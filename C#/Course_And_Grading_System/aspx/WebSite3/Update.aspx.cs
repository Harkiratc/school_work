using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Client;
public partial class Update : System.Web.UI.Page
{
    Client.ServerServicesClient client = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        String updateInfo = "Latest changes -br--br-";
        client = new Client.ServerServicesClient();
        foreach(String values in Request.Form.Keys)
        {
            List<String> names = values.Split(',').ToList<String>();
            if (names != null)
            {
                String course = names.ElementAt(0).Trim();
                String courseuserid = names.ElementAt(2).Trim();
                System.Diagnostics.Debug.WriteLine(course + " userid: " + courseuserid);
                client.UpdateReported(Convert.ToInt32(course), Convert.ToInt32(courseuserid));
                Common.Task courseName = client.getTask(Convert.ToInt32(course));
                Common.User firstName = client.GetUserFromId(Convert.ToInt32(courseuserid));
                String cn = courseName.Name;
                String fn = firstName.Firstname;
                String ln = firstName.Lastname;
                updateInfo += "Approved " + courseName.Name + " for " + fn + " " + ln + "-br-"; 
            }
           
        }
        Response.Redirect("MainLadok.aspx?latest="+updateInfo, true);
    }
}