using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Compiler
{
    class Mean : IObject
    {
        public Mean()
        {
        }

        public override IObject MethodOperator(IObject[] strParams)
        {
            if(strParams.Length < 1)
                return new IObject();


            double retValue = 0;

            for (int i = 0; i < strParams.Length; ++i)
            {
                try
                {
                    switch (strParams[i].IType)
                    {
                        case IObjectType.I_Float:
                            retValue += ((I_Float)strParams[i]).VALUE;
                            break;

                        case IObjectType.I_Int:
                            retValue += ((I_Int)strParams[i]).VALUE;
                            break;
                    }
                }
                catch
                {
                }
            }

            return new I_Float(retValue / strParams.Length);

        }

        public override int GetAutoCompleteIconIndex() { return 4; }
        public override string GetAutoCompleteToolTip(string str) { return str + "(Number1, Number2, ..., NumberN) returns the mean number."; }
        public override string GetAutoCompleteText(string str) { return str; }
        public override string GetAutoCompleteListText(string str) { return str + "()"; }
    }
}
