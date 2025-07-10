using System;
using System.Linq;
using DllGame;
using FluentAssertions;
using TechTalk.SpecFlow;

namespace JustePrix.Specs.StepsDef
{
    [Binding]
    public class JustePrixSteps
    {
        private JustePrixGame ?_game;
        private string ?_lastResponse;
        private Exception ?_caughtException;
        private int _minPrix, _maxPrix, _maxEssais;
        private int _oldSecret;

        [Given(@"une nouvelle partie du Juste Prix avec")]
        public void GivenUneNouvellePartie(Table table)
        {
            var row = table.Rows.First();
            _minPrix = int.Parse(row["minPrix"]);
            _maxPrix = int.Parse(row["maxPrix"]);
            _maxEssais = int.Parse(row["maxEssais"]);

            _game = new JustePrixGame(_minPrix, _maxPrix, _maxEssais);
        }

        [Given(@"le prix secret est (\d+)")]
        public void GivenLePrixSecretEst(int prix)
        {
            _game.SetSecretPrice(prix);
        }

        [Given(@"j’ai déjà fait (\d+) essais")]
        public void GivenJaiDejaFaitEssais(int essais)
        {
            // On fait des propositions hors-du-secret pour épuiser les essais
            for (int i = 0; i < essais; i++)
                _game.Propose(_minPrix);
        }

        [When(@"je propose (\d+)")]
        public void WhenJePropose(int proposition)
        {
            try
            {
                _lastResponse = _game.Propose(proposition);
                _caughtException = null;
            }
            catch (Exception ex)
            {
                _caughtException = ex;
            }
        }

        [Then(@"une exception TooManyAttemptsException est levée")]
        public void ThenExceptionTooManyAttempts()
        {
            _caughtException.Should().BeOfType<TooManyAttemptsException>();
        }

        [Then(@"la réponse est ""(.*)""")]
        public void ThenLaReponseEst(string attendu)
        {
            _lastResponse.Should().Be(attendu);
        }

        [Then(@"la partie n’est pas terminée")]
        public void ThenLaPartieNestPasTerminee()
        {
            _game.IsFinished.Should().BeFalse();
        }

        [Then(@"la partie est terminée et gagnée")]
        public void ThenLaPartieEstTermineeEtGagnee()
        {
            _game.IsFinished.Should().BeTrue();
            _game.IsWon.Should().BeTrue();
        }

        [When(@"on réinitialise la partie")]
        public void WhenOnReinitialiseLaPartie()
        {
            _oldSecret = _game.SecretPrice;
            _game.Reset();
        }

        [Then(@"le nombre d’essais est égal à (\d+)")]
        public void ThenLeNombreDEssaisEstEgalA(int expected)
        {
            _game.AttemptCount.Should().Be(expected);
        }

        [Then(@"un nouveau prix secret est généré")]
        public void ThenUnNouveauPrixSecretEstGenere()
        {
            _game.SecretPrice.Should().NotBe(_oldSecret);
        }
    }
}
