using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Common
{
    /// <summary>
    /// DTO Containing user information
    /// </summary>
    [DataContract()]
    public class User
    {
        public const int ADMIN = 3;
        public const int RESULTS_REPORT = 2; 
        public const int LADOK_ADMIN = 1;
        public const int STUDENT = 0;
        [DataMember]
        private int id, accesslevel, sessionId, stateId, stateCourse;


        [DataMember]
        String username, password, ssn, firstname, lastname, email;

        public User(int id, string name, string password, string ssn, string lastname, string firstname, string email, int accesslevel, int sessionId,int stateId, int stateCourse)
                    : this(id, name, password, ssn, lastname, firstname,  email, accesslevel, sessionId)
        {
            StateId = stateId;
            StateCourse = stateCourse;
        }
        public User(int id, string name, string ssn, string lastname, string firstname, string email, int accesslevel)
        {
            Id = id;
            Username = name;
            Ssn = ssn;
            Lastname = lastname;
            Firstname = firstname;
            Email = email;
            Accesslevel = accesslevel;
            StateId = 0;
            StateCourse = 0;
        }
        public User(int id, string name, string password, string ssn, string lastname, string firstname, string email, int accesslevel, int sessionId)
        {
            Id = id;
            Username = name;
            Password = password;
            Ssn = ssn;
            Lastname = lastname;
            Firstname = firstname;
            Email = email;
            Accesslevel = accesslevel;
            SessionId = sessionId;
            StateId = 0;
            StateCourse = 0;
        }

        public int StateCourse
        {
            get { return stateCourse; }
            set { stateCourse = value; }
        }

        public int StateId
        {
            get { return stateId; }
            set { stateId = value; }
        }

        public int SessionId
        {
            get { return sessionId; }
            set { sessionId = value; }
        }

        public int Accesslevel
        {
            get { return accesslevel; }
            set { accesslevel = value; }
        }

        public int Id
        {
            get { return id; }
            set { id = value; }
        }



        public String Email
        {
            get { return email; }
            set { email = value; }
        }

        public String Lastname
        {
            get { return lastname; }
            set { lastname = value; }
        }

        public String Firstname
        {
            get { return firstname; }
            set { firstname = value; }
        }

        public String Ssn
        {
            get { return ssn; }
            set { ssn = value; }
        }

        public String Password
        {
            get { return password; }
            set { password = value; }
        }

        public String Username
        {
            get { return username; }
            set { username = value; }
        }

 
        public override string ToString()
        {
            return Id + " " + Username + " " + Password + " " + Ssn + " " + Lastname + " " + Firstname + " " + Email + " " + Accesslevel + " " + " " + SessionId;
        }

    }
}
