using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Compiler
{
    class I_Number : IObject
    {
        private double value;
        
        public I_Number(double v)
        {
            value = v;
            iType = IObjectType.I_Number;
        }

        public I_Number(I_Float v)
        {
            value = v;
            iType = IObjectType.I_Number;
        }

        public I_Number(I_Int v)
        {
            value = (double) v;
            iType = IObjectType.I_Number;
        }

        public I_Number(I_Number v)
        {
            value = v.value;
            iType = IObjectType.I_Number;
        }

        public double VALUE
        {
            get
            {
                return value;
            }
        }

        public static double ToDouble(IObject obj)
        {
            switch (obj.IType)
            {
                case IObjectType.I_Float:
                    return (double)((I_Float)obj).VALUE;
                case IObjectType.I_Int:
                    return (double)((I_Int)obj).VALUE;
                case IObjectType.I_Number:
                    return (double)((I_Number)obj).VALUE;
                default:
                    return 0;
            }
        }

        public static float ToFloat(IObject obj)
        {
            switch (obj.IType)
            {
                case IObjectType.I_Float:
                    return (float)((I_Float)obj).VALUE;
                case IObjectType.I_Int:
                    return (float)((I_Int)obj).VALUE;
                case IObjectType.I_Number:
                    return (float)((I_Number)obj).VALUE;
                default:
                    return 0;
            }
        }

        public static long ToLong(IObject obj)
        {
            switch (obj.IType)
            {
                case IObjectType.I_Float:
                    return (long)((I_Float)obj).VALUE;
                case IObjectType.I_Int:
                    return (long)((I_Int)obj).VALUE;
                case IObjectType.I_Number:
                    return (long)((I_Number)obj).VALUE;
                default:
                    return 0;
            }
        }

        public static int ToInt(IObject obj)
        {
            switch (obj.IType)
            {
                case IObjectType.I_Float:
                    return (int)((I_Float)obj).VALUE;
                case IObjectType.I_Int:
                    return (int)((I_Int)obj).VALUE;
                case IObjectType.I_Number:
                    return (int)((I_Number)obj).VALUE;
                default:
                    return 0;
            }
        }

        /*
        public override IObject EqualOperator(IObject iobj)
        {
            switch (iobj.IType)
            {
                case IObject.I_Float32:
                    value = ((I_Float32)iobj).VALUE;
                    return this;
                case IObject.I_Float64:
                    value = (float)((I_Float64)iobj).VALUE;
                    return this;
                case IObject.I_Int32:
                    value = ((I_Int32)iobj).VALUE;
                    return this;
                case IObject.I_Int64:
                    value = ((I_Int64)iobj).VALUE;
                    return this;
            }
            value = 0;
            return this;
        }
         

        public override bool EqualEqualOperator(IObject iobj)
        {
            switch (iobj.IType)
            {
                case IObject.I_Float64:
                    if (((I_Float64)iobj).VALUE == this.VALUE)
                        return true;
                    break;
                case IObject.I_Int32:
                    if (((I_Int32)iobj).VALUE == this.VALUE)
                        return true;
                    break;
                case IObject.I_Int64:
                    if (((I_Int64)iobj).VALUE == this.VALUE)
                        return true;
                    break;
                case IObject.I_Float32:
                    if (((I_Float32)iobj).VALUE == this.VALUE)
                        return true;
                    break;
            }

            return false;
        }

        public override bool LessOperator(IObject rightSide)
        {
            try
            {
                switch (rightSide.IType)
                {
                    case IObject.I_Float64:
                        return value < ((I_Float64)rightSide).VALUE;
                    case IObject.I_Float32:
                        return value < ((I_Float32)rightSide).VALUE;
                    case IObject.I_Int32:
                        return value < ((I_Int32)rightSide).VALUE;
                    case IObject.I_Int64:
                        return value < ((I_Int64)rightSide).VALUE;
                }
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
                switch (rightSide.IType)
                {
                    case IObject.I_Float64:
                        return value > ((I_Float64)rightSide).VALUE;
                    case IObject.I_Float32:
                        return value > ((I_Float32)rightSide).VALUE;
                    case IObject.I_Int32:
                        return value > ((I_Int32)rightSide).VALUE;
                    case IObject.I_Int64:
                        return value > ((I_Int64)rightSide).VALUE;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public override IObject PlusOperator(IObject rightSide)
        {
            switch (rightSide.IType)
            {
                case IObject.I_Float64:
                    return new I_Float64(value + ((I_Int64)rightSide).VALUE);
                case IObject.I_Int32:
                    return new I_Float64(value + ((I_Int32)rightSide).VALUE);
                case IObject.I_Int64:
                    return new I_Float64(value + ((I_Int64)rightSide).VALUE);
                case IObject.I_Float32:
                    return new I_Float64(value + ((I_Float32)rightSide).VALUE);
                default:
                    return new I_Float64(value);
            }
        }

        public override IObject MinusOperator(IObject rightSide)
        {
            switch (rightSide.IType)
            {
                case IObject.I_Float64:
                    return new I_Float64(value - ((I_Float64)rightSide).VALUE);
                case IObject.I_Int32:
                    return new I_Float64(value - ((I_Int32)rightSide).VALUE);
                case IObject.I_Int64:
                    return new I_Float64(value - ((I_Int64)rightSide).VALUE);
                case IObject.I_Float32:
                    return new I_Float64(value - ((I_Float32)rightSide).VALUE);
                default:
                    return new I_Float64(value);
            }
        }

        public override IObject MultiplierOperator(IObject rightSide)
        {
            switch (rightSide.IType)
            {
                case IObject.I_Float64:
                    return new I_Float64(value * ((I_Float64)rightSide).VALUE);
                case IObject.I_Int32:
                    return new I_Float64(value * ((I_Int32)rightSide).VALUE);
                case IObject.I_Int64:
                    return new I_Float64(value * ((I_Int64)rightSide).VALUE);
                case IObject.I_Float32:
                    return new I_Float64(value * ((I_Float32)rightSide).VALUE);
                default:
                    return new I_Float64(value);
            }
        }

        public override IObject DividerOperator(IObject rightSide)
        {
            try
            {
                switch (rightSide.IType)
                {
                    case IObject.I_Float64:
                        return new I_Float64(value / ((I_Float64)rightSide).VALUE);
                    case IObject.I_Int32:
                        return new I_Float64(value / ((I_Int32)rightSide).VALUE);
                    case IObject.I_Int64:
                        return new I_Float64(value / ((I_Int64)rightSide).VALUE);
                    case IObject.I_Float32:
                        return new I_Float64(value / ((I_Float32)rightSide).VALUE);
                    default:
                        return new I_Float64(value);
                }
            }
            catch { }
            return new I_Float64(value);
        }

        public override IObject PowerOperator(IObject rightSide)
        {
            switch (rightSide.IType)
            {
                case IObject.I_Float64:
                    return new I_Float64(Math.Pow(value, ((I_Float64)rightSide).VALUE));
                case IObject.I_Int32:
                    return new I_Float64(Math.Pow(value, ((I_Int32)rightSide).VALUE));
                case IObject.I_Int64:
                    return new I_Float64(Math.Pow(value, ((I_Int64)rightSide).VALUE));
                case IObject.I_Float32:
                    return new I_Float64(Math.Pow(value, ((I_Float32)rightSide).VALUE));
                default:
                    return new I_Float64(value);
            }
        }
        * */

        public static implicit operator double(I_Number x)
        {
            return x.value;
        }

        public override string ToString()
        {
            return value.ToString().Replace(',', '.');
        }

        public override string GetAutoCompleteToolTip(string str) { return "Number (" + value + ")"; }
    }
}
