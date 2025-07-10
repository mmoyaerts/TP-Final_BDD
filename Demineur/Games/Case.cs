using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Games
{
    public class Case
    {
        private bool aBombe;
        private bool visible;
        private int numero;
        private bool drapeau = false;

        public Case()
        {
            aBombe = false;
            visible = false;
        }

        public bool ContientBombe()
        {
            return aBombe;
        }

        public bool estVisible()
        { return visible; }

        public void Reveler() => visible = true;

        public void PlacerBombe()
        {
            aBombe = true;
        }

        public void SetNumero(int value) => numero = value;
        public int GetNumero() => numero;

        public void PoserDrapeau()
        {
            if (!visible)
                drapeau = true;
        }

        public void RetirerDrapeau()
        {
            drapeau = false;
        }

        public bool PeutEtreRevelee()
        {
            return !drapeau && !visible;
        }

        public bool AUnDrapeau() => drapeau;
    }
}
