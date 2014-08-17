using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
namespace Common
{
    [DataContract()]
    public class GradedTask
    {
        [DataMember]
        private int taskId, grade, gradeType, weight;

       
        [DataMember]
        private String taskName;

        public GradedTask(int taskId, String taskName, int weight, int gradeType)
        {
            this.taskId = taskId;
            this.taskName = taskName;
            this.gradeType = gradeType;
            this.weight = weight;
            this.grade = -1;
        }

        public GradedTask(int taskId, String taskName, int weight, int gradeType, int grade)
                          : this(taskId, taskName, weight, gradeType)
        {
            this.grade = grade;
        }


        public int Weight
        {
            get { return weight; }
            set { weight = value; }
        }

        public int GradeType
        {
            get { return gradeType; }
            set { gradeType = value; }
        }

        public int Grade
        {
            get { return grade; }
            set { grade = value; }
        }


        public String TaskName
        {
            get { return taskName; }
            set { taskName = value; }
        }
        public int TaskId
        {
            get { return taskId; }
            set { taskId = value; }
        }
    }
}
