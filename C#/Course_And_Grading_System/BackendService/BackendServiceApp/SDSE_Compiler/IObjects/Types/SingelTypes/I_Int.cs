using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Compiler
{
    class I_Int : IObjectWithMembers
    {
        private BigInteger bigValue;

        public I_Int(Int64 v)
        {
            bigValue = new BigInteger(v);
            iType = IObjectType.I_Int;
            InitMembers();
        }

        public I_Int(Int32 v)
        {
            bigValue = new BigInteger(v);
            iType = IObjectType.I_Int;
            InitMembers();
        }

        public I_Int(BigInteger v)
        {
            iType = IObjectType.I_Int;

            bigValue = new BigInteger(v);
            InitMembers();
        }

        public I_Int(String v)
        {
            iType = IObjectType.I_Int;

            bigValue = new BigInteger(v, 10);
            InitMembers();
        }

        public I_Int(I_Int v)
        {
            iType = IObjectType.I_Int;

            bigValue = new BigInteger(v.bigValue);
            InitMembers();
        }

        private void InitMembers()
        {
            AddMember("ModInverse", new ModInverse(this));
            AddMember("ModPower", new ModPower(this));
        }

        public Int64 VALUE
        {
            get
            {
                return bigValue.LongValue();
            }
        }

        public BigInteger BIG_VALUE
        {
            get
            {
                return bigValue;
            }
        }

        public override IObject EqualOperator(IObject iobj)
        {
            switch (iobj.IType)
            {
                case IObjectType.I_Float:
                    bigValue = new BigInteger((long)((I_Float)iobj).VALUE);
                    return this;
                case IObjectType.I_Int:
                    bigValue = new BigInteger(((I_Int)iobj).bigValue);
                    return this;
            }

            return this;
        }

        public override bool EqualEqualOperator(IObject iobj)
        {
            switch (iobj.IType)
            {
                case IObjectType.I_Float:
                    if (((I_Float)iobj).VALUE == this.VALUE)
                        return true;
                    break;
                case IObjectType.I_Int:
                    return bigValue.Equals(((I_Int)iobj).bigValue);
            }

            return false;
        }

        // ERROR if bigInteger > 64 bit and rightside == float
        public override bool LessOperator(IObject rightSide)
        {
            switch (rightSide.IType)
            {
                case IObjectType.I_Float:
                    return VALUE < ((I_Float)rightSide).VALUE;
                case IObjectType.I_Int:
                    return bigValue < ((I_Int)rightSide).bigValue;
            }

            return false;
        }

        public override bool LargerOperator(IObject rightSide)
        {
            switch (rightSide.IType)
            {
                case IObjectType.I_Float:
                    return VALUE > ((I_Float)rightSide).VALUE;
                case IObjectType.I_Int:
                    return bigValue > ((I_Int)rightSide).bigValue;
            }
            return false;
        }

        public override IObject PlusOperator(IObject rightSide)
        {
            switch (rightSide.IType)
            {
                case IObjectType.I_Float:
                    return new I_Float(VALUE + ((I_Float)rightSide).VALUE);
                case IObjectType.I_Int:
                    return new I_Int(bigValue + ((I_Int)rightSide).bigValue);
                default:
                    return new I_Int(bigValue);
            }
        }

        public override IObject MinusOperator(IObject rightSide)
        {
            switch (rightSide.IType)
            {
                case IObjectType.I_Float:
                    return new I_Float(VALUE - ((I_Float)rightSide).VALUE);
                case IObjectType.I_Int:
                    return new I_Int(bigValue - ((I_Int)rightSide).bigValue);
                default:
                    return new I_Int(bigValue);
            }
        }

        public override IObject MultiplierOperator(IObject rightSide)
        {
            switch (rightSide.IType)
            {
                case IObjectType.I_Float:
                    return new I_Float(VALUE * ((I_Float)rightSide).VALUE);
                case IObjectType.I_Int:
                    return new I_Int(bigValue * ((I_Int)rightSide).bigValue);
                default:
                    return new I_Int(bigValue);
            }
        }

        public override IObject DividerOperator(IObject rightSide)
        {
            try
            {
                switch (rightSide.IType)
                {
                    case IObjectType.I_Float:
                        return new I_Float(VALUE / ((I_Float)rightSide).VALUE);
                    case IObjectType.I_Int:
                        return new I_Int(bigValue / ((I_Int)rightSide).bigValue);
                    default:
                        return new I_Int(bigValue);
                }
            }
            catch { }
            return new I_Int(bigValue);
        }

        public override IObject PowerOperator(IObject rightSide)
        {
            switch (rightSide.IType)
            {
                case IObjectType.I_Float:
                    double value = ((I_Float)rightSide).VALUE;
                    if(value == 0.5f)
                        return new I_Int(bigValue.sqrt());
                    return new I_Float(Math.Pow(VALUE, value));
                case IObjectType.I_Int:
                    try
                    {
                        BigInteger tmp = bigValue.Power((uint)((I_Int)rightSide));
                        return new I_Int(tmp);
                    }
                    catch
                    {
                        return new I_Error("Overflow");
                    }
                default:
                    return new I_Error("Calculation error");
            }
        }

        public override IObject ModuloOperator(IObject rightSide)
        {
            if (rightSide.IType == IObjectType.I_Int)
            {
                return new I_Int(BIG_VALUE % ((I_Int)rightSide).BIG_VALUE);
            }
            else
            {
                return new I_Error("Argument must be Int.");
            }
        }

        public override IObject FactorialOperator(IObject rightSide)
        {
            try
            {
                return new I_Int(BIG_VALUE.Factorial());
            }
            catch
            {
                return new I_Error("Overflow");
            }
        }


        public static implicit operator long(I_Int x)
        {
            return x.bigValue.LongValue();
        }

        public static implicit operator Int32(I_Int x)
        {
            return (Int32) x.bigValue.IntValue();
        }

        public static implicit operator float(I_Int x)
        {
            return (float) x.bigValue.LongValue();
        }

        public static implicit operator double(I_Int x)
        {
            return (double)x.bigValue.LongValue();
        }

        public override string ToString()
        {
            return bigValue.ToString();
        }

        public override string GetAutoCompleteToolTip(string str) { return "Int (" + VALUE + ")"; }
    }

    class ModInverse : IObject
    {
        private I_Int referens;

        public ModInverse(I_Int referens)
        {
            this.referens = referens;
        }

        public override IObject MethodOperator(IObject[] strParams)
        {
            if (strParams.Length != 1)
                return new I_Error("Number of arguments must be 1.");

            try
            {
                return new I_Int(referens.BIG_VALUE.modInverse(((I_Int)strParams[0]).BIG_VALUE));
            }
            catch
            {
                return new I_Error("No invers found!");
            }
        }

        public override string GetAutoCompleteToolTip(string str) { return "ModInverse " + str + "(Integer n) - Get invers using mod n."; }
        public override string GetAutoCompleteListText(string str) { return str + "()"; }
    }

    class ModPower : IObject
    {
        private I_Int referens;

        public ModPower(I_Int referens)
        {
            this.referens = referens;
        }

        public override IObject MethodOperator(IObject[] strParams)
        {
            if (strParams.Length != 2)
                return new I_Error("Number of arguments must be 2. x.ModPower(Int exp, Int n) -> (x ^ exp) % n");

            try
            {
                return new I_Int(referens.BIG_VALUE.modPow(((I_Int)strParams[0]).BIG_VALUE, ((I_Int)strParams[1]).BIG_VALUE));
            }
            catch
            {
                return new I_Error("No invers found!");
            }
        }

        public override string GetAutoCompleteToolTip(string str) { return "ModPower " + str + "(Integer exp, Integer n) - x.ModPower(Int exp, Int n) -> (x ^ exp) % n"; }
        public override string GetAutoCompleteListText(string str) { return str + "()"; }
    }
}
