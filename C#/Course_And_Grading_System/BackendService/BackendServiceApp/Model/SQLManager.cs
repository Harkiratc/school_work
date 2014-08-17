using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Collections;
using Common;

namespace BackendService.Model
{

    public class SQLManager
    {
        private SqlConnection connection;
        private const String USERNAME_AND_PASSWORD_VALID = "SELECT COUNT(*) FROM databas.dbo.Users WHERE name = @name AND passwd = @password";
        private const String GET_USER_INFO      = "SELECT * FROM databas.dbo.Users WHERE sessionId = @sessionid";
        private const String SET_SESSION_ID     = "UPDATE databas.dbo.Users SET sessionid = @sessionid WHERE name = @name";
        private const String COUNT_SESSIONID    = "SELECT COUNT(*) FROM databas.dbo.Users WHERE sessionid = @sessionid";
        private const String DISCONNECT_USER    = "UPDATE databas.dbo.Users SET sessionid = NULL WHERE sessionid = @sessionid";
        private const String ADD_COURSE         = "INSERT INTO databas.dbo.Courses (code,name,period) VALUES (@code,@name,@period);\nSELECT id AS LastId FROM databas.dbo.Courses WHERE id = @@Identity;";
        private const String ADD_COURSE_ATTENDANT = "INSERT INTO databas.dbo.CourseAttendants (user_id, course_id) VALUES (@user, @course);\nSELECT id AS LastId FROM databas.dbo.CourseAttendants WHERE id = @@Identity;";
        private const String ADD_TASK_GROUP     = "INSERT INTO databas.dbo.TaskGroups (course_id, name) VALUES (@course, @name);\nSELECT id AS LastId FROM databas.dbo.TaskGroups WHERE id = @@Identity;";
        private const String ADD_TASK           = "INSERT INTO databas.dbo.Tasks (taskgroup_id, name, gradetype, weight) VALUES (@taskgroup, @name, @gradetype, @weight);\nSELECT id AS LastId FROM databas.dbo.Tasks WHERE id = @@Identity;";
        private const String ADD_USER_TASK      = "INSERT INTO databas.dbo.UserTasks (user_id, task_id, grade, datetime, reported_by) VALUES (@user, @task, @grade, @datetime, @reported_by);\nSELECT id AS LastId FROM databas.dbo.UserTasks WHERE id = @@Identity;";
        private const String ADD_TASK_GROUP_GRADE = "INSERT INTO databas.dbo.TaskGroupGrades (taskgroup_id, limit, value, grade_name, expression) VALUES (@task, @limit, @value, @gradeName, @expression);\nSELECT id AS LastId FROM databas.dbo.TaskGroupGrades WHERE id = @@Identity;";
        private const String ADD_COURSE_GRADE   = "INSERT INTO databas.dbo.CourseGrades (course_id, grade_name, limit, expression) VALUES (@course, @grade, @limit, @expression);\nSELECT id AS LastId FROM databas.dbo.CourseGrades WHERE id = @@Identity;";
        private const String ADD_USER           = "INSERT INTO databas.dbo.Users (name, passwd, ssn, lastname, firstname, email, accesslevel, state_id, state_course) VALUES (@name, @password, @ssn, @lastname, @firstname, @email, @access, 0, 0);\nSELECT id AS LastId FROM databas.dbo.Users WHERE id = @@Identity;";
        private const String SELECT_COURSES     = "SELECT * FROM databas.dbo.Courses";
        private const String SELECT_STUDENT_COURSES = "SELECT * FROM databas.dbo.Courses WHERE id IN (SELECT course_id FROM databas.dbo.CourseAttendants WHERE user_id = @user)";
        private const String SELECT_TASK_GROUPS = "SELECT * FROM databas.dbo.TaskGroups WHERE course_id = @course";
        private const String SELECT_TASKS = "SELECT * FROM databas.dbo.Tasks WHERE taskgroup_id = @task";
        private const String SELECT_USER_TASKS_FOR_TASK = "SELECT * FROM databas.dbo.UserTasks WHERE user_id = @user AND task_id = @task";
        private const String SELECT_COURSE_GRADES_FOR_COURSE = "SELECT * FROM databas.dbo.CourseGrades where course_id = @course ORDER BY limit DESC";
        private const String SELECT_COURSE_WITH_ID = "SELECT * FROM databas.dbo.Courses WHERE id = @course";
        private const String SELECT_COURSE_ATTENDANTS_FOR_COURSE = "SELECT * FROM databas.dbo.CourseAttendants WHERE course_id = @course";
        private const String SELECT_USER_WITH_ID = "SELECT * FROM databas.dbo.Users WHERE id = @user";
        private const String SELECT_TASK_GROUP_GRADES_FOR_TASKGROUP = "SELECT * FROM databas.dbo.TaskGroupGrades WHERE taskgroup_id = @taskgroup ORDER BY limit DESC";
        private const String SELECT_TASK_REPORTED_STATUS = "SELECT reported FROM databas.dbo.Reported WHERE task_id = @task AND user_id = @user";
        private const String UPDATE_TASK_REPORTED_STATUS = "UPDATE databas.dbo.Reported SET reported = 1 WHERE task_id = @task AND user_id = @user ";
        private const String COUNT_REPORTED_TASKS_FOR_USER_IN_COURSE = "SELECT COUNT(*) FROM databas.dbo.Reported WHERE reported = 1 AND user_id = @user AND task_id IN (SELECT task_id FROM databas.dbo.Tasks WHERE taskgroup_id IN (SELECT id from databas.dbo.TaskGroups WHERE course_id = @course))";
        private const String COUNT_USER_TASKS_FOR_USER = "SELECT COUNT(*) FROM databas.dbo.UserTasks WHERE user_id = @user AND task_id = @task";
        private const String UPDATE_USER_TASK = "UPDATE databas.dbo.UserTasks SET grade = @grade, datetime = @datetime, reported_by = @reported_by WHERE user_id = @user AND task_id = @task";
        private const String ADD_REPORTED_STATUS = "INSERT INTO databas.dbo.Reported (task_id, user_id, reported) VALUES (@task, @user, @reported)";
        private const String SELECT_ALL_USERS = "SELECT * FROM databas.dbo.Users";
        private const String SELECT_TASK_BY_ID = "SELECT * FROM databas.dbo.Tasks WHERE id = @task";
        private const String SET_USER_STATE = "UPDATE databas.dbo.Users SET state_id = @state, state_course = @course WHERE sessionid = @sessionId";

