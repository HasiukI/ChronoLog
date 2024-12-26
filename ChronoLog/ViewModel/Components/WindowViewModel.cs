using ChronoLog.View;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ChronoLog.ViewModel.Components
{
    internal class WindowViewModel : BaseViewModel
    {
        private Dictionary<string, Page> _pages;

        public WindowViewModel(MainViewModel mainViewModel)
        {
            WidthWindow = 120;
            HeightWindow = 120; 

           _pages = new Dictionary<string, Page>();
            _pages.Add("StopWatch_120", new StopWatchView() { DataContext = mainViewModel });
            _pages.Add("History_200", new HistoryView() { DataContext = mainViewModel });
            _pages.Add("Setting_200", new SettingView() { DataContext = mainViewModel });

            CurentPage = _pages.First(p => p.Key.StartsWith("StopWatch")).Value;
        }

        private Page _curentPage;
        public Page CurentPage
        {
            get => _curentPage;
            set
            {
                if (_curentPage != value)
                {
                    _curentPage = value;
                    OnPropertyChanged(nameof(CurentPage));
                }
            }
        }

        private double _heightWindow;
        public double HeightWindow
        {
            get => _heightWindow;
            set
            {
                if(value == _heightWindow) 
                    return;
                _heightWindow = value;
                OnPropertyChanged(nameof(HeightWindow));
            }
        }

        private double _widthWindow;
        public double WidthWindow
        {
            get => _widthWindow;
            set
            {
                if (value == _widthWindow) 
                    return;
                _widthWindow = value;
                OnPropertyChanged(nameof(WidthWindow));
            }
        }

        private ICommand _showWindowCommand;
        public ICommand ShowWindowCommand
        {
            get
            {
                if (_showWindowCommand == null)
                {
                    _showWindowCommand = new RelayCommand(
                        () =>
                        {
                            Application.Current.MainWindow.WindowState = WindowState.Normal;
                            Application.Current.MainWindow.Show();
                            Application.Current.MainWindow.Activate();
                        });
                }
                return _showWindowCommand;
            }
        }

        private ICommand _hideWindowCommand;
        public ICommand HideWindowCommand
        {
            get
            {
                if (_hideWindowCommand == null)
                {
                    _hideWindowCommand = new RelayCommand(() =>
                    {
                        Application.Current.MainWindow.Hide();
                    });

                }
                return _hideWindowCommand;
            }

        }

        public ICommand CloseWindowCommand
        {
            get => new RelayCommand(() => {
                Application.Current.Shutdown();
            });
        }

        private ICommand _dragWindowCommand;
        public ICommand DragWindowCommand {
            get
            {
                if( _dragWindowCommand == null)
                {
                    _dragWindowCommand = new RelayCommand<Window>((window) =>
                    {
                        if(Mouse.LeftButton == MouseButtonState.Pressed)
                        {
                            window?.DragMove();
                        } 
                    });
                }
                return _dragWindowCommand;
            }
        }


        private ICommand _changePageCommand;
        public ICommand ChangePageCommand
        {
            get
            {
                if (_changePageCommand == null)
                {
                    _changePageCommand = new RelayCommand<string>(param =>
                    {
                        var result = _pages.First(kv => kv.Key.StartsWith(param));
                        CurentPage = result.Value;

                        double heightWidth =double.Parse(result.Key.Split('_')[1]);
                        HeightWindow = heightWidth;
                        WidthWindow = heightWidth;
                    });
                }

                return _changePageCommand;
            }
        }
    }
}
