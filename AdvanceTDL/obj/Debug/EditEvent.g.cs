﻿#pragma checksum "..\..\EditEvent.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "DF1821DA68855F6CC237D02BF4F22315F859482880220DD338365FBC87FAC400"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using AdvanceTDL;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace AdvanceTDL {
    
    
    /// <summary>
    /// EditEvent
    /// </summary>
    public partial class EditEvent : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 34 "..\..\EditEvent.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txt_edit_tenSK;
        
        #line default
        #line hidden
        
        
        #line 41 "..\..\EditEvent.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txt_edit_motaSK;
        
        #line default
        #line hidden
        
        
        #line 49 "..\..\EditEvent.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DatePicker date_edit;
        
        #line default
        #line hidden
        
        
        #line 55 "..\..\EditEvent.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox cb_gio;
        
        #line default
        #line hidden
        
        
        #line 62 "..\..\EditEvent.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox cb_phut;
        
        #line default
        #line hidden
        
        
        #line 67 "..\..\EditEvent.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock txb_Id;
        
        #line default
        #line hidden
        
        
        #line 144 "..\..\EditEvent.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox check_remind;
        
        #line default
        #line hidden
        
        
        #line 161 "..\..\EditEvent.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btn_ok;
        
        #line default
        #line hidden
        
        
        #line 176 "..\..\EditEvent.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btn_cancel;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/AdvanceTDL;component/editevent.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\EditEvent.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 11 "..\..\EditEvent.xaml"
            ((AdvanceTDL.EditEvent)(target)).Closing += new System.ComponentModel.CancelEventHandler(this.EditWindow_Closing);
            
            #line default
            #line hidden
            return;
            case 2:
            this.txt_edit_tenSK = ((System.Windows.Controls.TextBox)(target));
            
            #line 39 "..\..\EditEvent.xaml"
            this.txt_edit_tenSK.MouseDoubleClick += new System.Windows.Input.MouseButtonEventHandler(this.txt_edit_tenSK_MouseDoubleClick);
            
            #line default
            #line hidden
            return;
            case 3:
            this.txt_edit_motaSK = ((System.Windows.Controls.TextBox)(target));
            
            #line 46 "..\..\EditEvent.xaml"
            this.txt_edit_motaSK.MouseDoubleClick += new System.Windows.Input.MouseButtonEventHandler(this.txt_edit_motaSK_MouseDoubleClick);
            
            #line default
            #line hidden
            return;
            case 4:
            this.date_edit = ((System.Windows.Controls.DatePicker)(target));
            return;
            case 5:
            this.cb_gio = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 6:
            this.cb_phut = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 7:
            this.txb_Id = ((System.Windows.Controls.TextBlock)(target));
            return;
            case 8:
            this.check_remind = ((System.Windows.Controls.CheckBox)(target));
            return;
            case 9:
            this.btn_ok = ((System.Windows.Controls.Button)(target));
            
            #line 167 "..\..\EditEvent.xaml"
            this.btn_ok.Click += new System.Windows.RoutedEventHandler(this.btn_ok_Click);
            
            #line default
            #line hidden
            return;
            case 10:
            this.btn_cancel = ((System.Windows.Controls.Button)(target));
            
            #line 182 "..\..\EditEvent.xaml"
            this.btn_cancel.Click += new System.Windows.RoutedEventHandler(this.btn_cancel_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

