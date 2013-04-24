﻿/*
* XibParser.
* Copyright (C) 2013 Smartmobili SARL
* 
* This library is free software; you can redistribute it and/or
* modify it under the terms of the GNU Library General Public
* License as published by the Free Software Foundation; either
* version 2 of the License, or (at your option) any later version.
* 
* This library is distributed in the hope that it will be useful,
* but WITHOUT ANY WARRANTY; without even the implied warranty of
* MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
* Library General Public License for more details.
* 
* You should have received a copy of the GNU Library General Public
* License along with this library; if not, write to the
* Free Software Foundation, Inc., 51 Franklin St, Fifth Floor,
* Boston, MA  02110-1301, USA. 
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Smartmobili.Cocoa
{
    public class NSString : NSObject, INSNumber, IEquatable<NSString>
    {
        new public static Class Class = new Class(typeof(NSString));
        

        
        public string Value { get; set; }

        [ObjcPropAttribute("DoubleValue", SetName = null)]
        public double DoubleValue { get  { return Value.ToDouble(); } }

        [ObjcPropAttribute("FloatValue", SetName = null)]
        public float FloatValue { get { return Value.ToFloat(); } }
        
        [ObjcPropAttribute("IntValue", SetName = null)]
        public int IntValue { get { return Value.ToInt(); } }
        
        [ObjcPropAttribute("IntegerValue", SetName = null)]
        public int IntegerValue { get { return Value.ToInt(); } }


       

        public NSString()
        {

        }


        public NSString(string value) : this()
        {
            Value = value;
        }

        public override string ToString()
        {
            return Value;
        }

        public bool Contains(NSString aString)
        {
          return (Value != null) ? Value.Contains(aString) : false;
        }

        public NSString Replace(char oldChar, char newChar)
        {
            return (Value != null) ? Value.Replace(oldChar, newChar) : null;
        }

        public NSString Replace(NSString oldValue, NSString newValue)
        {
            return (Value != null) ? Value.Replace(oldValue, newValue) : null;
        }



        # region IEquatable

        public override bool Equals(object obj)
        {
            return Equals(obj as NSString);
        }

        public bool Equals(NSString obj)
        {
            return obj != null && obj.Value.Equals(this.Value);
        }

        #endregion //IEquatable

        public override int GetHashCode()
        {
            return (Value != null) ? Value.GetHashCode() : base.GetHashCode();
        }


        public static implicit operator NSString(string csString)
        {
            return (csString != null) ? new NSString(csString) : null;
        }

        // implicit NSString to string conversion operator
        public static implicit operator string(NSString nsString)  
        {
            if (nsString == null)
                return null;
#if TRACE_IMPLICIT_OPERATOR
            System.Console.WriteLine("NSString: implicit conversion to string occurred");
#endif
            return nsString.Value;  // implicit conversion
        }


        public static implicit operator NSPoint(NSString nsString)
        {
#if TRACE_IMPLICIT_OPERATOR
            System.Console.WriteLine("NSString: implicit conversion to NSPoint occurred");
#endif
            return NSPoint.Create(nsString.Value);
        }

        // implicit NSString to string conversion operator
        public static implicit operator NSSize(NSString nsString)
        {
#if TRACE_IMPLICIT_OPERATOR
            System.Console.WriteLine("NSString: implicit conversion to NSSize occurred");
#endif
            return NSSize.Create(nsString.Value);
        }

        // implicit NSString to string conversion operator
        public static implicit operator NSRect(NSString nsString)
        {
#if TRACE_IMPLICIT_OPERATOR
            System.Console.WriteLine("NSString: implicit conversion to NSRect occurred");
#endif
            return NSRect.Create(nsString.Value); 
        }

       

    }
}
