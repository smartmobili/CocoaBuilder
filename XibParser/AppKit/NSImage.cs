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
    public enum NSImageScaling
    {
        NSImageScaleProportionallyDown = 0,
        NSImageScaleAxesIndependently,
        NSImageScaleNone,
        NSImageScaleProportionallyUpOrDown
    }

    public class NSImage : NSObject
    {
        new public static Class Class = new Class(typeof(NSImage));
        new public static NSImage alloc() { return new NSImage(); }

        public NSString Name { get; set; }

        public NSString ResourceName { get; set; }

       
        public NSImage()
        {
        }


        //public NSImage(NSObjectDecoder aDecoder)
        //    : base(aDecoder)
        //{
            
        //}

        public override id initWithCoder(NSCoder aDecoder)
        {
            base.initWithCoder(aDecoder);

            return this;
        }


        public static NSImage ImageNamed(NSString anImage)
        {
            return new NSImage();
        }
    }
}
