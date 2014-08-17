using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Common;

public partial class MainStudent : System.Web.UI.Page
{
    Client.ServerServicesClient client = new Client.ServerServicesClient();
    int sessionId = -1;
    public void MainStudentView()
    {
        System.Diagnostics.Debug.WriteLine("entered mainStudent view:" + Server.HtmlEncode(Request.Cookies["daisySession"]["session"]));
        if (Request.Cookies["daisySession"] != null && Server.HtmlEncode(Request.Cookies["daisySession"]["session"]) != "-1")
        {
            sessionId = Convert.ToInt32(Server.HtmlEncode(Request.Cookies["daisySession"]["session"]));
            System.Diagnostics.Debug.WriteLine("Sessionid:" + sessionId);
            Common.User user = client.GetUser(sessionId);
            //set header content of page
            HtmlGenericControl newHeadInner = new HtmlGenericControl("p");
            newHeadInner.InnerHtml = "Welcome <span>" + user.Firstname + " " + user.Lastname + "</span>, you are logged in as a student"; 
            HtmlGenericControl newHeader = new HtmlGenericControl("div");
            newHeader.Attributes.Add("class", "headerInfo");
            newHeader.Controls.Add(newHeadInner);
            headerInfo.Controls.Add(newHeader);

            //Set main content of page
            System.Collections.Generic.List<Course> userCourses = client.getCourses(sessionId);
            foreach (Course course in userCourses)
            {
                Common.GradedCourse graded = client.GetCourseGrade(user.Id, course.Id);
                String grade = graded.GradeName;
                if (String.IsNullOrEmpty(grade))
                    grade = "n/a";
                
                HtmlGenericControl newInner = new HtmlGenericControl("p");
                newInner.Attributes.Add("class", "centerMainCourse");
                newInner.InnerHtml = "<span>Course</span>: " + course.Code + " " + " - " + course.Name + " <span class=\"studentGrade\">  Grade: <span class=\"studentGrade1\">" + grade + "</span></span>";
                
                HtmlGenericControl newCource = new HtmlGenericControl("div");
                newCource.Attributes.Add("class", "studentInfo");
                newCource.Controls.Add(newInner);
 
                HtmlGenericControl newGrades = new HtmlGenericControl("div");
                newGrades.Attributes.Add("class", "courseGrades");
                newGrades.Attributes.Add("display", "none");

                System.Collections.Generic.List<TaskGroup> taskgroup = client.GetTaskGroups(course.Id);
                foreach (GradedTaskGroup tg in graded.GradedTaskGroups)
                {
                    HtmlGenericControl newInnerTg = new HtmlGenericControl("div");
                    newInnerTg.Attributes.Add("class", "innertaskgroup");

                    HtmlGenericControl newInnerTgH = new HtmlGenericControl("h4");
                    newInnerTgH.InnerHtml = tg.TaskGroup.Name + "<span> - " + tg.GradeName + "</span>";
                    newInnerTg.Controls.Add(newInnerTgH);

                    System.Collections.Generic.List<Task> task = client.GetTasks(tg.TaskGroup.Id);
                    foreach (Task cTask in task)
                    {
                        HtmlGenericControl newInnerTask = new HtmlGenericControl("p");
                        newInnerTask.InnerHtml = " - " + cTask.Name;
                        newInnerTg.Controls.Add(newInnerTask);
                    }
                    newGrades.Controls.Add(newInnerTg);
                }
                courses.Controls.Add(newCource);
                courses.Controls.Add(newGrades);
            }
        }
        else
            Response.Redirect("Login.aspx", true);
    }
    public void Logout(object sender, EventArgs e)
    {
        if (client != null && sessionId != 0)
        {
            System.Diagnostics.Debug.WriteLine("entered if statement in logout");
            System.Diagnostics.Debug.WriteLine("Sessionid in logout:" + Convert.ToInt32(Server.HtmlEncode(Request.Cookies["daisySession"]["session"])));
            client.Logout(Convert.ToInt32(Server.HtmlEncode(Request.Cookies["daisySession"]["session"])));
            HttpCookie session = new HttpCookie("daisySession");
            session.Expires = DateTime.Now.AddDays(-1);
            Response.Cookies.Add(session);
            System.Diagnostics.Debug.WriteLine("before end");
            Response.Redirect("Login.aspx", true);
        }
    }
}