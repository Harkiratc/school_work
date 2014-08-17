using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Compiler
{
    class IObject
    {
        public static string[] PublicTypeName = {
                                                   "Null", 
                                                   "Int32", 
                                                   "Int64", 
                                                   "Float32", 
                                                   "Matrix", 
                                                   "String",  
                                                   "Float64", 
                                                   "Error",
                                                   "Regex",
                                                   "Image2D",
                                                   "Number",
                                                   "Matrix<Float64>"
                                               };

        protected IObjectType iType;

        public IObject() { iType = IObjectType.I_Null; }

        public IObject(IObjectType type) { iType = type; }

        public IObjectType IType { get { return iType; } }

        public virtual bool LessOperator(IObject rightSide) { return false; }
        public virtual bool LargerOperator(IObject rightSide) { return false; }
        public virtual IObject EqualOperator(IObject iobj) { return this; }
        public virtual bool EqualEqualOperator(IObject iobj) { return false; }

        public virtual IObject PlusOperator(IObject rightSide) { return this; }
        public virtual IObject MinusOperator(IObject rightSide) { return this; }
        public virtual IObject MultiplierOperator(IObject rightSide) { return this; }
        public virtual IObject DividerOperator(IObject rightSide) { return this; }
        public virtual IObject PowerOperator(IObject rightSide) { return this; }
        public virtual IObject ModuloOperator(IObject rightSide) { return this; }
        public virtual IObject FactorialOperator(IObject rightSide) { return this; }

        public virtual IObject MethodOperator(params IObject[] iobjs) { return this; }
        public virtual IObject BracketOperator(string str) { return this; }

        public virtual Dictionary<string, IObject> GetMembers() { return new Dictionary<string, IObject>(); }

        //protected virtual void ExtendedCall(string str) { }

        public virtual void Reset() {}

        public virtual bool UseLineBreakAfterCommand() { return true; }
        public new virtual string ToString() { return "Null"; }
        public virtual string ToHtmlString() { return ToString(); }

        public virtual int GetAutoCompleteIconIndex() { return 0; }
        public virtual string GetAutoCompleteToolTip(string str) { return ""; }
        public virtual string GetAutoCompleteText(string str) { return str; }
        public virtual string GetAutoCompleteListText(string str) { return str; }
    }
}
