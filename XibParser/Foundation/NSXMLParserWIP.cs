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
using Org.System.Xml.Sax;
using Org.System.Xml.Sax.Helpers;
using SaxConsts = Org.System.Xml.Sax.Constants;
using System.Runtime.InteropServices;
using xmlEntityPtr = System.IntPtr;



namespace Smartmobili.Cocoa
{
    public enum NSXMLParserFlags : uint
    {
        NSXMLParserFlagsNone = 0,
        NSXMLParserFlagsProcessNs = 4,
        NSXMLParserFlagsReportNsPrefixes = 8,
        NSXMLParserFlagsResolveExternalEntities = 16,
        NSXMLParserFlagsContinueAfterFatalError = 32
    }

    public class NSXMLParserInfo : NSObject
    {
        new public static Class Class = new Class(typeof(NSXMLParserInfo));
        new public static NSXMLParserInfo alloc() { return new NSXMLParserInfo(); }

        public LibXml.xmlSAXHandler saxHandler; //0x04
        public IntPtr parserContext; //0x08
        public uint parserFlags; // 0x0C
        public NSError error; //0x10
        public NSMutableArray namespaces; //0x14
        public NSMapTable/*NSMapTable*/ slowStringMap; //0x18
        public bool delegateAborted; //0x1C
        public bool haveDetectedEncoding; //0x1D
        public NSData bomChunk; //0x20
        public int chunkSize; //0x24
        public int nestingLevel;

 

    }

    public class NSXMLParserWIP : NSObject, IDisposable
    {
        new public static Class Class = new Class(typeof(NSXMLParserWIP));
        new public static NSXMLParserWIP alloc() { return new NSXMLParserWIP(); }

        private bool _disposed;

        
        private static volatile bool _isInited;
        private static volatile bool _isCleanedUp;
        private static object syncRoot = new Object();

        //Unamanaged resources (TO RELEASE)
        GCHandle _instanceHandle;
        IntPtr _instancePtr;
        IntPtr _saxHandlerPtr;

        IntPtr _xmlStructuredErrorFuncPtr; // Do not release

        List<GCHandle> _pinnedHandles = new List<GCHandle>();

        IntPtr _reserved0; //0x04
        id _delegate; //0x08
        NSXMLParserInfo _reserved1; //0x0C
        int _reserved2; // 0x10
        NSInputStream _reserved3; //0x14


        public NSXMLParserWIP()
        {
            _instanceHandle = GCHandle.Alloc(this);
            _instancePtr = (IntPtr)_instanceHandle;
        }
        public virtual id Delegate
        {
            get { return getDelegate(); }
            set { setDelegate(value); }
        }
        
        public virtual void setDelegate(id dlgate) 
        { 
            _delegate = dlgate; 
        }
        
        public virtual id getDelegate() 
        { 
            return _delegate; 
        }

        protected virtual NSXMLParserInfo info()
        {
            return this._reserved1;
        }

        public virtual bool shouldProcessNamespaces()
        {
            return parserFlagValue(NSXMLParserFlags.NSXMLParserFlagsProcessNs);
        }
        public virtual void setShouldProcessNamespaces(bool shouldProcessNamespaces)
        {
            setParserFlagValue(NSXMLParserFlags.NSXMLParserFlagsProcessNs, shouldProcessNamespaces);
        }

        public virtual bool shouldResolveExternalEntities()
        {
            return parserFlagValue(NSXMLParserFlags.NSXMLParserFlagsResolveExternalEntities);
        }
        public virtual void setShouldResolveExternalEntities(bool shouldResolveExternal)
        {
            setParserFlagValue(NSXMLParserFlags.NSXMLParserFlagsResolveExternalEntities, shouldResolveExternal);
        }

