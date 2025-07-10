namespace Games
{
    public class Grille
    {
        private readonly int lignes;
        private readonly int colonnes;
        private readonly Case[,] cases;
        private readonly int nbrBombesRestantes;
        private int bombesRestantesAffichees;
        private bool partiePerdue = false;
        private bool partieGagnee = false;

        public Grille(int lignes = 9, int colonnes = 9, int nbrBombesRestantes = 10)
        {
            this.lignes = lignes;
            this.colonnes = colonnes;
            this.nbrBombesRestantes = nbrBombesRestantes;
            this.bombesRestantesAffichees = nbrBombesRestantes;
            cases = new Case[lignes, colonnes];

            // Initialisation des cases, aucune bombe placée
            for (int i = 0; i < lignes; i++)
            {
                for (int j = 0; j < colonnes; j++)
                {
                    cases[i, j] = new Case();
                }
            }
        }

        public int GetLignes()
        {
            return lignes;
        }

        public int GetColonnes()
        {
            return colonnes;
        }

        public int GetNbrBombesRestante()
        {
            return nbrBombesRestantes;
        }
        public bool EstGagnee() => partieGagnee;

        public int GetNombreDeBombesPlacees()
        {
            int count = 0;
            for (int i = 0; i < lignes; i++)
            {
                for (int j = 0; j < colonnes; j++)
                {
                    if (cases[i, j].ContientBombe())
                    {
                        count++;
                    }
                }
            }
            return count;
        }

        public int GetBombesRestantesAffichees() => bombesRestantesAffichees;
        public bool EstPerdue() => partiePerdue;

        public void InitialiserBombes(int x, int y)
        {
            Random rand = new Random();
            int bombesPlacées = 0;
            bombesRestantesAffichees = nbrBombesRestantes;

            while (bombesPlacées < nbrBombesRestantes)
            {
                int i = rand.Next(0, lignes);
                int j = rand.Next(0, colonnes);

                if ((Math.Abs(i - x) <= 1 && Math.Abs(j - y) <= 1) || cases[i, j].ContientBombe())
                    continue;

                cases[i, j].PlacerBombe();
                bombesPlacées++;
            }

            CalculerNumeros();
        }

        private void CalculerNumeros()
        {
            for (int i = 0; i < lignes; i++)
            {
                for (int j = 0; j < colonnes; j++)
                {
                    if (cases[i, j].ContientBombe())
                        continue;

                    int count = 0;
                    for (int dx = -1; dx <= 1; dx++)
                    {
                        for (int dy = -1; dy <= 1; dy++)
                        {
                            int ni = i + dx;
                            int nj = j + dy;

                            if (ni >= 0 && ni < lignes && nj >= 0 && nj < colonnes && cases[ni, nj].ContientBombe())
                                count++;
                        }
                    }
                    cases[i, j].SetNumero(count);
                }
            }
        }

        public List<(int, int)> RevelerZone(int x, int y)
        {
            var révélées = new List<(int, int)>();
            var queue = new Queue<(int, int)>();
            queue.Enqueue((x, y));

            while (queue.Count > 0)
            {
                var (i, j) = queue.Dequeue();
                if (cases[i, j].estVisible())
                    continue;

                cases[i, j].Reveler();
                révélées.Add((i, j));

                if (cases[i, j].GetNumero() == 0)
                {
                    for (int dx = -1; dx <= 1; dx++)
                    {
                        for (int dy = -1; dy <= 1; dy++)
                        {
                            int ni = i + dx;
                            int nj = j + dy;

                            if (ni >= 0 && ni < lignes && nj >= 0 && nj < colonnes)
                                queue.Enqueue((ni, nj));
                        }
                    }
                }
            }

            return révélées;
        }

        public Case GetCase(int x, int y) => cases[x, y];

        public void PoserDrapeauSur(int x, int y)
        {
            if (!cases[x, y].estVisible() && !cases[x, y].AUnDrapeau())
            {
                cases[x, y].PoserDrapeau();
                bombesRestantesAffichees--;
            }
            else if(!cases[x, y].AUnDrapeau())
            {
                cases[x, y].RetirerDrapeau();
                bombesRestantesAffichees++;
            }
        }

        public void CliquerSur(int x, int y)
        {
            var c = cases[x, y];
            if (c.ContientBombe())
            {
                partiePerdue = true;
                RévélerToutesLesBombes();
            }
            else
            {
                RevelerZone(x, y);
            }
        }

        private void RévélerToutesLesBombes()
        {
            for (int i = 0; i < lignes; i++)
            {
                for (int j = 0; j < colonnes; j++)
                {
                    if (cases[i, j].ContientBombe())
                        cases[i, j].Reveler();
                }
            }
        }

        public void VerifierVictoire()
        {
            for (int i = 0; i < lignes; i++)
            {
                for (int j = 0; j < colonnes; j++)
                {
                    if (!cases[i, j].ContientBombe() && !cases[i, j].estVisible())
                        return; // Il reste au moins une case sans bombe non révélée
                }
            }

            partieGagnee = true;
        }

    }
}
