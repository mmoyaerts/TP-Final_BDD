using System;
using System.Linq;
using System.Windows.Forms;
using DllGame;

namespace MastermindApp
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MastermindForm());
        }
    }

    public class MastermindForm : Form
    {
        private MastermindGame _game;
        private TextBox _inputBox;
        private Button _proposeButton;
        private Button _resetButton;
        private ListBox _historyList;
        private Label _promptLabel;
        private readonly Random _rng = new Random();

        // Couleurs autorisées uniquement
        private readonly ColorMast[] _allowedPalette = new[]
        {
            ColorMast.Rouge,
            ColorMast.Vert,
            ColorMast.Bleu,
            ColorMast.Jaune
        };

        public MastermindForm()
        {
            InitializeComponent();
            StartNewGame();
        }

        private void InitializeComponent()
        {
            this.Text = "Mastermind";
            this.Width = 400;
            this.Height = 500;

            _promptLabel = new Label
            {
                Text = $"Entrez exactement 4 couleurs parmi : {string.Join(", ", _allowedPalette)} (ex: Rouge, Vert, Bleu, Jaune)",
                AutoSize = true,
                Top = 10,
                Left = 10
            };

            _inputBox = new TextBox
            {
                Width = 350,
                Top = _promptLabel.Bottom + 5,
                Left = 10
            };

            _proposeButton = new Button
            {
                Text = "Proposer",
                Top = _inputBox.Bottom + 10,
                Left = 10
            };
            _proposeButton.Click += ProposeButton_Click;

            _resetButton = new Button
            {
                Text = "Nouvelle Partie",
                Top = _inputBox.Bottom + 10,
                Left = _proposeButton.Right + 10
            };
            _resetButton.Click += ResetButton_Click;

            _historyList = new ListBox
            {
                Top = _proposeButton.Bottom + 10,
                Left = 10,
                Width = 360,
                Height = 300
            };

            this.Controls.AddRange(new Control[]
            {
                _promptLabel,
                _inputBox,
                _proposeButton,
                _resetButton,
                _historyList
            });
        }

        private void StartNewGame()
        {
            _game = new MastermindGame(4, _allowedPalette, 10);
            // Génération aléatoire d'un code secret parmi les 4 couleurs autorisées
            var secret = Enumerable.Range(0, 4)
                .Select(_ => _allowedPalette[_rng.Next(_allowedPalette.Length)])
                .ToList();
            _game.SetSecret(secret);

            _historyList.Items.Clear();
            _historyList.Items.Add("Nouvelle partie commencée !");
        }

        private void ProposeButton_Click(object sender, EventArgs e)
        {
            try
            {
                // Extraction et validation
                var entries = _inputBox.Text.Split(',').Select(s => s.Trim()).ToList();
                if (entries.Count != 4)
                    throw new ArgumentException("Vous devez proposer exactement 4 couleurs.");

                var guess = entries
                    .Select(s =>
                    {
                        if (!Enum.TryParse<ColorMast>(s, true, out var c) || !_allowedPalette.Contains(c))
                            throw new ArgumentException($"Couleur invalide ou non autorisée : {s}");
                        return c;
                    })
                    .ToList();

                // Proposition
                var result = _game.Propose(guess);
                _historyList.Items.Add($"{_inputBox.Text} → {result.Noirs} noirs, {result.Blancs} blancs");

                if (_game.IsFinished)
                {
                    var message = _game.IsWinner ? "Vous avez gagné !" : "Vous avez perdu !";
                    MessageBox.Show(message, "Résultat", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ResetButton_Click(object sender, EventArgs e)
        {
            StartNewGame();
        }
    }
}