        public virtual bool shouldReportNamespacePrefixes()
        {
            return parserFlagValue(NSXMLParserFlags.NSXMLParserFlagsReportNsPrefixes);
        }
        public virtual void setShouldReportNamespacePrefixes(bool shouldReportNamespacePrefixes)
        {
            setParserFlagValue(NSXMLParserFlags.NSXMLParserFlagsReportNsPrefixes, shouldReportNamespacePrefixes);
        }

        IntPtr parserContext() 
        {
            return this._reserved1.parserContext;
        }


        private bool parserFlagValue(NSXMLParserFlags flag)
        {
            uint mask = (uint)flag;
            return ((this._reserved1.parserFlags & mask) != 0);
        }
        private void setParserFlagValue(NSXMLParserFlags flag, bool value)
        {
            uint mask = (uint)flag;

            if (value == true)
                this._reserved1.parserFlags |= mask;
            else
                this._reserved1.parserFlags &= ~mask;
        }

        public static void SetupLibXml()
        {
            if (_isInited == false)
            {
                lock (syncRoot)
                {
                    if (_isInited == false)
                    {
                        LibXml.xmlInitParser();
                        _isInited = true;
                    }
                }
            }
        }


        public virtual id initWithData(NSData data)
        {
            id self = this;

            SetupLibXml();

            if (base.init() != null)
            {
                _reserved1 = (NSXMLParserInfo)NSXMLParserInfo.alloc().init();
                
                _reserved1.saxHandler = new LibXml.xmlSAXHandler();
                _xmlStructuredErrorFuncPtr = Marshal.GetFunctionPointerForDelegate(new LibXml.xmlStructuredErrorFunc(_StructuredErrorFunc));

                _reserved1.parserContext = IntPtr.Zero;
                _reserved1.error = null;
                _reserved1.parserFlags = 0;
                _reserved1.slowStringMap = (NSMapTable)NSMapTable.alloc().init();

                initializeSAX2Callbacks();
                _reserved1.parserFlags |= 0x40;
                _reserved1.parserFlags |= 0x80;

                if (data != null)
                {
                    _reserved3 = NSInputStream.inputStreamWithData(data);
                    if (data.Length < 0x100000)
                        _reserved1.chunkSize = data.Length;
                    else
                        _reserved1.chunkSize = 0x100000 * 256;
                }
            }

            return self;
        }

         public virtual id initWithStream(NSInputStream aStream)
         {
             id self = this;

             NSInputStream stream = initForIncrementalParsing(aStream);
             if (stream != null)
             {
                 _reserved1.parserFlags |= 0x80;
                 _reserved3 = stream;
             }

             return self;
         }


         protected virtual NSInputStream initForIncrementalParsing(NSInputStream stream)
        {
            id self = this.initWithData(null);
            if (self != null)
            {
                _reserved1.parserFlags = (uint)_reserved1.parserFlags & 0xffffff7f;
            }
            return stream;
        }

