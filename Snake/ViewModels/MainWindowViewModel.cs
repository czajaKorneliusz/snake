using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using Snake.HighScores;
using Snake.Models;
using Snake.Others;
using Snake.Strings;
using Snake.Utilities;
using Snake.Views;

namespace Snake.ViewModels
{
    public sealed class MainWindowViewModel : INotifyPropertyChanged
    {
        private readonly IHighScores _highScoreOption =
            new StorageMethodFactory().GetHighScoreOption(ConfigurationManager.AppSettings["highScoreStorageMethod"]);

        private Direction _actualMove;
        private Apple _apple;
        private Level _currentLevel = Level.Lvl1;

        private Uri _pictureSourceUri;
        private Player _player;

        private bool _possibilityToEatHimself;
        private int _score;

        private DispatcherTimer _timer;

        public MainWindowViewModel()
        {
            GameCanvas = new Canvas
            {
                Style = Application.Current.FindResource("GameCanvasStyle") as Style
            };


            StartCommand = new RelayCommand(o => { StartGame(); }, null);
            ShowHighScoresWindowCommand = new RelayCommand(o => { new HighScoreView().ShowDialog(); },
                () => _timer == null || !_timer.IsEnabled
            );

            ShowAboutWindowCommand = new RelayCommand(o => { new AboutWindowView().ShowDialog(); },
                () => _timer == null || !_timer.IsEnabled
            );
        }

        private ObservableCollection<Level> ListOfLevels { get; } =
            new ObservableCollection<Level>((Level[]) Enum.GetValues(typeof(Level)));

        public ObservableCollection<string> ListOfLevelsDescriptions { get; set; } =
            new ObservableCollection<string>(
                Enum.GetNames(typeof(Level)).Select(x => x.Replace("Lvl", Resources.Level)));

        public int CurrentLevel
        {
            get => ListOfLevels.IndexOf(_currentLevel);
            set
            {
                _currentLevel = ListOfLevels[value];

                OnPropertyChanged(nameof(CurrentLevel));
            }
        }

        private int Score
        {
            get => _score;
            set
            {
                _score = value;
                OnPropertyChanged(nameof(ScoreString));
            }
        }

        public Uri PictureSourceUri
        {
            get => _pictureSourceUri;
            set
            {
                _pictureSourceUri = value;
                OnPropertyChanged(nameof(PictureSourceUri));
            }
        }

        public string ScoreString => $"{Resources.Score}: {Score}";
        public Canvas GameCanvas { get; }
        public ICommand StartCommand { get; }

        public object ShowHighScoresWindowCommand { get; }
        public object ShowAboutWindowCommand { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        private void StartGame()
        {
            Score = 0;
            _possibilityToEatHimself = false;
            GameCanvas.Children.Clear();
            _player = new Player();
            _actualMove = _player.LastMove;
            _player.SnakeQueue = new LinkedList<Rectangle>();

            //Rectangle for decoration
            GameCanvas.Children.Add(new Rectangle
            {
                Width = GameCanvas.Width,
                Height = GameCanvas.Height,
                Stroke = Brushes.Black
            });

            if (Application.Current.MainWindow != null) Application.Current.MainWindow.KeyDown += MainWindow_KeyDown;

            var segment = new Rectangle
            {
                Style = Application.Current.FindResource("SnakeSegmentStyle") as Style
            };


            for (var i = 0; i < 4; i++)
                if (DeepCloningTool<Rectangle>.DeepCopy(segment) is Rectangle element)
                {
                    _player.SnakeQueue.AddFirst(element);
                    GameCanvas.Children.Add(element);
                }

            _apple = new Apple();
            var random = new Random();
            _apple.X = random.Next(5, (int) GameCanvas.Width / 10) * 10;
            _apple.Y = random.Next(5, (int) GameCanvas.Height / 10) * 10;
            _apple.AppleEllipse = new Ellipse
            {
                Style = Application.Current.FindResource("AppleStyle") as Style,
                Margin = new Thickness(_apple.X, _apple.Y, 0, 0)
            };

            GameCanvas.Children.Add(_apple.AppleEllipse);
            GameCanvas.Focus();
            SetupTimer();
        }

