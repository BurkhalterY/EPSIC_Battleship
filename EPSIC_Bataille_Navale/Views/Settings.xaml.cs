using System.Windows;
using System.Windows.Controls;

namespace EPSIC_Bataille_Navale.Views
{
    /// <summary>
    /// Logique d'interaction pour Credits.xaml
    /// </summary>
    public partial class Settings : Page
    {
        public Settings()
        {
            InitializeComponent();
        }

        private void Btn_back_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Content = new Home();
        }
    }
}
