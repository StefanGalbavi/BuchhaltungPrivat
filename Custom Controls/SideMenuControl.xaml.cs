﻿using System;
using System.Windows;
using System.Windows.Controls;

namespace BuchhaltungPrivat.Custom_Controls
{
    /// <summary>
    /// Interaktionslogik für SideMenuControl.xaml
    /// </summary>
    public partial class SideMenuControl : UserControl
    {
        public SideMenuControl()
        {
            InitializeComponent();
            DataContext = new ViewModel();
        }

        public int SelectedIndex
        {
            get { return (int) GetValue(SelectedIndexProperty); }
            set { SetValue(SelectedIndexProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedIndex.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedIndexProperty =
            DependencyProperty.Register("SelectedIndex", typeof(int), typeof(SideMenuControl));


        //Since we want to navigate through different pages in listbox item selection change event we are declaring  an event in our usercontrol for that...
        public event EventHandler SelectionChanged;
        private void MyListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectionChanged?.Invoke(sender, e);

            //That was simple.. wasnt it?
        }
    }
}
