using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compiler
{
    class I_Int_Constructor : IObject_Constructor
    {
        public I_Int_Constructor()
        {
        }

        public override IObject GetNewInstans()
        {
            return new I_Int(0);
        }

        public override IObject GetNewInstans(IObject[] objs)
        {
            if (objs.Length == 1)
            {

                switch (objs[0].IType)
                {
                    case IObjectType.I_Float:
                        return new I_Int((int)((I_Float)objs[0]).VALUE);
                    case IObjectType.I_Int:
                        return new I_Int(((I_Int)objs[0]).VALUE);
                }
            }

            return new I_Error("Can not convert to Int32");
        }

        public override IObject GetNewInstans(IObject obj)
        {
            switch (obj.IType)
            {
                case IObjectType.I_Int:
                    return new I_Int((I_Int)obj);
                default:
                    return new I_Error("Can not create copy.");
            }
        }

        public override IObjectType GetObjectType()
        {
            return IObjectType.I_Int;
        }
    }
}
