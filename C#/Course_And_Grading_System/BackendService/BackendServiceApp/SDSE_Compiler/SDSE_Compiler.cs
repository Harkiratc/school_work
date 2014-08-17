using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Drawing;

namespace Compiler
{
    class SDSE_Compiler
    {
        private string lastInterpretateStr = "";
        
        // Regex
        private Regex commentParsing = new Regex("^ *(.*) *$");
        private Regex posibleCommandParsing = new Regex("^([^;]+)(;?)");

        private Regex SimpleMethodCall = new Regex("^ ?([a-zA-Z]+[a-zA-Z0-9_]*)\\((.*)\\) ?$");
        private Regex SimpleVariable = new Regex("^ ?([a-zA-Z]+[a-zA-Z0-9_]*) ?$");
        private Regex SimpleInteger = new Regex("^ ?(-?[0-9]+) ?$");
        private Regex SimpleFloat = new Regex("^ ?(-?[0-9]+\\.[0-9]+) ?$");
        private Regex SimpleEqual = new Regex("^ ?([a-zA-Z]+[a-zA-Z0-9]*) ?= ?(.*)");
        private Regex SimpleEqualBracket = new Regex("^ ?([a-zA-Z]+[a-zA-Z0-9]*) ?= ?$");
        private Regex SimpleDotSeparator = new Regex("^ ?([^.+\\-\\*\\/\\(\\)\\0-9]+[^.+\\-\\*\\/\\(\\)]*)\\.(.*)$");
        private Regex SimpleDeclaration = new Regex("^ ?([a-zA-Z]+[a-zA-Z0-9<>_]*) ([a-zA-Z]+[a-zA-Z0-9]*)( ?= ?(\"?.*\"?))?;?$");
        private Regex SimpleString = new Regex("^ ?\"(.*)\" ?$");
        private Regex SimpleContainsParentheses = new Regex("^.*\\(.*\\).*");
        private Regex SimplePowerMathExpression = new Regex("^ ?(.*) ?\\^ ?(.*)$");
        private Regex SimpleMultiMathExpression = new Regex("^ ?(.*) ?\\* ?(.*)$");
        private Regex SimpleMathExpression = new Regex("^(.*) ?([\\^\\*\\+\\-]) ?([\\- ].*)$");

        private OperandTree operandTree;

        // Variable Dictionarys
        private Dictionary<string, IObject_Constructor> types;

        private Dictionary<string, IObject> userdefinedIObjects;
        private Dictionary<string, IObject> preDefinedIObjects;

        private IBracketHandler variableBracket;

        private I_Types iTypes;

        //!private string resultDocument = "";
        private List<Image> result2DImage = new List<Image>();

        private SDSE_Diagnostics diagnstics;

        public SDSE_Compiler()
        {
            variableBracket = new IBracketHandler(this);
            types = new Dictionary<string, IObject_Constructor>();
            iTypes = new I_Types(this, ref types);

            userdefinedIObjects = new Dictionary<string, IObject>();
            preDefinedIObjects = new Dictionary<string, IObject>();

            operandTree = new OperandTree(ParsWithScope);
            diagnstics = new SDSE_Diagnostics();
        }

        public void AddIObjectWithMembers(string name, IObject iobj)
        {
            if (preDefinedIObjects.ContainsKey(name))
                preDefinedIObjects.Remove(name);

            preDefinedIObjects.Add(name, iobj);
        }

        public IObject GetVarAsIObject(string name)
        {
            if (userdefinedIObjects.ContainsKey(name))
                return GetUserDefinedIObject(name);
            else if (preDefinedIObjects.ContainsKey(name))
                return GetUserDefinedIObject(name);

            return new I_Error("Var dose not exist");
        }

        public void SetVarAsIObjectIfExist(string name, IObject obj)
        {
            if (userdefinedIObjects.ContainsKey(name))
                userdefinedIObjects[name] = obj;
        }

