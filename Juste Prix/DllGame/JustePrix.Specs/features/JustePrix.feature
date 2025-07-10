Feature: Jeu du Juste Prix
  Pour permettre à un joueur de deviner un prix secret
  En tant que service de gestion de partie
  Je veux renvoyer à chaque proposition si c'est trop bas, trop haut ou exact,
  et gérer la fin de partie.

  Background:
  Given une nouvelle partie du Juste Prix avec
    | minPrix | maxPrix | maxEssais |
    | 0       | 100     | 10        |


    And le prix secret est 42

  Scenario: Proposition trop basse
    When je propose 10
    Then la réponse est "trop bas"
    And la partie n’est pas terminée

  Scenario: Proposition trop haute
    When je propose 90
    Then la réponse est "trop haut"
    And la partie n’est pas terminée

  Scenario: Proposition exacte
    When je propose 42
    Then la réponse est "exact"
    And la partie est terminée et gagnée

  Scenario: Dépassement du nombre d’essais
    Given j’ai déjà fait 10 essais
    When je propose 50
    Then une exception TooManyAttemptsException est levée

  Scenario: Réinitialiser la partie remet tout à zéro
    When on réinitialise la partie
    Then le nombre d’essais est égal à 0
    And un nouveau prix secret est généré
