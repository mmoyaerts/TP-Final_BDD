using System;

namespace DllGame
{
    public class JustePrixGame
    {
        private readonly int _minPrix;
        private readonly int _maxPrix;
        private readonly int _maxEssais;
        private int _secretPrix;
        private int _attemptCount;
        private bool _isFinished;
        private bool _isWon;
        private readonly Random _random = new Random();

        public JustePrixGame(int minPrix, int maxPrix, int maxEssais)
        {
            _minPrix = minPrix;
            _maxPrix = maxPrix;
            _maxEssais = maxEssais;
            _attemptCount = 0;
            _isFinished = false;
            _isWon = false;
            GenerateSecret();
        }

        private void GenerateSecret()
        {
            // Génère un prix secret aléatoire entre min et max inclus
            _secretPrix = _random.Next(_minPrix, _maxPrix + 1);
        }

        public int AttemptCount => _attemptCount;
        public int SecretPrice => _secretPrix;
        public bool IsFinished => _isFinished;
        public bool IsWon => _isWon;

        public void SetSecretPrice(int prix)
        {
            if (prix < _minPrix || prix > _maxPrix)
                throw new ArgumentOutOfRangeException(nameof(prix));
            _secretPrix = prix;
        }

        public string Propose(int proposition)
        {
            if (_attemptCount >= _maxEssais)
                throw new TooManyAttemptsException();

            _attemptCount++;

            if (proposition == _secretPrix)
            {
                _isFinished = true;
                _isWon = true;
                return "exact";
            }

            if (proposition < _secretPrix)
                return "trop bas";

            return "trop haut";
        }

        public void Reset()
        {
            _attemptCount = 0;
            _isFinished = false;
            _isWon = false;
            GenerateSecret();
        }
    }
    public class TooManyAttemptsException : Exception
    {
        public TooManyAttemptsException()
            : base("Trop d'essais")
        { }
    }
}
