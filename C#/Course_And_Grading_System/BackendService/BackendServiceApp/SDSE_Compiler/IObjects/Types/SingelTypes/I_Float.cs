using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Compiler
{
    class I_Float : IObject
    {
        private double value;
        
        public I_Float(double v)
        {
            value = v;
            iType = IObjectType.I_Float;
        }

        public double VALUE
        {
            get
            {
                return value;
            }
        }

        public override IObject EqualOperator(IObject iobj)
        {
            switch (iobj.IType)
            {
                case IObjectType.I_Float:
                    value = (float)((I_Float)iobj).VALUE;
                    return this;
                case IObjectType.I_Int:
                    value = ((I_Int)iobj).VALUE;
                    return this;
            }
            value = 0;
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
                    if (((I_Int)iobj).VALUE == this.VALUE)
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
                    case IObjectType.I_Float:
                        return value < ((I_Float)rightSide).VALUE;
                    case IObjectType.I_Int:
                        return value < ((I_Int)rightSide).VALUE;
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
                    case IObjectType.I_Float:
                        return value > ((I_Float)rightSide).VALUE;
                    case IObjectType.I_Int:
                        return value > ((I_Int)rightSide).VALUE;
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
                case IObjectType.I_Float:
                    return new I_Float(value + ((I_Float)rightSide).VALUE);
                case IObjectType.I_Int:
                    return new I_Float(value + ((I_Int)rightSide).VALUE);
                default:
                    return new I_Float(value);
            }
        }

        public override IObject MinusOperator(IObject rightSide)
        {
            switch (rightSide.IType)
            {
                case IObjectType.I_Float:
                    return new I_Float(value - ((I_Float)rightSide).VALUE);
                case IObjectType.I_Int:
                    return new I_Float(value - ((I_Int)rightSide).VALUE);
                default:
                    return new I_Float(value);
            }
        }

        public override IObject MultiplierOperator(IObject rightSide)
        {
            switch (rightSide.IType)
            {
                case IObjectType.I_Float:
                    return new I_Float(value * ((I_Float)rightSide).VALUE);
                case IObjectType.I_Int:
                    return new I_Float(value * ((I_Int)rightSide).VALUE);
                default:
                    return new I_Float(value);
            }
        }

        public override IObject DividerOperator(IObject rightSide)
        {
            try
            {
                switch (rightSide.IType)
                {
                    case IObjectType.I_Float:
                        return new I_Float(value / ((I_Float)rightSide).VALUE);
                    case IObjectType.I_Int:
                        return new I_Float(value / ((I_Int)rightSide).VALUE);
                    default:
                        return new I_Float(value);
                }
            }
            catch { }
            return new I_Float(value);
        }

        public override IObject PowerOperator(IObject rightSide)
        {
            switch (rightSide.IType)
            {
                case IObjectType.I_Float:
                    return new I_Float(Math.Pow(value, ((I_Float)rightSide).VALUE));
                case IObjectType.I_Int:
                    return new I_Float(Math.Pow(value, ((I_Int)rightSide).VALUE));
                default:
                    return new I_Float(value);
            }
        }

        public static implicit operator double(I_Float x)
        {
            return x.value;
        }

        public override string ToString()
        {
            return Math.Round(value, 6).ToString().Replace(',', '.');
            //return value.ToString().Replace(',', '.');
        }

        public override string GetAutoCompleteToolTip(string str) { return "Float64 (" + value + ")"; }
    }
}
