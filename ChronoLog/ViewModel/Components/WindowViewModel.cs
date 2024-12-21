using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ChronoLog.ViewModel.Components
{
    internal class WindowViewModel
    {

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
    }
}
