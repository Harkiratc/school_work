using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compiler
{
    class I_String_Constructor : IObject_Constructor
    {
        public I_String_Constructor()
        {
        }

        public override IObject GetNewInstans()
        {
            return new I_String();
        }

        public override IObject GetNewInstans(IObject obj)
        {
            switch (obj.IType)
            {
                case IObjectType.I_String:
                    return new I_String((I_String)obj);
                default:
                    return new I_Error("Can not create copy.");
            }
        }

        public override IObject GetNewInstans(IObject[] objs)
        {
            if (objs.Length == 1 && objs[0].IType == IObjectType.I_String)
                return new I_String(((I_String) objs[0]).VALUE);

            return new I_Error("Can not convert to String");
        }

        public override IObjectType GetObjectType()
        {
            return IObjectType.I_String;
        }
    }
}
