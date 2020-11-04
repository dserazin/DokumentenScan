using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DScan
{
    class DokumentenTyp
    {
        public string Bezeichnung { get; set; }
        public DokumentenKategorie Dokumententyp { get; set; }

        public DokumentenTyp(string bezeichnung, DokumentenKategorie dokumententyp)
        {
            this.Bezeichnung = bezeichnung;
            this.Dokumententyp = dokumententyp;
        }

        public override string ToString()

        {
            return Bezeichnung;
        }

    }
}