        public float GetVarAsFloat(string name)
        {
            if (userdefinedIObjects.ContainsKey(name))
            {
                switch (GetUserDefinedIObject(name).IType)
                {
                    case IObjectType.I_Int:
                        return ((I_Int)GetUserDefinedIObject(name)).VALUE;
                    case IObjectType.I_Float:
                        return (float)((I_Float)GetUserDefinedIObject(name)).VALUE;
                }
            }
            else if (preDefinedIObjects.ContainsKey(name))
            {
                switch (preDefinedIObjects[name].IType)
                {
                    case IObjectType.I_Int:
                        return ((I_Int)preDefinedIObjects[name]).VALUE;
                    case IObjectType.I_Float:
                        return (float)((I_Float)preDefinedIObjects[name]).VALUE;
                }
            }
            return 0;
        }

        public IObject AddOrUpdateUserDefinedIObject(string name, IObject getValue)
        {
            if (userdefinedIObjects.ContainsKey(name))
                userdefinedIObjects[name] = getValue;
            else
                userdefinedIObjects.Add(name, getValue);

            return getValue;
        }

        public void RemoveUserDefinedIObjectIfExsist(string name)
        {
            if (userdefinedIObjects.ContainsKey(name))
                userdefinedIObjects.Remove(name);
        }

        public IObject GetUserDefinedIObject(string name)
        {
            if (userdefinedIObjects.ContainsKey(name))
                return userdefinedIObjects[name];
            return null;
        }

        public IObject GetIObjectFromScope(Dictionary<string, IObject> scope, string name)
        {
            if (scope.ContainsKey(name))
                return scope[name];
            return null;
        }
        
        public bool DoseUserDefinedExist(string name)
        {
            return userdefinedIObjects.ContainsKey(name);
        }

        public void ReInterpretate()
        {
            // Lägg till så detta inte körs för ofta.
            Interpretate(lastInterpretateStr);
        }

        public void Interpretate(string comandStr)
        {
            lastInterpretateStr = comandStr;

            foreach (KeyValuePair<string, IObject> it in preDefinedIObjects)
                it.Value.Reset();

            userdefinedIObjects.Clear();
            variableBracket.Clear();

            comandStr = comandStr.Replace('\t', ' ');
            string[] comands = comandStr.Split(Environment.NewLine.ToCharArray());

            diagnstics.Start();

            for (int i = 0; i < comands.Length - 1; ++i)
            {
                if (commentParsing.IsMatch(comands[i]))
                {
                    string comand = commentParsing.Match(comands[i]).Groups[1].Value;

                    if (variableBracket.OPEN)
                    {
                        variableBracket.Insert(comand);
                    }
                    else
                    {
                        if (posibleCommandParsing.IsMatch(comand))
                        {
                            string comandToPars = posibleCommandParsing.Match(comand).Groups[1].Value;
                            ParsString(preDefinedIObjects, comandToPars);
                        }
                    }
                }
                diagnstics.AddNewTime();
            }

            diagnstics.Stop();
        }

        public SDSE_Diagnostics_Result GetDiagnosticResult()
        {
            return diagnstics.GetResult();
        }

        public IObject ParsWithRootScope(string str)
        {
            return ParsString(preDefinedIObjects, str);
        }

        public IObject ParsWithScope(Dictionary<string, IObject> scope, string str)
        {
            return ParsString(scope, str);
        }

