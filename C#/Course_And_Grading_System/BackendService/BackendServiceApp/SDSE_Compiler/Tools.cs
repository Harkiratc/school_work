using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;
using System.Drawing;

namespace Compiler
{
    class Tools
    {
        private Tools()
        {
        }

        static public Bitmap GetBitmapFromEmbeddedResource(string path)
        {
            Assembly myAssembly = Assembly.GetExecutingAssembly();
            Stream myStream = myAssembly.GetManifestResourceStream(path);
            return new Bitmap(myStream);
        }

        /*
         * Tar en sträng och splitar den efter ','-tecken på root-djupet av paranteser. 
         */
        static public string[] SplitArgs(string str)
        {
            List<string> args = new List<string>();
            char[] chars = str.ToCharArray();
            StringBuilder tmp = new StringBuilder();

            int parDepth = 0;
            int brackDepth = 0;
            int i = 0;

            for (; i < chars.Length; ++i)
            {
                if (chars[i] != ' ')
                    break;
            }

            for (; i < chars.Length; ++i)
            {
                if (chars[i] == ',')
                {
                    if (parDepth == 0 && brackDepth == 0)
                    {
                        for (; i < chars.Length - 1; ++i)
                        {
                            if (chars[i + 1] != ' ')
                                break;
                        }
                        args.Add(tmp.ToString());
                        tmp = new StringBuilder();
                    }
                    else
                    {
                        tmp.Append(chars[i]);
                    }
                }
                else
                {
                    tmp.Append(chars[i]);

                    if (chars[i] == '(')
                    {
                        ++parDepth;
                    }
                    else if (chars[i] == ')')
                    {
                        --parDepth;
                    }
                    else if (chars[i] == '{')
                    {
                        ++brackDepth;
                    }
                    else if (chars[i] == '}')
                    {
                        --brackDepth;
                    }
                }
            }
            args.Add(tmp.ToString());
            return args.ToArray();
        }

        /*
        static public void AppendAutoListFromDictionary(ref string str, ref List<ListViewItemTag> retStrs, ref Dictionary<string, IObject> dc)
        {
            string thisLevel = "";
            string restOfComand = "";

            int firstComandSep = str.IndexOf('.');

            if (firstComandSep < 0)
                thisLevel = str;
            else
            {
                thisLevel = str.Substring(0, firstComandSep);

                int restStartIndex = firstComandSep + 1;
                int restLengthIndex = str.Length - restStartIndex;
                if(restLengthIndex > 0)
                    restOfComand = str.Substring(restStartIndex, restLengthIndex);
            }

            bool hasMore = str.Contains('.');

            foreach (KeyValuePair<String, IObject> it in dc)
            {
                if (it.Key.Length >= thisLevel.Length)
                {
                    if (it.Key.Substring(0, thisLevel.Length) == thisLevel)
                    {
                        if (thisLevel == it.Key)
                        {
                            if (hasMore)
                            {
                                ListViewItemTag[] membersItms = it.Value.GetAutoCompleat(restOfComand);

                                for (int i = 0; i < membersItms.Length; ++i)
                                {
                                    retStrs.Add(membersItms[i]);
                                }
                                break;
                            }
                        }
                        else
                        {
                            int iconIndex = it.Value.GetAutoCompleteIconIndex();
                            string tooltip = it.Value.GetAutoCompleteToolTip(it.Key);
                            string listText = it.Value.GetAutoCompleteListText(it.Key);
                            string inportText = it.Value.GetAutoCompleteText(it.Key);
                            retStrs.Add(new ListViewItemTag(iconIndex, tooltip, listText, inportText));
                        }
                    }
                }
            }
        }
         * */

        /*
        static public void AppendAutoListFromDictionary(ref string str, ref List<ListViewItemTag> retStrs, ref Dictionary<string, IObject_Constructor> dc)
        {
            string[] dotSeparated = str.Split(".".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            bool hasMore = str.Contains('.');

            if (dotSeparated.Length < 1)
                dotSeparated = new String[1] { str };

            foreach (KeyValuePair<String, IObject_Constructor> it in dc)
            {
                if (it.Key.Length >= dotSeparated[0].Length)
                {
                    if (it.Key.Substring(0, dotSeparated[0].Length) == dotSeparated[0])
                    {
                        if (dotSeparated[0] == it.Key)
                        {
                            if (hasMore)
                            {
                                
                                string restMatch = "";

                                for (int i = 1; i < dotSeparated.Length; ++i)
                                {
                                    restMatch += dotSeparated[i];
                                }

                                ListViewItemTag[] membersItms = it.Value.GetAutoCompleat(restMatch);

                                for (int i = 0; i < membersItms.Length; ++i)
                                {
                                    retStrs.Add(membersItms[i]);
                                }
                                
                                break;
                            }
                        }
                        else
                        {
                            int iconIndex = it.Value.GetAutoCompleteIconIndex();
                            string tooltip = it.Value.GetAutoCompleteToolTip(it.Key);
                            string listText = it.Value.GetAutoCompleteListText(it.Key);
                            string inportText = it.Value.GetAutoCompleteText(it.Key);

                            retStrs.Add(new ListViewItemTag(iconIndex, tooltip, listText, inportText));
                        }
                    }
                }
            }
        }
        */
    }
}
