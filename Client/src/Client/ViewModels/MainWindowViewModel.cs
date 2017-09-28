using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.ComTypes;
using System.Threading;
using System.Windows.Media;
using System.Windows.Threading;
using Client.Models;
using LiveCharts;
using LiveCharts.Configurations;
using LiveCharts.Wpf;
using RestSharp;

namespace Client.ViewModels
{
    public class CardHolder : INotifyPropertyChanged
    {
        private string _firstname;
        private string _lastname;
        private string _version;

        public string Firstname
        {
            get => _firstname;
            set
            {
                _firstname = value;
                OnPropertyChanged();
                OnPropertyChanged("Name");
            }
        }

        public string Lastname
        {
            get => _lastname;
            set
            {
                _lastname = value;
                OnPropertyChanged();
                OnPropertyChanged("Name");
            }
        }

        public string Version
        {
            get => _version;
            set
            {
                _version = value;
                OnPropertyChanged();
            }
        }

        public string Name => $"{Firstname} {Lastname}";

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public enum TileStatus
    {
        Loading = 0,
        Success = 1,
        Failed = -1
    }

    public class Tile : INotifyPropertyChanged, IDisposable
    {
        public TileStatus _status;
        public CardHolder _cardHolder;

        public CardHolder CardHolder
        {
            get => _cardHolder;
            set
            {
                _cardHolder = value;
                OnPropertyChanged();
            }
        }

        public TileStatus Status
        {
            get => _status;
            set
            {
                _status = value;
                OnPropertyChanged();
                OnPropertyChanged("IsLoading");
                OnPropertyChanged("IsSuccess");
                OnPropertyChanged("IsFailed");
            }
        }

        public bool IsLoading
        {
            get { return _status == TileStatus.Loading; }
        }

        public bool IsSuccess
        {
            get { return _status == TileStatus.Success; }
        }

        public bool IsFailed
        {
            get { return _status == TileStatus.Failed; }
        }

        private MainWindowViewModel _viewModel;

        public Tile(MainWindowViewModel viewModel)
        {
            _viewModel = viewModel;

            Load();
        }

        private void DelayLoad()
        {
            Thread.Sleep(_viewModel.Settings.Delay);
            Load();
        }


        private BackgroundWorker _bw;

        private void Load()
        {
            _bw = new BackgroundWorker();

            _bw.DoWork += (evt, args) =>
            {
                _viewModel.Statistics.AddCount();

                try
                {
                    Status = TileStatus.Loading;

                    var client = new RestClient("http://issuing-emaissuing.a3c1.starter-us-west-1.openshiftapps.com");

                    var request = new RestRequest("api/cardholder/1", Method.GET) {Timeout = 2000};

                    var response = client.Execute<CardHolderModel>(request);

                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        Status = TileStatus.Success;
                        CardHolder.Firstname = response.Data.Firstname;
                        CardHolder.Lastname = response.Data.Lastname;
                        _viewModel.Statistics.AddSuccesses();
                    }
                    else
                    {
                        Status = TileStatus.Failed;
                        _viewModel.Statistics.AddErrors();
                    }
                }
                catch (Exception ex)
                {
                    Status = TileStatus.Failed;
                    _viewModel.Statistics.AddErrors();
                }

                DelayLoad();
            };

            _bw.RunWorkerAsync();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Dispose()
        {
            _bw.CancelAsync();
            _bw.Dispose();
        }
    }

    public class Settings : INotifyPropertyChanged
    {
        public int _tiles = 15;
        public int _delay = 5000;

        public int Tiles
        {
            get => _tiles;
            set
            {
                _tiles = value;
                OnPropertyChanged();
                _viewModel.Update(Tiles);
            }
        }

        public int Delay
        {
            get => _delay;
            set => _delay = value;
        }

        private readonly MainWindowViewModel _viewModel;

