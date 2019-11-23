using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

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
            Window.GetWindow(this).Content = new Home();
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
                Window.GetWindow(this).Content = new Home();
            }
        }

        /// <summary>
        /// Validation du paramètre boatsList
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
                        MessageBox.Show("Aucune taille de bateau ne peut être plus grande que la grille.");
                        return false;
                    }
                }
                return true;
            }
            catch (FormatException)
            {
                MessageBox.Show("Chaque valeur doit être séparée par une virgule.");
                return false;
            }
        }

        /// <summary>
        /// Validation d'un paramètre de type int
        /// </summary>
        /// <param name="textbox">TextBox</param>
        /// <param name="min">Valeur minimum</param>
        /// <param name="max">Valeur maximum</param>
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
                MessageBox.Show("La valeur de \""+ label.Content+"\" doit être un nombre compris entre "+min+" et "+max+".");
            }
            return !error;
        }

        /// <summary>
        /// Validation d'un paramètre de type double
        /// </summary>
        /// <param name="textbox">TextBox</param>
        /// <param name="min">Valeur minimum</param>
        /// <param name="max">Valeur maximum</param>
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
                MessageBox.Show("La valeur de \"" + label.Content + "\" doit être un nombre compris entre " + min + " et " + max + ".");
            }
            return !error;
        }
    }
}
