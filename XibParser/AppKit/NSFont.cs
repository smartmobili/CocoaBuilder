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
    public class NSFont : NSObject
    {
        new public static Class Class = new Class(typeof(NSFont));

        public static double SmallSystemFontSize
        {
            get { return 8; }
        }


        public NSFont()
        {

        }

        new public static NSFont alloc()
        {
            return new NSFont();
        }


        public static NSFont FontWithNameSize(string aFontName, double aSize)
        {
            NSFont font = new NSFont();


            return font;
        }

        public static NSFont SystemFontOfSize(double aFontSize)
        {
            NSFont font = null;

            if (aFontSize > 0)
            {
                font = NSFont.FontWithNameSize("Helvetica", aFontSize);
            }
            else
            {
                // Default System size
                font = NSFont.FontWithNameSize("Helvetica", 12);
            }

            return font;
        }

        public override id initWithCoder(NSCoder aDecoder)
        {
            id self = this;

            if (aDecoder.AllowsKeyedCoding)
            {
                NSString name = (NSString)aDecoder.decodeObjectForKey(@"NSName");
                float size = aDecoder.decodeFloatForKey(@"NSSize");

                self = NSFont.FontWithNameSize(name, size);
                if (self == null)
                {

                }

            }

            return self;
        }

    }
}
