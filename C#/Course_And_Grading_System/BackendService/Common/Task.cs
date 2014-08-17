using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Common
{
    [DataContract()]
    public class Task
    {
        public static int GRADED_GRADE_TYPE = 1;
        public static int PASS_FAIL_GRADE_TYPE = 2;
        public static int BONUS_ON_EXAM_GRADE_TYPE = 3;
        [DataMember]
        private int id, taskGroupId, gradeType, weight;
        [DataMember]
        private String name;

        public Task(int id, int taskGroupId, String name, int gradeType, int weight)
        {
            this.id = id;
            this.taskGroupId = taskGroupId;
            this.name = name;
            this.gradeType = gradeType;
            this.weight = weight;
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

        public int TaskGroupId
        {
            get { return taskGroupId; }
            set { taskGroupId = value; }
        }

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public String Name
        {
            get { return name; }
            set { name = value; }
        }
    }
}
