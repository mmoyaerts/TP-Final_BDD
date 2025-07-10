using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public class Case
    {
        private bool aBombe;
        private bool visible;

        public Case()
        {
            aBombe = false;
            visible = false;
        }

        public bool ContientBombe()
        {
            return aBombe;
        }

        public bool getVisible()
        { return visible; }

        public void PlacerBombe()
        {
            aBombe = true;
        }
    }
}