        private IObject ParsString(Dictionary<string, IObject> scope, string str)
        {
            if (str == null || str == "" || str == " ")
                return new IObject();
            
            if (SimpleMethodCall.IsMatch(str))
            {
                string methodName = SimpleMethodCall.Match(str).Groups[1].Value;
                if (scope.ContainsKey(methodName))
                {
                    string methodArgsStr = SimpleMethodCall.Match(str).Groups[2].Value;
                    string[] inParams = Tools.SplitArgs(methodArgsStr);
                    List<IObject> inIObjectParams = new List<IObject>();

                    for (int i = 0; i < inParams.Length; ++i)
                    {
                        IObject obj = ParsString(scope, inParams[i]);
                        if (!(obj.IType == IObjectType.I_Error || obj.IType == IObjectType.I_Null))
                            inIObjectParams.Add(obj);
                    }

                    if (scope.ContainsKey(methodName))
                    {
                        IObject methodObj = GetIObjectFromScope(scope, methodName);
                        IObject ret = methodObj.MethodOperator(inIObjectParams.ToArray());
                        return ret;
                    }
                }
            }
            else if(SimpleString.IsMatch(str))
            {
                IObject ret = new I_String(SimpleString.Match(str).Groups[1].Value);
                return ret;
            }
            else if (SimpleVariable.IsMatch(str))
            {
                string variableName = SimpleVariable.Match(str).Groups[1].Value;

                if (scope.ContainsKey(variableName))
                {
                    IObject ret = GetIObjectFromScope(scope, variableName);
                    return ret;
                }

                if (userdefinedIObjects.ContainsKey(variableName))
                {
                    IObject ret = userdefinedIObjects[variableName];
                    return ret;
                }
            }
            else if (SimpleInteger.IsMatch(str))
            {
                string integerValueStr = SimpleInteger.Match(str).Groups[1].Value;
                return new I_Int(integerValueStr);
            }
            else if (SimpleFloat.IsMatch(str))
            {
                string floatValueStr = SimpleFloat.Match(str).Groups[1].Value;
                floatValueStr = floatValueStr.Replace('.', ',');

                if (floatValueStr.Length < 10)
                {
                    IObject ret = new I_Float(Double.Parse(floatValueStr));
                    return ret;
                }
                else
                {
                    try
                    {
                        IObject ret = new I_Float(double.Parse(floatValueStr));
                        return ret;
                    }
                    catch
                    {
                        IObject ret = new I_Error("Float to big. Must be in range (10^k where |k| < 308).");
                        return ret;
                    }
                }
                    
            }
            else if (SimpleDeclaration.IsMatch(str))
            {
                string type = SimpleDeclaration.Match(str).Groups[1].Value;
                string name = SimpleDeclaration.Match(str).Groups[2].Value;
                string operand = SimpleDeclaration.Match(str).Groups[3].Value;
                string value = SimpleDeclaration.Match(str).Groups[4].Value;

                if (!types.ContainsKey(type))
                    return new I_Error("Type not recognized.");

                IObject_Constructor ioc = types[type];
                
                if (value == "")
                {
                    if (operand == "")
                    {
                        IObject newInstanse = ioc.GetNewInstans();
                        IObject ret = AddOrUpdateUserDefinedIObject(name, newInstanse);
                        return ret;
                    }
                    else
                    {
                        variableBracket.Name = name;
                        variableBracket.IType = ioc.GetObjectType();

                        return new IObject();
                    }
                }
                else
                {
                    IObject getValue = ParsString(scope, value);
                    IObject[] objInParams = { getValue };
                    IObject newValue = ioc.GetNewInstans(objInParams);

                    if (newValue.IType == IObjectType.I_Error)
                    {
                        IObject ret = newValue;
                        return ret;
                    }
                    else
                    {
                        IObject ret = AddOrUpdateUserDefinedIObject(name, newValue);
                        return ret;
                    }
                }
            }
            else if (SimpleEqual.IsMatch(str))
            {
                string variableName = SimpleEqual.Match(str).Groups[1].Value;
                string variableValueStr = SimpleEqual.Match(str).Groups[2].Value;

                if (userdefinedIObjects.ContainsKey(variableName))
                {
                    // The variable exists, need to do a new version of it.
                    IObject compiledValue = ParsString(scope, variableValueStr);
                    IObject previousVersionValue = GetUserDefinedIObject(variableName);
                    IObject newVersionCopy;

                    switch (compiledValue.IType)
                    {
                        case IObjectType.I_Null:
                            newVersionCopy = new IObject();
                            break;
                        case IObjectType.I_Error:
                            newVersionCopy = new I_Error(((I_Error)compiledValue));
                            break;
                        default:
                            newVersionCopy = iTypes.GetConstructor(compiledValue.IType).GetNewInstans(compiledValue);
                            break;
                    }

                    AddOrUpdateUserDefinedIObject(variableName, newVersionCopy);
                    newVersionCopy.EqualOperator(compiledValue);
                    return newVersionCopy;
                }
                else
                {
                    IObject compiledValue = ParsString(scope, variableValueStr);
                    AddOrUpdateUserDefinedIObject(variableName, compiledValue);
                    return compiledValue;
                }
            }
            else if (SimpleEqualBracket.IsMatch(str))
            {
                string variableName = SimpleEqualBracket.Match(str).Groups[1].Value;
                variableBracket.Name = variableName;

                return new IObject();
            }
            else if (SimpleDotSeparator.IsMatch(str))
            {
                string memberName = SimpleDotSeparator.Match(str).Groups[1].Value;
                string rest = SimpleDotSeparator.Match(str).Groups[2].Value;

                Dictionary<string, IObject> newScope = ParsString(scope, memberName).GetMembers();

                if (newScope.Count == 0)
                {
                    newScope = ParsString(preDefinedIObjects, memberName).GetMembers();
                }

                if (newScope.Count == 0 && types.ContainsKey(memberName))
                {
                    newScope = types[memberName].GetMembers();
                }

                IObject ret = ParsString(newScope, rest);
                return ret;
            }

            if (operandTree.ContainOperands(str))
            {
                return operandTree.ParseOperands(str, scope);
            }
            
            if (SimpleContainsParentheses.IsMatch(str))
            {
                IObject ret = ParsParentheses(scope, str);
                if (ret.IType != IObjectType.I_Null)
                {
                    return ret;
                }
            }

            IObject finalReturn = new I_Error("Command not understood (" + str + ")");
            return finalReturn;
        }

