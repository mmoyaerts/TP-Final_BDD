using System;

namespace TP3_TicTacToe
{
    public enum Player { None, X, O }
    public enum GameStatus { InProgress, Draw, Win }

    public class TicTacToeManager
    {
        private readonly Player[,] _board;
        private Player _currentPlayer;
        private Player _winner;
        private GameStatus _status;
        private int _moves;

        public TicTacToeManager()
        {
            _board = new Player[3, 3];
            _currentPlayer = Player.X;
            _winner = Player.None;
            _status = GameStatus.InProgress;
            _moves = 0;
        }

        public Player CurrentPlayer => _currentPlayer;
        public Player Winner => _winner;
        public GameStatus Status => _status;
        public bool IsGameOver => _status != GameStatus.InProgress;

        public void Play(string joueur, int row, int col)
        {
            if (IsGameOver)
                throw new InvalidOperationException("La partie est déjà terminée.");

            if (row < 0 || row > 2 || col < 0 || col > 2)
                throw new ArgumentOutOfRangeException("Les coordonnées doivent être entre 0 et 2.");

            Player player = ParsePlayer(joueur);

            if (player != _currentPlayer)
                throw new InvalidOperationException($"Ce n'est pas le tour du joueur {joueur}.");

            if (_board[row, col] != Player.None)
                throw new InvalidOperationException($"La case ({row},{col}) est déjà occupée.");

            _board[row, col] = player;
            _moves++;

            if (CheckWin(row, col, player))
            {
                _winner = player;
                _status = GameStatus.Win;
            }
            else if (_moves == 9)
            {
                _status = GameStatus.Draw;
            }
            else
            {
                _currentPlayer = (_currentPlayer == Player.X) ? Player.O : Player.X;
            }
        }

        public string GetWinner()
        {
            return _winner != Player.None ? _winner.ToString() : "Aucun";
        }

        private Player ParsePlayer(string joueur)
        {
            return joueur.ToUpper() switch
            {
                "X" => Player.X,
                "O" => Player.O,
                _ => throw new ArgumentException("Joueur invalide. Utilisez 'X' ou 'O'.")
            };
        }

        private bool CheckWin(int row, int col, Player player)
        {
            // Ligne
            if (_board[row, 0] == player && _board[row, 1] == player && _board[row, 2] == player)
                return true;

            // Colonne
            if (_board[0, col] == player && _board[1, col] == player && _board[2, col] == player)
                return true;

            // Diagonale principale
            if (row == col && _board[0, 0] == player && _board[1, 1] == player && _board[2, 2] == player)
                return true;

            // Diagonale secondaire
            if (row + col == 2 && _board[0, 2] == player && _board[1, 1] == player && _board[2, 0] == player)
                return true;

            return false;
        }
    }
}
