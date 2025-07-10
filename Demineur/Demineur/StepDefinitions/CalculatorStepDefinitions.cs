using NUnit.Framework;
using Games;

namespace Demineur.StepDefinitions
{
    [Binding]
    public sealed class CalculatorStepDefinitions
    {
        private Grille grille;
        private int clicX, clicY;
        private List<(int, int)> casesRévélées;
        private int bombeX = 1;
        private int bombeY = 1;
        private int x = 1;
        private int y = 1;

        [Given("un joueur lance une nouvelle partie")]
        public void un_joueur_lance_une_nouvelle_partie()
        {
            
        }


        [When("le jeu initialise la grille (.*) par (.*)")]
        public void le_jeu_initialise_la_grille(int lignes, int colonnes)
        {
            grille = new Grille(lignes, colonnes);
        }

        [Then(@"une grille de (.*) lignes sur (.*) colonnes est créée")]
        public void ThenUneGrilleDeLignesSurColonnesEstCreee(int lignes, int colonnes)
        {
            Assert.AreEqual(lignes, grille.GetLignes());
            Assert.AreEqual(colonnes, grille.GetColonnes());
        }


        [Then("aucune bombe n'est encore placée")]
        public void aucune_bombe_n_est_encore_placee()
        {
            Assert.AreEqual(0, grille.GetNombreDeBombesPlacees());
        }

        /////////////////////////////////////////////////////////////////////////////

        [Given("le joueur clique sur la case (.*), (.*) pour la première fois")]
        public void GivenLeJoueurClique(int X, int Y)
        {
            grille = new Grille();
            clicX = X;
            clicY = Y;
        }

        [When("le jeu place les bombes")]
        public void WhenPlacementDesBombes()
        {
            grille.InitialiserBombes(clicX, clicY);
            casesRévélées = grille.RevelerZone(clicX, clicY);
        }

        [Then("aucune bombe ne doit se trouver sur la case cliquée ni dans ses 8 cases adjacentes")]
        public void ThenPasDeBombeAutour()
        {
            for (int dx = -1; dx <= 1; dx++)
            {
                for (int dy = -1; dy <= 1; dy++)
                {
                    int i = clicX + dx;
                    int j = clicY + dy;

                    if (i >= 0 && i < 9 && j >= 0 && j < 9)
                        Assert.IsFalse(grille.GetCase(i, j).ContientBombe(), $"Bombe trouvée en ({i},{j})");
                }
            }
        }

        [Then("10 bombes sont placées aléatoirement ailleurs")]
        public void ThenBombesPlacées()
        {
            Assert.AreEqual(10, grille.GetNombreDeBombesPlacees());
        }

        [Then("les cases numérotées sont calculées")]
        public void ThenNumerosCalculés()
        {
            bool auMoinsUnNumero = false;

            for (int i = 0; i < grille.GetLignes(); i++)
            {
                for (int j = 0; j < grille.GetColonnes(); j++)
                {
                    if (!grille.GetCase(i, j).ContientBombe() && grille.GetCase(i, j).GetNumero() > 0)
                        auMoinsUnNumero = true;
                }
            }

            Assert.IsTrue(auMoinsUnNumero);
        }

        [Then("les cases vides adjacentes sont automatiquement révélées")]
        public void ThenCasesVidesRévélées()
        {
            foreach (var (i, j) in casesRévélées)
            {
                Assert.IsTrue(grille.GetCase(i, j).estVisible());
            }
        }

        /////////////////////////////////////////////////////////////////////////////
        [Given("une partie est en cours")]
        public void GivenPartieEnCours()
        {
            grille = new Grille();
        }

        [When("le joueur place un drapeau sur sur la case (.*), (.*)")]
        public void WhenJoueurPlaceDrapeau(int X, int Y)
        {
            grille.PoserDrapeauSur(X, Y);
        }

        [Then("le nombre de bombes restantes affiché diminue de 1")]
        public void ThenBombesRestantesDiminuent()
        {
            Assert.AreEqual(9, grille.GetBombesRestantesAffichees());
        }

        ////////////////////////////////////////////////////////////////////////////////
        [Given("une partie avec une bombe sur une case est en cours")]
        public void GivenPartieAvecBombe()
        {
            grille = new Grille();
            grille.GetCase(bombeX, bombeY).PlacerBombe();
        }

        [When("le joueur clique sur une case contenant une bombe")]
        public void WhenJoueurCliqueSurBombe()
        {
            grille.CliquerSur(bombeX, bombeY);
        }

        [Then("le jeu est perdu")]
        public void ThenPartiePerdue()
        {
            Assert.IsTrue(grille.EstPerdue(), "La partie n'a pas été marquée comme perdue.");
        }

