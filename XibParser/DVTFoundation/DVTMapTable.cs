﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smartmobili.Cocoa
{
    public class DVTMapTable : NSObject
    {
        new public static Class Class = new Class(typeof(DVTMapTable));
        new public static DVTMapTable alloc() { return new DVTMapTable(); }
    }

//    + (id)mapTableWithWeakToWeakObjects;
//+ (id)mapTableWithStrongToWeakObjects;
//+ (id)mapTableWithWeakToStrongObjects;
//+ (id)mapTableWithStrongToStrongObjects;
//+ (id)mapTableWithKeyOptions:(int)arg1 valueOptions:(int)arg2;
//+ (id)allocWithZone:(struct _NSZone *)arg1;
//+ (id)alloc;
//- (id)mutableCopyWithZone:(struct _NSZone *)arg1;
//- (id)copyWithZone:(struct _NSZone *)arg1;
//- (void)enumerateKeysAndObjectsUsingBlock:(id)arg1;
//- (id)dictionaryRepresentation;
//- (id)mutableDictionary;
//- (void)removeAllObjects;
//- (unsigned long long)getKeys:(const void **)arg1 values:(const void **)arg2;
//- (BOOL)mapMember:(const void *)arg1 originalKey:(const void **)arg2 value:(const void **)arg3;
//- (id)enumerator;
//- (void)removeAllItems;
//- (id)allValues;
//- (id)allKeys;
//- (void)replaceItem:(const void *)arg1 forExistingKey:(const void *)arg2;
//- (void *)existingItemForSetItem:(const void *)arg1 forAbsentKey:(const void *)arg2;
//- (void)setItem:(const void *)arg1 forKnownAbsentKey:(const void *)arg2;
//- (void)setItem:(const void *)arg1 forAbsentKey:(const void *)arg2;
//- (void)setItem:(const void *)arg1 forKey:(const void *)arg2;
//- (void)removeObjectForKey:(id)arg1;
//- (void)setObject:(id)arg1 forKey:(id)arg2;
//- (id)objectForKey:(id)arg1;
//- (id)valuePointerFunctions;
//- (id)keyPointerFunctions;
//- (unsigned long long)countByEnumeratingWithState:(CDStruct_70511ce9 *)arg1 objects:(id *)arg2 count:(unsigned long long)arg3;
//- (id)copy;
//- (id)objectEnumerator;
//- (id)keyEnumerator;
//- (unsigned long long)count;
//- (void)encodeWithCoder:(id)arg1;
//- (id)description;
//- (id)init;
//- (id)initWithKeyPointerFunctions:(id)arg1 valuePointerFunctions:(id)arg2 capacity:(unsigned long long)arg3;
//- (id)initWithKeyOptions:(int)arg1 valueOptions:(int)arg2 capacity:(unsigned long long)arg3;
//- (id)initWithCoder:(id)arg1;
//- (id)dvt_removeObjectForKey:(id)arg1;
//- (void)dvt_enumerateKeysAndObjectsUsingBlock:(id)arg1;
}
