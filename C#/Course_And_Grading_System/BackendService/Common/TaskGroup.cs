using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Common
{
    [DataContract()]
    public class TaskGroup
    {
        [DataMember]
        private int id, courseId;
        [DataMember]
        private String name;

        public TaskGroup(int id, int courseId, String name)
        {
            this.id = id;
            this.courseId = courseId;
            this.name = name;
        }

        public int CourseId
        {
            get { return courseId; }
            set { courseId = value; }
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
