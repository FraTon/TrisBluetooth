﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace TrisBluetooth
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            this.BindingContext = new ViewModel();

            Picker cerca = new Picker  { Title = "Cerca" };
            cerca.SetBinding(Picker.ItemsSourceProperty, "ListOfDevices");
            cerca.SetBinding(Picker.SelectedItemProperty, "SelectedBthDevice");
            cerca.SetBinding(VisualElement.IsEnabledProperty, "IsPickerEnabled");

            Entry entrySleepTime = new Entry() { Keyboard = Keyboard.Numeric, Placeholder = "Sleep time" };
            entrySleepTime.SetBinding(Entry.TextProperty, "SleepTime");

            Button buttonConnect = new Button() { Text = "Connect" };
            buttonConnect.SetBinding(Button.CommandProperty, "ConnectCommand");
            buttonConnect.SetBinding(VisualElement.IsEnabledProperty, "IsConnectEnabled");

            Button buttonDisconnect = new Button() { Text = "Disconnect" };
            buttonDisconnect.SetBinding(Button.CommandProperty, "DisconnectCommand");
            buttonDisconnect.SetBinding(VisualElement.IsEnabledProperty, "IsDisconnectEnabled");

            StackLayout slButtons = new StackLayout() { Orientation = StackOrientation.Horizontal, Children = { buttonDisconnect, buttonConnect } };

            ListView lv = new ListView();
            lv.SetBinding(ListView.ItemsSourceProperty, "ListOfBarcodes");
            lv.ItemTemplate = new DataTemplate(typeof(TextCell));
            lv.ItemTemplate.SetBinding(TextCell.TextProperty, ".");

            int topPadding = 0;
            if (Device.RuntimePlatform == Device.iOS)
                topPadding = 20;

            StackLayout sl = new StackLayout { Children = { cerca, entrySleepTime, slButtons, lv }, Padding = new Thickness(0, topPadding, 0, 0) };
            Content = sl;
        }
        protected override bool OnBackButtonPressed()
        {
            return true;
        }
    }
}
