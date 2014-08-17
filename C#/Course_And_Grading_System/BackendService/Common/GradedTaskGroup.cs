using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Common
{
    [DataContract()]
    public class GradedTaskGroup
    {
        [DataMember]
        private TaskGroup taskGroup;

        [DataMember]
        private List<GradedTask> tasks;
        [DataMember]
        private bool allTasksGraded, allTasksPassed;

        [DataMember]
        private String gradeExpression, gradeName;

        public GradedTaskGroup(TaskGroup taskGroup)
        {
            tasks = new List<GradedTask>();
            allTasksGraded = false;
            gradeExpression = String.Empty;
            this.taskGroup = taskGroup;
            gradeName = String.Empty;
            allTasksPassed = true;
        }

        public bool AllTasksPassed
        {
            get { return allTasksPassed; }
            set { allTasksPassed = value; }
        }

        public String GradeName
        {
            get { return gradeName; }
            set { gradeName = value; }
        }

        public TaskGroup TaskGroup
        {
            get { return taskGroup; }
            set { taskGroup = value; }
        }

        public List<GradedTask> Tasks
        {
            get { return tasks; }
            set { tasks = value; }
        }
        

        public bool AllTasksGraded
        {
            get { return allTasksGraded; }
            set { allTasksGraded = value; }
        }

        public String GradeExpression
        {
            get { return gradeExpression; }
            set { gradeExpression = value; }
        }


    }
}