        private void initializeSAX2Callbacks()
        {
            unsafe 
            { 
                _reserved1.saxHandler.internalSubset = Marshal.GetFunctionPointerForDelegate(new LibXml.internalSubsetSAXFunc(_internalSubset2));
                _reserved1.saxHandler.isStandalone = Marshal.GetFunctionPointerForDelegate(new LibXml.isStandaloneSAXFunc(_isStandalone));
                _reserved1.saxHandler.hasInternalSubset = Marshal.GetFunctionPointerForDelegate(new LibXml.hasInternalSubsetSAXFunc(_hasInternalSubset2));
                _reserved1.saxHandler.hasExternalSubset = Marshal.GetFunctionPointerForDelegate(new LibXml.hasExternalSubsetSAXFunc(_hasExternalSubset2));
                _reserved1.saxHandler.resolveEntity = IntPtr.Zero;//Marshal.GetFunctionPointerForDelegate(new LibXml.resolveEntitySAXFunc(_resolveEntity));
                _reserved1.saxHandler.getEntity = Marshal.GetFunctionPointerForDelegate(new LibXml.getEntitySAXFunc(_getEntity));
                _reserved1.saxHandler.entityDecl = Marshal.GetFunctionPointerForDelegate(new LibXml.entityDeclSAXFunc(_entityDecl));
                _reserved1.saxHandler.notationDecl = Marshal.GetFunctionPointerForDelegate(new LibXml.notationDeclSAXFunc(_notationDecl));
                _reserved1.saxHandler.attributeDecl = Marshal.GetFunctionPointerForDelegate(new LibXml.attributeDeclSAXFunc(_attributeDecl));
                _reserved1.saxHandler.elementDecl = Marshal.GetFunctionPointerForDelegate(new LibXml.elementDeclSAXFunc(_elementDecl));
                _reserved1.saxHandler.unparsedEntityDecl = Marshal.GetFunctionPointerForDelegate(new LibXml.unparsedEntityDeclSAXFunc(_unparsedEntityDecl));
                _reserved1.saxHandler.startDocument = Marshal.GetFunctionPointerForDelegate(new LibXml.startDocumentSAXFunc(_startDocument));
                _reserved1.saxHandler.endDocument = Marshal.GetFunctionPointerForDelegate(new LibXml.endDocumentSAXFunc(_endDocument));
                _reserved1.saxHandler.startElementNs = Marshal.GetFunctionPointerForDelegate(new LibXml.startElementNsSAX2Func(_startElementNs));
                _reserved1.saxHandler.endElementNs = Marshal.GetFunctionPointerForDelegate(new LibXml.endElementNsSAX2Func(_endElementNs));
                _reserved1.saxHandler.characters = Marshal.GetFunctionPointerForDelegate(new LibXml.charactersSAXFunc(_characters));
                _reserved1.saxHandler.error = Marshal.GetFunctionPointerForDelegate(new LibXml.errorSAXFunc(_errorCallback));
                _reserved1.saxHandler.getParameterEntity = IntPtr.Zero;
                _reserved1.saxHandler.cdataBlock = Marshal.GetFunctionPointerForDelegate(new LibXml.cdataBlockSAXFunc(_cdataBlock));
                _reserved1.saxHandler.comment = Marshal.GetFunctionPointerForDelegate(new LibXml.commentSAXFunc(_comment));
                _reserved1.saxHandler.externalSubset = Marshal.GetFunctionPointerForDelegate(new LibXml.externalSubsetSAXFunc(_externalSubset2));
                _reserved1.saxHandler.initialized = LibXml.XML_SAX2_MAGIC;

                int iSizeOfXmlSAXHandler = Marshal.SizeOf(typeof(LibXml.xmlSAXHandler));
                _saxHandlerPtr = Marshal.AllocHGlobal(iSizeOfXmlSAXHandler);
                Marshal.StructureToPtr(_reserved1.saxHandler, _saxHandlerPtr, false);
            }
        }

        private unsafe void _cdataBlock(IntPtr ctx, IntPtr value, int len)
        {
            id dlegate = getDelegate();
            //parser:foundCDATA:
            if (dlegate != null && dlegate.respondsToSelector(new SEL("parserFoundCDATA")) == true)
            {
                NSString cdata = (NSString)NSString.alloc().initWithBytes(value, (uint)len, NSStringEncoding.NSUTF8StringEncoding);
                Objc.MsgSend(dlegate, "parserFoundCDATA", this, cdata);
            }
            else
            {
                _characters(ctx, value, len);
            }
        }

        private unsafe void _externalSubset2(IntPtr ctx, IntPtr name, IntPtr ExternalID, IntPtr SystemID)
        {
            LibXml.xmlSAX2ExternalSubset(parserContext(), name, ExternalID, SystemID);
        }

