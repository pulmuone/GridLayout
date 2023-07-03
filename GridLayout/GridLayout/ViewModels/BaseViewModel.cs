using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;

namespace GridLayout.ViewModels
{
    public class BaseViewModel : ObservableObject
    {
        public ICommand SortedCommand { get; set; }

        public BaseViewModel()
        {

        }

        bool isBusy = false;
        bool isEnabled = true;
        bool _isControlEnable = true;
        bool _isCamera;
        string _scannerStatus;
        Xamarin.Forms.Color _scannerStatusColor;

        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }

        public bool IsEnabled
        {
            get { return isEnabled; }
            set { SetProperty(ref isEnabled, value); }
        }

        public bool IsControlEnable
        {
            get => _isControlEnable;
            set => SetProperty(ref this._isControlEnable, value);
        }

        string title = string.Empty;

        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        public bool IsCamera
        {
            get => _isCamera;
            set => SetProperty(ref this._isCamera, value);
        }

        public string ScannerStatus
        {
            get => _scannerStatus;
            set => SetProperty(ref this._scannerStatus, value);
        }

        public Xamarin.Forms.Color ScannerStatusColor
        {
            get => _scannerStatusColor;
            set => SetProperty(ref this._scannerStatusColor, value);
        }
    }
}
