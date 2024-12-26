using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace ChronoLog.ViewModel.Components
{
    internal class StopWatchViewModel : BaseViewModel
    {
        private Stopwatch _stopwatch;
        private DispatcherTimer _timer;
        private HistoryViewModel _history;

        public StopWatchViewModel(HistoryViewModel history)
        {
            _history = history;

            _stopwatch = new Stopwatch();
            _timer = new DispatcherTimer(); 
            _timer.Interval = TimeSpan.FromMilliseconds(100);
            _timer.Tick += Timer_Tick; 
            _currentTime = "00:00:00";
        }

        private string _currentTime;
        public string CurrentTime { 
            get => _currentTime; 
            set 
            { 
                if (_currentTime != value) { 
                    _currentTime = value;
                    OnPropertyChanged(nameof(CurrentTime)); 
                } 
            } 
        }

        private void Timer_Tick(object sender, EventArgs e) {
            CurrentTime = _stopwatch.Elapsed.ToString(@"hh\:mm\:ss");

        }

        private ICommand _startStopWatchCommand;
        public ICommand StartStopWatchCommand
        {
            get
            {
                if (_startStopWatchCommand == null)
                {
                   
                    _startStopWatchCommand = new RelayCommand(() =>
                        {
                            _stopwatch.Start(); 
                            _timer.Start();
                        });
                }
                return _startStopWatchCommand;
            }
        }

        private ICommand _stopStopWatchCommand; 
        public ICommand StopStopWatchCommand { 
            get { 
                if (_stopStopWatchCommand == null) 
                {
                    _stopStopWatchCommand = new RelayCommand(() => 
                    { 
                        _stopwatch.Stop(); 
                        _timer.Stop(); 
                    }); 
                } 
                return _stopStopWatchCommand; 
            } 
        }

        private ICommand _resetStopWatchCommand;
        public ICommand ResetStopWatchCommand { get { 
                if (_resetStopWatchCommand == null) 
                { 
                    _resetStopWatchCommand = new RelayCommand(() => 
                    { 
                        _stopwatch.Reset(); 
                        CurrentTime = "00:00:00"; 
                    }); 
                } 
                return _resetStopWatchCommand; 
            } 
        }

        private ICommand _saveResultCommand;
        public ICommand SaveResultCommand
        {
            get
            {
                if (_saveResultCommand == null)
                {
                    _saveResultCommand = new RelayCommand(() =>
                    {
                        _history.WriteResult(_stopwatch.Elapsed);
                        _stopwatch.Reset();
                        CurrentTime = "00:00:00";
                    });
                }
                return _saveResultCommand;
            }
        }

       
    }
}
