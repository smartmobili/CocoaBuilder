﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Smartmobili.Cocoa
{
    public class GSXibLoader : GSModelLoader
    {
        new public static Class Class = new Class(typeof(GSXibLoader));
        new public static GSXibLoader alloc() { return new GSXibLoader(); }

        new public static NSString Type
        {
            get { return @"xib"; }
        }

        new public static float Priority
        {
            get { return 4.0f; }
        }

        public virtual void Awake(NSArray rootObjects, IBObjectContainer objects, NSDictionary context)
        {
            NSEnumerator en;
            id obj;
            NSMutableArray topLevelObjects = (NSMutableArray)context.objectForKey((id)NS.NibTopLevelObjects);
            id owner = context.objectForKey(NS.NibOwner);
            id first = null;
            id app = null;

            // get the file's owner and NSApplication object references...
            if (((NSCustomObject)rootObjects.objectAtIndex(1)).ClassName.isEqualToString(@"FirstResponder"))
                first = ((NSCustomObject)rootObjects.objectAtIndex(1)).RealObject;
            else
                NS.Log(@"%s:first responder missing\n", "Awake");

            if (((NSCustomObject)rootObjects.objectAtIndex(2)).ClassName.isEqualToString(@"NSApplication"))
                app = ((NSCustomObject)rootObjects.objectAtIndex(2)).RealObject;
            else
                NS.Log(@"%s:NSApplication missing\n", "Awake");

            // Use the owner as first root object
            ((NSCustomObject)rootObjects.objectAtIndex(0)).SetRealObject(owner);
            en = rootObjects.objectEnumerator();
            while ((obj = en.nextObject()) != null)
            {
                if (obj.respondsToSelector(new SEL("NibInstantiate")))
                {
                    obj = (id)Objc.MsgSend(obj, "NibInstantiate", null);
                }

                // IGNORE file's owner, first responder and NSApplication instances...
                if ((obj != null) && (obj != owner) && (obj != first) && (obj != app))
                {
                    topLevelObjects.addObject(obj);
                    // All top level objects must be released by the caller to avoid
                    // leaking, unless they are going to be released by other nib
                    // objects on behalf of the owner.
                    //RETAIN(obj);
                }

                //FIXME
                //if ((obj.isKindOfClass(NSMenu.Class)) &&
                //    (((NSMenu)obj _isMainMenu]))
                //  {
                //    // add the menu...
                //    NSApp._setMainMenu(obj);
                //  }
            }

            // Load connections and awaken objects
            Objc.MsgSend(objects, "NibInstantiate", null);

        }

        public override bool LoadModelData(NSData data, NSDictionary context)
        {
            bool loaded = false;
            NSKeyedUnarchiver unarchiver = null;

            try
            {
                if (data != null)
                {
                    unarchiver = GSXibKeyedUnarchiver.alloc().initForReadingWithData(data);
                    if (unarchiver != null)
                    {
                        NSArray rootObjects;
                        IBObjectContainer objects;

                        //NSDebugLLog(@"XIB", @"Invoking unarchiver");
                        // unarchiver setObjectZone: zone];
                        rootObjects = (NSArray)unarchiver.decodeObjectForKey(@"IBDocument.RootObjects");
                        objects = (IBObjectContainer)unarchiver.decodeObjectForKey(@"IBDocument.Objects");
                        //NSDebugLLog(@"XIB", @"rootObjects %@", rootObjects);
                        //[self awake: rootObjects inContainer: objects withContext: context];
                        this.Awake(rootObjects, objects, context);
                        loaded = true;
                        unarchiver = null;
                    }
                    else
                    {
                        NS.Log(@"Could not instantiate Xib unarchiver.");
                    }
                }
                else
                {
                    NS.Log(@"data passed to Xib loading method is nil.");
                }
            }
            catch (Exception ex)
            {
                NS.Log(@"Exception occured while loading model: %s",ex.Message);
            }

            if (loaded == false)
            {
                NS.Log(@"Failed to load Xib\n");
            }

            return loaded;
        }

        public override NSData DataForFile(NSString fileName)
        {
            NSFileManager mgr = NSFileManager.DefaultManager;
            bool isDir = false;

            //NSDebugLLog(@"XIB", @"Loading Xib `%@'...\n", fileName);
            if (mgr.fileExistsAtPath(fileName, ref isDir))
            {
                if (isDir == false)
                {
                    return NSData.dataWithContentsOfFile(fileName);
                }
                else
                {
                    NS.Log(@"Xib file specified %@, is directory.", fileName);
                }
            }
            else
            {
                NS.Log(@"Xib file specified %@, could not be found.", fileName);
            }
            return null;
        }

    }
}
