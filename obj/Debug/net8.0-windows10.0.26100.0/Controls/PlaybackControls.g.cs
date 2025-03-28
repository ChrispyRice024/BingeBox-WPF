﻿#pragma checksum "..\..\..\..\Controls\PlaybackControls.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "E27BBCE598E82F14918590EE79F9E9CB9F9C804B"
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
using System.Windows.Controls.Ribbon;
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
using WPF_BingeBox.Controls;


namespace WPF_BingeBox.Controls {
    
    
    /// <summary>
    /// PlaybackControls
    /// </summary>
    public partial class PlaybackControls : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 15 "..\..\..\..\Controls\PlaybackControls.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid EventGrid;
        
        #line default
        #line hidden
        
        
        #line 18 "..\..\..\..\Controls\PlaybackControls.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.StackPanel ControlPanel;
        
        #line default
        #line hidden
        
        
        #line 20 "..\..\..\..\Controls\PlaybackControls.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Slider TrackBar;
        
        #line default
        #line hidden
        
        
        #line 23 "..\..\..\..\Controls\PlaybackControls.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button PrevBtn;
        
        #line default
        #line hidden
        
        
        #line 24 "..\..\..\..\Controls\PlaybackControls.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button PlayBtn;
        
        #line default
        #line hidden
        
        
        #line 25 "..\..\..\..\Controls\PlaybackControls.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button NextBtn;
        
        #line default
        #line hidden
        
        
        #line 26 "..\..\..\..\Controls\PlaybackControls.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button FullScreenBtn;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "9.0.2.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/WPF_BingeBox;component/controls/playbackcontrols.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Controls\PlaybackControls.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "9.0.2.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.EventGrid = ((System.Windows.Controls.Grid)(target));
            return;
            case 2:
            this.ControlPanel = ((System.Windows.Controls.StackPanel)(target));
            return;
            case 3:
            this.TrackBar = ((System.Windows.Controls.Slider)(target));
            return;
            case 4:
            this.PrevBtn = ((System.Windows.Controls.Button)(target));
            
            #line 23 "..\..\..\..\Controls\PlaybackControls.xaml"
            this.PrevBtn.Click += new System.Windows.RoutedEventHandler(this.PrevBtn_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.PlayBtn = ((System.Windows.Controls.Button)(target));
            
            #line 24 "..\..\..\..\Controls\PlaybackControls.xaml"
            this.PlayBtn.Click += new System.Windows.RoutedEventHandler(this.PlayBtn_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            this.NextBtn = ((System.Windows.Controls.Button)(target));
            
            #line 25 "..\..\..\..\Controls\PlaybackControls.xaml"
            this.NextBtn.Click += new System.Windows.RoutedEventHandler(this.NextBtn_Click);
            
            #line default
            #line hidden
            return;
            case 7:
            this.FullScreenBtn = ((System.Windows.Controls.Button)(target));
            
            #line 26 "..\..\..\..\Controls\PlaybackControls.xaml"
            this.FullScreenBtn.Click += new System.Windows.RoutedEventHandler(this.FullScreenBtn_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

