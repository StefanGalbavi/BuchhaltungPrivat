using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace BuchhaltungPrivat
{
    class ViewModel
    {
        //To call resource dictionary in our codebehind
        ResourceDictionary dict = (ResourceDictionary) Application.LoadComponent(new Uri("/BuchhaltungPrivat;component/IconDictionary.xaml", UriKind.RelativeOrAbsolute));

        //Source list for our Menu Items Listbox
        public List<MenuItemsData> ItemsList
        {
            get
            {
                return new List<MenuItemsData> {
                new MenuItemsData(){ PathData = (PathGeometry)dict["Booking"], MenuText="Booking" },
                new MenuItemsData(){ PathData = (PathGeometry)dict["StandingOrder"], MenuText="Dauerauftrag" },
                new MenuItemsData(){ PathData= (PathGeometry)dict["Category"], MenuText="Kategorien" },
                new MenuItemsData(){ PathData = (PathGeometry)dict["Konten"], MenuText="Konten" },
                new MenuItemsData(){ PathData = (PathGeometry)dict["Report"], MenuText="Berichte" },
                new MenuItemsData(){ PathData = (PathGeometry)dict["Close"], MenuText="Beenden" } };
            }
        }
    }

    public class MenuItemsData
    {
        public PathGeometry PathData { get; set; }
        public bool IsItemSelected { get; set; }
        public string MenuText { get; set; }
    }
}
