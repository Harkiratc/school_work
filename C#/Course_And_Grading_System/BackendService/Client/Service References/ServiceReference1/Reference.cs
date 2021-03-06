﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.17929
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Client.ServiceReference1 {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="ServiceReference1.IServerService")]
    public interface IServerService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServerService/GetUsers", ReplyAction="http://tempuri.org/IServerService/GetUsersResponse")]
        Common.User[] GetUsers(int sessionId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServerService/GetUsers", ReplyAction="http://tempuri.org/IServerService/GetUsersResponse")]
        System.Threading.Tasks.Task<Common.User[]> GetUsersAsync(int sessionId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServerService/AddCoursesFromXml", ReplyAction="http://tempuri.org/IServerService/AddCoursesFromXmlResponse")]
        bool AddCoursesFromXml(int sessionId, string xmlString);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServerService/AddCoursesFromXml", ReplyAction="http://tempuri.org/IServerService/AddCoursesFromXmlResponse")]
        System.Threading.Tasks.Task<bool> AddCoursesFromXmlAsync(int sessionId, string xmlString);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServerService/GetTaskGroupGrades", ReplyAction="http://tempuri.org/IServerService/GetTaskGroupGradesResponse")]
        Common.TaskGroupGrade[] GetTaskGroupGrades(int taskGroupId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServerService/GetTaskGroupGrades", ReplyAction="http://tempuri.org/IServerService/GetTaskGroupGradesResponse")]
        System.Threading.Tasks.Task<Common.TaskGroupGrade[]> GetTaskGroupGradesAsync(int taskGroupId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServerService/CountReportedTasksForUserInCourse", ReplyAction="http://tempuri.org/IServerService/CountReportedTasksForUserInCourseResponse")]
        int CountReportedTasksForUserInCourse(int userId, int courseId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServerService/CountReportedTasksForUserInCourse", ReplyAction="http://tempuri.org/IServerService/CountReportedTasksForUserInCourseResponse")]
        System.Threading.Tasks.Task<int> CountReportedTasksForUserInCourseAsync(int userId, int courseId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServerService/GetCourseGrades", ReplyAction="http://tempuri.org/IServerService/GetCourseGradesResponse")]
        Common.CourseGrade[] GetCourseGrades(int courseId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServerService/GetCourseGrades", ReplyAction="http://tempuri.org/IServerService/GetCourseGradesResponse")]
        System.Threading.Tasks.Task<Common.CourseGrade[]> GetCourseGradesAsync(int courseId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServerService/GetUserTask", ReplyAction="http://tempuri.org/IServerService/GetUserTaskResponse")]
        Common.UserTask GetUserTask(int userId, int taskId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServerService/GetUserTask", ReplyAction="http://tempuri.org/IServerService/GetUserTaskResponse")]
        System.Threading.Tasks.Task<Common.UserTask> GetUserTaskAsync(int userId, int taskId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServerService/GetCourse", ReplyAction="http://tempuri.org/IServerService/GetCourseResponse")]
        Common.Course GetCourse(int courseId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServerService/GetCourse", ReplyAction="http://tempuri.org/IServerService/GetCourseResponse")]
        System.Threading.Tasks.Task<Common.Course> GetCourseAsync(int courseId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServerService/AddUserTask", ReplyAction="http://tempuri.org/IServerService/AddUserTaskResponse")]
        bool AddUserTask(int sessionId, int userId, int taskId, int grade);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServerService/AddUserTask", ReplyAction="http://tempuri.org/IServerService/AddUserTaskResponse")]
        System.Threading.Tasks.Task<bool> AddUserTaskAsync(int sessionId, int userId, int taskId, int grade);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServerService/GetCourseGrade", ReplyAction="http://tempuri.org/IServerService/GetCourseGradeResponse")]
        Common.GradedCourse GetCourseGrade(int userId, int courseId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServerService/GetCourseGrade", ReplyAction="http://tempuri.org/IServerService/GetCourseGradeResponse")]
        System.Threading.Tasks.Task<Common.GradedCourse> GetCourseGradeAsync(int userId, int courseId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServerService/GetTaskGroups", ReplyAction="http://tempuri.org/IServerService/GetTaskGroupsResponse")]
        Common.TaskGroup[] GetTaskGroups(int courseId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServerService/GetTaskGroups", ReplyAction="http://tempuri.org/IServerService/GetTaskGroupsResponse")]
        System.Threading.Tasks.Task<Common.TaskGroup[]> GetTaskGroupsAsync(int courseId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServerService/GetTasks", ReplyAction="http://tempuri.org/IServerService/GetTasksResponse")]
        Common.Task[] GetTasks(int taskGroupId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServerService/GetTasks", ReplyAction="http://tempuri.org/IServerService/GetTasksResponse")]
        System.Threading.Tasks.Task<Common.Task[]> GetTasksAsync(int taskGroupId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServerService/GetUsersForCourse", ReplyAction="http://tempuri.org/IServerService/GetUsersForCourseResponse")]
        Common.User[] GetUsersForCourse(int courseId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServerService/GetUsersForCourse", ReplyAction="http://tempuri.org/IServerService/GetUsersForCourseResponse")]
        System.Threading.Tasks.Task<Common.User[]> GetUsersForCourseAsync(int courseId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServerService/GetUserFromId", ReplyAction="http://tempuri.org/IServerService/GetUserFromIdResponse")]
        Common.User GetUserFromId(int userId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServerService/GetUserFromId", ReplyAction="http://tempuri.org/IServerService/GetUserFromIdResponse")]
        System.Threading.Tasks.Task<Common.User> GetUserFromIdAsync(int userId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServerService/GetCourseAttentandsForCourse", ReplyAction="http://tempuri.org/IServerService/GetCourseAttentandsForCourseResponse")]
        Common.CourseAttentant[] GetCourseAttentandsForCourse(int courseId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServerService/GetCourseAttentandsForCourse", ReplyAction="http://tempuri.org/IServerService/GetCourseAttentandsForCourseResponse")]
        System.Threading.Tasks.Task<Common.CourseAttentant[]> GetCourseAttentandsForCourseAsync(int courseId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServerService/AddCourseAttendant", ReplyAction="http://tempuri.org/IServerService/AddCourseAttendantResponse")]
        int AddCourseAttendant(int sessionId, int userId, int courseId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServerService/AddCourseAttendant", ReplyAction="http://tempuri.org/IServerService/AddCourseAttendantResponse")]
        System.Threading.Tasks.Task<int> AddCourseAttendantAsync(int sessionId, int userId, int courseId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServerService/getCourses", ReplyAction="http://tempuri.org/IServerService/getCoursesResponse")]
        Common.Course[] getCourses(int sessionId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServerService/getCourses", ReplyAction="http://tempuri.org/IServerService/getCoursesResponse")]
        System.Threading.Tasks.Task<Common.Course[]> getCoursesAsync(int sessionId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServerService/AddTask", ReplyAction="http://tempuri.org/IServerService/AddTaskResponse")]
        int AddTask(int sessionId, int taskGroupId, string name, int gradeType, int weight);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServerService/AddTask", ReplyAction="http://tempuri.org/IServerService/AddTaskResponse")]
        System.Threading.Tasks.Task<int> AddTaskAsync(int sessionId, int taskGroupId, string name, int gradeType, int weight);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServerService/AddTaskGroupGrade", ReplyAction="http://tempuri.org/IServerService/AddTaskGroupGradeResponse")]
        int AddTaskGroupGrade(int sessionId, int taskgroupId, int limit, int value, string gradeName, string expression);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServerService/AddTaskGroupGrade", ReplyAction="http://tempuri.org/IServerService/AddTaskGroupGradeResponse")]
        System.Threading.Tasks.Task<int> AddTaskGroupGradeAsync(int sessionId, int taskgroupId, int limit, int value, string gradeName, string expression);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServerService/AddTaskGroup", ReplyAction="http://tempuri.org/IServerService/AddTaskGroupResponse")]
        int AddTaskGroup(int sessionId, int courseId, string taskGroupName);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServerService/AddTaskGroup", ReplyAction="http://tempuri.org/IServerService/AddTaskGroupResponse")]
        System.Threading.Tasks.Task<int> AddTaskGroupAsync(int sessionId, int courseId, string taskGroupName);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServerService/AddCourseGrade", ReplyAction="http://tempuri.org/IServerService/AddCourseGradeResponse")]
        int AddCourseGrade(int sessionId, int courseId, string gradeName, int limit, string expression);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServerService/AddCourseGrade", ReplyAction="http://tempuri.org/IServerService/AddCourseGradeResponse")]
        System.Threading.Tasks.Task<int> AddCourseGradeAsync(int sessionId, int courseId, string gradeName, int limit, string expression);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServerService/AddCourse", ReplyAction="http://tempuri.org/IServerService/AddCourseResponse")]
        int AddCourse(int sessionId, string code, string courseName, int period);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServerService/AddCourse", ReplyAction="http://tempuri.org/IServerService/AddCourseResponse")]
        System.Threading.Tasks.Task<int> AddCourseAsync(int sessionId, string code, string courseName, int period);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServerService/GetAccessLevel", ReplyAction="http://tempuri.org/IServerService/GetAccessLevelResponse")]
        int GetAccessLevel(int sessionId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServerService/GetAccessLevel", ReplyAction="http://tempuri.org/IServerService/GetAccessLevelResponse")]
        System.Threading.Tasks.Task<int> GetAccessLevelAsync(int sessionId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServerService/GetTaskReported", ReplyAction="http://tempuri.org/IServerService/GetTaskReportedResponse")]
        int GetTaskReported(int taskId, int userId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServerService/GetTaskReported", ReplyAction="http://tempuri.org/IServerService/GetTaskReportedResponse")]
        System.Threading.Tasks.Task<int> GetTaskReportedAsync(int taskId, int userId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServerService/Logout", ReplyAction="http://tempuri.org/IServerService/LogoutResponse")]
        void Logout(int sessionId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServerService/Logout", ReplyAction="http://tempuri.org/IServerService/LogoutResponse")]
        System.Threading.Tasks.Task LogoutAsync(int sessionId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServerService/UpdateReported", ReplyAction="http://tempuri.org/IServerService/UpdateReportedResponse")]
        void UpdateReported(int taskId, int userId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServerService/UpdateReported", ReplyAction="http://tempuri.org/IServerService/UpdateReportedResponse")]
        System.Threading.Tasks.Task UpdateReportedAsync(int taskId, int userId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServerService/Login", ReplyAction="http://tempuri.org/IServerService/LoginResponse")]
        int Login(string username, string password);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServerService/Login", ReplyAction="http://tempuri.org/IServerService/LoginResponse")]
        System.Threading.Tasks.Task<int> LoginAsync(string username, string password);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServerService/GetUser", ReplyAction="http://tempuri.org/IServerService/GetUserResponse")]
        Common.User GetUser(int sessionId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServerService/GetUser", ReplyAction="http://tempuri.org/IServerService/GetUserResponse")]
        System.Threading.Tasks.Task<Common.User> GetUserAsync(int sessionId);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServerService/AddUser", ReplyAction="http://tempuri.org/IServerService/AddUserResponse")]
        int AddUser(int sessionId, Common.User user);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IServerService/AddUser", ReplyAction="http://tempuri.org/IServerService/AddUserResponse")]
        System.Threading.Tasks.Task<int> AddUserAsync(int sessionId, Common.User user);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IServerServiceChannel : Client.ServiceReference1.IServerService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ServerServiceClient : System.ServiceModel.ClientBase<Client.ServiceReference1.IServerService>, Client.ServiceReference1.IServerService {
        
        public ServerServiceClient() {
        }
        
        public ServerServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public ServerServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ServerServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ServerServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public Common.User[] GetUsers(int sessionId) {
            return base.Channel.GetUsers(sessionId);
        }
        
        public System.Threading.Tasks.Task<Common.User[]> GetUsersAsync(int sessionId) {
            return base.Channel.GetUsersAsync(sessionId);
        }
        
        public bool AddCoursesFromXml(int sessionId, string xmlString) {
            return base.Channel.AddCoursesFromXml(sessionId, xmlString);
        }
        
        public System.Threading.Tasks.Task<bool> AddCoursesFromXmlAsync(int sessionId, string xmlString) {
            return base.Channel.AddCoursesFromXmlAsync(sessionId, xmlString);
        }
        
        public Common.TaskGroupGrade[] GetTaskGroupGrades(int taskGroupId) {
            return base.Channel.GetTaskGroupGrades(taskGroupId);
        }
        
        public System.Threading.Tasks.Task<Common.TaskGroupGrade[]> GetTaskGroupGradesAsync(int taskGroupId) {
            return base.Channel.GetTaskGroupGradesAsync(taskGroupId);
        }
        
        public int CountReportedTasksForUserInCourse(int userId, int courseId) {
            return base.Channel.CountReportedTasksForUserInCourse(userId, courseId);
        }
        
        public System.Threading.Tasks.Task<int> CountReportedTasksForUserInCourseAsync(int userId, int courseId) {
            return base.Channel.CountReportedTasksForUserInCourseAsync(userId, courseId);
        }
        
        public Common.CourseGrade[] GetCourseGrades(int courseId) {
            return base.Channel.GetCourseGrades(courseId);
        }
        
        public System.Threading.Tasks.Task<Common.CourseGrade[]> GetCourseGradesAsync(int courseId) {
            return base.Channel.GetCourseGradesAsync(courseId);
        }
        
        public Common.UserTask GetUserTask(int userId, int taskId) {
            return base.Channel.GetUserTask(userId, taskId);
        }
        
        public System.Threading.Tasks.Task<Common.UserTask> GetUserTaskAsync(int userId, int taskId) {
            return base.Channel.GetUserTaskAsync(userId, taskId);
        }
        
        public Common.Course GetCourse(int courseId) {
            return base.Channel.GetCourse(courseId);
        }
        
        public System.Threading.Tasks.Task<Common.Course> GetCourseAsync(int courseId) {
            return base.Channel.GetCourseAsync(courseId);
        }
        
        public bool AddUserTask(int sessionId, int userId, int taskId, int grade) {
            return base.Channel.AddUserTask(sessionId, userId, taskId, grade);
        }
        
        public System.Threading.Tasks.Task<bool> AddUserTaskAsync(int sessionId, int userId, int taskId, int grade) {
            return base.Channel.AddUserTaskAsync(sessionId, userId, taskId, grade);
        }
        
        public Common.GradedCourse GetCourseGrade(int userId, int courseId) {
            return base.Channel.GetCourseGrade(userId, courseId);
        }
        
        public System.Threading.Tasks.Task<Common.GradedCourse> GetCourseGradeAsync(int userId, int courseId) {
            return base.Channel.GetCourseGradeAsync(userId, courseId);
        }
        
        public Common.TaskGroup[] GetTaskGroups(int courseId) {
            return base.Channel.GetTaskGroups(courseId);
        }
        
        public System.Threading.Tasks.Task<Common.TaskGroup[]> GetTaskGroupsAsync(int courseId) {
            return base.Channel.GetTaskGroupsAsync(courseId);
        }
        
        public Common.Task[] GetTasks(int taskGroupId) {
            return base.Channel.GetTasks(taskGroupId);
        }
        
        public System.Threading.Tasks.Task<Common.Task[]> GetTasksAsync(int taskGroupId) {
            return base.Channel.GetTasksAsync(taskGroupId);
        }
        
        public Common.User[] GetUsersForCourse(int courseId) {
            return base.Channel.GetUsersForCourse(courseId);
        }
        
        public System.Threading.Tasks.Task<Common.User[]> GetUsersForCourseAsync(int courseId) {
            return base.Channel.GetUsersForCourseAsync(courseId);
        }
        
        public Common.User GetUserFromId(int userId) {
            return base.Channel.GetUserFromId(userId);
        }
        
        public System.Threading.Tasks.Task<Common.User> GetUserFromIdAsync(int userId) {
            return base.Channel.GetUserFromIdAsync(userId);
        }
        
        public Common.CourseAttentant[] GetCourseAttentandsForCourse(int courseId) {
            return base.Channel.GetCourseAttentandsForCourse(courseId);
        }
        
        public System.Threading.Tasks.Task<Common.CourseAttentant[]> GetCourseAttentandsForCourseAsync(int courseId) {
            return base.Channel.GetCourseAttentandsForCourseAsync(courseId);
        }
        
        public int AddCourseAttendant(int sessionId, int userId, int courseId) {
            return base.Channel.AddCourseAttendant(sessionId, userId, courseId);
        }
        
        public System.Threading.Tasks.Task<int> AddCourseAttendantAsync(int sessionId, int userId, int courseId) {
            return base.Channel.AddCourseAttendantAsync(sessionId, userId, courseId);
        }
        
        public Common.Course[] getCourses(int sessionId) {
            return base.Channel.getCourses(sessionId);
        }
        
        public System.Threading.Tasks.Task<Common.Course[]> getCoursesAsync(int sessionId) {
            return base.Channel.getCoursesAsync(sessionId);
        }
        
        public int AddTask(int sessionId, int taskGroupId, string name, int gradeType, int weight) {
            return base.Channel.AddTask(sessionId, taskGroupId, name, gradeType, weight);
        }
        
        public System.Threading.Tasks.Task<int> AddTaskAsync(int sessionId, int taskGroupId, string name, int gradeType, int weight) {
            return base.Channel.AddTaskAsync(sessionId, taskGroupId, name, gradeType, weight);
        }
        
        public int AddTaskGroupGrade(int sessionId, int taskgroupId, int limit, int value, string gradeName, string expression) {
            return base.Channel.AddTaskGroupGrade(sessionId, taskgroupId, limit, value, gradeName, expression);
        }
        
        public System.Threading.Tasks.Task<int> AddTaskGroupGradeAsync(int sessionId, int taskgroupId, int limit, int value, string gradeName, string expression) {
            return base.Channel.AddTaskGroupGradeAsync(sessionId, taskgroupId, limit, value, gradeName, expression);
        }
        
        public int AddTaskGroup(int sessionId, int courseId, string taskGroupName) {
            return base.Channel.AddTaskGroup(sessionId, courseId, taskGroupName);
        }
        
        public System.Threading.Tasks.Task<int> AddTaskGroupAsync(int sessionId, int courseId, string taskGroupName) {
            return base.Channel.AddTaskGroupAsync(sessionId, courseId, taskGroupName);
        }
        
        public int AddCourseGrade(int sessionId, int courseId, string gradeName, int limit, string expression) {
            return base.Channel.AddCourseGrade(sessionId, courseId, gradeName, limit, expression);
        }
        
        public System.Threading.Tasks.Task<int> AddCourseGradeAsync(int sessionId, int courseId, string gradeName, int limit, string expression) {
            return base.Channel.AddCourseGradeAsync(sessionId, courseId, gradeName, limit, expression);
        }
        
        public int AddCourse(int sessionId, string code, string courseName, int period) {
            return base.Channel.AddCourse(sessionId, code, courseName, period);
        }
        
        public System.Threading.Tasks.Task<int> AddCourseAsync(int sessionId, string code, string courseName, int period) {
            return base.Channel.AddCourseAsync(sessionId, code, courseName, period);
        }
        
        public int GetAccessLevel(int sessionId) {
            return base.Channel.GetAccessLevel(sessionId);
        }
        
        public System.Threading.Tasks.Task<int> GetAccessLevelAsync(int sessionId) {
            return base.Channel.GetAccessLevelAsync(sessionId);
        }
        
        public int GetTaskReported(int taskId, int userId) {
            return base.Channel.GetTaskReported(taskId, userId);
        }
        
        public System.Threading.Tasks.Task<int> GetTaskReportedAsync(int taskId, int userId) {
            return base.Channel.GetTaskReportedAsync(taskId, userId);
        }
        
        public void Logout(int sessionId) {
            base.Channel.Logout(sessionId);
        }
        
        public System.Threading.Tasks.Task LogoutAsync(int sessionId) {
            return base.Channel.LogoutAsync(sessionId);
        }
        
        public void UpdateReported(int taskId, int userId) {
            base.Channel.UpdateReported(taskId, userId);
        }
        
        public System.Threading.Tasks.Task UpdateReportedAsync(int taskId, int userId) {
            return base.Channel.UpdateReportedAsync(taskId, userId);
        }
        
        public int Login(string username, string password) {
            return base.Channel.Login(username, password);
        }
        
        public System.Threading.Tasks.Task<int> LoginAsync(string username, string password) {
            return base.Channel.LoginAsync(username, password);
        }
        
        public Common.User GetUser(int sessionId) {
            return base.Channel.GetUser(sessionId);
        }
        
        public System.Threading.Tasks.Task<Common.User> GetUserAsync(int sessionId) {
            return base.Channel.GetUserAsync(sessionId);
        }
        
        public int AddUser(int sessionId, Common.User user) {
            return base.Channel.AddUser(sessionId, user);
        }
        
        public System.Threading.Tasks.Task<int> AddUserAsync(int sessionId, Common.User user) {
            return base.Channel.AddUserAsync(sessionId, user);
        }
    }
}
