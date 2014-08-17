using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compiler
{
    public enum IObjectType
    {
        I_Null,
        I_Int,
        I_Matrix,
        I_String,
        I_Float,
        I_Error,
        I_Regex,
        I_2D,
        I_Number,
        I_Matrix_Float64,
        I_World3D,
        I_Object3D,
        I_Formula,
        I_Expression
    };

    class I_Types
    {
        private Dictionary<IObjectType, IObject_Constructor> constructors;

        public I_Types(SDSE_Compiler compiler, ref Dictionary<string, IObject_Constructor> types)
        {
            constructors = new Dictionary<IObjectType, IObject_Constructor>
            {
                {IObjectType.I_Int, new I_Int_Constructor()},
                {IObjectType.I_String, new I_String_Constructor()},
                {IObjectType.I_Float, new I_Float_Constructor()},
            };

            types.Add("String", GetConstructor(IObjectType.I_String));
            types.Add("Float", GetConstructor(IObjectType.I_Float));
            types.Add("Int", GetConstructor(IObjectType.I_Int));
        }

        public IObject_Constructor GetConstructor(IObjectType objType)
        {
            return constructors[objType];
        }
    }
}
