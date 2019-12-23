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
    [FlagsAttribute]
    public enum NSWindowStyleMasks
    {
        NSBorderlessWindowMask = 0,
        NSTitledWindowMask = 1,
        NSClosableWindowMask = 2,
        NSMiniaturizableWindowMask = 4,
        NSResizableWindowMask = 8,
        NSTexturedBackgroundWindowMask = 16,
    }

	public enum NSWindowOrderingMode : int
	{
		NSWindowAbove         =  1,
		NSWindowBelow         = -1,
		NSWindowOut           =  0
	}

    //https://github.com/gnustep/gnustep-gui/blob/master/Source/NSWindow.m
    public class NSWindow : NSResponder
    {
        new public static Class Class = new Class(typeof(NSWindow));

        protected NSRect        _frame;
        protected NSSize        _minimumSize;
        protected NSSize        _maximumSize;
        protected NSSize        _increments;
        protected NSString	_autosaveName;
        //protected GSWindowDecorationView *_wv;
        protected id            _contentView;
        protected NSResponder _firstResponder;
        protected NSResponder _futureFirstResponder;
        protected NSView        _initialFirstResponder;


        public virtual NSResponder FirstResponder 
        {
            get { return GetFirstResponder(); }
        }


        public virtual NSResponder GetFirstResponder()
        {
            return _firstResponder;
        }
        
    }
}
