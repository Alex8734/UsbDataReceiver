﻿#pragma checksum "..\..\..\..\..\MVVM\View\ChartItem.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "6BAFC4D1C723268C600911CE371510F9A8A4E1E6"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using InteractiveDataDisplay.WPF;
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
using UsbDataReceiver.GUI.MVVM.View;
using UsbDataReceiver.GUI.MVVM.ViewModel;


namespace UsbDataReceiver.GUI.MVVM.View {
    
    
    /// <summary>
    /// ChartItem
    /// </summary>
    public partial class ChartItem : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 17 "..\..\..\..\..\MVVM\View\ChartItem.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal InteractiveDataDisplay.WPF.Chart plotter;
        
        #line default
        #line hidden
        
        
        #line 46 "..\..\..\..\..\MVVM\View\ChartItem.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid lines;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "7.0.9.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/UsbDataReceiver.GUI;component/mvvm/view/chartitem.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\..\MVVM\View\ChartItem.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "7.0.9.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 11 "..\..\..\..\..\MVVM\View\ChartItem.xaml"
            ((UsbDataReceiver.GUI.MVVM.View.ChartItem)(target)).Unloaded += new System.Windows.RoutedEventHandler(this.ChartLayout_OnUnloaded);
            
            #line default
            #line hidden
            
            #line 12 "..\..\..\..\..\MVVM\View\ChartItem.xaml"
            ((UsbDataReceiver.GUI.MVVM.View.ChartItem)(target)).Loaded += new System.Windows.RoutedEventHandler(this.ChartLayout_OnLoaded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.plotter = ((InteractiveDataDisplay.WPF.Chart)(target));
            
            #line 30 "..\..\..\..\..\MVVM\View\ChartItem.xaml"
            this.plotter.PreviewDragLeave += new System.Windows.DragEventHandler(this.Plotter_OnDragLeave);
            
            #line default
            #line hidden
            
            #line 31 "..\..\..\..\..\MVVM\View\ChartItem.xaml"
            this.plotter.DragLeave += new System.Windows.DragEventHandler(this.Plotter_OnDragLeave);
            
            #line default
            #line hidden
            return;
            case 3:
            this.lines = ((System.Windows.Controls.Grid)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

