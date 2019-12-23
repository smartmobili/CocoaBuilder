﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Smartmobili.Cocoa
{
    public class NSXMLDTD : NSXMLNode
    {
        new public static Class Class = new Class(typeof(NSXMLDTD));
        new public static NSXMLDTD alloc() { return new NSXMLDTD(); }

        protected NSString _name;
        protected NSString _publicID;
        protected NSString _systemID;
        protected NSXMLChildren _children;
        protected bool _childrenHaveMutated;
        protected NSMutableDictionary _entities;
        protected NSMutableDictionary _elements;
        protected NSMutableDictionary _notations;
        protected NSMutableDictionary _attributes;
        protected NSString _original;
        protected bool _modified; //0x3C(x86) - 0x70(x64)
        

        private static NSDictionary _predefinedEntities;
        


        public static NSDictionary _initializePredefinedEntities()
        {
            NSXMLDTDNode gtNode = (NSXMLDTDNode)NSXMLDTDNode.alloc().initWithKind(NSXMLNodeKind.NSXMLEntityDeclarationKind);
            gtNode.setDTDKind(5); gtNode.setName("gt"); gtNode.setObjectValue((NSString)">");
            
            NSXMLDTDNode ltNode = (NSXMLDTDNode)NSXMLDTDNode.alloc().initWithKind(NSXMLNodeKind.NSXMLEntityDeclarationKind);
            ltNode.setDTDKind(5); ltNode.setName("lt"); ltNode.setObjectValue((NSString)"<");
            
            NSXMLDTDNode ampNode = (NSXMLDTDNode)NSXMLDTDNode.alloc().initWithKind(NSXMLNodeKind.NSXMLEntityDeclarationKind);
            ampNode.setDTDKind(5); ampNode.setName("amp"); ampNode.setObjectValue((NSString)"&");
            
            NSXMLDTDNode aposNode = (NSXMLDTDNode)NSXMLDTDNode.alloc().initWithKind(NSXMLNodeKind.NSXMLEntityDeclarationKind);
            aposNode.setDTDKind(5); aposNode.setName("apos"); aposNode.setObjectValue((NSString)"'");
            
            NSXMLDTDNode quotNode = (NSXMLDTDNode)NSXMLDTDNode.alloc().initWithKind(NSXMLNodeKind.NSXMLEntityDeclarationKind);
            quotNode.setDTDKind(5); quotNode.setName("quot"); quotNode.setObjectValue((NSString)"\\\"");

            _predefinedEntities = (NSDictionary)NSDictionary.alloc().initWithObjectsAndKeys(
                gtNode, (NSString)"gt",
                ltNode, (NSString)"lt",
                ampNode, (NSString)"amp",
                aposNode, (NSString)"apos",
                quotNode, (NSString)"quot",
                null);

            return _predefinedEntities;
        }

        public virtual NSString _DTDString()
        {
            if (this._modified == true)
            {
                return null;
            }
            else
            {
                return (NSString)_original.retain().autorelease();            
            }
        }
        public virtual void _setDTDString(NSString dtdString)
        {
            if (_original != dtdString)
            {
                _original.release();
                _original = (NSString)dtdString.retain();
                _modified = false;
            }
        }


        public virtual void _setModified(bool modified)
        {
            _modified = modified;
        }
        

        
        public virtual void setSystemID(NSString systemID)
        {
            if (_systemID != systemID)
            { 
                _systemID = systemID;
                if(_parent != null)
                {
                    this._setModified(true);
                }
            }
        }

        public virtual NSString systemID()
        {
            return _systemID;
        }

        public virtual void setPublicID(NSString publicID)
        {
            _publicID = publicID;
        }

        public virtual NSString publicID()
        {
            return _publicID;
        }

        public static NSXMLNode predefinedEntityDeclarationForName(NSString name)
        {
            NSXMLNode result;

            if (_predefinedEntities == null)
                NSXMLDTD._initializePredefinedEntities();
            result = (NSXMLNode)_predefinedEntities.objectForKey(name);

            return result;
        }

        public virtual NSXMLNode entityDeclarationForName(NSString name)
        {
            NSXMLNode result;

            result = (NSXMLNode)_entities.objectForKey(name);
            if (result == null)
            {
                if (_predefinedEntities == null)
                    NSXMLDTD._initializePredefinedEntities();
                result = (NSXMLNode)_predefinedEntities.objectForKey(name);
            }

            return result;
        }
    }
}
