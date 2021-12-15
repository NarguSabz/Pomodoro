using PomodoroTimer;
using System.Collections.ObjectModel;
using System.ComponentModel;

public class ViewModel : INotifyPropertyChanged

{
    //variable qui va heberge le model
    private Model leModel;
    //indice de la tache qui est selectionne actuellement
    private int indiceTacheSelectionne;

    //indice de la tache qui a ete demarre est ici
    public int IndexTacheDemarree
    {
        get;
        set;
    }
    public int IndiceTacheSelectionne
    {
        get
        {
            return indiceTacheSelectionne;
        }
        set
        {
            //elle est changee siile selecteditem du listbox chnage, puisque elle est liee a selecteditem
            indiceTacheSelectionne = value;
            UpdateTache();
        }
    }
    //liste observable qui cntient les taches 
    public ObservableCollection<Tache> ListeTaches
    {
        get;
        set;
    }


    public event PropertyChangedEventHandler? PropertyChanged;

    //les variables suivants contient les infos de la tache selectionnne

    //description de la tache actuelle
    public string DescriptionTache
    {
        get
        {

            return ListeTaches[IndiceTacheSelectionne].DescriptionTache;

        }
    }
    //statut de la tache actuelle
    public string StatutTachee
    {
        get
        {
            if (indiceTacheSelectionne < 0) { return ""; }
            else
            {
                switch (ListeTaches[IndiceTacheSelectionne].StatutTachee)
                {
                    case StatutTache.COMPLETEE:
                        return "Complétée";
                        break;
                    case StatutTache.PLANIFIEE:
                        return "Planifiée";
                        break;
                    case StatutTache.EN_COURS:
                        return "En cours";
                        break;
                }
            }
            return null;
        }
    }
    //pomodoros completees de la tache actuelle
    public int PomodorosCompleteeTache
    {
        get
        {
            if (indiceTacheSelectionne < 0) { return 0; }
            else
            {
                return ListeTaches[IndiceTacheSelectionne].NombrePomodorosTerminee;
            }
        }

    }
    //pomodoros prevus de la tache actuelle
    public int PomodorosPrevueTache
    {
        get
        {
            if (indiceTacheSelectionne < 0) { return 0; }
            else
            {
                return ListeTaches[IndiceTacheSelectionne].NombrePomodorosPrevue;
            }
        }


    }
    //constructeur de la classe ViewModel
    public ViewModel()
    {
        leModel = new Model();
        //on cree une copie de la liste de taches qui est dans l objet model
        ListeTaches = new ObservableCollection<Tache>(leModel.LesTaches);
        indiceTacheSelectionne = -1;
    }

    //cette methode permet de mettre a jour une tache apres que l utilisateur a declare sa fin
    public void MettreAJourTache()
    {
        leModel.MettreAJourTacheFin(IndexTacheDemarree);

        UpdateTache();
        leModel.SauvegarderXML();
    }
    //cette methode permet de mettre a jour la tache, quand l utilisateur dit que le pomodoros est fini a la fin de minutrie
    public void MettreAJourPomodoro()
    {
        leModel.MettreAJourPomodoro(IndexTacheDemarree);
        UpdateTache();
        leModel.SauvegarderXML();
    }
    //cette methode permet de definir l indice de la tache qui a ete demarre par l utilisateur pour plus tard dans le programme
    public void DemarrerPomodoros()
    {
        IndexTacheDemarree = indiceTacheSelectionne;
        UpdateTache();
        leModel.SauvegarderXML();

    }
    //cette methode permet de supprimer une tache
    public void SupprimerTache()
    {
        leModel.SupprimerTache(indiceTacheSelectionne);
        ListeTaches.RemoveAt(indiceTacheSelectionne);
        if (indiceTacheSelectionne < IndexTacheDemarree)
        {
            //si une tache qui est situe avant d une tache qui est demarre est supprime l indice de la tache demarre diminue
            IndexTacheDemarree--;
        }

        leModel.SauvegarderXML();

    }
    //cette methode permet d ajouter une tache
    public void AjouterTache(string descriptionTache, string nbrePomodorosPrevue)
    {
        Tache nouveauTache = new Tache(descriptionTache, int.Parse(nbrePomodorosPrevue), 0, StatutTache.PLANIFIEE);
        leModel.AjouterTache(nouveauTache);
        UpdateTache();
        ListeTaches.Add(nouveauTache);
        UpdateTache();
        leModel.SauvegarderXML();
    }
    //cette methode permet de mettre a jour les proprietes d une tache 
    private void UpdateTache()
    {
        OnPropertyChange("DescriptionTache");
        OnPropertyChange("StatutTachee");
        OnPropertyChange("PomodorosCompleteeTache");
        OnPropertyChange("PomodorosPrevueTache");
    }


    public void OnPropertyChange(string name)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

}