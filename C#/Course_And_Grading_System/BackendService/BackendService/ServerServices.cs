using System.ServiceModel;
using BackendService.Model;
using System.Collections.Generic;
using Common;

namespace Server
{

    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]

    public class ServerServices : IServerService
    {
        ServerImpl server;

        public ServerServices()
        {
            server = new ServerImpl();
        }

        public void SetUserState(int sessionId, int stateId, int stateCourse)
        {
            server.SetUserState(sessionId, stateId, stateCourse);
        }

        public Common.Task getTask(int taskId)
        {
            return server.getTask(taskId);
        }

        public List<User> GetUsers(int sessionId)
        {
            return server.GetUsers(sessionId);
        }

        public bool AddCoursesFromXml(int sessionId, string xmlString)
        {
            return server.AddCoursesFromXml(sessionId, xmlString);
        }

        public List<TaskGroupGrade> GetTaskGroupGrades(int taskGroupId)
        {
            return server.GetTaskGroupGrades(taskGroupId);
        }

        public int CountReportedTasksForUserInCourse(int userId, int courseId)
        {
            return server.CountReportedTasksForUserInCourse(userId, courseId);
        }

        public List<CourseGrade> GetCourseGrades(int courseId)
        {
            return server.GetCourseGrades(courseId);
        }

        public UserTask GetUserTask(int userId, int taskId)
        {
            return server.GetUserTask(userId, taskId);
        }

        public Course GetCourse(int courseId)
        {
            return server.GetCourse(courseId);
        }

        public bool AddUserTask(int sessionId, int userId, int taskId, int grade)
        {
            return server.AddUserTask(sessionId, userId, taskId, grade);
        }
        public GradedCourse GetCourseGrade(int userId, int courseId)
        {
            return server.GetCourseGrade(userId, courseId);
        }
        public List<TaskGroup> GetTaskGroups(int courseId)
        {
            return server.GetTaskGroups(courseId);
        }
        public List<Common.Task> GetTasks(int taskGroupId)
        {
            return server.GetTasks(taskGroupId);
        }

        public List<User> GetUsersForCourse(int courseId)
        {
            return server.GetUsersForCourse(courseId);
        }

        public List<CourseAttentant> GetCourseAttentandsForCourse(int courseId)
        {
            return server.GetCourseAttendantsForCourse(courseId);
        }

        public User GetUserFromId(int userId)
        {
            return server.GetUserFromId(userId);
        }

        public System.Collections.Generic.List<Course> getCourses(int sessionId)
        {
            return server.GetCourses(sessionId);
        }

        public int AddCourseAttendant(int sessionId, int userId, int courseId)
        {
            return server.AddCourseAttendant(sessionId, userId, courseId);
        }

        public int AddTask(int sessionId, int taskGroupId, string name, int gradeType, int weight)
        {
            return server.AddTask(sessionId, taskGroupId, name, gradeType, weight);
        }

        public int AddTaskGroupGrade(int sessionId, int taskgroupId, int limit, int value, string gradeName, string expression)
        {
            return server.AddTaskGroupGrade(sessionId,taskgroupId,limit,value,gradeName,expression);
        }

        public int AddTaskGroup(int sessionId, int courseId, string taskGroupName)
        {
            return server.AddTaskGroup(sessionId, courseId, taskGroupName);
        }

        public int AddCourseGrade(int sessionId, int courseId, string gradeName, int limit,  string expression)
        {
            return server.AddCourseGrade(sessionId, courseId, gradeName, limit, expression);
        }

        public void Logout(int sessionId)
        {
            server.Logout(sessionId);
        }

        public int Login(string username, string password)
        {

            return server.Login(username, password);
        }

        public User GetUser(int sessionId)
        {
            return server.GetUser(sessionId);
        }

        public int AddUser(int sessionId,User user)
        {
            return server.AddUser(sessionId,user);
        }

        public int AddCourse(int sessionId, string code, string courseName, int period)
        {
            return server.AddCourse(sessionId, code, courseName, period);
        }
        public int GetAccessLevel(int sessionId)
        {
            return server.GetAccessLevel(sessionId);
        }
        public int GetTaskReported(int taskId,int userId)
        {
            return server.GetTaskReported(taskId, userId);
        }
        public void UpdateReported(int taskId, int userId)
        {
            server.UpdateReported(taskId, userId);
        }
    }

}