using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace PomodoroTimer
{
    public partial class MainWindow : Window
    {

        public static RoutedCommand cmdDemarrerPomodoro = new RoutedCommand();
        public static RoutedCommand cmdInterromprePomodoro = new RoutedCommand();

        private const int NOMBRE_SECONDES = 60;
        private BackgroundWorker _tempsPomodoro;

        public MainWindow()
        {
            InitializeComponent();
        }

        // Début du code pour la minuterie, donné aux étudiants
		// Il devra être adapté aux spécifications du travail
        private void DemarrerPomodoro()
        {
            if (_tempsPomodoro != null && _tempsPomodoro.IsBusy)
            {
                _tempsPomodoro.RunWorkerCompleted -= TerminerPomodoro;
                _tempsPomodoro.ProgressChanged -= AfficherTemps;
                _tempsPomodoro.CancelAsync();
            }

            AfficherTemps(NOMBRE_SECONDES);

            _tempsPomodoro = new BackgroundWorker();
            _tempsPomodoro.WorkerSupportsCancellation = true;
            _tempsPomodoro.WorkerReportsProgress = true;
            _tempsPomodoro.DoWork += DeduireTemps;
            _tempsPomodoro.ProgressChanged += AfficherTemps;
            _tempsPomodoro.RunWorkerCompleted += TerminerPomodoro;
            _tempsPomodoro.RunWorkerAsync();
        }

        private void DeduireTemps(object sender, DoWorkEventArgs e)
        {
            int progress = 0;
            int secondes = NOMBRE_SECONDES;

            BackgroundWorker worker = sender as BackgroundWorker;

            while (secondes > 0 && ! worker.CancellationPending)
            {
                Thread.Sleep(1000);
                secondes--;
                progress++;
                worker.ReportProgress(progress * 100 / NOMBRE_SECONDES, secondes);
            }
        }

        private void AfficherTemps(object sender, ProgressChangedEventArgs e)
        {
            AfficherTemps((int)e.UserState);
        }

        private void AfficherTemps(int secondesRestantes)
        {
            TimeSpan ts = TimeSpan.FromSeconds(secondesRestantes);
            TextTemps.Text = ts.ToString(@"mm\:ss");
        }

        private void TerminerPomodoro(object sender, RunWorkerCompletedEventArgs e)
        {
            MessageBox.Show("Pomodoro terminé");
        }

        private void InterromprePomodoro()
        {
            if (_tempsPomodoro != null && _tempsPomodoro.IsBusy)
            {
                _tempsPomodoro.RunWorkerCompleted -= TerminerPomodoro;
                _tempsPomodoro.ProgressChanged -= AfficherTemps;
                _tempsPomodoro.CancelAsync();
            }
            AfficherTemps(0);
            MessageBox.Show("Pomodoro interrompu");
        }

        private void CommandedDemarrerPomodoro_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void CommandedDemarrerPomodoro_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            DemarrerPomodoro();
        }

        private void CommandeInterromprePomodoro_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = _tempsPomodoro != null && _tempsPomodoro.IsBusy;
        }

        private void CommandeInterromprePomodoro_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            InterromprePomodoro();
        }
        // Fin du code relié à la minuterie 

    }
}