        private IObject ParseMultiOperators(Dictionary<string, IObject> scope, char searchOperand, string str)
        {
            char[] chars = str.ToCharArray();
            
            int searchOperandIndex = -1;
            int searchOperandDepth = 0;
            for (int i = 0; i < chars.Length; ++i)
            {
                char c = chars[i];

                if (c == '(')
                {
                    ++searchOperandDepth;
                    continue;
                }
                else if (c == ')')
                {
                    --searchOperandDepth;
                    continue;
                }
                else if (searchOperandDepth == 0)
                {
                    if (c == searchOperand)
                    {
                        searchOperandIndex = i;
                        break;
                    }
                }
            }

            if (searchOperandIndex == -1)
                return new IObject(IObjectType.I_Null);

            bool leftOperandFound = false;
            int leftOperandIndex = -1;
            int leftEndIndex = 0;
            int leftOperandDepth = 0;

            for (int i = searchOperandIndex-1; i >= 0; --i)
            {
                char c = chars[i];

                if (c == '(')
                {
                    ++leftOperandDepth;
                    continue;
                }
                else if (c == ')')
                {
                    --leftOperandDepth;
                    continue;
                }
                else if (leftOperandDepth == 0)
                {
                    if (c == '^' || c == '*' || c == '/' || c == '+' || c == '-')
                    {
                        if (c == '-')
                        {
                            bool isJustNegativSignNotOperator = false;

                            for (int j = i - 1; j >= 0; --j)
                            {
                                char h = chars[j];
                                if ((h == '^' || h == '*' || h == '/' || h == '+' || h == '-' || h == '('))
                                {
                                    isJustNegativSignNotOperator = true;
                                    break;
                                }
                                else if(j == 0)
                                {
                                    isJustNegativSignNotOperator = false;
                                    break;
                                }
                            }
                            if(i == 0)
                                isJustNegativSignNotOperator = true;

                            if (isJustNegativSignNotOperator)
                                continue;
                        }

                        leftOperandFound = true;
                        leftEndIndex = i + 1;
                        leftOperandIndex = i;
                        break;
                    }
                }
            }

            bool rightOperandFound = false;
            int rightOperandIndex = -1;
            int rightEndIndex = chars.Length-1;
            int rightOperandDepth = 0;

            for (int i = searchOperandIndex+1; i < chars.Length; ++i)
            {
                char c = chars[i];

                if (c == '(')
                {
                    ++rightOperandDepth;
                    continue;
                }
                else if (c == ')')
                {
                    --rightOperandDepth;
                    continue;
                }
                else if (rightOperandDepth == 0)
                {
                    if (c == '^' || c == '*' || c == '/' || c == '+' || c == '-')
                    {
                        rightOperandFound = true;
                        rightEndIndex = i - 1;
                        rightOperandIndex = i;
                        break;
                    }
                }
            }

            int leftCommandLength = searchOperandIndex - leftEndIndex;
            string leftCommand = new string(chars, leftEndIndex, leftCommandLength);

            IObject leftResult = ParsString(scope, leftCommand);
            if (leftResult.IType == IObjectType.I_Null || leftResult.IType == IObjectType.I_Error)
                return new IObject(IObjectType.I_Null); //Problem


            int rightCommandLength = rightEndIndex - searchOperandIndex;
            string rightCommand = new string(chars, searchOperandIndex + 1, rightCommandLength);

            IObject rightResult = ParsString(scope, rightCommand);
            if (rightResult.IType == IObjectType.I_Null || rightResult.IType == IObjectType.I_Error)
                return new IObject(IObjectType.I_Null); //Problem


            IObject midleResult = DoOperand(leftResult, chars[searchOperandIndex], rightResult);
            if (midleResult.IType == IObjectType.I_Null || midleResult.IType == IObjectType.I_Error)
                return new IObject(IObjectType.I_Null); //Problem

            if(leftOperandFound)
            {
                string leftOfLeftCommand = new string(chars, 0, leftOperandIndex);

                IObject leftOfLeftResult = ParsString(scope, leftOfLeftCommand);
                if (!(leftOfLeftResult.IType == IObjectType.I_Null || leftOfLeftResult.IType == IObjectType.I_Error))
                {
                    // Finns
                    IObject leftPartResult = DoOperand(leftOfLeftResult, chars[leftOperandIndex], midleResult);
                    if (!(leftPartResult.IType == IObjectType.I_Null || leftPartResult.IType == IObjectType.I_Error))
                    {
                        if (rightOperandFound)
                        {
                            //Finns
                            int rightOfRightCommandLength = chars.Length - rightOperandIndex - 1;
                            string rightOfRightCommand = new string(chars, rightOperandIndex + 1, rightOfRightCommandLength);
                            IObject rightOfRightResult = ParsString(scope, rightOfRightCommand);

                            if (!(rightOfRightResult.IType == IObjectType.I_Null || rightOfRightResult.IType == IObjectType.I_Error))
                            {
                                //Finns
                                IObject result = DoOperand(leftPartResult, chars[rightOperandIndex], rightOfRightResult);
                                if (!(result.IType == IObjectType.I_Null || result.IType == IObjectType.I_Error))
                                {
                                    return result;
                                }
                                    
                            }
                            else
                                return new IObject(IObjectType.I_Null);
                        }
                        // Inte säker på att denna undre rad ska va där
                        return leftPartResult;
                    }
                }
                // Här har det varit problem förut....
                return new IObject(IObjectType.I_Null);
                //return leftPartResult;
            }
            else if(rightOperandFound)
            {
                int rightOfRightCommandLength = chars.Length - rightOperandIndex - 1;
                string rightOfRightCommand = new string(chars, rightOperandIndex + 1, rightOfRightCommandLength);
                IObject rightOfRightResult = ParsString(scope, rightOfRightCommand);
                if (!(rightOfRightResult.IType == IObjectType.I_Null || rightOfRightResult.IType == IObjectType.I_Error))
                {
                    //Finns
                    IObject rightPartResult = DoOperand(midleResult, chars[rightOperandIndex], rightOfRightResult);
                    if (!(rightPartResult.IType == IObjectType.I_Null || rightPartResult.IType == IObjectType.I_Error))
                    {
                        //Finns
                        return rightPartResult;
                    }
                }
                return new IObject(IObjectType.I_Null);
            }
            else
            {
                return midleResult;
            }
        }


