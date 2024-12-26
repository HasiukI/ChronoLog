using ChronoLog.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using GalaSoft.MvvmLight.Command;
using System.Globalization;
using System.IO;
using System.Windows.Input;
using System.Windows.Shapes;
using static System.Runtime.InteropServices.JavaScript.JSType;
using ChronoLog.Repository;

namespace ChronoLog.ViewModel.Components
{
    class HistoryViewModel : BaseViewModel
    {
        private readonly FileRepository _repository;

        public ObservableCollection<History> Histories {  get; set; }

        public HistoryViewModel(FileRepository repository) {
            _repository = repository;
            Histories = new ObservableCollection<History>(_repository.GetHistories());
        }

        public void WriteResult(TimeSpan duration)
        {
            var newHistory = new History { Date = DateTime.Now, Duration = duration };
            Histories.Add(newHistory);
            _repository.WriteHistory(newHistory);
        }
    }
}

