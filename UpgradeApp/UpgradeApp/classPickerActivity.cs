﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace UpgradeApp
{
    [Activity(Label = "classPickerActivity")]
    public class classPickerActivity : Activity
    {
        ListView list;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.classPickerScreen);
            
            list = FindViewById<ListView>(Resource.Id.classPicker);
            
            //list.ItemClick += List_ItemClick;
        }
    }
}