﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Smartmobili.Cocoa
{
    public class NSUnarchiver : NSCoder
    {
        new public static Class Class = new Class(typeof(NSUnarchiver));
        new public static NSUnarchiver alloc() { return new NSUnarchiver(); }



        public static id unarchiveObjectWithData(NSData data)
        {
            return null;
        }

    }
}