        private unsafe void _comment(IntPtr ctx, IntPtr pValue)
        {
            id dlegate = getDelegate();
            //parser:foundComment:
            if (dlegate != null && dlegate.respondsToSelector(new SEL("parserFoundComment")) == true)
            {
                NSString chars = null;
                if (pValue != IntPtr.Zero)
                {
                    chars = _NSXMLParserNSStringFromBytes(pValue, this.info());
                }
                Objc.MsgSend(dlegate, "parserFoundComment", this, chars);
            }
        }

        private unsafe void _errorCallback(IntPtr ctx, IntPtr msg, params IntPtr[] prms)
        {
            throw new NotImplementedException();
        }

        private unsafe void _unparsedEntityDecl(IntPtr ctx, IntPtr pName, IntPtr pPublicId, IntPtr pSystemId, IntPtr pNotationName)
        {
            LibXml.xmlSAX2UnparsedEntityDecl(parserContext(), pName, pPublicId, pSystemId, pNotationName);

            id dlegate = getDelegate();
            //parser:foundUnparsedEntityDeclarationWithName:publicID:systemID:notationName:
            if (dlegate != null && dlegate.respondsToSelector(new SEL("parserFoundUnparsedEntityDeclarationWithName")) == true)
            {
                NSString name = null;
                if (pName != IntPtr.Zero)
                {
                    name = _NSXMLParserNSStringFromBytes(pName, this.info());
                }
                NSString publicId = null;
                if (pPublicId != IntPtr.Zero)
                {
                    publicId = _NSXMLParserNSStringFromBytes(pPublicId, this.info());
                }
                NSString systemId = null;
                if (pSystemId != IntPtr.Zero)
                {
                    systemId = _NSXMLParserNSStringFromBytes(pSystemId, this.info());
                }
                NSString notationName = null;
                if (pNotationName != IntPtr.Zero)
                {
                    notationName = _NSXMLParserNSStringFromBytes(pNotationName, this.info());
                }
                Objc.MsgSend(dlegate, "parserFoundUnparsedEntityDeclarationWithName", this, name, publicId, systemId, notationName);
            }
        }

        private unsafe void _elementDecl(IntPtr ctx, IntPtr pName, int type, IntPtr content)
        {
            id dlegate = getDelegate();
            if (dlegate != null && dlegate.respondsToSelector(new SEL("parserFoundElementDeclarationWithName")) == true)
            {
                NSString name = null;
                if (pName != IntPtr.Zero)
                {
                    name = _NSXMLParserNSStringFromBytes(pName, this.info());
                }
                Objc.MsgSend(dlegate, "parserFoundElementDeclarationWithName", this, name, "");
            }
        }

        private unsafe void _attributeDecl(IntPtr ctx, IntPtr pElem, IntPtr pFullname, int type, int def, IntPtr pDefaultValue, IntPtr tree)
        {
            id dlegate = getDelegate();
            //parser:foundAttributeDeclarationWithName:forElement:type:defaultValue:
            if (dlegate != null && dlegate.respondsToSelector(new SEL("parserFoundAttributeDeclarationWithName")) == true)
            {
                NSString elem = null;
                if (pElem != IntPtr.Zero)
                {
                    elem = _NSXMLParserNSStringFromBytes(pElem, this.info());
                }
                NSString fullname = null;
                if (pFullname != IntPtr.Zero)
                {
                    fullname = _NSXMLParserNSStringFromBytes(pFullname, this.info());
                }
                NSString defaultValue = null;
                if (pDefaultValue != IntPtr.Zero)
                {
                    defaultValue = _NSXMLParserNSStringFromBytes(pDefaultValue, this.info());
                }
                Objc.MsgSend(dlegate, "parserFoundAttributeDeclarationWithName", this, fullname, elem, type, defaultValue);

            }
            LibXml.xmlFreeEnumeration(tree);
        }

