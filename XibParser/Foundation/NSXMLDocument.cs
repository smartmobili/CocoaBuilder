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
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;


namespace Smartmobili.Cocoa
{
    public class NSXMLDocument : NSXMLNode
    {
        new public static Class Class = new Class(typeof(NSXMLDocument));
        new public static NSXMLDocument alloc() { return new NSXMLDocument(); }

        protected NSString _encoding;

        protected NSString _version;

        protected NSXMLDTD _docType;

        protected NSArray _children;

        protected bool _childrenHaveMutated;

        protected bool _standalone;

        protected NSXMLElement _rootElement;

        protected NSString _URI;

        protected id _extraIvars;

        protected uint _fidelityMask;

        protected uint _contentKind;

       
       public virtual void setRootElement(NSXMLElement rootElement)
       {
           _rootElement = rootElement;
       }

       public virtual NSXMLElement rootElement()
       {
           return _rootElement;
       }

       public override id init()
       {
           id self = null;

           if(base.init() != null)
           {
               self = this;
               this._kind = NSXMLNodeKind.NSXMLDocumentKind;
               this._fidelityMask = 0x0;
               this._contentKind = 0x0;
           }

           return self;
       }

        public virtual id initWithRootElement(NSXMLElement rootElement)
        {
            id self = this;

            if (base.init() != null)
            {
                this.setRootElement(rootElement);
            }

            return self;
        }

        public virtual id initWithData(NSData data, uint mask, ref NSError error)
        {
            id self = this;

            return self;
        }

        public override uint childCount()
        {
            return this._children.count();
        }

    }
}
