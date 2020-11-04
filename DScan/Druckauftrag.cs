using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DScan
{
    public class Druckauftrag
    {

        public Druckauftrag(string dokumentenBezeichnung, string patientenName, string fallNummer, string lastDbId)
        {
            this._dokumentenbezeichnung = dokumentenBezeichnung;
            this.PatientenName = patientenName;
            this._fallnummer = fallNummer;
            this.LastDbId = lastDbId;

        }
        private string _dokumentenbezeichnung;
        private string PatientenName;
        private string _fallnummer;
        private string LastDbId;


        public override string ToString()
        {
            string DruckInhalt = @"^Q25,3
^W40
^H10
^S1
^P1
^E14
^C1
^O0
^L
AB,15,105,1,1,0,0," + _dokumentenbezeichnung + "\n" +
"AA,15,135,1,1,0,0,Scannen | BarcodeID: " + LastDbId + "\n" +
"AA,15,155,1,1,0,0,E-Mail: mi@augusta-bochum.de\n" +
"AB,15,175,1,1,0,0,"+PatientenName+"\n"+
"AA,310,10,1,1,0,1,Gescanntes Dokument\n"+
"AA,290,10,1,1,0,1,Fallnummer "+_fallnummer+"\n"+
"BN,25,0,2,6,100,0,0,"+LastDbId+"\n"+
"E\n";
            return DruckInhalt;
        }
    }
}
