using Avalonia.Controls;
using System.IO;

namespace _1x6Helper.Views
{
    public partial class MainWindow : Window
    {
        private const string SavePath = "imba_heroes.json";
        public MainWindow()
        {

            InitializeComponent();
            if (!File.Exists(SavePath))
            {
                File.WriteAllText(SavePath, "{}");
            }
        }
    }
}