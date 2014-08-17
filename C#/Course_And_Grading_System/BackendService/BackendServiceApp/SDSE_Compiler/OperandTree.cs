using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compiler
{
    class OperandTree
    {
        

        public delegate IObject ParserMethodDelegate(Dictionary<string, IObject> scope, string str);

        private ParserMethodDelegate parserDelegate;

        public OperandTree(ParserMethodDelegate parser)
        {
            parserDelegate = parser;
        }

        public IObject ParseOperands(string str, Dictionary<string, IObject> scope)
        {
            List<char> tmpOperands = new List<char>();
            List<int> tmpOperandsIndex = new List<int>();
            List<string> tmpParts= new List<string>();
            List<IObject> tmpPartsResult = new List<IObject>();

            int parDepth = 0;
            char[] chars = str.ToCharArray();

            for (int i = 0; i < chars.Length; ++i)
            {
                switch (chars[i])
                {
                    case '(':
                        ++parDepth;
                        break;
                    case ')':
                        --parDepth;
                        break;
                    default:
                        if (parDepth == 0)
                        {
                            switch (chars[i])
                            {
                                case '+':
                                    tmpOperands.Add('+');
                                    tmpOperandsIndex.Add(i);
                                    break;
                                case '-':
                                    tmpOperands.Add('-');
                                    tmpOperandsIndex.Add(i);
                                    break;
                                case '*':
                                    tmpOperands.Add('*');
                                    tmpOperandsIndex.Add(i);
                                    break;
                                case '/':
                                    tmpOperands.Add('/');
                                    tmpOperandsIndex.Add(i);
                                    break;
                                case '^':
                                    tmpOperands.Add('^');
                                    tmpOperandsIndex.Add(i);
                                    break;
                                case '%':
                                    tmpOperands.Add('%');
                                    tmpOperandsIndex.Add(i);
                                    break;
                                case '!':
                                    tmpOperands.Add('!');
                                    tmpOperandsIndex.Add(i);
                                    break;  
                            }
                            
                        }
                        break;
                }
            }

            int lastOperandIndex = -1;
            for(int i = 0; i < tmpOperands.Count; ++i)
            {
                tmpParts.Add(str.Substring(lastOperandIndex + 1, tmpOperandsIndex[i] - lastOperandIndex - 1));
                lastOperandIndex = tmpOperandsIndex[i];
            }

            if(tmpOperands.Count > 0)
            {
                tmpParts.Add(str.Substring(tmpOperandsIndex[tmpOperandsIndex.Count - 1] + 1));
            }

            for(int i = 0; i < tmpParts.Count; ++i)
            {
                tmpPartsResult.Add(parserDelegate(scope, tmpParts[i]));
            }

            /*
            * The operand ! (Factorial)
            */
            for (int i = 0; i < tmpOperands.Count; ++i)
            {
                if (tmpOperands[i] != '!')
                    continue;

                tmpPartsResult[i] = tmpPartsResult[i].FactorialOperator(tmpPartsResult[i + 1]);
                tmpPartsResult.RemoveAt(i + 1);
                tmpOperands.RemoveAt(i);
                --i;
            }

            /*
             * The operand % (Modulo)
             */
            for (int i = 0; i < tmpOperands.Count; ++i)
            {
                if (tmpOperands[i] != '%')
                    continue;

                tmpPartsResult[i] = tmpPartsResult[i].ModuloOperator(tmpPartsResult[i + 1]);
                tmpPartsResult.RemoveAt(i + 1);
                tmpOperands.RemoveAt(i);
                --i;
            }

            /*
             * The operand ^ (Power To)
             */
            for(int i = 0; i < tmpOperands.Count; ++i)
            {
                if(tmpOperands[i] != '^')
                    continue;

                tmpPartsResult[i] = tmpPartsResult[i].PowerOperator(tmpPartsResult[i+1]);
                tmpPartsResult.RemoveAt(i+1);
                tmpOperands.RemoveAt(i);
                --i;
            }

            /*
             * The operand * and / (Multiply and Divide Whith)
             */
            for(int i = 0; i < tmpOperands.Count; ++i)
            {
                if (tmpOperands[i] == '*')
                {
                    tmpPartsResult[i] = tmpPartsResult[i].MultiplierOperator(tmpPartsResult[i + 1]);
                    tmpPartsResult.RemoveAt(i + 1);
                    tmpOperands.RemoveAt(i);
                    --i;
                }

                if (i > -1 && i < tmpOperands.Count)
                {
                    if (tmpOperands[i] == '/')
                    {

                        tmpPartsResult[i] = tmpPartsResult[i].DividerOperator(tmpPartsResult[i + 1]);
                        tmpPartsResult.RemoveAt(i + 1);
                        tmpOperands.RemoveAt(i);
                        --i;
                    }
                }
            }

            /*
             * The operand + and - (Pluss and Minus With)
             */
            for(int i = 0; i < tmpOperands.Count; ++i)
            {
                if (tmpOperands[i] == '+')
                {

                    tmpPartsResult[i] = tmpPartsResult[i].PlusOperator(tmpPartsResult[i + 1]);
                    tmpPartsResult.RemoveAt(i + 1);
                    tmpOperands.RemoveAt(i);
                    --i;
                }

                if (i > -1 && i < tmpOperands.Count)
                {
                    if (tmpOperands[i] == '-')
                    {

                        tmpPartsResult[i] = tmpPartsResult[i].MinusOperator(tmpPartsResult[i + 1]);
                        tmpPartsResult.RemoveAt(i + 1);
                        tmpOperands.RemoveAt(i);
                        --i;
                    }
                }
            }

            return tmpPartsResult[0];
        }

        public bool ContainOperands(string str)
        {
            int parDepth = 0;

            char[] chars = str.ToCharArray();

            for (int i = 0; i < chars.Length; ++i)
            {
                switch (chars[i])
                {
                    case '(':
                        ++parDepth;
                        break;
                    case ')':
                        --parDepth;
                        break;
                    default:
                        if (parDepth == 0)
                        {
                            switch (chars[i])
                            {
                                case '+':
                                    return true;
                                case '-':
                                    return true;
                                case '*':
                                    return true;
                                case '/':
                                    return true;
                                case '^':
                                    return true;
                                case '%':
                                    return true;
                                case '!':
                                    return true;
                            }
                        }
                        break;
                }
            }

            return false;
        }
    }
}
