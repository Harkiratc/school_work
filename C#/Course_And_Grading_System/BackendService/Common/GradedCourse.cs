using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Common
{
    [DataContract()]
    public class GradedCourse
    {
        [DataMember]
        private List<GradedTaskGroup> gradedTaskGroups;
        [DataMember]
        private String gradeName = "";
        [DataMember]
        private Course course;

        public GradedCourse(Course course)
        {
            this.course = course;
        }

        public List<GradedTaskGroup> GradedTaskGroups
        {
            get { return gradedTaskGroups; }
            set { gradedTaskGroups = value; }
        }

        public String GradeName
        {
            get { return gradeName; }
            set { gradeName = value; }
        }

        public Course Course
        {
            get { return course; }
            set { course = value; }
        }

    }
}