        private unsafe void _notationDecl(IntPtr ctx, IntPtr pName, IntPtr pPublicId, IntPtr pSystemId)
        {
             id dlegate = getDelegate();
             //parser:foundNotationDeclarationWithName:publicID:systemID:
             if (dlegate != null && dlegate.respondsToSelector(new SEL("parserFoundNotationDeclarationWithName")) == true)
             {
                 NSString name = null;
                 if (pName != IntPtr.Zero)
                 {
                     name = _NSXMLParserNSStringFromBytes(pName, this.info());
                 }
                 NSString publicId = null;
                 if (pPublicId != IntPtr.Zero)
                 {
                     publicId = _NSXMLParserNSStringFromBytes(pPublicId, this.info());
                 }
                 NSString systemId = null;
                 if (pSystemId != IntPtr.Zero)
                 {
                     systemId = _NSXMLParserNSStringFromBytes(pSystemId, this.info());
                 }
                 Objc.MsgSend(dlegate, "parserFoundNotationDeclarationWithName", this, name, publicId, systemId);
             }

        }

        private unsafe void _entityDecl(IntPtr ctx, IntPtr pName, int type, IntPtr pPublicId, IntPtr pSystemId, IntPtr pContent)
        {
            LibXml.xmlSAX2EntityDecl(parserContext(), pName, type, pPublicId, pSystemId, pContent);

            NSString content = null;
            if (pContent != IntPtr.Zero)
            {
                content = _NSXMLParserNSStringFromBytes(pContent, this.info());
            }
            NSString name = null;
            if (pName != IntPtr.Zero)
            {
                name = _NSXMLParserNSStringFromBytes(pName, this.info());
            }
            if (content.length() == 0)
            {
                if (shouldResolveExternalEntities())
                {
                    id dlegate = getDelegate();
                    //parser:foundExternalEntityDeclarationWithName:publicID:systemID:
                    if (dlegate != null && dlegate.respondsToSelector(new SEL("parserFoundExternalEntityDeclarationWithName")) == true)
                    {
                        NSString publicId = null;
                        if (pPublicId != IntPtr.Zero)
                        {
                            publicId = _NSXMLParserNSStringFromBytes(pPublicId, this.info());
                        }
                        NSString systemId = null;
                        if (pSystemId != IntPtr.Zero)
                        {
                            systemId = _NSXMLParserNSStringFromBytes(pSystemId, this.info());
                        }
                        Objc.MsgSend(dlegate, "parserFoundExternalEntityDeclarationWithName", this, name, publicId, systemId);
                    }
                    //parser:foundInternalEntityDeclarationWithName:value:
                    if (dlegate != null && dlegate.respondsToSelector(new SEL("parserFoundInternalEntityDeclarationWithName")) == true)
                    {
                        Objc.MsgSend(dlegate, "parserFoundInternalEntityDeclarationWithName", this, name, content);
                    }
                }
            }
            else
            {
                 id dlegate = getDelegate();
                 //parser:foundInternalEntityDeclarationWithName:value:
                 if (dlegate != null && dlegate.respondsToSelector(new SEL("parserFoundInternalEntityDeclarationWithName")) == true)
                 {
                     Objc.MsgSend(dlegate, "parserFoundInternalEntityDeclarationWithName", this, name, content);
                 }
            }

        }

