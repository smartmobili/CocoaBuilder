﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Smartmobili.Cocoa
{
    //https://developer.apple.com/library/mac/#documentation/Cocoa/Reference/ApplicationKit/Classes/NSPopUpButton_Class/Reference/Reference.html
    //https://github.com/gnustep/gnustep-gui/blob/master/Headers/AppKit/NSPopUpButton.h
    //https://github.com/gnustep/gnustep-gui/blob/master/Source/NSPopUpButton.m
    public class NSPopUpButton : NSButton
    {
        private static Class _class = new Class(typeof(NSPopUpButton));
        private static Class _nspopupbuttonCellClass;




        new public static Class CellClass
        {
            get { return _nspopupbuttonCellClass; }
            set { _nspopupbuttonCellClass = value; }
        }

        static NSPopUpButton() { initialize(); }
        new static void initialize()
        {
            //this.Version = 1;
            NSPopUpButton.CellClass = NSPopUpButtonCell.Class;

            //[self exposeBinding: NSSelectedIndexBinding];
            //[self exposeBinding: NSSelectedObjectBinding];
            //[self exposeBinding: NSSelectedTagBinding];
            //[self exposeBinding: NSContentValuesBinding];
        }
        





        public NSPopUpButton()
        {

        }

        [ObjcProp("menu")]
        public override NSMenu Menu 
        {
            get 
            {
                return (_cell  != null) ? ((NSPopUpButtonCell)_cell).Menu : null; 
            }
            set 
            { 
                if (_cell != null)
                    ((NSPopUpButtonCell)_cell).Menu = value; 
            }
        }

        //[ObjcProp("pullsDown")]
        //public bool PullsDown
        //{
        //    get { return _cell.Pu; }
        //    set { _cell.Menu = value; }
        //}

    }
}
