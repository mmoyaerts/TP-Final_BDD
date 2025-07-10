using System;
using System.Collections.Generic;

namespace TP3_Snake
{
    public enum Direction { Up, Down, Left, Right }

    public struct Position
    {
        public int Row;
        public int Col;

        public Position(int row, int col)
        {
            Row = row;
            Col = col;
        }

        public static Position operator +(Position a, Position b) =>
            new Position(a.Row + b.Row, a.Col + b.Col);

        public override bool Equals(object obj) =>
            obj is Position p && Row == p.Row && Col == p.Col;

        public override int GetHashCode() => HashCode.Combine(Row, Col);
    }

    public class SnakeManager
    {
        private LinkedList<Position> _snake;
        private HashSet<Position> _bodySet;
        private Direction _direction;
        private Position _food;
        private int _gridSize;

        public bool IsGameOver { get; private set; }
        public int Score { get; private set; }

        public int Length => _snake.Count;

        public SnakeManager(int gridSize)
        {
            _gridSize = gridSize;
            _snake = new LinkedList<Position>();
            _bodySet = new HashSet<Position>();
            IsGameOver = false;
            Score = 0;
        }

        public void Initialize(Position start, Direction direction)
        {
            _snake.Clear();
            _bodySet.Clear();
            _snake.AddFirst(start);
            _bodySet.Add(start);
            _direction = direction;
            IsGameOver = false;
            Score = 0;
        }

        public LinkedList<Position> GetSnakePositions() => _snake;

        public void ChangeDirection(Direction direction)
        {
            // ici tu peux ajouter une logique anti-retour immédiat si tu veux
            _direction = direction;
        }

        public void PlaceApple(Position pos)
        {
            _food = pos;
        }

        public Position GetHeadPosition() => _snake.First.Value;

        public bool Move()
        {
            if (IsGameOver) return false;

            Position head = GetHeadPosition();
            Position next = head + GetOffset(_direction);

            if (!IsInsideGrid(next) || _bodySet.Contains(next))
            {
                IsGameOver = true;
                return false;
            }

            _snake.AddFirst(next);
            _bodySet.Add(next);

            if (next.Equals(_food))
            {
                Score++;
                // Ne pas enlever la queue -> serpent grandit
            }
            else
            {
                Position tail = _snake.Last.Value;
                _snake.RemoveLast();
                _bodySet.Remove(tail);
            }

            return true;
        }

        private bool IsInsideGrid(Position pos) =>
            pos.Row >= 0 && pos.Col >= 0 && pos.Row < _gridSize && pos.Col < _gridSize;

        private Position GetOffset(Direction direction) =>
            direction switch
            {
                Direction.Up => new Position(-1, 0),
                Direction.Down => new Position(1, 0),
                Direction.Left => new Position(0, -1),
                Direction.Right => new Position(0, 1),
                _ => new Position(0, 0)
            };
    }
}