        private unsafe IntPtr _getEntity(IntPtr ctx, IntPtr pName)
        {
            IntPtr parserCtx = this.parserContext();
            id dlegate = getDelegate();

            xmlEntityPtr pEntity = LibXml.xmlGetPredefinedEntity(pName);
            if (pEntity == IntPtr.Zero)
            {
                pEntity = LibXml.xmlSAX2GetEntity(parserCtx, pName);
                if (pEntity != IntPtr.Zero)
                {
                    //FIXME (x86)
                    //if (*(edi + 0xac) == 0x7)
                    //{
                    //    *(edi + 0x110) = 0x1;
                    //}

                    // (x64)
                    //if (*(int32_t*)(r15 + 0x110) == 0x7)
                    //{
                    //    *(r15 + 0x1a8) = 0x1;
                    //}
                }
                else
                {
                    
                    //parser:resolveExternalEntityName:systemID:
                    if (dlegate != null && dlegate.respondsToSelector(new SEL("parserResolveExternalEntityName")) == true)
                    {
                        NSString name = null;
                        if (pName != IntPtr.Zero)
                        {
                            name = _NSXMLParserNSStringFromBytes(pName, this.info());
                        }

                        NSData data = (NSData)Objc.MsgSend(dlegate, "parserResolveExternalEntityName", this, name, 0);
                        if (data != null && IntPtr.Add(parserCtx, 0x8) != IntPtr.Zero)
                        {
                            IntPtr pChars = ((NSString)(NSString.alloc().initWithData(data, NSStringEncoding.NSUTF8StringEncoding))).UTF8String();
                            if (pChars != IntPtr.Zero)
                            {
                                _characters(ctx, pChars, pChars.strlen());
                            }
                        }
                    }
                }

                
            }
           


            return pEntity;
        }

        
        private unsafe int _hasExternalSubset2(IntPtr ctx)
        {
            // This offset is calculated from a console application using libxml2
            // void * pExtSubset = &(parserCtx.myDoc->extSubset);
            //fprintf(f, "----->&(parserCtx.myDoc->extSubset) 0x%p (+0x%x)\n", pExtSubset, (uint32_t)pExtSubset-(uint32_t) & (parserCtx.myDoc->_private));
            
            IntPtr parserCtx = this.parserContext();
            IntPtr parserCtxMyDoc = IntPtr.Add(parserCtx, 0x8);
            IntPtr extSubset = IntPtr.Add(Marshal.ReadIntPtr(parserCtxMyDoc), 0x30);

            return (extSubset != IntPtr.Zero) ? 1 : 0; 
        }

        private unsafe int _hasInternalSubset2(IntPtr ctx)
        {
            IntPtr parserCtx = this.parserContext();
            IntPtr parserCtxMyDoc = IntPtr.Add(parserCtx, 0x8);
            IntPtr intSubset = IntPtr.Add(Marshal.ReadIntPtr(parserCtxMyDoc), 0x2C);

            return (intSubset != IntPtr.Zero) ? 1 : 0; 
        }

        private unsafe int _isStandalone(IntPtr ctx)
        {
            IntPtr parserCtx = this.parserContext();
            IntPtr parserCtxMyDoc = IntPtr.Add(parserCtx, 0x8);
            IntPtr standalone = IntPtr.Add(Marshal.ReadIntPtr(parserCtxMyDoc), 0x28);

            return (standalone != IntPtr.Zero) ? 1 : 0;
        }
        
        private unsafe void _internalSubset2(IntPtr ctx, IntPtr pName, IntPtr pExternalID, IntPtr pSystemID)
        {
            IntPtr parserCtx = this.parserContext();
            LibXml.xmlSAX2InternalSubset(parserCtx, pName, pExternalID, pSystemID);
        }

        public virtual bool parse()
        {
            return this.parseFromStream();
        }

        protected virtual bool parseFromStream()
        {
            bool result = true;

            NSInputStream stream = _reserved3;
            if (stream != null)
            {
                stream.open();
                byte[] buffer = new byte[_reserved1.chunkSize];
                int readBytes = stream.read(buffer, _reserved1.chunkSize);
                if (readBytes != -1)
                {
                    do
                    {
                        parseData(NSData.dataWithBytes(buffer));
                        readBytes = stream.read(buffer, _reserved1.chunkSize);
                    }
                    while (readBytes > 0);

                    result = finishIncrementalParse();
                }
                stream.close();
            }
            else
            {
                result = false;
                //*err = [NSError errorWithDomain:"NSCocoaErrorDomain" ,-1, userInfo:[NSDictionary dictionaryWithObjectsAndKeys:@"Could not open data stream",
                //sourcePath,destinationPath],NSLocalizedDescriptionKey,[outputStream streamError],NSUnderlyingErrorKey,nil]];
                //NSError.ErrorWithDomain("NSCocoaErrorDomain", -1, )
                //this._setExpandedParserError();
            }

            return result;
        }

