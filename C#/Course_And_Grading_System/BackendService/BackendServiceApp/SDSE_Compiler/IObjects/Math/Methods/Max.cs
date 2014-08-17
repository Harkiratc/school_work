using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Compiler
{
    class Max : IObject
    {
        public Max()
        {
        }

        public override IObject MethodOperator(IObject[] strParams)
        {
            if(strParams.Length < 2)
                return new IObject();

            IObject maxValue = strParams[0];
            for (int i = 1; i < strParams.Length; ++i)
            {
                if (maxValue.LessOperator(strParams[i]))
                    maxValue = strParams[i];
            }
            return maxValue;
        }

        public override int GetAutoCompleteIconIndex() { return 4; }
        public override string GetAutoCompleteToolTip(string str) { return str + "(Number1, Number2, ..., NumberN) returns the largest number."; }
        public override string GetAutoCompleteText(string str) { return str; }
        public override string GetAutoCompleteListText(string str) { return str + "()"; }
    }
}
