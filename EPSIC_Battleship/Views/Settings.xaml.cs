using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using EPSIC_Battleship.I18n;

namespace EPSIC_Battleship.Views
{
    /// <summary>
    /// Interaction logic for Settings.xaml
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
            txt_nbSonars.Text = Properties.Settings.Default.nbSonars.ToString();
            txt_nbNuclearBombs.Text = Properties.Settings.Default.nbNuclearBombs.ToString();
            txt_nuclearBombRange.Text = Properties.Settings.Default.nuclearBombRange.ToString();
        }

        private void Btn_reset_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.Reset();
            txt_size.Text = Properties.Settings.Default.size.ToString();
            txt_boatsList.Text = Properties.Settings.Default.boatsList;
            txt_nbMines.Text = Properties.Settings.Default.nbMines.ToString();
            txt_iaSleepTime.Text = Properties.Settings.Default.iaSleepTime.ToString();
            txt_nbSonars.Text = Properties.Settings.Default.nbSonars.ToString();
            txt_nbNuclearBombs.Text = Properties.Settings.Default.nbNuclearBombs.ToString();
            txt_nuclearBombRange.Text = Properties.Settings.Default.nuclearBombRange.ToString();
        }

        private void Btn_cancel_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.LoadPage(new Home());
        }

        private void Btn_save_Click(object sender, RoutedEventArgs e)
        {
            if (ValidInt(txt_size, 2, 50, lbl_size)
             && Valid_boatsList()
             && ValidInt(txt_nbMines, 0, 50, lbl_nbMines)
             && ValidDouble(txt_iaSleepTime, 0, 5, lbl_iaSleepTime)
             && ValidInt(txt_nbSonars, 0, 50, lbl_nbSonars)
             && ValidInt(txt_nbNuclearBombs, 0, 50, lbl_nbNuclearBombs)
             && ValidDouble(txt_nuclearBombRange, 1, 10, lbl_nuclearBombRange))
            {
                Properties.Settings.Default.size = int.Parse(txt_size.Text);
                Properties.Settings.Default.boatsList = txt_boatsList.Text;
                Properties.Settings.Default.nbMines = int.Parse(txt_nbMines.Text);
                Properties.Settings.Default.iaSleepTime = double.Parse(txt_iaSleepTime.Text);
                Properties.Settings.Default.nbSonars = int.Parse(txt_nbSonars.Text);
                Properties.Settings.Default.nbNuclearBombs = int.Parse(txt_nbNuclearBombs.Text);
                Properties.Settings.Default.nuclearBombRange = double.Parse(txt_nuclearBombRange.Text);
                Properties.Settings.Default.Save();
                MainWindow.LoadPage(new Home());
            }
        }

        /// <summary>
        /// Validate boatsList parameter
        /// </summary>
        /// <returns>isValid</returns>
        private bool Valid_boatsList()
        {
            try
            {
                List<int> list = txt_boatsList.Text.Split(',').Select(int.Parse).ToList();
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i] < 2 || list[i] > int.Parse(txt_size.Text))
                    {
                        MessageBox.Show(Strings.ErrParamBoatGtGrid);
                        return false;
                    }
                }
                return true;
            }
            catch (FormatException)
            {
                MessageBox.Show(Strings.ErrParamBoatListFormat);
                return false;
            }
        }

        /// <summary>
        /// Validate parameters of type `int`
        /// </summary>
        /// <param name="textbox">TextBox</param>
        /// <param name="min">Minimum value</param>
        /// <param name="max">Maximum value</param>
        /// <param name="label">Label</param>
        /// <returns></returns>
        private bool ValidInt(TextBox textbox, int min, int max, Label label)
        {
            bool error = false;
            int value;
            if (int.TryParse(textbox.Text, out value))
            {
                if (value < min || value > max)
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
                MessageBox.Show(string.Format(Strings.ErrParamNumber, label.Content, min, max));
            }
            return !error;
        }

        /// <summary>
        /// Validate parameters of type `double`
        /// </summary>
        /// <param name="textbox">TextBox</param>
        /// <param name="min">Minimum value</param>
        /// <param name="max">Maximum value</param>
        /// <param name="label">Label</param>
        /// <returns></returns>
        private bool ValidDouble(TextBox textbox, double min, double max, Label label)
        {
            bool error = false;
            double value;
            if (double.TryParse(textbox.Text, out value))
            {
                if (value < min || value > max)
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
                MessageBox.Show(string.Format(Strings.ErrParamNumber, label.Content, min, max));
            }
            return !error;
        }
    }
}
