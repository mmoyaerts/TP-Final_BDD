Feature: Déplacement du serpent

  Scenario: Le serpent se déplace vers la droite
    Given une nouvelle partie de Snake
    When le joueur dirige le serpent vers la droite
    Then la tête du serpent doit se déplacer d'une case vers la droite

  Scenario: Le serpent se déplace vers le haut
    Given une nouvelle partie de Snake
    When le joueur dirige le serpent vers le haut
    Then la tête du serpent doit se déplacer d'une case vers le haut
