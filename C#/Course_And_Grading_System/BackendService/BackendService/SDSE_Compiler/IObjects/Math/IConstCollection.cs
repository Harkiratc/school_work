using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Compiler
{
    class IConstCollection : IObjectWithMembers
    {
        public IConstCollection()
        {
        }

        public void AddInt(string name, Int32 value)
        {
            AddMember(name, new I_Int(value));
        }

        public void AddObjectWithMembers(string name, IObjectWithMembers obj)
        {
            AddMember(name, obj);
        }
    }
}
