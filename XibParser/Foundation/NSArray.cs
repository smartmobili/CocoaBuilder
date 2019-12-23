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
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Collections;

namespace Smartmobili.Cocoa
{
    //https://github.com/gnustep/gnustep-base/blob/master/Source/NSArray.m
    public class NSArray : NSObject, IList<id>
    {
        new public static Class Class = new Class(typeof(NSArray));
        new public static NSArray alloc() { return new NSArray(); }

        protected List<id> _list = new List<id>();

        public NSArray()
        {
           
        }

        // C# interop
        public NSArray(IEnumerable<id> collection)
        {
            _list = collection.ToList();
        }

        public static NSArray emptyArray()
        {
            return arrayWithCapacity(5);
        }

        public static NSArray arrayWithArray(NSArray anArray)
        {
            return (NSArray)alloc().initWithArray(anArray);
        }

        public static NSArray arrayWithCapacity(uint numItems)
        {
            return (NSArray)alloc().initWithCapacity(numItems);
        }


        public static NSArray arrayWithObject(id anObject)
        {
            NSArray array = new NSArray();
            array.addObject(anObject);

            return array;
        }

        // TODO: implements InitWithobjects
        public static NSArray arrayWithObjects(params id[] list)
        {
            if (list == null)
                return null;

            NSArray array = new NSArray();

            foreach (id elm in list)
            {
                array.addObject(elm);
            }

            return array;
        }

        public virtual uint count()
        {
            return (uint)this.Count;
        }

        public virtual void addObject(id anObject)
        {
            if (anObject == null)
                throw new ArgumentNullException("anObject");

            this.Add(anObject);
        }

        public virtual id lastObject()
        {
            id lastObj = null;

            if (this.Count != 0)
            {
                return this[this.Count - 1];
            }

            return lastObj;
        }

        public virtual bool containsObject(id anObject)
        {
            bool found = false;

            found = _list.Contains(anObject);

            return found;
        }

        public virtual void getObjects(id[] aBuffer)
        {
            uint i, c = (uint)this.Count;

            for (i = 0; i < c; i++)
                aBuffer[i] = objectAtIndex(i);
        }

		public virtual uint indexOfObjectIdenticalTo(id anObject)
		{
			uint c = (uint)this.Count;

			if (c > 0)
			{
				uint i;

				for (i = 0; i < c; i++)
					if (anObject == this.objectAtIndex(i))
						return i;
			}
			return NS.NotFound;
		}

        public virtual uint indexOfObject(id anObject)
        {
            uint foundIndex = NS.NotFound;
            int idx = _list.IndexOf(anObject);
            if (idx != -1)
                foundIndex = (uint)idx;

            return foundIndex;
        }

        public virtual id objectAtIndex(uint index)
        {
            id obj = this[(int)index];
            return obj;
        }

        public virtual id initWithArray(NSArray anArray)
        {
            id self = this;

            _list = new List<id>((int)anArray.Count);
            foreach (var arr in anArray)
            {
                _list.Add(arr);
            }

            return self;
        }

        public virtual id initWithCapacity(uint numItems)
        {
            id self = this;

            _list = new List<id>((int)numItems);

            return self;
        }

        public virtual id initWithObjects(params id[] objects)
        {
            id self = this;

            if (objects == null)
                return null;

            id[] arr = objects.Where(x => x != null).ToArray();
            _list = new List<id>(arr);

            return self;
        }

        public override id initWithCoder(NSCoder aCoder)
        {
            id self = this;

            if (aCoder.AllowsKeyedCoding)
            {
                id array = ((NSKeyedUnarchiver)aCoder)._decodeArrayOfObjectsForKey(@"NS.objects");
                if (array == null)
                {
                    uint i = 0;
                    NSString key;
                    id val;

                    array = NSMutableArray.arrayWithCapacity(2);
                    key = NSString.stringWithFormat(@"NS.object.%u", i);
                    //key = (NSString)string.Format(@"NS.object.{0}", i);
                    val = ((NSKeyedUnarchiver)aCoder).decodeObjectForKey(key);
                    //array = [NSMutableArray arrayWithCapacity: 2];
                    //key = [NSString stringWithFormat: @"NS.object.%u", i];
                    //val = [(NSKeyedUnarchiver*)aCoder decodeObjectForKey: key];

                    while (val != null)
                    {
                        ((NSMutableArray)array).addObject(val);
                        i++;
                        //key = (NSString)string.Format(@"NS.object.{0}", i);
                        key = NSString.stringWithFormat(@"NS.object.%u", i);
                        val = ((NSKeyedUnarchiver)aCoder).decodeObjectForKey(key);
                    }
                }

                self = initWithArray((NSArray)array);
            }
            else
            {

            }

            return self;
        }


        public virtual NSEnumerator objectEnumerator()
        {
            return (NSEnumerator)NSArrayEnumerator.alloc().initWithArray(this);
        }


        //- (NSUInteger)indexOfObjectPassingTest:(BOOL (^)(id obj, NSUInteger idx, BOOL *stop))predicate
        public virtual uint indexOfObjectPassingTest(Predicate<id> match)
        {
            uint index = NS.NotFound;

            int idx = _list.FindIndex(match);
            index = (idx != -1) ? (uint)idx : NS.NotFound;
            
            return index;
        }





        #region Implementation of IEnumerable

        public IEnumerator<id> GetEnumerator()
        {
            return _list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region Implementation of ICollection<NSObject>

        public void Add(id item)
        {
            _list.Add(item);
        }

        public void Clear()
        {
            _list.Clear();
        }

        public bool Contains(id item)
        {
            return _list.Contains(item);
        }

        public void CopyTo(id[] array, int arrayIndex)
        {
            _list.CopyTo(array, arrayIndex);
        }

        public bool Remove(id item)
        {
            return _list.Remove(item);
        }

        public int Count
        {
            get { return _list.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
            //get { return _list.IsReadOnly; }
        }

        #endregion

        #region Implementation of IList<INSObject>

        public int IndexOf(id item)
        {
            return _list.IndexOf(item);
        }

        public void Insert(int index, id item)
        {
            _list.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            _list.RemoveAt(index);
        }

        public id this[int index]
        {
            get { return _list[index]; }
            set { _list[index] = value; }
        }

        #endregion


        public override int GetHashCode()
        {
            return (_list != null) ? _list.GetHashCode() : base.GetHashCode();
        }
    }

    class NSArrayEnumerator : NSEnumerator
    {
        new public static NSArrayEnumerator alloc() { return new NSArrayEnumerator(); }

        protected NSArray _array;
        protected int _curIndex;

        public id initWithArray(NSArray anArray)
        {
            id self = this;

            _array = anArray;

            return self;
        }


         public override NSArray allObjects()
         {
             return _array;
         }

         public override id nextObject()
         {
             id nextObj = null;

             if (_array == null)
                 return null;

             if (_curIndex < _array.Count)
             {
                 nextObj = _array.objectAtIndex((uint)_curIndex++);
             }

             return nextObj;
         }

        
    }

    
}
