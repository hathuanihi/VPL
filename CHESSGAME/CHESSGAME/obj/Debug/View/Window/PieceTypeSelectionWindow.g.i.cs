﻿#pragma checksum "..\..\..\..\View\Window\PieceTypeSelectionWindow.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "6FC28F857F7E26B45BC556F7B6A16F8174E8DF897D3603E6502DCA2C963AF1E6"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using MahApps.Metro.Controls;
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


namespace CHESSGAME.View.Window {
    
    
    /// <summary>
    /// PieceTypeSelectionWindow
    /// </summary>
    public partial class PieceTypeSelectionWindow : MahApps.Metro.Controls.MetroWindow, System.Windows.Markup.IComponentConnector {
        
        
        #line 26 "..\..\..\..\View\Window\PieceTypeSelectionWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.UserControl UserControlQueen;
        
        #line default
        #line hidden
        
        
        #line 27 "..\..\..\..\View\Window\PieceTypeSelectionWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.UserControl UserControlBishop;
        
        #line default
        #line hidden
        
        
        #line 28 "..\..\..\..\View\Window\PieceTypeSelectionWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.UserControl UserControlRook;
        
        #line default
        #line hidden
        
        
        #line 29 "..\..\..\..\View\Window\PieceTypeSelectionWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.UserControl UserControlKnight;
        
        #line default
        #line hidden
        
        
        #line 32 "..\..\..\..\View\Window\PieceTypeSelectionWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button ButtonValidation;
        
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
            System.Uri resourceLocater = new System.Uri("/CHESSGAME;component/view/window/piecetypeselectionwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\View\Window\PieceTypeSelectionWindow.xaml"
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
            this.UserControlQueen = ((System.Windows.Controls.UserControl)(target));
            
            #line 26 "..\..\..\..\View\Window\PieceTypeSelectionWindow.xaml"
            this.UserControlQueen.MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.UserControlQueen_OnMouseDown);
            
            #line default
            #line hidden
            return;
            case 2:
            this.UserControlBishop = ((System.Windows.Controls.UserControl)(target));
            
            #line 27 "..\..\..\..\View\Window\PieceTypeSelectionWindow.xaml"
            this.UserControlBishop.MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.UserControlBishop_OnMouseDown);
            
            #line default
            #line hidden
            return;
            case 3:
            this.UserControlRook = ((System.Windows.Controls.UserControl)(target));
            
            #line 28 "..\..\..\..\View\Window\PieceTypeSelectionWindow.xaml"
            this.UserControlRook.MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.UserControlRook_OnMouseDown);
            
            #line default
            #line hidden
            return;
            case 4:
            this.UserControlKnight = ((System.Windows.Controls.UserControl)(target));
            
            #line 29 "..\..\..\..\View\Window\PieceTypeSelectionWindow.xaml"
            this.UserControlKnight.MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.UserControlKnight_OnMouseDown);
            
            #line default
            #line hidden
            return;
            case 5:
            this.ButtonValidation = ((System.Windows.Controls.Button)(target));
            
            #line 32 "..\..\..\..\View\Window\PieceTypeSelectionWindow.xaml"
            this.ButtonValidation.Click += new System.Windows.RoutedEventHandler(this.ButtonValidation_OnClick);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

