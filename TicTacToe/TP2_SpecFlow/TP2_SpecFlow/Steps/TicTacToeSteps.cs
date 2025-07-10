using NUnit.Framework;
using TP3_TicTacToe;
using System;

namespace TP3_SpecFlow.Steps.TicTacToe
{
    [Binding]
    public class TicTacToeSteps
    {
        private TicTacToeManager _game;
        private Exception _caughtException;

        // ----------------------------- GIVEN ----------------------------- //

        [Given("une nouvelle partie de morpion")]
        public void GivenUneNouvellePartie()
        {
            _game = new TicTacToeManager();
            _caughtException = null;
        }

        // ----------------------------- WHEN ----------------------------- //

        [When("le joueur (.*) joue en \\((.*),(.*)\\)")]
        public void WhenLeJoueurJoueEn(string joueur, int x, int y)
        {
            try
            {
                _game.Play(joueur, x, y);
            }
            catch (Exception ex)
            {
                _caughtException = ex;
            }
        }

        // ----------------------------- THEN ----------------------------- //

        [Then("le joueur (.*) doit être déclaré vainqueur")]
        public void ThenLeJoueurDoitEtreDeclareVainqueur(string joueur)
        {
            Assert.AreEqual(joueur, _game.GetWinner());
        }

        [Then("aucun joueur ne doit être déclaré vainqueur")]
        public void ThenAucunJoueurNeDoitEtreDeclareVainqueur()
        {
            Assert.AreEqual("Aucun", _game.GetWinner());
        }

        [Then("la partie doit être déclarée comme terminée par égalité")]
        public void ThenLaPartieDoitEtreDeclareeCommeEgalite()
        {
            Assert.AreEqual(GameStatus.Draw, _game.Status);
        }

        [Then("une erreur doit être levée indiquant que la case est occupée")]
        public void ThenErreurCaseOccupee()
        {
            Assert.IsNotNull(_caughtException);
            Assert.IsTrue(_caughtException.Message.Contains("occupée"));
        }

        [Then("une erreur doit être levée indiquant que ce n'est pas le tour du joueur (.*)")]
        public void ThenErreurTourDuJoueur(string joueur)
        {
            Assert.IsNotNull(_caughtException);
            Assert.IsTrue(_caughtException.Message.Contains($"Ce n'est pas le tour du joueur {joueur}"));
        }

        [Then("une erreur doit être levée indiquant que la partie est déjà terminée")]
        public void ThenErreurPartieTerminee()
        {
            Assert.IsNotNull(_caughtException);
            Assert.IsTrue(_caughtException.Message.Contains("déjà terminée"));
        }

        [Then("une erreur doit être levée indiquant que le joueur est invalide")]
        public void ThenErreurJoueurInvalide()
        {
            Assert.IsNotNull(_caughtException);
            Assert.IsTrue(_caughtException.Message.Contains("Joueur invalide"));
        }

        [Then("une erreur doit être levée indiquant que la position est invalide")]
        public void ThenErreurPositionInvalide()
        {
            Assert.IsNotNull(_caughtException);
            Assert.IsTrue(_caughtException.Message.Contains("coordonnées"));
        }
    }
}
