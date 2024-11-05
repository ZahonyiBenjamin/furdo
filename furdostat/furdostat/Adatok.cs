using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace furdostat
{
    internal class Adatok
    {
        public int azonosito { get; set; }
        public int furdoreszleg { get; set; }
        public int kibelepes { get; set; }
        public int ora { get; set; }
        public int perc { get; set; }
        public int masodperc { get; set; }
        public DateTime ido { get; set; }

        public Adatok(string line)
        {
            string[] s = line.Split(' ');

            azonosito = int.Parse(s[0]);
            furdoreszleg = int.Parse(s[1]);
            kibelepes = int.Parse(s[2]);
            ora = int.Parse(s[3]);
            perc = int.Parse(s[4]);
            masodperc = int.Parse(s[5]);
            ido = DateTime.Parse($"{ora}:{perc}:{masodperc}");
        }
    }
}