        private IObject ParsParentheses(Dictionary<string, IObject> scope, string str)
        {
            char[] ca = str.ToCharArray();
            Int32 startP = -1;
            Int32 endP = -1;
            Int32 depth = -1;

            for (int i = 0; i < ca.Length; ++i)
            {
                if (ca[i] == '(')
                {
                    if(i > 0)
                        if (ca[i - 1] != ' ') // Borde va okej med +-*/^ inte bara blanksteg
                            return new IObject();

                    depth = 1;
                    startP = i;
                    break;
                }
            }

            for (int i = startP+1; i < ca.Length; ++i)
            {
                if (ca[i] == '(')
                    ++depth;
                
                else if (ca[i] == ')')
                    --depth;

                if (depth == 0)
                {
                    endP = i;
                    break;
                }
            }

            if(depth != 0)
                return new IObject();

            IObject middlePart = ParsString(scope, new string(ca, startP + 1, endP - startP - 1));
            
            int leftPrio = 0;
            int leftOperandIndex = -1;
            for (int i = startP-1; i > -1; --i)
            {
                if (OperandPrio(ca[i]) != 0)
                {
                    leftPrio = OperandPrio(ca[i]);
                    leftOperandIndex = i;
                    break;
                }
            }

            int rightPrio = 0;
            int rightOperandIndex = -1;
            for (int i = endP + 1; i < ca.Length; ++i)
            {
                if (OperandPrio(ca[i]) != 0)
                {
                    rightPrio = OperandPrio(ca[i]);
                    rightOperandIndex = i;
                    break;
                }
            }

            

            if (leftOperandIndex == -1 && rightOperandIndex == -1)
            {
                return middlePart;
            }

            IObject leftHand = new IObject();

            if (leftOperandIndex - 1 > 0)
                leftHand = ParsString(scope, new string(ca, 0, leftOperandIndex - 1));
            else if (startP - 1 > 0)
                leftHand = ParsString(scope, new string(ca, 0, startP - 1));


            IObject rightHand = new IObject();

            if (rightOperandIndex + 1 < ca.Length-1 && rightOperandIndex > endP)
                rightHand = ParsString(scope, new string(ca, rightOperandIndex + 1, ca.Length - 1 - rightOperandIndex));
            else if (endP + 1 < ca.Length-1)
                rightHand = ParsString(scope, new string(ca, endP + 1, ca.Length - 1 - endP));


            if (leftPrio < rightPrio && rightPrio > 0)
            {
                IObject firstResult = DoOperand(middlePart, new string(ca[rightOperandIndex], 1), rightHand);

                if (leftPrio > 0)
                    return DoOperand(leftHand, new string(ca[leftOperandIndex], 1), firstResult);

                return firstResult;
            }
            else if (leftPrio >= rightPrio && leftPrio > 0)
            {
                IObject firstResult = DoOperand(leftHand, new string(ca[leftOperandIndex], 1), middlePart);

                if (rightPrio > 0)
                    return DoOperand(firstResult, new string(ca[rightOperandIndex], 1), rightHand);

                return firstResult;
            }

            return middlePart;
        }

