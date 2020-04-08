using System;
using System.Reflection;
using System.Windows;

namespace Snake.ViewModels
{
    public class AboutWindowViewModel
    {
        public AboutWindowViewModel()
        {
            OkCommand = new RelayCommand(o => { ((Window) o).Close(); }, null);
        }

        public RelayCommand OkCommand { get; set; }

        public string AssemblyCopyrightAttribute => ((AssemblyCopyrightAttribute) Attribute.GetCustomAttribute(
            Assembly.GetExecutingAssembly(), typeof(AssemblyCopyrightAttribute), false)).Copyright;
    }
}