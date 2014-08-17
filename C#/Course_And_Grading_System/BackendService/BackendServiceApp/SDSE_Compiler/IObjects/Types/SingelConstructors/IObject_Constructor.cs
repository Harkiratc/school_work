using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compiler
{
    abstract class IObject_Constructor : IObjectWithMembers
    {
        public IObject_Constructor()
        {
        }

        public abstract IObject GetNewInstans(IObject obj);

        public virtual IObject GetNewInstans() { return new I_Error("Can not create new instans."); }

        public virtual IObject GetNewInstans(IObject[] objs)
        {
            if (objs.Length == 1)
                return objs[0];
            return new I_Error("Can not create new instans.");
        }

        public abstract IObjectType GetObjectType();

        public override int GetAutoCompleteIconIndex() { return 1; }
        public override string GetAutoCompleteToolTip(string str) { return str + " type."; }
        public override string GetAutoCompleteText(string str) { return str; }
        public override string GetAutoCompleteListText(string str) { return str; }
    }
}
