Feature: Tic Tac Toe

  Scenario: Le joueur X gagne en complétant une ligne
    Given une nouvelle partie de morpion
    When le joueur X joue en (0,0)
    And le joueur O joue en (1,0)
    And le joueur X joue en (0,1)
    And le joueur O joue en (1,1)
    And le joueur X joue en (0,2)
    Then le joueur X doit être déclaré vainqueur

  Scenario: La partie se termine par une égalité
    Given une nouvelle partie de morpion
    When le joueur X joue en (0,0)
    And le joueur O joue en (0,1)
    And le joueur X joue en (0,2)
    And le joueur O joue en (1,0)
    And le joueur X joue en (1,2)
    And le joueur O joue en (1,1)
    And le joueur X joue en (2,0)
    And le joueur O joue en (2,2)
    And le joueur X joue en (2,1)
    Then aucun joueur ne doit être déclaré vainqueur
    And la partie doit être déclarée comme terminée par égalité

  Scenario: Un joueur tente de jouer sur une case déjà occupée
    Given une nouvelle partie de morpion
    When le joueur X joue en (0,0)
    And le joueur O joue en (0,0)
    Then une erreur doit être levée indiquant que la case est occupée

  Scenario: Le joueur O gagne en complétant une colonne
    Given une nouvelle partie de morpion
    When le joueur X joue en (0,1)
    And le joueur O joue en (0,0)
    And le joueur X joue en (1,1)
    And le joueur O joue en (1,0)
    And le joueur X joue en (2,2)
    And le joueur O joue en (2,0)
    Then le joueur O doit être déclaré vainqueur

  Scenario: Le joueur X gagne en complétant la diagonale principale
    Given une nouvelle partie de morpion
    When le joueur X joue en (0,0)
    And le joueur O joue en (0,1)
    And le joueur X joue en (1,1)
    And le joueur O joue en (0,2)
    And le joueur X joue en (2,2)
    Then le joueur X doit être déclaré vainqueur



Scenario: Le joueur O gagne en complétant la diagonale secondaire
  Given une nouvelle partie de morpion
  When le joueur X joue en (0,0)
  And le joueur O joue en (0,2)
  And le joueur X joue en (1,0)
  And le joueur O joue en (1,1)
  And le joueur X joue en (2,2)
  And le joueur O joue en (2,0)
  Then le joueur O doit être déclaré vainqueur



