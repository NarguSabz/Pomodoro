using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

[assembly: InternalsVisibleTo("TestTache")]

namespace PomodoroTimer
{
    public class Tache
    {
        private StatutTache statutTache;
        private int nombrePomodorosTerminee;
        public string DescriptionTache
        {
            get;
            set;
        }
        public int NombrePomodorosPrevue
        {
            get;
            set;
        }

        public int NombrePomodorosTerminee
        {
            get { return nombrePomodorosTerminee; }
            set
            {
                nombrePomodorosTerminee = value;

                if (nombrePomodorosTerminee > 0 && statutTache != StatutTache.COMPLETEE)
                {
                    statutTache = StatutTache.EN_COURS;
                }
            }
        }
        public StatutTache StatutTachee
        {
            get { return statutTache; }
            set
            {
                if (value == StatutTache.COMPLETEE)
                {
                    statutTache = value;
                    NombrePomodorosTerminee += 1;
                }

            }
        }
        public Tache(string descriptionTache, int nombrePomodorosPrevue, int nombrePomodorosTerminee, StatutTache statutTachee)
        {
            DescriptionTache = descriptionTache;
            NombrePomodorosPrevue = nombrePomodorosPrevue;
            NombrePomodorosTerminee = nombrePomodorosTerminee;
            StatutTachee = statutTachee;
        }
        //cette methode toString() permet de determiner le format de la representation sous forme de chaine de caracteres d une tache
        public override string ToString()
        {
            //retourn la description de la tache
            return DescriptionTache;
        }

        //cette constructeur permet d initialiser chaque tache  a partir d un element xml
        //recoit en parametre un element tache xml
        public Tache(XmlElement tacheXml)
        {
            //affecte les variables d instances a partir de differents noeds qui sont dans l element tache d un document xml
            DescriptionTache = tacheXml.GetElementsByTagName("Description")[0].InnerText;
            NombrePomodorosPrevue = int.Parse(tacheXml.GetAttribute("PomodorosPrevus"));
            NombrePomodorosTerminee = int.Parse(tacheXml.GetAttribute("PomodorosCompletes"));
            switch (tacheXml.GetAttribute("Statut"))
            {
                case "PLANIFIEE":
                    statutTache = StatutTache.PLANIFIEE;
                    break;
                case "EN_COURS":
                    StatutTachee = StatutTache.EN_COURS;
                    break;
                case "COMPLETEE":
                    StatutTachee = StatutTache.COMPLETEE;
                    break;
            }
        }
        //ceci permet de creer une partie du document xml qui contient le taches
        internal XmlElement ToXml(XmlDocument document)
        {
            XmlElement elementTache = document.CreateElement("Tache");
            elementTache.SetAttribute("Statut", statutTache.ToString());
            elementTache.SetAttribute("PomodorosPrevus", NombrePomodorosPrevue.ToString());
            elementTache.SetAttribute("PomodorosCompletes", NombrePomodorosTerminee.ToString());
            XmlElement noedDescription = document.CreateElement("Description");
            noedDescription.InnerText = DescriptionTache;
            elementTache.AppendChild(noedDescription);
            return elementTache;
        }
    }
}
