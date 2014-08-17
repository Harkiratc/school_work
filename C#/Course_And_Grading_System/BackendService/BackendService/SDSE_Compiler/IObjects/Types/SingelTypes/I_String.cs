using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;

namespace Compiler
{
    class I_String : IObjectWithMembers
    {
        private StringBuilder strBuilder;
        public bool BreakLineAtEnd = true;

        public I_String()
        {
            strBuilder = new StringBuilder();
            iType = IObjectType.I_String;

            Init();
        }

        public I_String(string s)
        {
            strBuilder = new StringBuilder(s);
            iType = IObjectType.I_String;

            Init();
        }

        public I_String(I_String s)
        {
            strBuilder = new StringBuilder(s.VALUE);
            iType = IObjectType.I_String;

            Init();
        }

        private void Init()
        {
            AddMember("AppendFromFile", new AppendFromFile(this));
        }

        public override bool UseLineBreakAfterCommand()
        {
            return BreakLineAtEnd;
        }

        public static string ToString(IObject obj)
        {
            switch (obj.IType)
            {
                case IObjectType.I_Float:
                    return ((I_Float)obj).VALUE.ToString();
                case IObjectType.I_Int:
                    return ((I_Int)obj).VALUE.ToString();
                case IObjectType.I_String:
                    return ((I_String)obj).VALUE.ToString();
                default:
                    return "";
            }
        }

        public string VALUE
        {
            get
            {
                return strBuilder.ToString();
            }
        }

        public I_String Append(String str)
        {
            strBuilder.Append(str);
            return this;
        }

        public override IObject BracketOperator(string str)
        {
            strBuilder = new StringBuilder(str.Remove(0, 2));
            return this;
        }

        public override IObject EqualOperator(IObject iobj)
        {
            if (iobj.IType == IObjectType.I_String)
                strBuilder = new StringBuilder(((I_String)iobj).strBuilder.ToString());

            return this;
        }

        public override bool EqualEqualOperator(IObject iobj)
        {
            if (iobj.IType == IObjectType.I_String)
            {
                try
                {
                    if (((I_String)iobj).strBuilder == strBuilder)
                        return true;
                }
                catch
                {
                }
            }

            return false;
        }

        public override bool LessOperator(IObject rightSide)
        {
            try
            {
                return false;
            }
            catch
            {
                return false;
            }
        }

        public override bool LargerOperator(IObject rightSide)
        {
            try
            {
                return false;
            }
            catch
            {
                return false;
            }
        }

        public override string ToString()
        {
            return strBuilder.ToString();
        }

        public override int GetAutoCompleteIconIndex() { return 3; }
        public override string GetAutoCompleteToolTip(string str) { return this.strBuilder.ToString(); }
        public override string GetAutoCompleteText(string str) { return str; }
        public override string GetAutoCompleteListText(string str) { return str; }

        class AppendFromFile : IObject
        {
            I_String theCurrentString;

            public AppendFromFile(I_String str) 
            {
                theCurrentString = str;
            }

            public override IObject MethodOperator(IObject[] strParams)
            {
                if (strParams.Length < 1)
                    return new I_Error("Must have arguments. Ex: (String pathToFile, Otional Int32 loadOption = {0: Save Linebreakes (default), 1: Erase Linebreaks})");

                if (strParams[0].IType == IObjectType.I_String)
                {
                    string filepath = ((I_String)strParams[0]).VALUE;

                    try
                    {
                        StreamReader fileToRead = new StreamReader(filepath);
                        StringBuilder sb = new StringBuilder();
                        string line = fileToRead.ReadLine();
                        int lineOption = 0;

                        if (strParams.Length > 1)
                            if (strParams[1].IType == IObjectType.I_Int)
                                lineOption = (I_Int)strParams[1];

                        while (line != null)
                        {
                            sb.Append(line);
                            line = fileToRead.ReadLine();
                            if (line != null && lineOption == 0)
                            {
                                sb.Append("<br/>");
                            }
                        }
                        fileToRead.Close();
                        theCurrentString.Append(sb.ToString());
                        return theCurrentString;
                    }
                    catch
                    {
                        return new I_Error("File dose not excist");
                    }
                }

                return new I_Error("Unexpected error in fileload.");
            }

            public override int GetAutoCompleteIconIndex() { return 4; }
            public override string GetAutoCompleteToolTip(string str) { return str + "(String filePath, Int32 loadOption) returns content of file as string. First argument is filePath as String. Second argument (optional) loadOption as Int32. If loadOption = 0 (default) linebreaks will be showed. If loadOption = 1 no Linebreaks will be shown."; }
            public override string GetAutoCompleteText(string str) { return str; }
            public override string GetAutoCompleteListText(string str) { return str + "()"; }
        }
    }
}