        public Settings(MainWindowViewModel viewModel)
        {
            _viewModel = viewModel;
            _viewModel.Update(Tiles);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class MeasureModel
    {
        public DateTime DateTime { get; set; }
        public double Value { get; set; }
    }
    
    public class Statistics : INotifyPropertyChanged
    {
        private double _axisMax;
        private double _axisMin;
        
        private int _count = 0;
        private int _errors = 0;
        private int _successes = 0;
        private int _processedCount = 0;
        private int _processedSuccesses = 0;
        private int _processedErrors  = 0;

        public Statistics()
        {
            var mapper = Mappers.Xy<MeasureModel>()
                .X(model => model.DateTime.Ticks) 
                .Y(model => model.Value);
 
            Charting.For<MeasureModel>(mapper);
 
            CountValues = new ChartValues<MeasureModel>();
            SuccessesValues = new ChartValues<MeasureModel>();
            ErrorsValues = new ChartValues<MeasureModel>();
 
            DateTimeFormatter = value => new DateTime((long) value).ToString("mm:ss");
 
            AxisStep = TimeSpan.FromSeconds(1).Ticks;
            AxisUnit = TimeSpan.TicksPerSecond;
 
            SetAxisLimits(DateTime.Now);
 
            Timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(1000)
            };
            Timer.Tick += TimerOnTick;
            Timer.Start();
        }
        
        public ChartValues<MeasureModel> CountValues { get; set; }
        public ChartValues<MeasureModel> SuccessesValues { get; set; }
        public ChartValues<MeasureModel> ErrorsValues { get; set; }
        public Func<double, string> DateTimeFormatter { get; set; }
        public double AxisStep { get; set; }
        public double AxisUnit { get; set; }
        public DispatcherTimer Timer { get; set; }
 
        public double AxisMax
        {
            get => _axisMax;
            set
            {
                _axisMax = value;
                OnPropertyChanged();
            }
        }
        public double AxisMin
        {
            get => _axisMin;
            set
            {
                _axisMin = value;
                OnPropertyChanged();
            }
        }
        
        public int Count
        {
            get => _count;            
        }
        
        public int Errors
        {
            get => _errors;            
        }
        
        public int Successes
        {
            get => _successes;            
        }
 
        private void TimerOnTick(object sender, object eventArgs)
        {
            var now = DateTime.Now;

            var toProcessCount = _count - _processedCount;
            var toProcessSuccesses = _successes - _processedSuccesses;
            var toProcessErrors = _errors - _processedErrors;

            if (toProcessCount > 0)
            {
                CountValues.Add(new MeasureModel
                {
                    DateTime = now,
                    Value = toProcessCount
                });

                _processedCount = _count;
                if (CountValues.Count > 30) CountValues.RemoveAt(0);
            }
            
            if (toProcessSuccesses > 0)
            {
                SuccessesValues.Add(new MeasureModel
                {
                    DateTime = now,
                    Value = toProcessSuccesses
                });

                _processedSuccesses = _successes;
                if (SuccessesValues.Count > 30) SuccessesValues.RemoveAt(0);
            }
            
            
            if (toProcessErrors > 0)
            {
                ErrorsValues.Add(new MeasureModel
                {
                    DateTime = now,
                    Value = toProcessErrors
                });

                _processedErrors = _errors;
                if (ErrorsValues.Count > 30) ErrorsValues.RemoveAt(0);
            }

            SetAxisLimits(now);
        }
 
        private void SetAxisLimits(DateTime now)
        {
            AxisMax = now.Ticks + TimeSpan.FromSeconds(1).Ticks; // lets force the axis to be 1 second ahead
            AxisMin = now.Ticks - TimeSpan.FromSeconds(8).Ticks; // and 8 seconds behind
        } 

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void AddCount()
        {
            Interlocked.Add(ref _count, 1);
        }
        
        public void AddSuccesses()
        {
            Interlocked.Add(ref _successes, 1);
        } 
        
        public void AddErrors()
        {
            Interlocked.Add(ref _errors, 1);
        }
    }

    public class MainWindowViewModel
    {
        public ObservableCollection<Tile> Tiles { get; set; } = new ObservableCollection<Tile>();
        public Settings Settings { get; set; }
        public Statistics Statistics { get; set; }

        public MainWindowViewModel()
        {
            Statistics = new Statistics();

            Settings = new Settings(this);
        }

        public void Update(int count)
        {
            Tiles.Clear();

            for (int i = 0; i < count; i++)
            {
                Tiles.Add(new Tile(this));
            }
        }
    }
}