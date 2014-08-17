using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compiler
{
    class IObjectWithMembers : IObject
    {
        protected Dictionary<String, IObject> members = new Dictionary<string, IObject>();

        protected bool HasMember(string str)
        {
            return members.ContainsKey(str);
        }

        protected void AddMember(string str, IObject iobj)
        {
            if (members.ContainsKey(str))
                members.Remove(str);

            members.Add(str, iobj);
        }

        protected IObject GetMember(string str)
        {
            return members[str];
        }


        public override Dictionary<string, IObject> GetMembers()
        {
            return members;
        }

        /*
        public override ListViewItemTag[] GetAutoCompleat(String str) 
        {
            List<ListViewItemTag> retStrs = new List<ListViewItemTag>();
            Tools.AppendAutoListFromDictionary(ref str, ref retStrs, ref members);

            return retStrs.ToArray();
        }
         * */

        public new virtual int GetAutoCompleteIconIndex() { return 2; }
        public new virtual string GetAutoCompleteToolTip(string str) { return "Library"; }
        public new virtual string GetAutoCompleteText(string str) { return str; }
        public new virtual string GetAutoCompleteListText(string str) { return str + "1"; }
    }
}
