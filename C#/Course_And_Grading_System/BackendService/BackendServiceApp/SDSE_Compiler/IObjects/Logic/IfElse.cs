using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Compiler
{
    class IfElse : IObject
    {
        public IfElse()
        {
        }

        public override IObject MethodOperator(IObject[] strParams)
        {
            if(strParams.Length != 4)
                return new IObject();

            if (strParams[0].EqualEqualOperator(strParams[1]))
            {
                return strParams[2];
            }
            return strParams[3];
        }

        public override int GetAutoCompleteIconIndex() { return 4; }
        public override string GetAutoCompleteToolTip(string str) { return str + "(a, b, c, d) If a == b return c otherwise d."; }
        public override string GetAutoCompleteText(string str) { return str; }
        public override string GetAutoCompleteListText(string str) { return str + "()"; }
    }
}
