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

        public MainViewModel() { 
            Window = new WindowViewModel();
        }
    }
}
