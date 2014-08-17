using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Common
{
    [DataContract()]
    public class CourseGrade
    {
        [DataMember]
        private int id, courseId, limit;


        [DataMember]
        private String expression, gradeName;

        public CourseGrade(int id, int courseId, String gradeName, int limit, String expression)
        {
            this.id = id;
            this.courseId = courseId;
            this.gradeName = gradeName;
            this.limit = limit;
            this.expression = expression;
        }

        public String GradeName
        {
            get { return gradeName; }
            set { gradeName = value; }
        }

        public int Limit
        {
            get { return limit; }
            set { limit = value; }
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

        public String Expression
        {
            get { return expression; }
            set { expression = value; }
        }
    }
}
