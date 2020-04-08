using System.Windows;

namespace Snake.ViewModels
{
    public class NameInputWindowViewModel
    {
        private string _textBoxString;


        public NameInputWindowViewModel()
        {
            OkCommand = new RelayCommand(o =>
            {
                ((Window) o).DialogResult = true;
                ((Window) o).Close();
            }, () => !string.IsNullOrEmpty(_textBoxString));
        }

        public string TextBoxString
        {
            get => _textBoxString;
            set
            {
                _textBoxString = value;
                OkCommand.RaiseCanExecuteChanged();
            }
        }


        public RelayCommand OkCommand { get; set; }
    }
}