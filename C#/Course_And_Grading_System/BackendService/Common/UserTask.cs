using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Common
{
    [DataContract()]
    public class UserTask
    {
        [DataMember]
        private int id, userId, taskId, grade, reportedBy;
        [DataMember]
        private DateTime dateTime;

        public UserTask(int id, int userId, int taskId, int grade, DateTime dateTime, int reportedBy)
        {
            this.id = id;
            this.userId = userId;
            this.taskId = taskId;
            this.grade = grade;
            this.dateTime = dateTime;
            this.reportedBy = reportedBy;

        }

        public int ReportedBy
        {
            get { return reportedBy; }
            set { reportedBy = value; }
        }

        public int Grade
        {
            get { return grade; }
            set { grade = value; }
        }

        public int TaskId
        {
            get { return taskId; }
            set { taskId = value; }
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

        public DateTime DateTime
        {
            get { return dateTime; }
            set { dateTime = value; }
        }

    }
}
