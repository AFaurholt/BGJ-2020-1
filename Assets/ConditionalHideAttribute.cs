using UnityEngine;
using System;
using System.Collections;

namespace com.editor.GameJamBois.BGJ20201.Attributes
{
    //ripped from http://www.brechtos.com/hiding-or-disabling-inspector-properties-using-propertydrawers-within-unity-5/
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property |
        AttributeTargets.Class | AttributeTargets.Struct, Inherited = true)]

    public class ConditionalHideAttribute : PropertyAttribute
    {
        //The name of the bool field that will be in control
        public string ConditionalSourceField = "";
        //TRUE = Hide in inspector / FALSE = Disable in inspector 
        public bool HideInInspector = false;
        //hide when true
        public bool FlipHidden = false;

        public ConditionalHideAttribute(string conditionalSourceField)
        {
            ConditionalSourceField = conditionalSourceField;
            HideInInspector = false;
        }

        public ConditionalHideAttribute(string conditionalSourceField, bool hideInInspector)
        {
            ConditionalSourceField = conditionalSourceField;
            HideInInspector = hideInInspector;
        }

        public ConditionalHideAttribute(string conditionalSourceField, bool hideInInspector, bool flipHidden)
        {
            ConditionalSourceField = conditionalSourceField;
            HideInInspector = hideInInspector;
            FlipHidden = flipHidden;
        }
    }
}