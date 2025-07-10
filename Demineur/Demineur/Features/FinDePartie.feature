Feature: FinDePartie

A short summary of the feature

@tag1
Scenario: Cliquer sur une bombe
  Given une partie avec une bombe sur une case est en cours
  When le joueur clique sur une case contenant une bombe
  Then le jeu est perdu
  And toutes les bombes sont révélées

Scenario: Révéler toutes les cases sans bombe
  Given une partie prête à être gagnée
  When le joueur a révélé toutes les cases sans bombe
  Then la partie est gagnée
