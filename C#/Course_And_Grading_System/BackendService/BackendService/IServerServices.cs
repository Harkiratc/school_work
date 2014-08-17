using System.ServiceModel;
using Common;
using System.Collections.Generic;
namespace Server
{

    [ServiceContract]

    public interface IServerService
    {
        [OperationContract]
        void SetUserState(int sessionId, int stateId, int stateCourse);
        [OperationContract]
        Common.Task getTask(int taskId);
        [OperationContract]
        List<User> GetUsers(int sessionId);
        [OperationContract]
        bool AddCoursesFromXml(int sessionId, string xmlString);
        [OperationContract]
        List<TaskGroupGrade> GetTaskGroupGrades(int taskGroupId);
        [OperationContract]
        int CountReportedTasksForUserInCourse(int userId, int courseId);
        [OperationContract]
        List<CourseGrade> GetCourseGrades(int courseId);
        [OperationContract]
        UserTask GetUserTask(int userId, int taskId);
        [OperationContract]
        Course GetCourse(int courseId);
        [OperationContract]
        bool AddUserTask(int sessionId, int userId, int taskId, int grade);
        [OperationContract]
        GradedCourse GetCourseGrade(int userId, int courseId);
        [OperationContract]
        List<TaskGroup> GetTaskGroups(int courseId);
        [OperationContract]
        List<Common.Task> GetTasks(int taskGroupId);
        [OperationContract]
        List<User> GetUsersForCourse(int courseId);
        [OperationContract]
        User GetUserFromId(int userId);
        [OperationContract]
        List<CourseAttentant> GetCourseAttentandsForCourse(int courseId);
        [OperationContract]
        int AddCourseAttendant(int sessionId, int userId, int courseId);
        [OperationContract]
        System.Collections.Generic.List<Course> getCourses(int sessionId);
        [OperationContract]
        int AddTask(int sessionId, int taskGroupId, string name, int gradeType, int weight);
        [OperationContract]
        int AddTaskGroupGrade(int sessionId, int taskgroupId, int limit, int value, string gradeName, string expression);
        [OperationContract]
        int AddTaskGroup(int sessionId, int courseId, string taskGroupName);
        [OperationContract]
        int AddCourseGrade(int sessionId, int courseId, string gradeName, int limit,  string expression);
        [OperationContract]
        int AddCourse(int sessionId, string code, string courseName, int period);
        [OperationContract]
        int GetAccessLevel(int sessionId);
        [OperationContract]
        int GetTaskReported(int taskId,int userId);
        [OperationContract]
        void Logout(int sessionId);
        [OperationContract]
        void UpdateReported(int taskId, int userId);

        [OperationContract]
        int Login(string username, string password);

        [OperationContract]
        User GetUser(int sessionId);

        [OperationContract]
        int AddUser(int sessionId, User user);
        


    }

}
