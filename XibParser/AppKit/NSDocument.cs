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
    public class NSDocument : NSObject
    {
        new public static Class Class = new Class(typeof(NSDocument));
        new public static NSDocument Alloc() { return new NSDocument(); }


        public virtual bool ReadFromURL(NSURL absoluteURL, NSString typeName, ref NSError outError)
        {
            NSFileWrapper fileWrapper = (NSFileWrapper)NSFileWrapper.Alloc().InitWithURL(absoluteURL, 0, ref outError);
            return this.ReadFromFileWrapper(fileWrapper, typeName, ref outError);
        }


        public virtual bool ReadFromFileWrapper(NSFileWrapper fileWrapper, NSString typeName, ref NSError outError)
        {
            return this.ReadFromData(fileWrapper.RegularFileContents(), typeName, ref outError);
        }

        public virtual bool ReadFromData(NSData data, NSString typeName, ref NSError outError)
        {
            return false;
        }


    }
}
