using ChronoLog.Model;
using ChronoLog.Repository;
using GalaSoft.MvvmLight.Command;
using SharpVectors.Converters;
using SharpVectors.Renderers.Wpf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace ChronoLog.ViewModel.Components
{
    class DesignViewModel : BaseViewModel
    {
        private readonly FileRepository _repository;

        public List<Model.Color> Colors { get; set; }

        public Dictionary<string, ImageSource> Svgs { get; set; }
        private string[] _svgsName = { "Cycle", "Hide", "History", "Oclock", "Pause", "Play", "Save", "Setting" };

        public Dictionary<string, bool> Visibility { get; set; }

        public Dictionary<string, bool> Pages { get; set; }

        public DesignViewModel(FileRepository repository) {
            _repository = repository;
            SetColor();

            Model.Color selected = Colors.First(c=>c.Main ==_repository.ReadConfig());

            SelectedColor = selected;

            if (selected == null)
                SetSvgs("#bd61d4");
            else
                SetSvgs(selected.Border);
              
            SetVisibility();
            SetPage();
        }

        private Model.Color _selectedColor;
        public Model.Color SelectedColor
        {
            get => _selectedColor;
            set
            {
                if (value == _selectedColor) return;
                _selectedColor = value;
                _repository.WriteConfig(value.Main);
                SetSvgs(value.Border);
                OnPropertyChanged(nameof(Svgs));
                OnPropertyChanged(nameof(SelectedColor));
            }
        }


        private ImageSource UpdateSvgColor(string name, string color)
        {
            string filePath = $"pack://application:,,,/Image/{name}.svg";
            string svgContent;

            using (var stream = Application.GetResourceStream(new Uri(filePath)).Stream)
            using (var reader = new StreamReader(stream))
            {
                svgContent = reader.ReadToEnd();
            }

            svgContent = svgContent.Replace("stroke=\"black\"", $"stroke=\"{color}\"");
            svgContent = svgContent.Replace("fill=\"black\"", $"fill=\"{color}\"");
            return LoadDrawingFromSvgString(svgContent);
        }

        private ImageSource LoadDrawingFromSvgString(string svgContent)
        {
            var converter = new FileSvgReader(new WpfDrawingSettings());
            using (var reader = new StringReader(svgContent))
            {
                var drawingGroup = converter.Read(reader);
                var drawingImage = new DrawingImage(drawingGroup);
                return drawingImage;
            }
        }

        private void SetVisibility() {
            Visibility = new Dictionary<string, bool>();

            Visibility.Add("Start", true);
            Visibility.Add("Pause", false);
            Visibility.Add("Repeat", false);
            Visibility.Add("Save", false);
        }

        private void SetPage()
        {
            Pages = new Dictionary<string, bool>();

            Pages.Add("StopWatch", true);
            Pages.Add("History", false);
            Pages.Add("Setting", false);
        }

        private ICommand _changePageCommand;
        public ICommand ChangePageCommand
        {
            get
            {
                if(_changePageCommand == null)
                {
                    _changePageCommand = new RelayCommand<string>((param) =>
                    {
                        switch (param)
                        {
                            case "StopWatch":
                                Pages["StopWatch"] = true;
                                Pages["History"] = false;
                                Pages["Setting"] = false;
                                break;
                            case "History":
                                Pages["StopWatch"] = false;
                                Pages["History"] = true;
                                Pages["Setting"] = false;
                                break;
                            case "Setting":
                                Pages["StopWatch"] = false;
                                Pages["History"] = false;
                                Pages["Setting"] = true;
                                break;
                        }
                        OnPropertyChanged(nameof(Pages));
                    });
                   
                }
                return _changePageCommand;
            }
        }

        private ICommand _stateStopWatchCommand;
        public ICommand StateStopWatchCommand
        {
            get
            {
                if(_stateStopWatchCommand == null)
                {
                    _stateStopWatchCommand = new RelayCommand<string>((param) =>
                    {
                        switch(param)
                        {
                            case "Start" :
                                Visibility["Start"] = false;
                                Visibility["Pause"] = true;
                                Visibility["Save"] = false;
                                Visibility["Repeat"] = false;
                                break;
                            case "Pause":
                                Visibility["Start"] = true;
                                Visibility["Pause"] = false;
                                Visibility["Save"] = true;
                                Visibility["Repeat"] = true;
                                break;
                            case "Repeat":
                                Visibility["Start"] = true;
                                Visibility["Pause"] = false;
                                Visibility["Save"] = false;
                                Visibility["Repeat"] = false;
                                break;
                            case "Save":
                                Visibility["Start"] = true;
                                Visibility["Pause"] = false;
                                Visibility["Save"] = false;
                                Visibility["Repeat"] = false;
                                break;
                        }
                        OnPropertyChanged(nameof(Visibility));
                    });
                }
                return _stateStopWatchCommand;
            }
        }

        private void SetColor()
        {
            Colors = new List<Model.Color>();
            Colors.Add(new Model.Color { Border = "#efd7f7", Header = "#883398", Main = "#702c7c", Enter = "#5f2966", Font = "#f7ecfb" });
            Colors.Add(new Model.Color { Border = "#bd61d4", Header = "#efd7f7", Main = "#f7ecfb", Enter = "#fcf6fd", Font = "#883398" });
            Colors.Add(new Model.Color { Border = "#5a88d7", Header = "#cadbf3", Main = "#e1ebf8", Enter = "#f2f6fc", Font = "#364c97" });
            Colors.Add(new Model.Color { Border = "#d95a67", Header = "#f7d4d6", Main = "#fbe8e8", Enter = "#fdf3f3", Font = "#a42c41" });
            Colors.Add(new Model.Color { Border = "#f7d4d7", Header = "#a42c41", Main = "#8a273b", Enter = "#772438", Font = "#fbe8e8" });
            Colors.Add(new Model.Color { Border = "#3bc456", Header = "#c2f0ca", Main = "#e0f8e4", Enter = "#f2fbf3", Font = "#217431" });
            Colors.Add(new Model.Color { Border = "#c2f0ca", Header = "#227332", Main = "#1f5c2c", Enter = "#1c4b27", Font = "#e0f8e4" });
            Colors.Add(new Model.Color { Border = "#c9dbf4", Header = "#466dca", Main = "#3c5cb9", Enter = "#304278", Font = "#e1ebf8" });

            Colors.Add(new Model.Color { Border = "#7c7c7c", Header = "#dcdcdc", Main = "#efefef", Enter = "#ffffff", Font = "#525252" });
            Colors.Add(new Model.Color { Border = "#d1d1d1", Header = "#5d5d5d", Main = "#4f4f4f", Enter = "#454545", Font = "#e7e7e7" });

            Colors.Add(new Model.Color { Border = "#e54f83", Header = "#f9d1e2", Main = "#fbe8f0", Enter = "#fdf2f6", Font = "#d32f60" });
            Colors.Add(new Model.Color { Border = "#ff9af2", Header = "#ff2cd6", Main = "#db0097", Enter = "#b4007d", Font = "#ffc7f9" });
           
        }

        private void SetSvgs(string color)
        {
            Svgs = new Dictionary<string, ImageSource>();

            foreach(string name in _svgsName)
            {
                Svgs.Add(name, UpdateSvgColor(name, color));
            }
      
        }

        
    }
}
