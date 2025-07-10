Feature: Système de score

  Scenario: Le score augmente après avoir mangé une pomme
    Given une nouvelle partie de Snake
    And le score initial est 0
    And une pomme est devant le serpent
    When le serpent mange la pomme
    Then le score doit être égal à 1
