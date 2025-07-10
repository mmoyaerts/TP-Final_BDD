Feature: Drapeaux

A short summary of the feature

@tag1
Scenario: Poser un drapeau sur une case suspectée
  Given une partie est en cours
  When le joueur place un drapeau sur sur la case <X>, <Y>
  Then le nombre de bombes restantes affiché diminue de 1
  Examples:
    | X | Y |
    | 4 | 4 |
    | 6 | 5 |

Scenario: Gérer les interactions avec une case marquée d’un drapeau
  Given une case est marquée d’un drapeau
  When le joueur retire un drapeau sur la case
  Then la case ne se dévoile pas
  And le compteur de bombes diminue de 1

  When le joueur clique à nouveau sur cette case
  Then le drapeau reste
  And le compteur de bombes ne change pas
