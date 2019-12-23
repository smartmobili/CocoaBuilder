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
using AT.MIN;

namespace Smartmobili.Cocoa
{
    public class NSMutableString : NSString
    {
        new public static Class Class = new Class(typeof(NSMutableString));

        public NSMutableString()
        {

        }

        public NSMutableString(string value)
            : base(value)
        {
        }

		public virtual void appendString(NSString aString)
		{
			this.Value += aString;
		}

		public virtual void appendFormat(NSString format, params object[] args)
		{
			string msg = Tools.sprintf(format, args);
			this.Value += msg;
		}


        //public override id initWithCoder(NSCoder aDecoder)
        //{
        //    base.initWithCoder(aDecoder);

        //    foreach (var xElement in aDecoder.XmlElement.elements())
        //    {
        //        string key = xElement.Attribute("key").Value;
        //        switch (key)
        //        {

        //            case "NS.bytes": { Value = xElement.Value; break; }

        //            default:
        //                System.Diagnostics.Debug.WriteLine("NSMutableString : unknown key " + key);
        //                break;

        //        }
        //    }
        //    return this;
        //}

        // implicit NSMutableString to string conversion operator
        public static implicit operator string(NSMutableString nsString)  
        {
            System.Console.WriteLine("NSMutableString: implicit conversion to string occurred");
            return nsString.Value;  // implicit conversion
        }

        public static implicit operator NSPoint(NSMutableString nsString)
        {
            System.Console.WriteLine("NSMutableString: implicit conversion to NSPoint occurred");
            return NSPoint.Create(nsString.Value);
        }

        // implicit NSMutableString to string conversion operator
        public static implicit operator NSSize(NSMutableString nsString)
        {
            System.Console.WriteLine("NSMutableString: implicit conversion to NSSize occurred");
            return NSSize.Create(nsString.Value);
        }

        // implicit NSMutableString to string conversion operator
        public static implicit operator NSRect(NSMutableString nsString)
        {
            System.Console.WriteLine("NSMutableString: implicit conversion to NSRect occurred");
            return NSRect.Create(nsString.Value); 
        }
    }
}
