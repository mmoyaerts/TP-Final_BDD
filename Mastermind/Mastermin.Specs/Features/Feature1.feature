Feature: Jeu de Mastermind
  Pour permettre à un joueur de deviner une combinaison secrète
  En tant que service de gestion de partie
  Je veux renvoyer, pour chaque proposition, 
  le nombre de pions noirs (bien placés) 
  et le nombre de pions blancs (corrects mais mal placés)

Background:
  Given une nouvelle partie de Mastermind avec
    | longueurDuCode | couleurs                                    | maxEssais |
    | 4              | Rouge, Vert, Bleu, Jaune, Noir, Blanc       | 10        |
  And le code secret est "Rouge, Vert, Bleu, Jaune"


  # Évaluation de propositions simples
  Scenario: Proposition exactement égale au code secret
    When le joueur propose "Rouge, Vert, Bleu, Jaune"
    Then la réponse contient 4 pions noirs et 0 pion blanc

  Scenario: Un seul pion bien placé
    When le joueur propose "Rouge, Noir, Noir, Noir"
    Then la réponse contient 1 pion noir et 0 pion blanc

  Scenario: Un seul pion mal placé
    When le joueur propose "Jaune, Noir, Noir, Noir"
    Then la réponse contient 0 pion noir et 1 pion blanc

  # Gestion des duplications
  Scenario: Plusieurs pions mal placés avec duplications dans la proposition
    Given le code secret est "Rouge, Rouge, Vert, Bleu"
    When le joueur propose "Rouge, Bleu, Rouge, Noir"
    Then la réponse contient 1 pion noir et 2 pions blancs

  Scenario: Proposition avec plus de doublons que dans le secret
    Given le code secret est "Vert, Bleu, Jaune, Noir"
    When le joueur propose "Vert, Vert, Vert, Vert"
    Then la réponse contient 1 pion noir et 0 pion blanc

  # Conditions de victoire et de défaite
  Scenario: Le joueur gagne lorsqu’il trouve le code avant la fin des essais
    Given attemptCount is 9
    When le joueur propose "Rouge, Vert, Bleu, Jaune"
    Then la partie est terminée et le joueur a gagné

  Scenario: Le joueur perd après avoir dépassé le nombre max d’essais
    Given attemptCount is 10
    When le joueur propose "Noir, Noir, Noir, Noir"
    Then la partie est terminée et le joueur a perdu

  # Réinitialisation de la partie
  Scenario: Réinitialiser la partie remet le compteur d’essais à zéro
    When on réinitialise la partie
    Then attemptCount est égal à 0
    And un nouveau code secret est généré

  # Reproposer après fin de partie
  Scenario: Proposer un code après la fin de la partie lève une exception
    Given attemptCount is 10
    And la partie est terminée
    When le joueur propose "Rouge, Vert, Bleu, Jaune"
    Then une exception InvalidOperationException est levée
