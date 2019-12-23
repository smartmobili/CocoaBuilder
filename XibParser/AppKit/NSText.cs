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
    //https://developer.apple.com/library/mac/#documentation/Cocoa/Reference/ApplicationKit/Classes/NSText_Class/Reference/Reference.html
    //https://github.com/gnustep/gnustep-gui/blob/master/Headers/AppKit/NSText.h

    public enum NSTextAlignment
    {
        NSLeftTextAlignment = 0,
        NSRightTextAlignment,
        NSCenterTextAlignment,
        NSJustifiedTextAlignment,
        NSNaturalTextAlignment
    }

    public enum NSWritingDirection
    {
        NSWritingDirectionNatural = -1,
        NSWritingDirectionLeftToRight = 0,
        NSWritingDirectionRightToLeft
    }

    public enum NSTextMovement
    {
        NSIllegalTextMovement = 0,
        NSReturnTextMovement = 0x10,
        NSTabTextMovement = 0x11,
        NSBacktabTextMovement = 0x12,
        NSLeftTextMovement = 0x13,
        NSRightTextMovement = 0x14,
        NSUpTextMovement = 0x15,
        NSDownTextMovement = 0x16
    }

    public enum NSTextCharacter
    {
        NSParagraphSeparatorCharacter = 0x2029,
        NSLineSeparatorCharacter = 0x2028,
        NSTabCharacter = 0x0009,
        NSFormFeedCharacter = 0x000c,
        NSNewlineCharacter = 0x000a,
        NSCarriageReturnCharacter = 0x000d,
        NSEnterCharacter = 0x0003,
        NSBackspaceCharacter = 0x0008,
        NSBackTabCharacter = 0x0019,
        NSDeleteCharacter = 0x007f,
    }



    public abstract class  NSText : NSView
    {
        public NSText()
        {

        }

        public virtual bool ImportsGraphics
        {
            get { return _ImportsGraphics(); }
            set { _SetImportsGraphics(value); }
        }

        public virtual bool Editable
        {
            get { return _IsEditable(); }
            set { _SetEditable(value); }
        }

        public virtual bool FieldEditor
        {
            get { return _IsFieldEditor(); }
            set { _SetFieldEditor(value); }
        }

        public virtual bool RichText
        {
            get { return _IsRichText(); }
            set { _SetRichText(value); }
        }

        public virtual bool Selectable
        {
            get { return _IsSelectable(); }
            set { _SetSelectable(value); }
        } 
        

        public virtual void ReplaceCharactersInRangeRTF(NSRange aRange, NSData rtfData)
        {
            NSDictionary dict = (NSDictionary)NSDictionary.alloc().init();
            NSAttributedString attr = (NSAttributedString)NSAttributedString.alloc().initWithRTF(rtfData, ref dict);
            this.ReplaceCharactersInRange(aRange, attr);
        }

        public virtual void ReplaceCharactersInRangeRTFD(NSRange aRange, NSData rtfData)
        {
            NSDictionary dict = (NSDictionary)NSDictionary.alloc().init();
            NSAttributedString attr = (NSAttributedString)NSAttributedString.alloc().initWithRTFD(rtfData, ref dict);
            this.ReplaceCharactersInRange(aRange, attr);
        }

        public virtual void ReplaceCharactersInRange(NSRange aRange, NSString aString)
        {}

        public virtual void ReplaceCharactersInRange(NSRange aRange, NSAttributedString attrString)
        {}

        public virtual NSData RTFDFromRange { get { return null; } }

        public virtual NSData RTFDromRange { get { return null; } }

        public virtual NSString String
        {
            get { return null; }
            set { this.ReplaceCharactersInRange(new NSRange(0, this.TextLength), value); }
        }

        public virtual void ReplaceRangeRTFD(NSRange aRange, NSData rtfdData)
        {
            this.ReplaceCharactersInRangeRTFD(aRange, rtfdData);
        }

        public virtual void ReplaceRangeRTF(NSRange aRange, NSData rtfData)
        {
            this.ReplaceCharactersInRangeRTF(aRange, rtfData);
        }

        public virtual void ReplaceRange(NSRange aRange, NSString aString)
        {
            this.ReplaceCharactersInRange(aRange, aString);
        }

        /*
         * Managing Global Characteristics
         */
        internal virtual bool _ImportsGraphics()
        {
            //[self subclassResponsibility: _cmd];
            return false;
        }

        internal virtual bool _IsEditable()
        {
            //[self subclassResponsibility: _cmd];
            return false; 
        }

        internal virtual bool _IsFieldEditor()
        {
            //[self subclassResponsibility: _cmd];
            return false;
        }

        internal virtual bool _IsRichText()
        {
            //[self subclassResponsibility: _cmd];
            return false;
        }

        internal virtual bool _IsSelectable()
        {
            //[self subclassResponsibility: _cmd];
            return false;
        }
        
        internal virtual void _SetEditable(bool flag)
        {

        }

        internal virtual void _SetFieldEditor(bool flag)
        {

        }

        internal virtual void _SetImportsGraphics(bool flag)
        {

        }

        internal virtual void _SetRichText(bool flag)
        {

        }

        internal virtual void _SetSelectable(bool flag)
        {

        }



        public virtual uint TextLength
        {
            get { return 0; }
        }

    }
}
