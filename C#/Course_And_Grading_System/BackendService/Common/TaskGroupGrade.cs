using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Common
{
    [DataContract()]
    public class TaskGroupGrade
    {   
        [DataMember]
        private int id, taskGroupId, limit, value;
        [DataMember]
        private String gradeName, expression;

        public TaskGroupGrade(int id, int taskGroupId, int limit, int value, String gradeName, String expression)
        {
            this.id = id;
            this.taskGroupId = taskGroupId;
            this.limit = limit;
            this.value = value;
            this.gradeName = gradeName;
            this.expression = expression;
        }


        public int Value
        {
            get { return this.value; }
            set { this.value = value; }
        }

        public int Limit
        {
            get { return limit; }
            set { limit = value; }
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
        

        public String Expression
        {
            get { return expression; }
            set { expression = value; }
        }

        public String GradeName
        {
            get { return gradeName; }
            set { gradeName = value; }
        }
    }
}
