using NUnit.Framework;
using TP3_Snake;
using System;
using System.Collections.Generic;
using TechTalk.SpecFlow;

[Binding]
public class SnakeSteps
{
    private SnakeManager _game;
    private Exception _caughtException;

    [Given("une nouvelle partie de Snake")]
    public void GivenUneNouvellePartieDeSnake()
    {
        _game = new SnakeManager(10);
        _game.Initialize(new Position(5, 5), Direction.Right);
    }

    [Given("le serpent est proche du bord droit")]
    public void GivenLeSerpentEstProcheDuBordDroit()
    {
        _game = new SnakeManager(10);
        _game.Initialize(new Position(5, 9), Direction.Right);
    }

    [Given("le serpent a une forme en U")]
    public void GivenLeSerpentAAUneFormeEnU()
    {
        _game = new SnakeManager(5);
        var snakeBody = new LinkedList<Position>(new[]
        {
            new Position(2,2),
            new Position(2,3),
            new Position(3,3),
            new Position(3,2),
            new Position(3,1),
            new Position(2,1),
            new Position(1,1),
            new Position(1,2),
            new Position(1,3)
        });

        typeof(SnakeManager).GetField("_snake", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            ?.SetValue(_game, snakeBody);

        var bodySet = new HashSet<Position>(snakeBody);
        typeof(SnakeManager).GetField("_bodySet", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            ?.SetValue(_game, bodySet);

        typeof(SnakeManager).GetField("_direction", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            ?.SetValue(_game, Direction.Left);
    }

    [When(@"^le joueur dirige le serpent vers le mur$")]
    public void WhenLeJoueurDirigeLeSerpentVersLeMur()
    {
        _game.ChangeDirection(Direction.Right);
        _game.Move();
    }

    [When("le joueur dirige le serpent vers lui-même")]
    public void WhenLeJoueurDirigeLeSerpentVersLuiMeme()
    {
        _game.ChangeDirection(Direction.Left);
        _game.Move();
    }

    [When("le serpent avance")]
    public void WhenLeSerpentAvance()
    {
        _game.Move();
    }

    [Given("une pomme est positionnée juste devant le serpent")]
    public void GivenUnePommeEstPositionneeJusteDevantLeSerpent()
    {
        var head = _game.GetHeadPosition();
        var offset = GetOffset(_game);
        var posPomme = new Position(head.Row + offset.Row, head.Col + offset.Col);
        _game.PlaceApple(posPomme);
    }

    [Given("le score initial est 0")]
    public void GivenLeScoreInitialEstZero()
    {
        Assert.AreEqual(0, _game.Score);
    }

    [Given("une pomme est devant le serpent")]
    public void GivenUnePommeEstDevantLeSerpent()
    {
        GivenUnePommeEstPositionneeJusteDevantLeSerpent();
    }

    [When("le serpent mange la pomme")]
    public void WhenLeSerpentMangeLaPomme()
    {
        _game.Move();
    }

    [Then("le serpent doit grandir")]
    public void ThenLeSerpentDoitGrandir()
    {
        Assert.Greater(_game.Length, 1);
    }

    [Then("le score doit augmenter")]
    public void ThenLeScoreDoitAugmenter()
    {
        Assert.Greater(_game.Score, 0);
    }

    [Then("le score doit être égal à (\\d+)")]
    public void ThenLeScoreDoitEtreEgalA(int score)
    {
        Assert.AreEqual(score, _game.Score);
    }

    [Then("la partie doit se terminer")]
    public void ThenLaPartieDoitSeTerminer()
    {
        Assert.IsTrue(_game.IsGameOver);
    }

    // ⛔ Ancienne version causait ambiguïté
    // ✅ Nouvelle version restreint aux directions valides uniquement
    [When(@"le joueur dirige le serpent vers (la|le) (haut|bas|gauche|droite)")]
    public void WhenLeJoueurDirigeLeSerpentVersLaOuLeDirection(string article, string direction)
    {
        Direction dir = direction.ToLower() switch
        {
            "haut" => Direction.Up,
            "bas" => Direction.Down,
            "gauche" => Direction.Left,
            "droite" => Direction.Right,
            _ => throw new ArgumentException("Direction invalide")
        };

        _game.ChangeDirection(dir);
        _game.Move();
    }

    [Then(@"la tête du serpent doit se déplacer d'une case vers (la|le) (haut|bas|gauche|droite)")]
    public void ThenLaTeteDuSerpentDoitSeDeplacerDuneCaseVersLaOuLeDirection(string article, string direction)
    {
        var head = _game.GetHeadPosition();

        Position expectedOffset = direction.ToLower() switch
        {
            "haut" => new Position(-1, 0),
            "bas" => new Position(1, 0),
            "gauche" => new Position(0, -1),
            "droite" => new Position(0, 1),
            _ => throw new ArgumentException("Direction invalide")
        };

        var startPos = new Position(5, 5);
        var expectedPos = new Position(startPos.Row + expectedOffset.Row, startPos.Col + expectedOffset.Col);

        Assert.AreEqual(expectedPos, head);
    }

    private Position GetOffset(SnakeManager game)
    {
        var directionField = typeof(SnakeManager).GetField("_direction", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var dir = (Direction)directionField.GetValue(game);

        return dir switch
        {
            Direction.Up => new Position(-1, 0),
            Direction.Down => new Position(1, 0),
            Direction.Left => new Position(0, -1),
            Direction.Right => new Position(0, 1),
            _ => new Position(0, 0)
        };
    }
}
