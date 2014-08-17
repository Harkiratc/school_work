using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Compiler
{
    class I_Error : IObject
    {
        private string str;

        public I_Error(I_Error err)
        {
            str = err.str;
            iType = IObjectType.I_Error;
        }

        public I_Error(string s)
        {
            str = s;
            iType = IObjectType.I_Error;
        }

        public string VALUE
        {
            get
            {
                return str;
            }
        }

        public override string ToString()
        {
            //return str;
            return "<div class=\"error\">" + str + "</div>";
        }

        public override string ToHtmlString()
        {
            return "<div class=\"error\">" + str + "</div>";
        }

        static public I_Error CanNotConvert(uint from, uint to)
        {
            return new I_Error("Can not convert " + IObject.PublicTypeName[from] + " to " + IObject.PublicTypeName[to] + ".");
        }
    }
}
