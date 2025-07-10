using System;
using System.Drawing;
using System.Windows.Forms;
using DllGame;

namespace JustePrixUI
{
    public class MainForm : Form
    {
        private readonly JustePrixGame _game;
        private readonly int _minPrix, _maxPrix, _maxEssais;

        private Label lblInfo;
        private Label lblAttempts;
        private TextBox txtGuess;
        private Button btnGuess;
        private Label lblResponse;
        private Button btnReset;

        public MainForm()
        {
            // Paramètres de la partie
            _minPrix = 0;
            _maxPrix = 100;
            _maxEssais = 10;

            _game = new JustePrixGame(_minPrix, _maxPrix, _maxEssais);

            InitializeComponent();
            UpdateStatus();
        }

        private void InitializeComponent()
        {
            this.Text = "Jeu du Juste Prix";
            this.ClientSize = new Size(300, 200);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;

            lblInfo = new Label
            {
                Text = $"Devinez un prix entre {_minPrix} et {_maxPrix}",
                Location = new Point(10, 10),
                AutoSize = true
            };
            this.Controls.Add(lblInfo);

            lblAttempts = new Label
            {
                Text = "",
                Location = new Point(10, 40),
                AutoSize = true
            };
            this.Controls.Add(lblAttempts);

            txtGuess = new TextBox
            {
                Location = new Point(10, 70),
                Width = 100
            };
            this.Controls.Add(txtGuess);

            btnGuess = new Button
            {
                Text = "Proposer",
                Location = new Point(120, 68),
                AutoSize = true
            };
            btnGuess.Click += BtnGuess_Click;
            this.Controls.Add(btnGuess);

            lblResponse = new Label
            {
                Text = "",
                Location = new Point(10, 110),
                AutoSize = true,
                Font = new Font(FontFamily.GenericSansSerif, 10, FontStyle.Bold)
            };
            this.Controls.Add(lblResponse);

            btnReset = new Button
            {
                Text = "Réinitialiser",
                Location = new Point(10, 140),
                AutoSize = true
            };
            btnReset.Click += BtnReset_Click;
            this.Controls.Add(btnReset);
        }

        private void BtnGuess_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtGuess.Text, out int proposition))
            {
                MessageBox.Show("Veuillez entrer un nombre valide.", "Entrée invalide",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var resp = _game.Propose(proposition);
                // Affichage français
                switch (resp)
                {
                    case "exact":
                        lblResponse.Text = "🎉 Exact ! Vous avez gagné !";
                        break;
                    case "trop bas":
                        lblResponse.Text = "⬇ Trop bas";
                        break;
                    case "trop haut":
                        lblResponse.Text = "⬆ Trop haut";
                        break;
                }
            }
            catch (TooManyAttemptsException ex)
            {
                MessageBox.Show(ex.Message, "Plus d'essais",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            UpdateStatus();
        }

        private void BtnReset_Click(object sender, EventArgs e)
        {
            _game.Reset();
            lblResponse.Text = "";
            txtGuess.Text = "";
            UpdateStatus();
        }

        private void UpdateStatus()
        {
            lblAttempts.Text = $"Essais : {_game.AttemptCount} / {_maxEssais}";
            // Si la partie est finie, on désactive la saisie
            if (_game.IsFinished)
            {
                btnGuess.Enabled = false;
                txtGuess.Enabled = false;
            }
            else
            {
                btnGuess.Enabled = true;
                txtGuess.Enabled = true;
            }
        }
    }
}
