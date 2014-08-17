using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Common;
public partial class MainLadok : System.Web.UI.Page
{
    Client.ServerServicesClient client = new Client.ServerServicesClient();
    int sessionId = -1;
    public void MainLadokView()
    {
        if (Request.Cookies["daisySession"] != null && Server.HtmlEncode(Request.Cookies["daisySession"]["session"]) != "-1")
        {
            sessionId = Convert.ToInt32(Server.HtmlEncode(Request.Cookies["daisySession"]["session"]));
            System.Diagnostics.Debug.WriteLine("Sessionid:" + sessionId);
            User user = client.GetUser(sessionId);
            //set header content of page
            HtmlGenericControl newHeadInner = new HtmlGenericControl("p");
            newHeadInner.InnerHtml = "Welcome <span>" + user.Firstname + " " + user.Lastname + "</span>, you are logged in as a admin";
            HtmlGenericControl newHeader = new HtmlGenericControl("div");
            newHeader.Attributes.Add("class", "headerInfo");
            newHeader.Controls.Add(newHeadInner);
            headerInfo.Controls.Add(newHeader);

            //Set the latest updates content of page
            HtmlGenericControl newLatestInner = new HtmlGenericControl("p");
            String latest = "";
            if (Request.QueryString["latest"] != null)
            {
                latest = Request.QueryString["latest"].ToString();
                latest = latest.Replace("-br-", "<br/>");
                if (latest != "Latest changes<br/><br/>")
                {
                    newLatestInner.InnerHtml = latest;
                    latestChanges.Controls.Add(newLatestInner);
                }
            }
            else
            {
                newLatestInner.InnerHtml = latest;
            }
            
            //Set main content of page
            System.Collections.Generic.List<Course> userCourses = client.getCourses(sessionId);
            foreach (Course course in userCourses)
            {
                HtmlGenericControl newCource = new HtmlGenericControl("div");
                newCource.Attributes.Add("class", "studentInfo");

                HtmlGenericControl newInner = new HtmlGenericControl("p");
                newInner.Attributes.Add("class", "centerMainCourse");
                String period = course.Period + "";
                String periodP = period.Substring(2,1);
                String p = period.Substring(0,2);
                newInner.InnerHtml = "<span>Course: </span>" + course.Code + " " + " - " + course.Name + ", period " + periodP + " -" + p;
                newCource.Controls.Add(newInner);
                HtmlGenericControl newGrades = new HtmlGenericControl("div");
                newGrades.Attributes.Add("class", "courseGrades");
                newGrades.Attributes.Add("display", "none");
                newGrades.Style["-webkit-box-shadow"] = "none";
                newGrades.Style["-moz-box-shadow:"] = "none";
                newGrades.Style["box-shadow"] = "none";

                HtmlGenericControl newIndexTable = new HtmlGenericControl("table");
                newIndexTable.Attributes.Add("id", "walla");
                newGrades.Controls.Add(newIndexTable);

                HtmlGenericControl newIndexRow = new HtmlGenericControl("tr");
                newIndexTable.Controls.Add(newIndexRow);

                HtmlGenericControl indexGrade = new HtmlGenericControl("th");
                indexGrade.InnerHtml = "Grade";

                HtmlGenericControl indexName = new HtmlGenericControl("th");
                indexName.InnerHtml = "Name";

                newIndexRow.Controls.Add(indexGrade);
                newIndexRow.Controls.Add(indexName);

                System.Collections.Generic.List<TaskGroup> taskgroups = client.GetTaskGroups(course.Id);
                int taskCounter = 0;
                foreach(TaskGroup tg in taskgroups)
                {
                    HtmlGenericControl indexTg = new HtmlGenericControl("th");
                    indexTg.InnerHtml = tg.Name;
                    newIndexRow.Controls.Add(indexTg);

                    System.Collections.Generic.List<Task> tasks = client.GetTasks(tg.Id);
                    foreach (Task task in tasks)
                    {
                        taskCounter++;
                    }
                }
                int counter = 0;
                System.Collections.Generic.List<CourseAttentant> attendants = client.GetCourseAttentandsForCourse(course.Id);
                foreach (CourseAttentant attendant in attendants)
                {
                    Common.GradedCourse graded = client.GetCourseGrade(attendant.UserId,course.Id);
                    int reportedCount = client.CountReportedTasksForUserInCourse(attendant.UserId,course.Id);
                    if (taskCounter != reportedCount)
                    {
                        HtmlGenericControl newAttendants = new HtmlGenericControl("tr");
                        User attendantUser = client.GetUserFromId(attendant.UserId);
                        if (counter % 2 == 0)
                        {
                            newAttendants.Style["background-color"] = "#a3a3a3";
                        }
                        counter++;
                        String grade = graded.GradeName;
                        if(grade == "")
                            grade = " n/a";

                        HtmlGenericControl index1Attendant = new HtmlGenericControl("td");
                        index1Attendant.InnerHtml = grade;
                        index1Attendant.Attributes.Add("class", "coursegrade");

                        HtmlGenericControl indexAttendant = new HtmlGenericControl("td");
                        indexAttendant.InnerHtml = attendantUser.Firstname + " " + attendantUser.Lastname + "  - " + attendantUser.Ssn;
                        indexAttendant.Attributes.Add("class", "name");
                        
                        newAttendants.Controls.Add(index1Attendant);
                        newAttendants.Controls.Add(indexAttendant);
                      
                        foreach(GradedTaskGroup tgroup in graded.GradedTaskGroups)
                        {
                            HtmlGenericControl indexTaskgroupTasks = new HtmlGenericControl("td");
                            indexTaskgroupTasks.Attributes.Add("class", "labels");
                            System.Collections.Generic.List<Task> tasks = client.GetTasks(tgroup.TaskGroup.Id);
                            HtmlGenericControl indexTaskGrade = new HtmlGenericControl("label");
                            indexTaskGrade.Attributes.Add("class", "tgGrade");
                            String tgGrade = tgroup.GradeName;
                            if(String.IsNullOrEmpty(tgGrade))
                                tgGrade = "n/a";
                            indexTaskGrade.InnerHtml = tgGrade;
                            indexTaskgroupTasks.Controls.Add(indexTaskGrade);

                            foreach (Task task in tasks)
                            {
                                int walla = 0;
                                walla = client.GetTaskReported(task.Id,attendant.UserId);
                                System.Diagnostics.Debug.WriteLine("getting reported or not: " + walla + "for task id: " + task.Id + "user id: " + attendant.UserId);
                                HtmlGenericControl indexTask = new HtmlGenericControl("label");
                                if (walla == 1)
                                {
                                    indexTask.InnerHtml = "<br/><input type=\"checkbox\" class=\"reported\" name=\"" + task.Id + "," + task.Name + "," + attendantUser.Id + "\" />" + task.Name;
                                }
                                else if (walla == 0)
                                {
                                    indexTask.InnerHtml = "<br/><input type=\"checkbox\" name=\"" + task.Id + "," + task.Name + "," + attendantUser.Id + "\" />" + task.Name;
                                }
                                indexTask.Attributes.Add("class", "labels");
                                indexTaskgroupTasks.Controls.Add(indexTask);
                            }
                            newAttendants.Controls.Add(indexTaskgroupTasks);

                        }
                        newIndexTable.Controls.Add(newAttendants);
                    }
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
    private Boolean checkUser()
    {
        return true;
    }
    protected override void OnError(EventArgs e)
    {
        if (Server.GetLastError().GetBaseException() is System.Web.HttpRequestValidationException)
        {
            System.Diagnostics.Debug.WriteLine("catched  shiet");
        }
    }
}