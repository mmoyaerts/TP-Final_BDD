Feature: Collision du serpent

  Scenario: Collision avec un mur
    Given une nouvelle partie de Snake
    And le serpent est proche du bord droit
    When le joueur dirige le serpent vers le mur
    Then la partie doit se terminer

  Scenario: Collision avec lui-même
    Given une nouvelle partie de Snake
    And le serpent a une forme en U
    When le joueur dirige le serpent vers lui-même
    Then la partie doit se terminer
