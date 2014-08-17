using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Common;
using Compiler;
using System.Xml;
using System.IO;


namespace BackendService.Model
{
    public class ServerImpl
    {
        private SQLManager sqlManager;
        private SDSE_Compiler compiler;


        public ServerImpl()
        {
            sqlManager = new SQLManager();
            compiler = new SDSE_Compiler();
            compiler.AddIObjectWithMembers("Max", new Max());
            compiler.AddIObjectWithMembers("Mean", new Mean());
            compiler.AddIObjectWithMembers("Min", new Min());
            compiler.AddIObjectWithMembers("Abs", new Abs());
            compiler.AddIObjectWithMembers("IfElse", new IfElse());
        }

        public void SetUserState(int sessionId, int stateId, int stateCourse)
        {
            sqlManager.SetUserState(sessionId, stateId, stateCourse);
        }

        public Common.Task getTask(int taskId)
        {
            return sqlManager.getTask(taskId);
        }
        public List<User> GetUsers(int sessionId)
        {
            User user = GetUser(sessionId);
            List<User> users = new List<User>();
            if (user.Accesslevel > User.STUDENT)
            {
                users = sqlManager.GetUsers();
            }
            return users;
        }

        public bool AddCoursesFromXml(int sessionId, String xmlString)
        {
            bool success = false;
            if (GetUser(sessionId).Accesslevel == User.ADMIN)
            {
                try
                {
                    AddCoursesFromXml(xmlString);
                    success = true;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            return success;
        }
        private void AddCoursesFromXml(String xmlString)
        {
        int courseId = -1, taskGroupId = -1;
       
            using (XmlReader reader = XmlReader.Create(new StringReader(xmlString)))
            {
                while (reader.Read())
                {
                    if (reader.IsStartElement())
                    {
                        String code, name, expression;
                        int limit, value, gradeType, weight;
                        switch (reader.Name)
                        {
                            case "Course":
                                code = reader.GetAttribute("code");
                                name = reader.GetAttribute("name");
                                int period = int.Parse(reader.GetAttribute("period"));
                                courseId = sqlManager.AddCourse(code, name, period);
                                break;
                            case "CourseGrade":
                                String courseGradeName = reader.GetAttribute("gradename");
                                limit = int.Parse(reader.GetAttribute("limit"));
                                expression = reader.GetAttribute("expression");
                               
                                sqlManager.AddCourseGrade(courseId, courseGradeName, limit, expression);
                                break;
                            case "TaskGroup":
                                taskGroupId = sqlManager.AddTaskGroup(courseId, reader.GetAttribute("name"));
                                break;
                            case "TaskGroupGrade":
                                limit = int.Parse(reader.GetAttribute("limit"));
                                value =  int.Parse(reader.GetAttribute("value"));
                                name = reader.GetAttribute("name");
                                expression = reader.GetAttribute("expression");
                                sqlManager.AddTaskGroupGrade(taskGroupId, limit, value, name, expression);
                                break;
                            case "Task":
                                name = reader.GetAttribute("name");
                                gradeType = int.Parse(reader.GetAttribute("gradetype"));
                                weight = int.Parse(reader.GetAttribute("weight"));
                                sqlManager.AddTask(taskGroupId, name, gradeType, weight);
                                break;
                        }
                    }
          
                }
            }
        }

        public List<TaskGroupGrade> GetTaskGroupGrades(int taskGroupId)
        {
            return sqlManager.GetTaskGroupGrades(taskGroupId);
        }

        public int CountReportedTasksForUserInCourse(int userId, int courseId)
        {
            return sqlManager.CountReportedTasksForUserInCourse(userId, courseId);
        }

        public UserTask GetUserTask(int userId, int taskId)
        {
            return sqlManager.GetUserTask(userId, taskId);
        }

        public List<CourseGrade> GetCourseGrades(int courseId)
        {
            return sqlManager.GetCourseGrades(courseId);
        }

        public Course GetCourse(int courseId)
        {
            return sqlManager.getCourse(courseId);

        }

        public List<Common.Task> GetTasks(int taskGroupId)
        {
            return sqlManager.GetTasks(taskGroupId);
        }

        public List<TaskGroup> GetTaskGroups(int courseId)
        {
            return sqlManager.GetTaskGroups(courseId);
        }

        public int GetTaskReported(int taskId, int userId)
        {
            return sqlManager.GetTaskReported(taskId,userId);
        }

        public void UpdateReported(int taskId, int userId)
        {
            sqlManager.UpdateReported(taskId, userId);
        }

        public List<User> GetUsersForCourse(int courseId)
        {
            List<CourseAttentant> attendants = GetCourseAttendantsForCourse(courseId);
            List<User> users = new List<User>();
            foreach (CourseAttentant a in attendants)
            {
                users.Add(GetUserFromId(a.UserId));
            }
            return users;
        }

        public List<CourseAttentant> GetCourseAttendantsForCourse(int courseId)
        {
            return sqlManager.GetCourseAttendantsForCourse(courseId);
        }

        public User GetUserFromId(int userId)
        {
            return sqlManager.GetUserFromId(userId);
        }

        // TODO IMPLEMENT WITH TASK GROUP GRADES
        public GradedCourse GetCourseGrade(int userId, int courseId)
        {
            List<TaskGroup> taskGroups = sqlManager.GetTaskGroups(courseId);
            GradedCourse gradedCourse = new GradedCourse(sqlManager.getCourse(courseId));
            List<GradedTaskGroup> gradedTaskGroups = new List<GradedTaskGroup>();
            gradedCourse.GradedTaskGroups = gradedTaskGroups;
            float grade = 0, bonus = 0;


            bool allTaskGroupsGraded = true;
            bool allTaskGroupsPassed = true;
            foreach(TaskGroup tg in taskGroups)
            {
                GradedTaskGroup gtg = GetTaskGroupGrade(userId, tg);

                if (!gtg.AllTasksGraded)
                    allTaskGroupsGraded = false;

                if (gtg.AllTasksPassed == false)
                    allTaskGroupsPassed = false;

                gradedTaskGroups.Add(gtg);

            }
            if (allTaskGroupsGraded && allTaskGroupsPassed)
            {
                foreach (GradedTaskGroup gtg in gradedTaskGroups)
                {
                    compiler.Interpretate(gtg.GradeExpression);
                    grade += compiler.GetVarAsFloat("g");
                    bonus += compiler.GetVarAsFloat("b");
                }
                List<CourseGrade> courseGrades = sqlManager.GetCourseGrades(courseId);
                String expressionParsed, tmp;
                float meetsGradeReq = 0,parsed;
                foreach (CourseGrade cg in courseGrades)
                {

                    expressionParsed = "g = " + grade + Environment.NewLine + "b = " + bonus + Environment.NewLine + "parsed = " + cg.Expression + Environment.NewLine;
                    compiler.Interpretate(expressionParsed);
                    parsed = compiler.GetVarAsFloat("parsed");
                    tmp = "meetsGradeReq = IfElse(Max(" + parsed + "," + cg.Limit + "), " + cg.Limit + ", 0, 1)" + Environment.NewLine;
                    compiler.Interpretate(tmp);
                    meetsGradeReq = compiler.GetVarAsFloat("meetsGradeReq");

                    if (meetsGradeReq == 1)
                    {
                        gradedCourse.GradeName = cg.GradeName;
                        break;
                    }
                }
            }

            return gradedCourse;
        }

        private GradedTaskGroup GetTaskGroupGrade(int userId, TaskGroup taskGroup)
        {
            String exp = "";
            List<Common.Task> tasks = sqlManager.GetTasks(taskGroup.Id);
            List<GradedTask> gradedTasks = new List<GradedTask>();
            GradedTaskGroup gtg = new GradedTaskGroup(taskGroup);
            bool allTasksGraded = true;
            UserTask userTask;
            foreach (Common.Task task in tasks)
            {
                if ((userTask = sqlManager.GetUserTask(userId, task.Id)) != null)
                {

                    gradedTasks.Add(new GradedTask(task.Id, task.Name, task.Weight, task.GradeType, userTask.Grade));


                }
                else
                {
                    gradedTasks.Add(new GradedTask(task.Id, task.Name, task.Weight, task.GradeType)); ;
                    allTasksGraded = false;
                }
            } 
            // All tasks graded, calc task group grade
            if (allTasksGraded)
            {
                int grade = 0, bonus = 0, pass = 1;
                foreach (GradedTask gtask in gradedTasks)
                {
                    // P/F, GRADED OR BONUS ON EXAM
                    if (gtask.GradeType == Common.Task.GRADED_GRADE_TYPE)
                    {
                        grade += gtask.Grade * gtask.Weight;

                    }
                    else if (gtask.GradeType == Common.Task.PASS_FAIL_GRADE_TYPE)
                    {
                        pass *= gtask.Grade;
                    }
                    else if (gtask.GradeType == Common.Task.BONUS_ON_EXAM_GRADE_TYPE)
                    {
                        bonus += gtask.Grade;
                    }

                }

                // Failed some pass/fail moment, can't pass course
                if (pass == 0)
                    grade = 0;

                List<TaskGroupGrade> grades = sqlManager.GetTaskGroupGrades(taskGroup.Id);
                exp = "g = 0" + Environment.NewLine + "b = 0" + Environment.NewLine;

                foreach (TaskGroupGrade tgg in grades)
                {
                    String expressionParsed = "g = " + grade + Environment.NewLine + "b = " + bonus + Environment.NewLine + "parsed = " + tgg.Expression + Environment.NewLine;
                    compiler.Interpretate(expressionParsed);
                    float parsed = compiler.GetVarAsFloat("parsed");
                    String tmp = "meetsGradeReq = IfElse(Max(" + parsed + "," + tgg.Limit + "), " + tgg.Limit + ", 0, 1)" + Environment.NewLine;
                    compiler.Interpretate(tmp);
                    float meetsGradeReq = compiler.GetVarAsFloat("meetsGradeReq");

                    // Meets grade requirements, do point calculations
                    if (meetsGradeReq == 1)
                    {
                        exp = "g = " + tgg.Value + Environment.NewLine + "b = " + bonus + Environment.NewLine;
                        gtg.GradeName = tgg.GradeName;

                         // FAILED TASK GROUP, set status bool
                        if (tgg.Value == 0)
                            gtg.AllTasksPassed = false;

                        break;
                    }

                }
            }


            gtg.Tasks = gradedTasks;
            gtg.AllTasksGraded = allTasksGraded;
            gtg.GradeExpression = exp;

            return gtg;
        }




        public bool AddUserTask(int sessionId, int userId, int taskId, int grade)
        {
            User user = GetUser(sessionId);
            if (user.Accesslevel >= User.RESULTS_REPORT)
            {
                try
                {
                    if (!sqlManager.IsUsertaskSet(userId, taskId))
                        sqlManager.AddUserTask(userId, taskId, grade, DateTime.Now, user.Id);
                    else
                        sqlManager.UpdateUserTask(userId, taskId, grade, DateTime.Now, user.Id);

                    return true;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            return false;

        }

        public List<Course> GetCourses(int sessionId)
        {
            User user = GetUser(sessionId);
            if (user.Accesslevel > User.STUDENT)
            {
                 return sqlManager.GetCourses();
            }
            else
            {
                return sqlManager.GetCourses(user.Id);
            }

        }

        public int AddCourseAttendant(int sessionId, int attendantId, int courseId)
        {
            int courseAttendantId = -1;
            User user = GetUser(sessionId);
            if (user.Accesslevel >= User.RESULTS_REPORT)
            {
                try
                {
                    courseAttendantId = sqlManager.AddCourseAttendant(attendantId, courseId);

                    // Add reported 0 for each task in course in Reported table
                    List<TaskGroup> taskGroups = sqlManager.GetTaskGroups(courseId);
                    foreach (TaskGroup tg in taskGroups)
                    {
                        List<Common.Task> tasks = sqlManager.GetTasks(tg.Id);
                        foreach (Common.Task t in tasks)
                            sqlManager.AddReported(t.Id, attendantId, 0);
                        
                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            return courseAttendantId;
        }

        public int AddTask(int sessionId, int taskGroupId, String name, int gradeType, int weight)
        {
            int taskId = -1;
            if (GetUser(sessionId).Accesslevel == User.ADMIN)
            {
                try
                {
                    taskId = sqlManager.AddTask(taskGroupId, name, gradeType, weight);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            return taskId;
        }

        public int AddTaskGroupGrade(int sessionId,int taskgroupId, int limit, int value, String gradeName, String expression)
        {
            int taskGroupGradeId = -1;
            if (GetUser(sessionId).Accesslevel == User.ADMIN)
            {
                try
                {
                    taskGroupGradeId = sqlManager.AddTaskGroupGrade(taskgroupId,limit,value,gradeName,expression);

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            return taskgroupId;
        }

        public int AddTaskGroup(int sessionId, int courseId, String taskGroupName)
        {
            int taskGroupId = -1;
            if (GetUser(sessionId).Accesslevel == User.ADMIN)
            {
                try
                {
                    taskGroupId = sqlManager.AddTaskGroup(courseId, taskGroupName);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            return taskGroupId;
        }

        public int AddCourseGrade(int sessionId, int courseId, String gradeName, int limit, String expression)
        {
            int courseGradeId = -1;
            if (GetUser(sessionId).Accesslevel == User.ADMIN)
            {
                try
                {
                    courseGradeId = sqlManager.AddCourseGrade(courseId,gradeName, limit, expression);
                    
                } 
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            return courseGradeId;
        }

        public int AddCourse(int sessionId, String code, String courseName, int period)
        {
            int courseId = -1;
            if (GetUser(sessionId).Accesslevel == User.ADMIN)
            {
                try
                {
                    courseId = sqlManager.AddCourse(code, courseName, period);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            return courseId;
        }

        public int GetAccessLevel(int sessionId)
        {
            return GetUser(sessionId).Accesslevel;
        }

        public void Logout(int sessionId)
        {
            sqlManager.Disconnect(sessionId);
        }

        public int Login(String username, String password)
        {
           bool success = sqlManager.Login(username, password);
           int sessionId = -1;
           if (success)
           {
               sessionId = sqlManager.GenerateAndSetSessionId(username);
           }
           return sessionId;

        }
        public User GetUser(int sessionId)
        {
            return sqlManager.GetUser(sessionId);
        }

        public int AddUser(int sessionId,User user)
        {
            int userId = -1;
            if (GetUser(sessionId).Accesslevel == User.ADMIN && user.Accesslevel >= User.STUDENT && user.Accesslevel <= User.ADMIN)
            {
                try
                {
                    userId = sqlManager.AddUser(user);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            return userId;

        }

        

    }
}
