using PomodoroTimer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

public class Model
{
    private string fichierXML = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/taches.xml";
    private XmlDocument document;

    //liste qui heberge les taches
    public List<Tache> LesTaches
    {
        private set;
        get;
    }

    public Model()
    {
        LesTaches = new List<Tache>();
        document = new XmlDocument();

        if (File.Exists(fichierXML))
        {
            document.Load(fichierXML);
            ChargerXML();
        }
    }

    //ceci permet de charger le fichier xml
    private void ChargerXML()
    {
        //on indique la racine du document xml
        XmlElement? racine = document.DocumentElement;
        if (racine != null)
        {
            LireTachesFichierXml(racine);
        }
    }

    //ceci permet de sauvegarder le fichier xml
    internal void SauvegarderXML()
    {
        document = new XmlDocument();
        XmlElement racine = document.CreateElement("Taches");
        document.AppendChild(racine);
        foreach (Tache tache in LesTaches)
        {
            racine.AppendChild(tache.ToXml(document));
        }
        document.Save(fichierXML);
    }
    //cette methode permet de lire les taches a partir de la racine du fichier xml et les mettre dans la liste du  taches
    private void LireTachesFichierXml(XmlElement racine)
    {
        //on cree un liste de noeds qui sont dans l element racine
        XmlNodeList lesTachesXml = racine.GetElementsByTagName("Tache");
        //on itere dans le noedlist qu on vient de creer et ajoute chaque tache dans la liste des taches
        foreach (XmlElement tache in lesTachesXml)
        {
            LesTaches.Add(new Tache(tache));
        }

    }
    //ceci permet de supprimer une tache
    internal void SupprimerTache(int indice)
    {
        LesTaches.RemoveAt(indice);
    }
    //ceci permet d ajouter une tache
    internal void AjouterTache(Tache uneTache)
    {
        LesTaches.Add(uneTache);
    }
    //ceci permet de mettre a jour une tache lors qu elle est declaree terminee par l utilisateur
    public void MettreAJourTacheFin(int index)
    {
        LesTaches[index].StatutTachee = StatutTache.COMPLETEE;
    }
    //cette methode permet de mettre a jour le pomodoro
    public void MettreAJourPomodoro(int index)
    {

        LesTaches[index].NombrePomodorosTerminee++;
    }
}
