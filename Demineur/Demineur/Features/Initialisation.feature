Feature: Initialisation

A short summary of the feature

@tag1
Scenario Outline: Lancer une nouvelle partie
  Given un joueur lance une nouvelle partie
  When le jeu initialise la grille <lignes> par <colonnes>
  Then une grille de <lignes> lignes sur <colonnes> colonnes est créée
  And aucune bombe n'est encore placée
  Examples:
    | lignes | colonnes |
    | 9      | 9        |
    | 16     | 16       |
