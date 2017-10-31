﻿using Sockets.Plugin;
using Sockets.Plugin.Abstractions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Innovation.AR
{
    public partial class MainPage : ContentPage
    {
        Dictionary<string, string> colorDict = new Dictionary<string, string>()
        {
            { "Default", "#000000" },       { "Amber", "#FFC107" },
            { "Black", "#212121" },         { "Blue", "#2196F3" },
            { "Blue Grey", "#607D8B" },     { "Brown", "#795548" },
            { "Cyan", "#00BCD4" },          { "Dark Orange", "#FF5722" },
            { "Dark Purple", "#673AB7" },   { "Green", "#4CAF50" },
            { "Grey", "#9E9E9E" },          { "Indigo", "#3F51B5" },
            { "Light Blue", "#02A8F3" },    { "Light Green", "#8AC249" },
            { "Lime", "#CDDC39" },          { "Orange", "#FF9800" },
            { "Pink", "#E91E63" },          { "Purple", "#94499D" },
            { "Red", "#D32F2F" },           { "Teal", "#009587" },
            { "White", "#FFFFFF" },         { "Yellow", "#FFEB3B" },

        };

        private static SituationAwareness sitAw;

        public MainPage()
        {
            InitializeComponent();
            sitAw = new SituationAwareness();
            BindingContext = sitAw;

            // populate picker with available colors
            foreach (string colorName in colorDict.Keys)
            {
                settingsColorPicker.Items.Add(colorName);
            }


            //listener = new TcpSocketListener();



            // when we get connections, read byte-by-byte from the socket's read stream
            // listener.ConnectionReceived += RequestProcessor;


        }


        async void RequestProcessor(object sender, TcpSocketListenerConnectEventArgs args)
        {
            var client = args.SocketClient;

            var bytesRead = -1;
            var buf = new byte[1];

            while (bytesRead != 0)
            {
                bytesRead = await args.SocketClient.ReadStream.ReadAsync(buf, 0, 1);
                if (bytesRead > 0)
                {
                    //Debug.Write(buf[0]);
                }
            }
        }


        private void TapColorPicker_Tapped(object sender, EventArgs e)
        {
            settingsColorPicker.Focus();
        }

        private void Picker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var picker = (Picker)sender;
            sitAw.ColorValue = colorDict[picker.Items[picker.SelectedIndex]];
        }

        protected override async void OnAppearing()
        {
            var listenPort = 12000;
            var listener = new TcpSocketListener();

            // when we get connections, read byte-by-byte from the socket's read stream
            listener.ConnectionReceived += async (sender, args) =>
            {
                var client = args.SocketClient;

                var bytesRead = -1;
                var buf = new byte[1024];

                while (bytesRead != 0)
                {
                    bytesRead = await args.SocketClient.ReadStream.ReadAsync(buf, 0, 1024);
                    if (bytesRead > 0)
                    {
                        //Console.Write("Read Bytes {0}", Encoding.UTF8.GetString(buf));
                    }
                    // Debug.Write(buf[0]);
                }
            };

            // bind to the listen port across all interfaces
            await listener.StartListeningAsync(listenPort);
        }
    }
}