        [Then("toutes les bombes sont révélées")]
        public void ThenBombesRévélées()
        {
            for (int i = 0; i < grille.GetLignes(); i++)
            {
                for (int j = 0; j < grille.GetColonnes(); j++)
                {
                    if (grille.GetCase(i, j).ContientBombe())
                        Assert.IsTrue(grille.GetCase(i, j).estVisible(), $"La bombe en ({i},{j}) n'est pas révélée.");
                }
            }
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////
        [Given("une grille avec une case en \\((.*),(.*)\\) contenant le numéro (.*)")]
        public void GivenGrilleAvecNumero(int x, int y, int valeur)
        {
            grille = new Grille();
            if (valeur > 0)
            {
                grille.GetCase(x, y).SetNumero(valeur);
            }
            else
            {
                for (int i = 0; i < grille.GetLignes(); i++)
                {
                    for (int j = 0; j < grille.GetColonnes(); j++)
                    {
                        grille.GetCase(i, j).SetNumero(0);
                    }
                }
                grille.GetCase(x, y).SetNumero(0);
            }
            clicX = x;
            clicY = y;
        }

        [When("le joueur clique sur cette case")]
        public void WhenJoueurClique()
        {
            grille.RevelerZone(clicX, clicY);
        }

        [Then("le numéro est affiché")]
        public void ThenNumeroAffiché()
        {
            Assert.IsTrue(grille.GetCase(clicX, clicY).estVisible());
            Assert.Greater(grille.GetCase(clicX, clicY).GetNumero(), 0);
        }

        [Then("aucune autre case n’est révélée")]
        public void ThenAucuneAutreCase()
        {
            for (int i = 0; i < grille.GetLignes(); i++)
            {
                for (int j = 0; j < grille.GetColonnes(); j++)
                {
                    if (i == clicX && j == clicY) continue;
                    Assert.IsFalse(grille.GetCase(i, j).estVisible());
                }
            }
        }

        [Then("toutes les cases connectées sans bombe ni numéro sont révélées")]
        public void ThenZoneVideRévélée()
        {
            for (int i = 0; i < grille.GetLignes(); i++)
            {
                for (int j = 0; j < grille.GetColonnes(); j++)
                {
                    if (grille.GetCase(i, j).GetNumero() == 0)
                        Assert.IsTrue(grille.GetCase(i, j).estVisible(), $"Case ({i},{j}) devrait être révélée");
                }
            }
        }

        [Then("les cases adjacentes contenant un numéro sont affichées")]
        public void ThenNumerosAdjacentsAffichés()
        {
            for (int i = 0; i < grille.GetLignes(); i++)
            {
                for (int j = 0; j < grille.GetColonnes(); j++)
                {
                    var caseActuelle = grille.GetCase(i, j);
                    if (caseActuelle.GetNumero() > 0)
                    {
                        // Adjacent à une case vide révélée
                        bool voisinVideRévélé = false;
                        for (int dx = -1; dx <= 1; dx++)
                        {
                            for (int dy = -1; dy <= 1; dy++)
                            {
                                int ni = i + dx;
                                int nj = j + dy;
                                if (ni >= 0 && ni < grille.GetLignes() && nj >= 0 && nj < grille.GetColonnes())
                                {
                                    if (grille.GetCase(ni, nj).GetNumero() == 0 && grille.GetCase(ni, nj).estVisible())
                                    {
                                        voisinVideRévélé = true;
                                    }
                                }
                            }
                        }

                        if (voisinVideRévélé)
                            Assert.IsTrue(caseActuelle.estVisible(), $"Case ({i},{j}) devrait être révélée car voisine d'une case vide");
                    }
                }
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////
        [Given("une partie prête à être gagnée")]
        public void GivenUnePartieEstEnCours()
        {
            grille = new Grille();

            // Simuler une partie sans bombes du tout (pour simplifier)
            // Ou bien forcer toutes les cases sauf bombes à être révélées
            for (int i = 0; i < grille.GetLignes(); i++)
            {
                for (int j = 0; j < grille.GetColonnes(); j++)
                {
                    grille.GetCase(i, j).SetNumero(0);
                }
            }
        }

        [When("le joueur a révélé toutes les cases sans bombe")]
        public void WhenRevelationTotale()
        {
            for (int i = 0; i < grille.GetLignes(); i++)
            {
                for (int j = 0; j < grille.GetColonnes(); j++)
                {
                    if (!grille.GetCase(i, j).ContientBombe())
                        grille.GetCase(i, j).Reveler();
                }
            }

            grille.VerifierVictoire();
        }

        [Then("la partie est gagnée")]
        public void ThenVictoire()
        {
            Assert.IsTrue(grille.EstGagnee(), "La partie devrait être gagnée.");
        }

        ////////////////////////////////////////////////////////////////////////////////
        [Given("une case est marquée d’un drapeau")]
        public void GivenCaseAvecDrapeau()
        {
            grille = new Grille();
            grille.PoserDrapeauSur(x, y);
        }

        [When("le joueur retire un drapeau sur la case")]
        public void WhenPremierClic()
        {
            grille.PoserDrapeauSur(x, y);
        }

        [Then("la case ne se dévoile pas")]
        public void ThenPasDeRevelation()
        {
            Assert.IsFalse(grille.GetCase(x, y).estVisible());
        }

        [Then("le compteur de bombes diminue de 1")]
        public void ThenCompteurDiminue()
        {
            Assert.AreEqual(9, grille.GetBombesRestantesAffichees());
        }

        [When("le joueur clique à nouveau sur cette case")]
        public void WhenDeuxiemeClic()
        {
            grille.PoserDrapeauSur(x, y); // Deuxième clic → retire le drapeau
        }

        [Then("le drapeau reste")]
        public void ThenDrapeauRetire()
        {
            Assert.IsTrue(grille.GetCase(x, y).AUnDrapeau());
        }

        [Then("le compteur de bombes ne change pas")]
        public void ThenCompteurAugmente()
        {
            Assert.AreEqual(9, grille.GetBombesRestantesAffichees());
        }
    }
}
