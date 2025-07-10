using DllGame;
using FluentAssertions;
using System;
using System.Linq;
using TechTalk.SpecFlow;

namespace Mastermind.Specs.StepDefinitions
{
    [Binding]
    public class MastermindSteps
    {
        private MastermindGame ?_game;
        private Response ?_result;
        private Exception? _caughtException;
        private ColorMast[]? _previousSecret;

        [Given(@"une nouvelle partie de Mastermind avec")]
        public void GivenUneNouvellePartie(Table table)
        {
            var row = table.Rows[0];
            var longueur = int.Parse(row["longueurDuCode"]);
            var palette = row["couleurs"].Split(", ").Select(Enum.Parse<ColorMast>);
            var maxEssais = int.Parse(row["maxEssais"]);
            _game = new MastermindGame(longueur, palette, maxEssais);

            // Mémoriser le secret initial pour tester Reset
            _previousSecret = _game.GetSecret().ToArray();
        }

        [Given(@"le code secret est ""(.*)""")]
        public void GivenLeCodeSecretEst(string code)
        {
            var seq = code.Split(", ").Select(Enum.Parse<ColorMast>);
            _game.SetSecret(seq);
        }

        [Given(@"attemptCount is (\d+)")]
        public void GivenAttemptCountIs(int count)
        {
            typeof(MastermindGame)
                .GetProperty("AttemptCount")!
                .SetValue(_game, count);
        }

        [Given(@"la partie est terminée")]
        public void GivenLaPartieEstTerminee()
        {
            typeof(MastermindGame)
                .GetProperty("IsFinished")!
                .SetValue(_game, true);
        }

        [When(@"le joueur propose ""(.*)""")]
        public void WhenLeJoueurPropose(string proposition)
        {
            var guess = proposition.Split(", ").Select(Enum.Parse<ColorMast>);
            try
            {
                _result = _game.Propose(guess);
            }
            catch (Exception ex)
            {
                _caughtException = ex;
            }
        }

        [When(@"on réinitialise la partie")]
        public void WhenOnReinitialiseLaPartie()
        {
            _game.Reset();
        }

        [Then(@"la réponse contient (\d+) pion(?:s)? noir(?:s)? et (\d+) pion(?:s)? blanc(?:s)?")]
        public void ThenLaReponseContientNoirsBlancs(int noirs, int blancs)
        {
            _result.Noirs.Should().Be(noirs);
            _result.Blancs.Should().Be(blancs);
        }

        [Then(@"la partie est terminée et le joueur a gagné")]
        public void ThenLaPartieEstTermineeEtLeJoueurAGagne()
        {
            _game.IsFinished.Should().BeTrue();
            _game.IsWinner.Should().BeTrue();
        }

        [Then(@"la partie est terminée et le joueur a perdu")]
        public void ThenLaPartieEstTermineeEtLeJoueurAPerdu()
        {
            _game.IsFinished.Should().BeTrue();
            _game.IsWinner.Should().BeFalse();
        }

        [Then(@"attemptCount est égal à (\d+)")]
        public void ThenAttemptCountEstEgalA(int expected)
        {
            _game.AttemptCount.Should().Be(expected);
        }

        [Then(@"un nouveau code secret est généré")]
        public void ThenUnNouveauCodeSecretEstGenere()
        {
            var newSecret = _game.GetSecret().ToArray();
            newSecret.Should().HaveCount(_previousSecret!.Length);
            newSecret.Should().NotBeEquivalentTo(_previousSecret);
        }

        [Then(@"une exception InvalidOperationException est levée")]
        public void ThenUneExceptionInvalidOperationExceptionEstLevee()
        {
            _caughtException.Should().BeOfType<InvalidOperationException>()
                .Which.Message.Should().Be("Partie terminée");
        }
    }
}
