/*
*
* Travail pratique #3
* Sabbagh Ziarani, Narges
* Date: 14 decembre 2021
* Description: pomodoro.
*
*/
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
        //Objet statique utilise pour gerer la commande demarrer qui permet de demarrer un pomodoro
        public static RoutedCommand CmdDemarrerPomodoro = new RoutedCommand();
        //Objet statique utilise pour gerer la commande interrompre
        public static RoutedCommand CmdInterromprePomodoro = new RoutedCommand();
        //Objet statique utilise pour gerer la commande Ajouter une tache
        public static RoutedCommand CmdAjouterTache = new RoutedCommand();
        //Objet statique utilise pour gerer la commande ajouter un pomodoro
        public static RoutedCommand CmdAjouterNombrePomodoro = new RoutedCommand();
        //Objet statique utilise pour gerer la commande diminuer un pomodoro
        public static RoutedCommand CmdDiminuerNombrePomodoro = new RoutedCommand();
        //Objet statique utilise pour gerer la commande supprimmer une tache
        public static RoutedCommand CmdSupprimerTache = new RoutedCommand();

        private const int NOMBRE_SECONDES = 60;
        private BackgroundWorker _tempsPomodoro;
        private ViewModel viewModel;


        public MainWindow()
        {
            viewModel = new ViewModel();
            InitializeComponent();
            DataContext = viewModel;
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

            while (secondes > 0 && !worker.CancellationPending)
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
            //si l utilisateur repond oui a cette question on ajoute le nombre de pomodoro completes de la tache
            if (MessageBox.Show("Est-ce que le pomodoro a été complétée?", "Temps expiré", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                viewModel.MettreAJourPomodoro();
            }
            //si l utilisateur repond oui a cette question on dit que la tache a ete complete, en faisant appel a la methode MettreAJOurTAche de la classe viewModel
            if (MessageBox.Show("Est-ce que la tâche a été complétée?", "Temps expiré", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                viewModel.MettreAJourTache();
            }
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
            //si l utilisateur repond oui a cette question on dit que la tache a ete complete, en faisant appel a la methode MettreAJOurTAche de la classe viewModel
            if (MessageBox.Show("Est-ce que la tâche a été complétée?", "Temps expiré", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                viewModel.MettreAJourTache();
            }

        }
        //canExecute de la commande demarrer s active si la tache n est pas encore completee
        private void CommandedDemarrerPomodoro_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = StatutTache != null && StatutTache.Text != "Complétée" && StatutTache.Text != "";
        }
        //execute de la commande demarrer
        private void CommandedDemarrerPomodoro_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            //fait appel a la methode DemarrerPomodoros de l objet viewModel
            viewModel.DemarrerPomodoros();
            DemarrerPomodoro();
        }
        //canexecute de la commande interrompre
        //canExecute est mis a vrai tant qu il y a la minutrie
        private void CommandeInterromprePomodoro_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = _tempsPomodoro != null && _tempsPomodoro.IsBusy;
        }

        //execute de la commande interrompre
        private void CommandeInterromprePomodoro_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            InterromprePomodoro();
        }
        // Fin du code relié à la minuterie 

        //can execute de la commande ajouterpomodoro
        private void CmdAjouterNombrePomodoro_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = NombrePomodoros != null;
        }
        //execute de la commande ajouter pomodoro, tant que champ NombrePomodoros existe celui ci peut etre execute
        private void CmdAjouterNombrePomodoro_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            NombrePomodoros.Text = "" + (int.Parse(NombrePomodoros.Text) + 1);
        }
        //can execute de la commande diminuerpomodoro

        private void CmdDiminuerNombrePomodoro_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = NombrePomodoros != null && int.Parse(NombrePomodoros.Text) != 0;
        }
        //execute de la commande diminuerpomodoro, tant que le nombre de pomodoros est tout autre que 0

        private void CmdDiminuerNombrePomodoro_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            NombrePomodoros.Text = "" + (int.Parse(NombrePomodoros.Text) - 1);
        }
        //can execute de la commande ajouter tache
        private void CmdAjouterTache_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = NombrePomodoros != null && DescriptionTache != null && int.Parse(NombrePomodoros.Text) != 0 && DescriptionTache.Text != "";
        }
        //execute de la commande tache, tant que il y a une description et le nombre de pomodoros prevue est superieur a 0 elle peut etre executee
        private void CmdAjouterTache_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            viewModel.AjouterTache(DescriptionTache.Text, NombrePomodoros.Text);
        }
        //can execute de la commande supprimer tache
        private void CmdSupprimerTache_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {

            e.CanExecute = ListboxTaches != null && ListboxTaches.SelectedIndex >= 0;

        }
        //execute de la commande supprimer, tant que la tache selectionne est valide on peut le supprimer
        private void CmdSupprimerTache_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            //fait appel au methode supprimertache de  l objet viewModel
            viewModel.SupprimerTache();
        }

    }
}
