using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compiler
{
    class I_Float_Constructor : IObject_Constructor
    {
        public I_Float_Constructor()
        {
        }

        public override IObject GetNewInstans()
        {
            return new I_Float(0);
        }

        public override IObject GetNewInstans(IObject obj)
        {
            switch (obj.IType)
            {
                case IObjectType.I_Float:
                    return new I_Float((I_Float)obj);
                default:
                    return new I_Error("Can not create copy.");
            }
        }

        public override IObject GetNewInstans(IObject[] objs)
        {
            if (objs.Length == 1)
            {

                switch (objs[0].IType)
                {
                    case IObjectType.I_Float:
                        return new I_Float((float)((I_Float)objs[0]).VALUE);
                    case IObjectType.I_Int:
                        return new I_Float(((I_Int)objs[0]).VALUE);
                }
            }

            return new I_Error("Can not convert to Float64");
        }

        public override IObjectType GetObjectType()
        {
            return IObjectType.I_Float;
        }
    }
}
