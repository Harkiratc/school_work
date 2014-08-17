using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Common
{
     [DataContract()]
    public class Course
    {
         public Course(int id, String code, String name, int period)
         {
             this.id = id;
             this.code = code;
             this.name = name;
             this.period = period;
         }

        [DataMember]
         private int id, period;

        public int Period
        {
            get { return period; }
            set { period = value; }
        }

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        [DataMember]
        private String code, name;

        public String Name
        {
            get { return name; }
            set { name = value; }
        }

        public String Code
        {
            get { return code; }
            set { code = value; }
        }

    }
}
