using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Compiler
{
    class Cos : IObject
    {
        public Cos()
        {
        }

        public override IObject MethodOperator(IObject[] strParams)
        {
            if (strParams.Length == 1)
            {
                double value = Compiler.I_Number.ToDouble(strParams[0]);
                return new I_Float(Math.Cos(value));
            }
            return new I_Error("Wrong number of arguments.");
        }

        public override int GetAutoCompleteIconIndex() { return 4; }
        public override string GetAutoCompleteToolTip(string str) { return str + "(Number angle)"; }
        public override string GetAutoCompleteText(string str) { return str; }
        public override string GetAutoCompleteListText(string str) { return str + "()"; }
    }
}
