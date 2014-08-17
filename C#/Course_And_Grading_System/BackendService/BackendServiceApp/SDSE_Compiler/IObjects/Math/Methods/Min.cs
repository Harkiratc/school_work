using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Compiler
{
    class Min : IObject
    {
        public Min()
        {
        }

        public override IObject MethodOperator(IObject[] strParams)
        {
            if(strParams.Length < 2)
                return new IObject();

            IObject minValue = strParams[0];
            for (int i = 1; i < strParams.Length; ++i)
            {
                if (minValue.LargerOperator(strParams[i]))
                    minValue = strParams[i];
            }
            return minValue;
        }

        public override int GetAutoCompleteIconIndex() { return 4; }
        public override string GetAutoCompleteToolTip(string str) { return str + "(Number1, Number2, ..., NumberN) returns the smallest number."; }
        public override string GetAutoCompleteText(string str) { return str; }
        public override string GetAutoCompleteListText(string str) { return str + "()"; }
    }
}
