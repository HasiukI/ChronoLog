using ChronoLog.Repository;
using ChronoLog.ViewModel.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChronoLog.ViewModel
{
    internal class MainViewModel
    {
        public WindowViewModel Window { get; set; }
        public StopWatchViewModel StopWatch { get; set; }
        public DesignViewModel Design { get; set; }
        public HistoryViewModel History { get; set; }

        private FileRepository _repository;

        private string _path;

        public MainViewModel() {
            _path = System.IO.Directory.GetCurrentDirectory();
            _repository = new FileRepository(_path + "\\history.bin", _path + "\\config.bin");

            Window = new WindowViewModel(this);
            History = new HistoryViewModel(_repository);
          
            StopWatch = new StopWatchViewModel(History);
            Design = new DesignViewModel(_repository);
           
        }
    }
}
