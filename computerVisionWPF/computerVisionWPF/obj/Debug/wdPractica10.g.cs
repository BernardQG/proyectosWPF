﻿#pragma checksum "..\..\wdPractica10.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "4159CBCE96E3A28F0D63669B12EA9243DBB08344"
//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.42000
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

using MaterialDesignThemes.Wpf;
using MaterialDesignThemes.Wpf.Transitions;
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
using computerVisionWPF;


namespace computerVisionWPF {
    
    
    /// <summary>
    /// wdPractica10
    /// </summary>
    public partial class wdPractica10 : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 11 "..\..\wdPractica10.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal MaterialDesignThemes.Wpf.Transitions.TransitioningContent TrainsitionigContentSlide;
        
        #line default
        #line hidden
        
        
        #line 22 "..\..\wdPractica10.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image ctlIma;
        
        #line default
        #line hidden
        
        
        #line 27 "..\..\wdPractica10.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnOpenImagen;
        
        #line default
        #line hidden
        
        
        #line 30 "..\..\wdPractica10.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnOriginal;
        
        #line default
        #line hidden
        
        
        #line 32 "..\..\wdPractica10.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnFeactures;
        
        #line default
        #line hidden
        
        
        #line 37 "..\..\wdPractica10.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock txtP;
        
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
            System.Uri resourceLocater = new System.Uri("/computerVisionWPF;component/wdpractica10.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\wdPractica10.xaml"
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
            this.TrainsitionigContentSlide = ((MaterialDesignThemes.Wpf.Transitions.TransitioningContent)(target));
            return;
            case 2:
            this.ctlIma = ((System.Windows.Controls.Image)(target));
            return;
            case 3:
            this.btnOpenImagen = ((System.Windows.Controls.Button)(target));
            
            #line 27 "..\..\wdPractica10.xaml"
            this.btnOpenImagen.Click += new System.Windows.RoutedEventHandler(this.btnOpenImagen_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.btnOriginal = ((System.Windows.Controls.Button)(target));
            
            #line 30 "..\..\wdPractica10.xaml"
            this.btnOriginal.Click += new System.Windows.RoutedEventHandler(this.btnOriginal_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.btnFeactures = ((System.Windows.Controls.Button)(target));
            
            #line 32 "..\..\wdPractica10.xaml"
            this.btnFeactures.Click += new System.Windows.RoutedEventHandler(this.btnFeactures_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            this.txtP = ((System.Windows.Controls.TextBlock)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

