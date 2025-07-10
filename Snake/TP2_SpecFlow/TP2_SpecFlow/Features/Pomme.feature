Feature: Le serpent mange une pomme

  Scenario: Le serpent mange une pomme
    Given une nouvelle partie de Snake
    And une pomme est positionnée juste devant le serpent
    When le serpent avance
    Then le serpent doit grandir
    And le score doit augmenter
