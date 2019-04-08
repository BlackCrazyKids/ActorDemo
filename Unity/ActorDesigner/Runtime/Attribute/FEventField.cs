using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Flux
{
    public class FEventField : PropertyAttribute
    {
        public string Name;
        public string Tip;

        public FEventField(string name, string tip)
        {
            Name = name;
            Tip = tip;
        }
        public FEventField(string name)
        {
            Name = name;
            Tip = "";
        }
    }
}



