#pragma checksum "..\..\..\..\View\DroneListView.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "78A7A93A54A6AC935DAF1CA5F454E8445BF67AD8"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using PL;
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


namespace PL {
    
    
    /// <summary>
    /// DroneListView
    /// </summary>
    public partial class DroneListView : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 19 "..\..\..\..\View\DroneListView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid UpGrid;
        
        #line default
        #line hidden
        
        
        #line 25 "..\..\..\..\View\DroneListView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button StatusSelectorButton;
        
        #line default
        #line hidden
        
        
        #line 26 "..\..\..\..\View\DroneListView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox StatusSelector;
        
        #line default
        #line hidden
        
        
        #line 29 "..\..\..\..\View\DroneListView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button WeightSelectorButton;
        
        #line default
        #line hidden
        
        
        #line 30 "..\..\..\..\View\DroneListView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox WeightSelector;
        
        #line default
        #line hidden
        
        
        #line 41 "..\..\..\..\View\DroneListView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button clear;
        
        #line default
        #line hidden
        
        
        #line 45 "..\..\..\..\View\DroneListView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid ListGrid;
        
        #line default
        #line hidden
        
        
        #line 51 "..\..\..\..\View\DroneListView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListView DronesListView;
        
        #line default
        #line hidden
        
        
        #line 104 "..\..\..\..\View\DroneListView.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid bottom;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "5.0.16.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/PL;component/view/dronelistview.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\View\DroneListView.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "5.0.16.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 9 "..\..\..\..\View\DroneListView.xaml"
            ((PL.DroneListView)(target)).Closing += new System.ComponentModel.CancelEventHandler(this.Window_Closing);
            
            #line default
            #line hidden
            return;
            case 2:
            this.UpGrid = ((System.Windows.Controls.Grid)(target));
            return;
            case 3:
            this.StatusSelectorButton = ((System.Windows.Controls.Button)(target));
            
            #line 25 "..\..\..\..\View\DroneListView.xaml"
            this.StatusSelectorButton.MouseDoubleClick += new System.Windows.Input.MouseButtonEventHandler(this.ButtonSelect);
            
            #line default
            #line hidden
            return;
            case 4:
            this.StatusSelector = ((System.Windows.Controls.ComboBox)(target));
            
            #line 26 "..\..\..\..\View\DroneListView.xaml"
            this.StatusSelector.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.Selector_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 5:
            this.WeightSelectorButton = ((System.Windows.Controls.Button)(target));
            
            #line 29 "..\..\..\..\View\DroneListView.xaml"
            this.WeightSelectorButton.MouseDoubleClick += new System.Windows.Input.MouseButtonEventHandler(this.ButtonSelect);
            
            #line default
            #line hidden
            return;
            case 6:
            this.WeightSelector = ((System.Windows.Controls.ComboBox)(target));
            
            #line 30 "..\..\..\..\View\DroneListView.xaml"
            this.WeightSelector.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.Selector_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 7:
            this.clear = ((System.Windows.Controls.Button)(target));
            
            #line 41 "..\..\..\..\View\DroneListView.xaml"
            this.clear.Click += new System.Windows.RoutedEventHandler(this.clear_Click);
            
            #line default
            #line hidden
            return;
            case 8:
            
            #line 42 "..\..\..\..\View\DroneListView.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.GroupBy);
            
            #line default
            #line hidden
            return;
            case 9:
            this.ListGrid = ((System.Windows.Controls.Grid)(target));
            return;
            case 10:
            this.DronesListView = ((System.Windows.Controls.ListView)(target));
            
            #line 51 "..\..\..\..\View\DroneListView.xaml"
            this.DronesListView.MouseDoubleClick += new System.Windows.Input.MouseButtonEventHandler(this.DronesListView_MouseDoubleClick);
            
            #line default
            #line hidden
            return;
            case 11:
            this.bottom = ((System.Windows.Controls.Grid)(target));
            return;
            case 12:
            
            #line 109 "..\..\..\..\View\DroneListView.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.ButtonView);
            
            #line default
            #line hidden
            return;
            case 13:
            
            #line 110 "..\..\..\..\View\DroneListView.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.ButtonClose);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

