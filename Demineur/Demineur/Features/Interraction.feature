Feature: Interaction
![Calculator](https://specflow.org/wp-content/uploads/2020/09/calculator.png)
Simple calculator for adding **two** numbers

Link to a feature: [Calculator](Demineur/Features/Calculator.feature)
***Further read***: **[Learn more about how to generate Living Documentation](https://docs.specflow.org/projects/specflow-livingdoc/en/latest/LivingDocGenerator/Generating-Documentation.html)**

@mytag


Scenario Outline: Cliquer sur une case pour la première fois
  Given le joueur clique sur la case <X>, <Y> pour la première fois
  When le jeu place les bombes
  Then aucune bombe ne doit se trouver sur la case cliquée ni dans ses 8 cases adjacentes
  And 10 bombes sont placées aléatoirement ailleurs
  And les cases numérotées sont calculées
  And les cases vides adjacentes sont automatiquement révélées
  Examples:
    | X | Y |
    | 4 | 4 |
    | 6 | 5 |

Scenario Outline: Cliquer sur une case avec ou sans numéro
  Given une grille avec une case en (<x>,<y>) contenant le numéro <valeur>
  When le joueur clique sur cette case
  Then <comportement_attendu_1>
  And <comportement_attendu_2>

Examples:
  | x | y | valeur | comportement_attendu_1                                      | comportement_attendu_2                                |
  | 2 | 2 | 2      | le numéro est affiché                                       | aucune autre case n’est révélée                       |
  | 1 | 1 | 0      | toutes les cases connectées sans bombe ni numéro sont révélées | les cases adjacentes contenant un numéro sont affichées |