        private void SetupTimer()
        {
            if (_timer != null) _timer.Tick -= Timer_Tick;

            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds((int) _currentLevel) //FromSeconds(1)
            };
            _timer.Tick += Timer_Tick;
            _timer.Start();
        }

        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            _actualMove = e.Key switch
            {
                Key.Left when _player.LastMove != Direction.Left && _player.LastMove != Direction.Right => Direction
                    .Left,
                Key.Right when _player.LastMove != Direction.Left && _player.LastMove != Direction.Right => Direction
                    .Right,
                Key.Up when _player.LastMove != Direction.Up && _player.LastMove != Direction.Down =>
                Direction.Up,
                Key.Down when _player.LastMove != Direction.Up && _player.LastMove != Direction.Down => Direction
                    .Down,
                _ => _actualMove
            };
        }


        private void Timer_Tick(object sender, EventArgs e)
        {
            var newHead = _player.SnakeQueue.Last.Value;
            _player.SnakeQueue.RemoveLast();


            _player.LastMove = _actualMove;
            _player.MovePlayer();
            newHead.Margin = new Thickness(_player.X, _player.Y, newHead.Margin.Right, newHead.Margin.Bottom);
            _player.SnakeQueue.AddFirst(newHead);


            if (CheckIfWallHit(newHead) || CheckIfSelfHit() && _possibilityToEatHimself)
            {
                GameOver();
                return;
            }


            if (!CheckIfApple()) return;

            if (DeepCloningTool<Rectangle>.DeepCopy(_player.SnakeQueue.Last.Value) is Rectangle element)
            {
                _player.SnakeQueue.AddLast(element);
                GameCanvas.Children.Add(element);
            }

            Score++;
            ChangeSnakePicture();
            MoveApple();
        }

        private void ChangeSnakePicture()
        {
            var random = new Random();
            PictureSourceUri = SnakeImage.ImageSourceList[random.Next(SnakeImage.ImageSourceList.Count - 1)];
        }

        private void GameOver()
        {
            if (_timer != null)
            {
                _timer.Stop();
                _timer.Tick -= Timer_Tick;
            }

            var textBlock = new TextBlock
            {
                Text = Resources.GameOver,
                Foreground = new SolidColorBrush(Colors.GreenYellow),
                Background = new SolidColorBrush(Colors.DimGray)
            };
            Canvas.SetLeft(textBlock, (GameCanvas.ActualWidth - textBlock.FontSize * textBlock.Text.Length / 2) / 2);
            Canvas.SetTop(textBlock, (GameCanvas.ActualHeight - textBlock.FontSize / 2) / 2);
            GameCanvas.Children.Add(textBlock);
            var inputWindowView = new NameInputWindowView();
            if (inputWindowView.ShowDialog() == true)
                try
                {
                    _highScoreOption.SaveHighScore(new HighScoreEntry(inputWindowView.TextBoxForName.Text, Score));
                }
                catch (NullReferenceException e)
                {
                    Console.WriteLine(e);
                }
        }

        private bool CheckIfWallHit(Rectangle newHead)
        {
            return _player.X + newHead.Width > GameCanvas.Width || _player.X < 0 ||
                   _player.Y + newHead.Height > GameCanvas.Height || _player.Y < 0;
        }

        private bool CheckIfSelfHit()
        {
            if (_player.SnakeQueue.Select(x => x.Margin).Distinct().Count() != _player.SnakeQueue.Count) return true;

            _possibilityToEatHimself = true;
            return false;
        }

        private bool CheckIfApple()
        {
            return _player.X.Equals(_apple.X) && _player.Y.Equals(_apple.Y);
        }

        private void MoveApple()
        {
            var random = new Random();
            while (_player.SnakeQueue.Any(x => x.Margin.Left.Equals(_apple.X)) ||
                   _player.SnakeQueue.Any(x => x.Margin.Top.Equals(_apple.Y))) //.X.Any(x=>x.)_apple.X)
            {
                _apple.X = random.Next(0, (int) GameCanvas.Width / 10) * 10;
                _apple.Y = random.Next(0, (int) GameCanvas.Height / 10) * 10;
            }

            _apple.AppleEllipse.Margin = new Thickness(_apple.X, _apple.Y, 0, 0);
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}