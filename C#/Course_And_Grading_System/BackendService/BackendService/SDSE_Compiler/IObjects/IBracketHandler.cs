using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Compiler
{
    class IBracketHandler
    {
        private StringBuilder insideString;
        private int bracketDepth = 0;
        private bool empty = true;

        private SDSE_Compiler rootCompiler;

        public string Name;
        public IObjectType IType = IObjectType.I_Matrix;

        public IBracketHandler(SDSE_Compiler compiler)
        {
            Name = "";
            rootCompiler = compiler;
            insideString = new StringBuilder();
        }

        //public void Insert(Dictionary<string, IObject> scope, string str)
        public void Insert(string str)
        {
            char[] chars = str.ToCharArray();

            if (chars.Length == 0 || !OPEN)
                return;

            int i = 0;

            if (empty)
            {
                for (; i < chars.Length; ++i)
                {
                    if (chars[i] == '{')
                    {
                        ++bracketDepth;
                        ++i;
                        empty = false;
                        break;
                    }
                    else if (chars[i] != ' ')
                    {
                        Clear();
                        return;
                    }
                }
            }

            for (; i < chars.Length; ++i)
            {
                if (chars[i] == '{')
                    ++bracketDepth;
                else if (chars[i] == '}')
                    --bracketDepth;

                if (bracketDepth == 0)
                {
                    IObject retIobj = BracketsDone();

                    rootCompiler.AddOrUpdateUserDefinedIObject(Name, retIobj);

                    Clear();
                    return;
                }
                else
                {
                    insideString.Append(chars[i]);
                }
            }
            insideString.Append(Environment.NewLine);
        }

        private IObject BracketsDone()
        {
            if (IType == IObjectType.I_String)
            {
                I_String str = new I_String();
                str.BracketOperator(insideString.ToString());
                return str;
            }
            return new IObject();
        }

        public bool OPEN
        {
            get
            {
                return Name != "";
            }
        }

        public void Clear()
        {
            IType = IObjectType.I_Matrix;
            Name = "";
            empty = true;
            bracketDepth = 0;
            insideString = new StringBuilder();
        }
    }
}