        private IObject DoOperand(IObject leftSide, string operand, IObject rightSide)
        {
            if (operand == "+")
                return leftSide.PlusOperator(rightSide);
            else if (operand == "-")
                return leftSide.MinusOperator(rightSide);
            else if (operand == "*")
                return leftSide.MultiplierOperator(rightSide);
            else if (operand == "/")
                return leftSide.DividerOperator(rightSide);
            else if (operand == "^")
                return leftSide.PowerOperator(rightSide);
            else
                return new IObject();
        }
        
        private IObject DoOperand(IObject leftSide, char operand, IObject rightSide)
        {
            if (operand == '+')
                return leftSide.PlusOperator(rightSide);
            else if (operand == '-')
                return leftSide.MinusOperator(rightSide);
            else if (operand == '*')
                return leftSide.MultiplierOperator(rightSide);
            else if (operand == '/')
                return leftSide.DividerOperator(rightSide);
            else if (operand == '^')
                return leftSide.PowerOperator(rightSide);
            else
                return new IObject();
        }

        private int OperandPrio(char c)
        {
            if (c == '+' || c == '-')
                return 1;
            else if (c == '*' || c == '/')
            {
                return 2;
            }
            else if (c == '^')
            {
                return 3;
            }
            return 0;
        }
    }
}
