using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace EPSIC_Bataille_Navale.Views
{
    /// <summary>
    /// Logique d'interaction pour Settings.xaml
    /// </summary>
    public partial class Settings : Page
    {
        public Settings()
        {
            InitializeComponent();
            txt_size.Text = Properties.Settings.Default.size.ToString();
            txt_boatsList.Text = Properties.Settings.Default.boatsList;
            txt_nbMines.Text = Properties.Settings.Default.nbMines.ToString();
            txt_iaSleepTime.Text = Properties.Settings.Default.iaSleepTime.ToString();
        }

        private void Btn_reset_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.Reset();
            txt_size.Text = Properties.Settings.Default.size.ToString();
            txt_boatsList.Text = Properties.Settings.Default.boatsList;
            txt_nbMines.Text = Properties.Settings.Default.nbMines.ToString();
            txt_iaSleepTime.Text = Properties.Settings.Default.iaSleepTime.ToString();
        }

        private void Btn_cancel_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Content = new Home();
        }

        private void Btn_save_Click(object sender, RoutedEventArgs e)
        {
            if (Valid_size() && Valid_boatsList() && Valid_nbMines() && Valid_iaSleepTime())
            {
                Properties.Settings.Default.size = int.Parse(txt_size.Text);
                Properties.Settings.Default.boatsList = txt_boatsList.Text;
                Properties.Settings.Default.nbMines = int.Parse(txt_nbMines.Text);
                Properties.Settings.Default.iaSleepTime = double.Parse(txt_iaSleepTime.Text);
                Properties.Settings.Default.Save();
                Window.GetWindow(this).Content = new Home();
            }
        }

        private bool Valid_size()
        {
            bool error = false;
            int value;
            if (int.TryParse(txt_size.Text, out value))
            {
                if (value < 2 || value > 30)
                {
                    error = true;
                }
            }
            else
            {
                error = true;
            }
            if (error)
            {
                MessageBox.Show("La valeur doit être un nombre compris entre 2 et 30.");
                txt_size.Text = Properties.Settings.Default.size.ToString();
            }
            return !error;
        }

        private bool Valid_boatsList()
        {
            try
            {
                List<int> list = txt_boatsList.Text.Split(',').Select(Int32.Parse).ToList();
                /*for (int i = 0; i < list.Count; i++)
                {
                    if (list[i] < 2 || list[i] > int.Parse(txt_size.Text))
                    {
                        break;
                    }
                }*/
                return true;
            }
            catch (FormatException)
            {
                MessageBox.Show("Chaque valeur doit être séparée par une virgule.");
                txt_boatsList.Text = Properties.Settings.Default.boatsList.ToString();
                return false;
            }
        }

        private bool Valid_nbMines()
        {
            bool error = false;
            int value;
            if (int.TryParse(txt_nbMines.Text, out value))
            {
                if (value < 0 || value > 15)
                {
                    error = true;
                }
            }
            else
            {
                error = true;
            }
            if (error)
            {
                MessageBox.Show("La valeur doit être un nombre compris entre 0 et 15.");
                txt_nbMines.Text = Properties.Settings.Default.size.ToString();
            }
            return !error;
        }

        private bool Valid_iaSleepTime()
        {
            bool error = false;
            double value;
            if (double.TryParse(txt_iaSleepTime.Text, out value))
            {
                if (value < 0 || value > 10)
                {
                    error = true;
                }
            }
            else
            {
                error = true;
            }
            if (error)
            {
                MessageBox.Show("La valeur doit être un nombre à virgule compris entre 0 et 10.");
                txt_iaSleepTime.Text = Properties.Settings.Default.iaSleepTime.ToString();
            }
            return !error;
        }
    }
}