        public SQLManager()
        {
            connection = new SqlConnection("user id=hej;" +
                                       "password=uk20fv$b;server=130.237.226.228;" +
                                       "database=databas; " +
                                       "connection timeout=30");
            connection.Open();
        }

        public void SetUserState(int sessionId, int stateId, int stateCourse)
        {
            using (SqlCommand c = new SqlCommand(SET_USER_STATE, connection))
            {
                c.Parameters.AddWithValue("state", stateId);
                c.Parameters.AddWithValue("course", stateCourse);
                c.Parameters.AddWithValue("sessionId", sessionId);
                c.ExecuteNonQuery();
            }
        }

        public Common.Task getTask(int taskId)
        {
            SqlDataReader reader = null;
            Common.Task task = null;
            using (SqlCommand c = new SqlCommand(SELECT_TASK_BY_ID, connection))
            {
                c.Parameters.AddWithValue("task", taskId);
                reader = c.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        int id = reader.GetInt32(0);
                        int taskGroupId = reader.GetInt32(1);
                        String name = reader.GetString(2);
                        int gradeType = reader.GetInt32(3);
                        int weight = reader.GetInt32(4);
                        task = new Common.Task(id, taskGroupId, name, gradeType, weight);
                    }
                }
            }
            reader.Close();
            return task;
        }

        public List<User> GetUsers()
        {
            SqlDataReader reader = null;
            List<User> users = new List<User>();
            using (SqlCommand c = new SqlCommand(SELECT_ALL_USERS, connection))
            {
                reader = c.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        int id = reader.GetInt32(0);
                        string username = reader.GetString(1);
                        string ssn = reader.GetString(3);
                        string lastname = reader.GetString(4);
                        string firstname = reader.GetString(5);
                        string email = reader.GetString(6);
                        int accesslevel = reader.GetInt32(7);
                        users.Add(new User(id, username, ssn, lastname, firstname, email, accesslevel));
                    }
                }
            }
            reader.Close();
            return users;
        }

        public void AddReported(int taskId, int userId, int reported)
        {
            using (SqlCommand c = new SqlCommand(ADD_REPORTED_STATUS, connection))
            {
                c.Parameters.AddWithValue("task",taskId);
                c.Parameters.AddWithValue("user", userId);
                c.Parameters.AddWithValue("reported",reported);
                c.ExecuteNonQuery();
            }
        }

        public void UpdateUserTask(int userId, int taskId, int grade, DateTime dateTime, int reportedBy)
        {

            using (SqlCommand c = new SqlCommand(UPDATE_USER_TASK, connection))
            {
                c.Parameters.AddWithValue("user", userId);
                c.Parameters.AddWithValue("task", taskId);
                c.Parameters.AddWithValue("grade", grade);
                c.Parameters.AddWithValue("datetime", dateTime);
                c.Parameters.AddWithValue("reported_by", reportedBy);
                c.ExecuteNonQuery();
            }
            
        }

        public bool IsUsertaskSet(int userId, int taskId)
        {
            SqlDataReader reader = null;
            bool isSet = false;
            using (SqlCommand c = new SqlCommand(COUNT_USER_TASKS_FOR_USER, connection))
            {
                
                c.Parameters.AddWithValue("user", userId);
                c.Parameters.AddWithValue("task", taskId);
                reader = c.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                    if (reader.GetInt32(0) == 1)
                        isSet = true;
                }
            }
            reader.Close();
            return isSet;
        }

        public int CountReportedTasksForUserInCourse(int userId, int courseId)
        {
            int count = 0;
            SqlDataReader reader = null;
            using (SqlCommand c = new SqlCommand(COUNT_REPORTED_TASKS_FOR_USER_IN_COURSE, connection))
            {
                c.Parameters.AddWithValue("user",userId);
                c.Parameters.AddWithValue("course",courseId);
                reader = c.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                    count = reader.GetInt32(0);
                 
                }
            }
            reader.Close();
            return count;
        }

        public List<TaskGroupGrade> GetTaskGroupGrades(int taskGroupId)
        {
            SqlDataReader reader = null;
            List<TaskGroupGrade> taskGroupGrades = new List<TaskGroupGrade>();
            using (SqlCommand c = new SqlCommand(SELECT_TASK_GROUP_GRADES_FOR_TASKGROUP, connection))
            {
                c.Parameters.AddWithValue("taskgroup", taskGroupId);
                reader = c.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        taskGroupGrades.Add(new TaskGroupGrade(reader.GetInt32(0), reader.GetInt32(1), reader.GetInt32(2), reader.GetInt32(3), reader.GetString(4), reader.GetString(5)));
                    }
                }
            }
            reader.Close();
            return taskGroupGrades;
        }

        public User GetUserFromId(int userId)
        {
            SqlDataReader reader = null;
            User user = null;
            using (SqlCommand c = new SqlCommand(SELECT_USER_WITH_ID, connection))
            {
                c.Parameters.AddWithValue("user", userId);
                reader = c.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                        
                        int id = reader.GetInt32(0);
                        string username = reader.GetString(1);
                        string ssn = reader.GetString(3);
                        string lastname = reader.GetString(4);
                        string firstname = reader.GetString(5);
                        string email = reader.GetString(6);
                        int accesslevel = reader.GetInt32(7);
                        user = new User(id, username, ssn, lastname, firstname, email, accesslevel);

                }
            }
            reader.Close();
            return user;
        }


        public List<CourseAttentant> GetCourseAttendantsForCourse(int courseId)
        {
            SqlDataReader reader = null;
            List<CourseAttentant> userRoles = new List<CourseAttentant>();
            using (SqlCommand c = new SqlCommand(SELECT_COURSE_ATTENDANTS_FOR_COURSE, connection))
            {
                c.Parameters.AddWithValue("course", courseId);
                reader = c.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        userRoles.Add(new CourseAttentant(reader.GetInt32(0), reader.GetInt32(1), reader.GetInt32(2)));
                    }
                }
            }
            reader.Close();
            return userRoles;
        }

        public Course getCourse(int courseId)
        {
            SqlDataReader reader = null;
            Course course = null;
            using (SqlCommand c = new SqlCommand(SELECT_COURSE_WITH_ID, connection))
            {
                c.Parameters.AddWithValue("course", courseId);
                reader = c.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                    course = new Course(reader.GetInt32(0), reader.GetString(1), reader.GetString(2),reader.GetInt32(3));
                    
                }
            }
            reader.Close();
            return course;
        }

        public List<CourseGrade> GetCourseGrades(int courseId)
        {
            SqlDataReader reader = null;
            List<CourseGrade> courseGrades = new List<CourseGrade>();
            using (SqlCommand c = new SqlCommand(SELECT_COURSE_GRADES_FOR_COURSE, connection))
            {
                c.Parameters.AddWithValue("course", courseId);
                reader = c.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        courseGrades.Add(new CourseGrade(reader.GetInt32(0), reader.GetInt32(1), reader.GetString(2), reader.GetInt32(3), reader.GetString(4)));
                    }
                }
            }
            reader.Close();
            return courseGrades;
        }

        // CAN 1 TASK ONLY HAVE 1 UserTask???????????
        public UserTask GetUserTask(int userId, int taskId)
        {
            SqlDataReader reader = null;
            UserTask ret = null;
            using (SqlCommand c = new SqlCommand(SELECT_USER_TASKS_FOR_TASK, connection))
            {
                c.Parameters.AddWithValue("user",userId);
                c.Parameters.AddWithValue("task", taskId);
                reader = c.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                    ret = new UserTask(reader.GetInt32(0), reader.GetInt32(1), reader.GetInt32(2), reader.GetInt32(3), reader.GetDateTime(4), reader.GetInt32(5));
                    
                }
            }
            reader.Close();
            return ret;
        }

        public List<Common.Task> GetTasks(int taskGroupId)
        {
            List<Common.Task> tasks = new List<Common.Task>();
            SqlDataReader reader = null;
            using (SqlCommand c = new SqlCommand(SELECT_TASKS, connection))
            {
                c.Parameters.AddWithValue("task", taskGroupId);
                reader = c.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        tasks.Add(new Common.Task(reader.GetInt32(0), reader.GetInt32(1), reader.GetString(2), reader.GetInt32(3), reader.GetInt32(4)));
                    }
                }
            }
            reader.Close();
            return tasks;
        }

        public List<TaskGroup> GetTaskGroups(int courseId)
        {
            List<TaskGroup> taskGroups = new List<TaskGroup>();
            SqlDataReader reader = null;
            using (SqlCommand c = new SqlCommand(SELECT_TASK_GROUPS, connection))
            {
                c.Parameters.AddWithValue("course", courseId);
                reader = c.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        taskGroups.Add(new TaskGroup(reader.GetInt32(0), reader.GetInt32(1), reader.GetString(2)));
                    }
                }
            }
            reader.Close();
            return taskGroups;
        }

        public List<Course> GetCourses(int userId)
        {
            List<Course> courses = new List<Course>();
            SqlDataReader reader = null;
            using (SqlCommand c = new SqlCommand(SELECT_STUDENT_COURSES, connection))
            {
                c.Parameters.AddWithValue("user", userId);
                reader = c.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        courses.Add(new Course(reader.GetInt32(0), reader.GetString(1), reader.GetString(2),reader.GetInt32(3)));
                    }
                }
            }
            reader.Close();
            return courses;

        }

        public List<Course> GetCourses()
        {
            List<Course> courses = new List<Course>();
            SqlDataReader reader = null;
            using (SqlCommand c = new SqlCommand(SELECT_COURSES, connection))
            {
                reader = c.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        courses.Add(new Course(reader.GetInt32(0), reader.GetString(1), reader.GetString(2),reader.GetInt32(3)));
                    }
                }
            }
            reader.Close();
            return courses;
 
        }

        public int AddUser(User user)
        {
            SqlDataReader reader = null;
            int userId;
            using (SqlCommand c = new SqlCommand(ADD_USER, connection))
            {
                c.Parameters.AddWithValue("name", user.Username);
                c.Parameters.AddWithValue("password", user.Password);
                c.Parameters.AddWithValue("ssn", user.Ssn);
                c.Parameters.AddWithValue("lastname", user.Lastname);
                c.Parameters.AddWithValue("firstname", user.Firstname);
                c.Parameters.AddWithValue("email", user.Email);
                c.Parameters.AddWithValue("access", user.Accesslevel);
                reader = c.ExecuteReader();
                reader.Read();
                userId = reader.GetInt32(0);
            }
            reader.Close();
            return userId;
        }

        public void UpdateReported(int taskId,int userId)
        {
            using (SqlCommand c = new SqlCommand(UPDATE_TASK_REPORTED_STATUS, connection))
            {
                c.Parameters.AddWithValue("task", taskId);
                c.Parameters.AddWithValue("user", userId);
                c.ExecuteNonQuery();
            }
        }

        public int AddCourseGrade(int courseId,String gradeName, int limit, String expression)
        {
            SqlDataReader reader = null;
            int courseGradeId;
            using (SqlCommand c = new SqlCommand(ADD_COURSE_GRADE, connection))
            {
                c.Parameters.AddWithValue("course", courseId);
                c.Parameters.AddWithValue("grade", gradeName);
                c.Parameters.AddWithValue("limit", limit);
                c.Parameters.AddWithValue("expression", expression);
                reader = c.ExecuteReader();
                reader.Read();
                courseGradeId = reader.GetInt32(0);
            }
            reader.Close();
            return courseGradeId;
        }

        public int AddTaskGroupGrade(int taskgroupId, int limit, int value, String gradeName, String expression)
        {
            SqlDataReader reader = null;
            int taskGroupGradeId;
            using (SqlCommand c = new SqlCommand(ADD_TASK_GROUP_GRADE, connection))
            {
                c.Parameters.AddWithValue("task", taskgroupId);
                c.Parameters.AddWithValue("limit", limit);
                c.Parameters.AddWithValue("value", value);
                c.Parameters.AddWithValue("gradeName", gradeName);
                c.Parameters.AddWithValue("expression", expression);
                reader = c.ExecuteReader();
                reader.Read();
                taskGroupGradeId = reader.GetInt32(0);
            }
            reader.Close();
            return taskGroupGradeId;
        }

        public int AddUserTask(int userId, int taskId, int grade, DateTime dateTime, int reportedBy)
        {
            SqlDataReader reader = null;
            int userTaskId;
            using (SqlCommand c = new SqlCommand(ADD_USER_TASK, connection))
            {
                c.Parameters.AddWithValue("user", userId);
                c.Parameters.AddWithValue("task", taskId);
                c.Parameters.AddWithValue("grade", grade);
                c.Parameters.AddWithValue("datetime", dateTime);
                c.Parameters.AddWithValue("reported_by", reportedBy);
                reader = c.ExecuteReader();
                reader.Read();
                userTaskId = reader.GetInt32(0);
            }
            reader.Close();
            return userTaskId;
        }

        public int AddTask(int taskGroupId, String name, int gradeType, int weight)
        {
            SqlDataReader reader = null;
            int taskId;
            using (SqlCommand c = new SqlCommand(ADD_TASK, connection))
            {
                c.Parameters.AddWithValue("taskgroup", taskGroupId);
                c.Parameters.AddWithValue("name", name);
                c.Parameters.AddWithValue("gradetype", gradeType);
                c.Parameters.AddWithValue("weight", weight);
                reader = c.ExecuteReader();
                reader.Read();
                taskId = reader.GetInt32(0);
            }
            reader.Close();
            return taskId;
        }

        public int GetTaskReported(int taskId, int userId)
        {
            SqlDataReader reader = null;
            int reported = 0;
            using (SqlCommand c = new SqlCommand(SELECT_TASK_REPORTED_STATUS, connection))
            {
                c.Parameters.AddWithValue("task", taskId);
                c.Parameters.AddWithValue("user", userId);
                reader = c.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                    reported = reader.GetInt32(0);
                } 
            }
            reader.Close();
            return reported;
        }

        public int AddTaskGroup(int course, String name)
        {
            SqlDataReader reader = null;
            int taskGroupId;
            using (SqlCommand c = new SqlCommand(ADD_TASK_GROUP, connection))
            {
                c.Parameters.AddWithValue("course", course);
                c.Parameters.AddWithValue("name", name);
                reader = c.ExecuteReader();
                reader.Read();
                taskGroupId = reader.GetInt32(0);
            }
            reader.Close();
            return taskGroupId;
        }


        public int AddCourse(String code, String name, int period)
        {
            SqlDataReader reader = null;
            int courseId;
            using (SqlCommand c = new SqlCommand(ADD_COURSE, connection))
            {
                c.Parameters.AddWithValue("code", code);
                c.Parameters.AddWithValue("name", name);
                c.Parameters.AddWithValue("period", period);
                reader = c.ExecuteReader();
                reader.Read();
                courseId = reader.GetInt32(0);
            }
            reader.Close();
            return courseId;
        }

        public int AddCourseAttendant(int userId, int courseId)
        {
            SqlDataReader reader = null;
            int attendantId;
            using (SqlCommand c = new SqlCommand(ADD_COURSE_ATTENDANT, connection))
            {
                c.Parameters.AddWithValue("user", userId);
                c.Parameters.AddWithValue("course", courseId);
                reader = c.ExecuteReader();
                reader.Read();
                attendantId = reader.GetInt32(0);
            }
            reader.Close();
            return attendantId;
        }

        public void Disconnect(int sessionId)
        {
            using (SqlCommand c = new SqlCommand(DISCONNECT_USER, connection))
            {
                c.Parameters.AddWithValue("sessionid", sessionId);
                c.ExecuteNonQuery();
            }
            connection.Close();
        }


        public User GetUser(int sessionId)
        {
            SqlDataReader reader = null;
            using (SqlCommand c = new SqlCommand(GET_USER_INFO, connection))
            {
                c.Parameters.AddWithValue("sessionid", sessionId);
                reader = c.ExecuteReader();
                User user = null;
                if (reader.HasRows)
                {
                    reader.Read();
                    int id = reader.GetInt32(0);
                    string username = reader.GetString(1);
                    string password = reader.GetString(2);
                    string ssn = reader.GetString(3);
                    string lastname = reader.GetString(4);
                    string firstname = reader.GetString(5);
                    string email = reader.GetString(6);
                    int accesslevel = reader.GetInt32(7);
                    int sessionid = reader.GetInt32(8);
                    int stateId = reader.GetInt32(9);
                    int stateCourse = reader.GetInt32(10);
                    user = new User(id, username, password, ssn , lastname , firstname , email , accesslevel , sessionid, stateId, stateCourse);
                }
                reader.Close();
                return user;
            }
        }


        public int GenerateAndSetSessionId(String username)
        {
            int sid = GenerateUniqueSessionId();
            if (sid != -1)
            {
                using (SqlCommand c = new SqlCommand(SET_SESSION_ID, connection))
                {
                    c.Parameters.AddWithValue("sessionid", sid);
                    c.Parameters.AddWithValue("name", username);
                    c.ExecuteNonQuery();
                }
                return sid;
            }
            else
                return -1;
            
        }

        public bool Login(String username, String password)
        {
            if (connection.State == System.Data.ConnectionState.Closed)
                connection.Open();
            SqlDataReader reader = null;
            Boolean result;
            using (SqlCommand c = new SqlCommand(USERNAME_AND_PASSWORD_VALID, connection))
            {
                
                c.Parameters.AddWithValue("name", username);
                c.Parameters.AddWithValue("password", password);
                reader = c.ExecuteReader();
                reader.Read();

                if (reader.GetInt32(0) == 1)
                    result = true;
                else
                    result = false;

                reader.Close();
            }
            return result;
        }

        private int GenerateUniqueSessionId()
        {
            Random r = new Random();
            Boolean unique = false;
            SqlDataReader reader = null;
            int sid = -1;
            using (SqlCommand c = new SqlCommand(COUNT_SESSIONID, connection))
            {
                while (!unique)
                {
                    sid = r.Next(Int32.MaxValue);
                    c.Parameters.AddWithValue("sessionid", sid);
                    reader = c.ExecuteReader();
                    reader.Read();
                    if (reader.GetInt32(0) == 0)
                        unique = true;
                }
                reader.Close();

            }
            return sid;
        }
    }

}
