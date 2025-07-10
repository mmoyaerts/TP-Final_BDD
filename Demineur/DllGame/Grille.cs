namespace Game
{
    public class Grille
    {
        private readonly int lignes;
        private readonly int colonnes;
        private readonly Case[,] cases;
        private readonly int nbrBombesRestantes;

        public Grille(int lignes = 9, int colonnes = 9, int nbrBombesRestantes = 10)
        {
            this.lignes = lignes;
            this.colonnes = colonnes;
            this.nbrBombesRestantes=nbrBombesRestantes;
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
    }
}
