using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Common;

namespace Client
{
    public class Client
    {
        
         private ChannelFactory<Server.IServerService> myChannelFactory;
         private Server.IServerService comInterface;
        public Client(String URL)
        {
            BasicHttpBinding myBinding = new BasicHttpBinding();
            EndpointAddress  myEndpoint = new EndpointAddress(URL);
            myChannelFactory = new ChannelFactory<Server.IServerService>(myBinding, myEndpoint);
            comInterface = myChannelFactory.CreateChannel();
        }

        public void SetUserState(int sessionId, int stateId, int stateCourse)
        {
            comInterface.SetUserState(sessionId, stateId, stateCourse);
        }

        public Common.Task getTask(int taskId)
        {
            return comInterface.getTask(taskId);
        }

        public List<User> GetUsers(int sessionId)
        {
            return comInterface.GetUsers(sessionId);
        }

        public bool AddCoursesFromXml(int sessionId, String xmlString)
        {
            return comInterface.AddCoursesFromXml(sessionId,xmlString);
        }

        public List<TaskGroupGrade> GetTaskGroupGrades(int taskGroupId)
        {
            return comInterface.GetTaskGroupGrades(taskGroupId);
        }

        public int CountReportedTasksForUserInCourse(int userId, int courseId)
        {
            return comInterface.CountReportedTasksForUserInCourse(userId, courseId);
        }

        public UserTask GetUserTask(int userId, int taskId)
        {
            return comInterface.GetUserTask(userId, taskId);
        }

        public List<CourseGrade> GetCourseGrades(int courseId)
        {
            return comInterface.GetCourseGrades(courseId);
        }

        public Course GetCourse(int courseId)
        {
            return comInterface.GetCourse(courseId);
        }

        public bool AddUserTask(int sessionId, int userId, int taskId, int grade)
        {
            return comInterface.AddUserTask(sessionId,userId,taskId,grade);
        }

        public GradedCourse GetCourseGrade(int userId, int courseId)
        {
            return comInterface.GetCourseGrade(userId,courseId);
        }

        public List<TaskGroup> GetTaskGroups(int courseId)
        {
            return comInterface.GetTaskGroups(courseId);
        }

        public List<Common.Task> GetTasks(int taskGroupId)
        {
            return comInterface.GetTasks(taskGroupId);
        }

        public List<User> GetUsersForCourse(int courseId)
        {
            return comInterface.GetUsersForCourse(courseId);
        }

        public User GetUserFromId(int userId)
        {
            return comInterface.GetUserFromId(userId);
        }

        public List<CourseAttentant> GetCourseAttendantsCourseForCourse(int courseId)
        {
            return comInterface.GetCourseAttentandsForCourse(courseId);
        }

        public System.Collections.Generic.List<Course> getCourses(int sessionId)
        {
            return comInterface.getCourses(sessionId);
        }

        public int AddCourseAttendant(int sessionId, int userId, int courseId)
        {
            return comInterface.AddCourseAttendant(sessionId, userId, courseId);
        }

        public int AddTask(int sessionId, int taskGroupId, string name, int gradeType, int weight)
        {
            return comInterface.AddTask(sessionId, taskGroupId, name, gradeType, weight);
        }

        public int AddTaskGroupGrade(int sessionId, int taskgroupId, int limit, int value, String gradeName, String expression)
        {
            return comInterface.AddTaskGroupGrade(sessionId, taskgroupId, limit, value, gradeName, expression);
        }

        public int AddTaskGroup(int sessionId, int courseId, string taskGroupName)
        {
            return comInterface.AddTaskGroup(sessionId, courseId, taskGroupName);
        }

        public int AddCourseGrade(int sessionId, int courseId, string gradeName, int limit, string expression)
        {
            return comInterface.AddCourseGrade(sessionId, courseId, gradeName, limit, expression);
        }

        public int AddUser(int sessionId, User user)
        {
            return comInterface.AddUser(sessionId, user);
        }

        public int AddCourse(int sessionId, string code, string courseName, int period)
        {
            return comInterface.AddCourse(sessionId, code, courseName, period);
        }
        public int GetAccessLevel(int sessionId)
        {
            return comInterface.GetAccessLevel(sessionId);
        }

        public void Logout(int sessionId)
        {
            comInterface.Logout(sessionId);
        }

        public int Login(String username, String password)
        {
            return comInterface.Login(username, password);
        }

        public User GetUser(int sessionId)
        {
            return comInterface.GetUser(sessionId);
        }
        public int GetTaskReported(int taskId, int userId)
        {
            return comInterface.GetTaskReported(taskId, userId);
        }
        public void UpdateReported(int taskId, int userId)
        {
            comInterface.UpdateReported(taskId, userId);
        }
    }
}
