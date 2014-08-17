using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Common
{
    [DataContract()]
    public class CourseAttentant
    {
        [DataMember]
        private int id, userId, courseId;

        public CourseAttentant(int id, int userId, int courseId)
        {
            this.id = id;
            this.userId = userId;
            this.courseId = courseId;
        }


        public int CourseId
        {
            get { return courseId; }
            set { courseId = value; }
        }

        public int UserId
        {
            get { return userId; }
            set { userId = value; }
        }

        public int Id
        {
            get { return id; }
            set { id = value; }
        }


    }
}
