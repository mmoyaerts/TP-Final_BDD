
namespace DllGame
{
    public class Response
    {
        public int Noirs { get; }
        public int Blancs { get; }
        public Response(int noirs, int blancs)
        {
            Noirs = noirs;
            Blancs = blancs;
        }
    }

    public class MastermindGame
    {
        private readonly List<ColorMast> _secret;
        public int AttemptCount { get; private set; }
        public bool IsFinished { get; private set; }
        public bool IsWinner { get; private set; }
        private readonly int _maxEssais;

        public MastermindGame(int longueur, IEnumerable<ColorMast> palette, int maxEssais)
        {
            _maxEssais = maxEssais;
            // pour l'instant on ne gère pas la palette – on fixe le secret via SetSecret en test
            _secret = new List<ColorMast>(new ColorMast[longueur]);
        }

        public void SetSecret(IEnumerable<ColorMast> secret)
        {
            _secret.Clear();
            _secret.AddRange(secret);
        }

        public Response Propose(IEnumerable<ColorMast> proposition)
        {
            if (IsFinished) throw new InvalidOperationException("Partie terminée");

            AttemptCount++;
            var guess = proposition.ToList();
            // calcul noirs
            int noirs = _secret.Zip(guess, (s, g) => s == g).Count(b => b);
            // calcul blancs : total correspondances moins noirs, en tenant compte des doublons
            var freqSecret = _secret.GroupBy(x => x).ToDictionary(g => g.Key, g => g.Count());
            int totalMatches = guess
                .Where(g => freqSecret.ContainsKey(g) && freqSecret[g] > 0)
                .GroupBy(g => g)
                .Sum(grp => Math.Min(grp.Count(), freqSecret[grp.Key]));
            int blancs = totalMatches - noirs;

            if (noirs == _secret.Count) { IsFinished = true; IsWinner = true; }
            else if (AttemptCount >= _maxEssais) { IsFinished = true; }

            return new Response(noirs, blancs);
        }

        public void Reset()
        {
            AttemptCount = 0;
            IsFinished = false;
            IsWinner = false;
        }

        public List<ColorMast> GetSecret()
        {
            return _secret;
        }
    }

    public enum ColorMast { Rouge, Vert, Bleu, Jaune, Noir, Blanc }
}