        protected virtual bool parseData(NSData data)
        {
            bool result = false;

            if ((this._reserved1.parserFlags & 0x40) == 0)
            {

            }

            LibXml.xmlSetStructuredErrorFunc(_instancePtr, _xmlStructuredErrorFuncPtr);
            if (this._reserved1.haveDetectedEncoding == true)
            {
                result = this._handleParseResult(LibXml.xmlParseChunk(_reserved1.parserContext, data.Bytes, data.Length, 0));
            }
            else
            {
                int bomChunkLen = (_reserved1.bomChunk != null) ? _reserved1.bomChunk.Length : 0;
                int dataLen = (data != null) ? data.Length : 0;

                if (bomChunkLen + dataLen <= 3)
                {
                    NSData chunkData = data;
                    if (_reserved1.bomChunk != null)
                    {
                        chunkData = (NSMutableData)NSMutableData.alloc().init();
                        ((NSMutableData)chunkData).appendData(_reserved1.bomChunk);
                        ((NSMutableData)chunkData).appendData(data);
                        _reserved1.bomChunk = chunkData;
                    }
                   
                   
                    result = true;
                }
                else
                {
                    NSData chunkData = data;
                    if (_reserved1.bomChunk != null)
                    {
                        chunkData = (NSMutableData)NSMutableData.alloc().init();
                        ((NSMutableData)chunkData).appendData(_reserved1.bomChunk);
                        ((NSMutableData)chunkData).appendData(data);
                    }

                   
                    IntPtr saxHandlerPtr = (_delegate != null) ? _saxHandlerPtr : IntPtr.Zero;
                    _reserved1.parserContext = LibXml.xmlCreatePushParserCtxt(saxHandlerPtr, _instancePtr, chunkData.Bytes, 4, null);

                    bool shouldResolveExternals = shouldResolveExternalEntities();
                    int parserFlags = (shouldResolveExternals) ? (int)LibXml.XmlParserOption.XML_PARSE_DTDLOAD : 0;
                    LibXml.xmlCtxtUseOptions(_reserved1.parserContext, parserFlags);
                    _reserved1.haveDetectedEncoding = true;
                    _reserved1.bomChunk = null;

                    if (bomChunkLen + dataLen >= 5)
                    {
                        byte[] dst = new byte[data.Length - 4];
                        Buffer.BlockCopy(data.Bytes, 4, dst, 0, data.Length - 4);
                        NSData tmpData = (NSMutableData)NSMutableData.alloc().initWithBytes(dst);
                        parseData(tmpData);
                    }

                    result = true;
                }
            }

            LibXml.xmlSetStructuredErrorFunc(IntPtr.Zero, IntPtr.Zero);
            return result;
        }

        protected virtual void _StructuredErrorFunc(IntPtr userData, IntPtr error)
        {
            // FIXME
            System.Diagnostics.Debug.WriteLine("_StructuredErrorFunc");
        }

        protected virtual bool _handleParseResult(int xmlParserError)
        {
            // FIXME
            return true;
        }

        protected virtual bool finishIncrementalParse()
        {
            return this._handleParseResult(LibXml.xmlParseChunk(_reserved1.parserContext,null,0,1));
        }

        protected virtual NSError _setExpandedParserError(NSError error)
        {
            _reserved1.error = error;
            return _reserved1.error;
        }
       

        

