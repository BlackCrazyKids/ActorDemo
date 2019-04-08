using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Flux
{
    public class FEnumField : PropertyAttribute
    {
        public Type EnumType;
        public FEnumField(Type enumType)
        {
            EnumType = enumType;
        }
    }
}