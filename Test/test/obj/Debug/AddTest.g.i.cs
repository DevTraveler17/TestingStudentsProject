﻿#pragma checksum "..\..\AddTest.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "9C650654BE53210FA3C1DA6EBB704AE7744570B6"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

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


namespace test {
    
    
    /// <summary>
    /// AddTest
    /// </summary>
    public partial class AddTest : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 4 "..\..\AddTest.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal test.AddTest AddTestWindow;
        
        #line default
        #line hidden
        
        
        #line 15 "..\..\AddTest.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox SubjectText;
        
        #line default
        #line hidden
        
        
        #line 16 "..\..\AddTest.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox TestNameText;
        
        #line default
        #line hidden
        
        
        #line 17 "..\..\AddTest.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button Add_question_button;
        
        #line default
        #line hidden
        
        
        #line 20 "..\..\AddTest.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ScrollViewer scrollViewer;
        
        #line default
        #line hidden
        
        
        #line 21 "..\..\AddTest.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.StackPanel ExplorerText;
        
        #line default
        #line hidden
        
        
        #line 24 "..\..\AddTest.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button Generate_button;
        
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
            System.Uri resourceLocater = new System.Uri("/test;component/addtest.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\AddTest.xaml"
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
            this.AddTestWindow = ((test.AddTest)(target));
            
            #line 5 "..\..\AddTest.xaml"
            this.AddTestWindow.Closed += new System.EventHandler(this.AddTestWindow_Closed);
            
            #line default
            #line hidden
            return;
            case 2:
            this.SubjectText = ((System.Windows.Controls.TextBox)(target));
            return;
            case 3:
            this.TestNameText = ((System.Windows.Controls.TextBox)(target));
            return;
            case 4:
            this.Add_question_button = ((System.Windows.Controls.Button)(target));
            
            #line 17 "..\..\AddTest.xaml"
            this.Add_question_button.Click += new System.Windows.RoutedEventHandler(this.Add_question_button_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.scrollViewer = ((System.Windows.Controls.ScrollViewer)(target));
            return;
            case 6:
            this.ExplorerText = ((System.Windows.Controls.StackPanel)(target));
            return;
            case 7:
            this.Generate_button = ((System.Windows.Controls.Button)(target));
            
            #line 24 "..\..\AddTest.xaml"
            this.Generate_button.Click += new System.Windows.RoutedEventHandler(this.Generate_button_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

