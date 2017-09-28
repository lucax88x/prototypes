using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using Client.Models;
using RestSharp;

namespace Client.ViewModels
{
    public class CardHolder: INotifyPropertyChanged
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
                _viewModel.Statistics.Count++;

                try
                {
                    Status = TileStatus.Loading;

                    var client = new RestClient("http://issuing-emaissuing.a3c1.starter-us-west-1.openshiftapps.com");

                    var request = new RestRequest("api/cardholder/1", Method.GET);

                    var response = client.Execute<CardHolderModel>(request);

                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        Status = TileStatus.Success;
                        CardHolder.Firstname = response.Data.Firstname;
                        CardHolder.Lastname = response.Data.Lastname;
                        _viewModel.Statistics.Successes++;
                    }
                    else
                    {
                        Status = TileStatus.Failed;
                        _viewModel.Statistics.Errors++;
                    }
                }
                catch (Exception ex)
                {
                    Status = TileStatus.Failed;
                    _viewModel.Statistics.Errors++;
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
        public int _tiles = 2;
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

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class Statistics : INotifyPropertyChanged
    {
        private int _errors;
        private int _successes;
        private int _count;

        public int Errors
        {
            get => _errors;
            set
            {
                _errors = value;
                OnPropertyChanged();
            }
        }
        public int Successes
        {
            get => _successes;
            set
            {
                _successes = value;
                OnPropertyChanged();
            }
        }
        public int Count
        {
            get => _count;
            set
            {
                _count = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class MainWindowViewModel
    {
        public ObservableCollection<Tile> Tiles { get; set; } = new ObservableCollection<Tile>();
        public Settings Settings { get; set; }
        public Statistics Statistics { get; set; }

        public MainWindowViewModel()
        {
            Settings = new Settings(this);
            Statistics = new Statistics();
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