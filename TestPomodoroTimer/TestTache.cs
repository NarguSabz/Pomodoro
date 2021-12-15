using NUnit.Framework;
using PomodoroTimer;

namespace TestPomodoroTimer
{
    public class Tests
    {
        private Tache tacheTestee;

        [SetUp]
        public void Setup()
        {
        }

        /* Un test bidon */
        [Test]
        public void TestCreation()
        {
            tacheTestee = new Tache("", 1, 1, StatutTache.PLANIFIEE);
            Assert.AreEqual(tacheTestee.StatutTachee, StatutTache.EN_COURS);
        }
    }
}