        private unsafe void _characters(IntPtr ctx, IntPtr ch, int len)
        {
            id dlegate = getDelegate();
            if (dlegate == null || dlegate.respondsToSelector(new SEL("parserFoundCharacters")) == false)
                return;

            NSString chars = (NSString)NSString.alloc().initWithBytes(ch, (uint)len, NSStringEncoding.NSUTF8StringEncoding);
            Objc.MsgSend(this.getDelegate(), "parserFoundCharacters", this, chars);
        }

        private unsafe void _endElementNs(IntPtr ctx, IntPtr localname, IntPtr prefix, IntPtr URI)
        {
            // FIXME
            throw new NotImplementedException();
        }
        private unsafe void _startElementNs(IntPtr ctx, IntPtr pLocalname, IntPtr pPrefix, IntPtr URI, int nb_namespaces, string[] namespaces, int nb_attributes, int nb_defaulted, string[] attributes)
        {
            bool shouldProcessNs = this.shouldProcessNamespaces();
            bool shouldReportNssPrefixes = this.shouldReportNamespacePrefixes();
            int prefixLen = LibXml.xmlStrlen(pLocalname);

            NSString qName = null;
            if (prefixLen != 0)
            {
                qName = _NewColonSeparatedStringFromPrefixAndSuffix(pPrefix, pLocalname);
            }
            else
            {
                qName = _NSXMLParserNSStringFromBytes(pLocalname, this.info());
            }

            // FIXME
            //if (nb_attributes != 0)
            //{
            //    NSMutableDictionary attrs = (NSMutableDictionary)NSMutableDictionary.alloc().initWithCapacity((uint)nb_attributes);
            //    NSMutableDictionary nssPrefixes = null;
            //    if (shouldReportNssPrefixes)
            //        nssPrefixes = (NSMutableDictionary)NSMutableDictionary.alloc().initWithCapacity((uint)nb_namespaces);
                

            //}

        }

        private NSString _NewColonSeparatedStringFromPrefixAndSuffix(IntPtr pPrefix, IntPtr pLocalname)
        {
            NSString str = null;

            string tmpStr = string.Format("{0}:{1}", pPrefix.GetString(), pLocalname.GetString());
            str = (NSString)tmpStr;

            return str;
        }

        private string _NSXMLParserNSStringFromBytes(IntPtr pLocalname, NSXMLParserInfo info)
        {
            NSString str = (NSString)NS.MapGet(info.slowStringMap, pLocalname);
            if (str == null)
            {
                str = (NSString)NSString.allocWithZone(null).initWithBytes(pLocalname, (uint)pLocalname.strlen(), NSStringEncoding.NSUTF8StringEncoding);
                NS.MapInsertKnownAbsent(info.slowStringMap, pLocalname, str);
            }
           

            return str;
        }

        
        protected virtual void _startDocument(IntPtr ctx)
        {
             id dlegate = getDelegate();
             //parserDidStartDocument:
             if (dlegate != null && dlegate.respondsToSelector(new SEL("parserDidStartDocument")) == true)
            {
                Objc.MsgSend(dlegate, "parserDidStartDocument", ((GCHandle)ctx).Target as NSXMLParserWIP);
            }
        }

        protected virtual void _endDocument(IntPtr ctx)
        {
            id dlegate = getDelegate();
            //parserDidEndDocument:
            if (dlegate != null && dlegate.respondsToSelector(new SEL("parserDidEndDocument")) == true)
            {
                Objc.MsgSend(dlegate, "parserDidEndDocument", ((GCHandle)ctx).Target as NSXMLParserWIP);
            }
        }

        

        ~NSXMLParserWIP()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            //If you need thread safety, use a lock around these  
            //operations, as well as in your methods that use the resource. 
            if (!_disposed)
            {
                if (disposing)
                {
                    if (_isCleanedUp != true)
                    {
                        //LibXml.xmlCleanupParser();
                        //foreach(GCHandle handle in _pinnedHandles)
                        //{
                        //    handle.Free();
                        //}
                    }
                }

                _isCleanedUp = true;
                _disposed = true;
            }
        }


    }
}
