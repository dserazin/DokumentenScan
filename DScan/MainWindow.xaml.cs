using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Net;
using System.Drawing.Printing;
using System.Drawing;
using Microsoft.Win32.SafeHandles;
using System.IO;
using System.Runtime.InteropServices;
using System.IO.Ports;
using System.Printing;


using Image = System.Drawing.Image;

namespace DScan
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // ÜbergabeParameter 
        string vorname;
        string nachname;
        DateTime geburtsdatum;
        string fallNr;
        string dayDate = DateTime.Now.ToString();

        //string alter = dayDate - geburtsdatum;

        public MainWindow()
        {
            InitializeComponent();
            initFile(); // Aufruf dynamischer Struktur

            // Angelegt wird (parameter)als lokale Variable 
            List<string> parameter = System.Environment.GetCommandLineArgs().ToList();
            parameter.RemoveAt(0); //Das erste Argument (= die EXE selbst) wird entfernt

            // string Übergabeparameter werden an lokale Variable gebunden
            vorname = parameter[0];
            nachname = parameter[1];
            geburtsdatum = Convert.ToDateTime(parameter[2]);
            fallNr = parameter[3];


            this.tbxVorname.Text = vorname;
            this.tbxNachname.Text = nachname;
            this.tbxGeburtstag.Text = geburtsdatum.ToShortDateString(); // Typ DateTime kann nicht inplizit in Typ string Konvertiert werden
            this.tbxFallNummer.Text = fallNr;
            //this.tbxJahre.Text =;
        }

        //_________________________________________________________________________
        //___________________Funktionen / Methoden
        //_________________________________________________________________________

        //neues Objekt um die Auswahl auch Anzeigen zu lassen
        List<DokumentenTyp> listeAuswahl = new List<DokumentenTyp>();

        // für den aktuellen Kontext der label.Background = Brushes.SkyBlue; in CheckBox_Checked
        //public Color LightBlue { get; private set; }
        // für den aktuellen Kontext der if (!listeAuswahl.Any()) in CheckBox_Checked
        public System.Windows.Media.Color Lavender { get; private set; }

        List<Druckauftrag> druckauftraege;

        // _____________________Funktion erlaubt Zugriff und verarbeitung der Auswahl_______________
        //__________________________________________________________________________________________
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            druckauftraege = new List<Druckauftrag>();
            if (!listeAuswahl.Any())
            {
                MessageBox.Show("    Zum Drucken Barcode wählen!    ");
                return; // Verbindung wird nicht aufgebaut
            }
            else
            {
                DBConnection dBConnection = new DBConnection();
                if (!dBConnection.CheckConnection())
                {
                    MessageBox.Show("Die Verbindung zur Datenbank konnte nicht hergestellt werden.");
                    return; // Methodenaustieg
                }

                MessageBox.Show(" Barcode wird gedruckt...!");
                List<DokumentenTyp> listeInDieDB = new List<DokumentenTyp>();

                foreach (DokumentenTyp auswahl in listeAuswahl)
                {
                    string lastInsertDb = "";
                    if (auswahl.Dokumententyp != DokumentenKategorie.XXX)
                    {
                        dBConnection.InsertData(auswahl.Dokumententyp, fallNr, auswahl.Bezeichnung, $"{nachname},{vorname}");
                        lastInsertDb = dBConnection.SelectData();
                    }
                    else
                    {
                        lastInsertDb = "NichtScannen";
                    }
                    druckauftraege.Add(new Druckauftrag(auswahl.Bezeichnung, $"{nachname},{vorname}", fallNr, lastInsertDb));
                    //MessageBox.Show($"Patient :{vorname} {nachname} Geburtstag : {geburtsdatum} PatientenNummer : 
                    //{fallNr} Barcode für Auswahl : {auswahl.Bezeichnung} wird gedruckt UND DAS IST DER TYP : {auswahl.Dokumententyp}");
                }
                DruckeEtiketten(druckauftraege);
                //MessageBox.Show(" Auftrag beendet! ")     
            }
        }
        [DllImport("kernel32.dll", SetLastError = true)]
        static extern SafeFileHandle CreateFile(string lpFileName, FileAccess dwDesiredAccess,
   uint dwShareMode, IntPtr lpSecurityAttributes, FileMode dwCreationDisposition,
   uint dwFlagsAndAttributes, IntPtr hTemplateFile);

        //___________________________DruckeEtiketten________________________________________________
        //__________________________________________________________________________________________
        private void DruckeEtiketten(List<Druckauftrag> druckAuftraege)
        {
            PrinterSettings printerSettings = new PrinterSettings();
            string printerName = "";
            int DruckerCounter = 0;
            var server = new PrintServer();
            var queues = server.GetPrintQueues(new[] { EnumeratedPrintQueueTypes.Shared, EnumeratedPrintQueueTypes.Connections });
            // ANZAHL LABETI ERMITTELN
            foreach (var item in queues)
            {
                if (item.Comment == "LABETI")
                {
                    DruckerCounter++;
                }
            }
            // WENN MEHR ALS 1x LABETI, DANN DRUCKERAUSWAHL
            if (DruckerCounter > 1)
            {
                //MessageBox.Show("Mehr als ein Drucker. Hier eine Druckerauswahl anbieten.");
                foreach (var item in queues)
                {
                    if (item.Comment == "LABETI")
                    {
                        //MessageBox.Show(item.FullName + " -> " + item.Comment);
                    }
                }
            }
            else
            {
                // NUR 1x LABETI, KEINE AUSWAHL, DRUCKEN!
                //MessageBox.Show("Es gibt nur einen Drucker. Das ist spitze!");
                foreach (var item in queues)
                {
                    if (item.Comment == "LABETI")
                    {
                        //MessageBox.Show(item.FullName + " -> " + item.Comment);
                        printerName = item.FullName;
                    }
                }
            }
            // MessageBox.Show("Druckername: " + printerName);
            foreach (Druckauftrag druckauftrag in druckAuftraege)
            {
                string s = druckauftrag.ToString();
                // Drucke den Inhalt von druckauftrag.ToString() auf den Drucker der in der Variable printerName
                RawPrinter.SendStringToPrinter(printerName, druckauftrag.ToString());
            }
        }
        //_________________________________________________________CheckBox Klickfunktion___________
        //__________________________________________________________________________________________
        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox checkbox = (CheckBox)sender;
            if (checkbox.IsChecked == true)
            {
                label.Foreground = System.Windows.Media.Brushes.Orange; // label Auswahl beim Klicken von CheckBox Makiert  /CornflowerBlue
                label2.Foreground = System.Windows.Media.Brushes.DarkBlue;
                RemoveAuswahl.Background = System.Windows.Media.Brushes.White;
                // schreiben in das Feld Auswahl inhalt der gewählten CheckBox
                DokumentenTyp typ = (DokumentenTyp)checkbox.Content;
                listeAuswahl.Add(typ);
            }
            else
            {
                // entfernt den Inhalt aus der Auswahl beim wiederholten Klick auf CheckBox
                listeAuswahl.Remove((DokumentenTyp)checkbox.Content);
                // label Auswahl beim remove von CheckBox, Makierung entfernen
                if (!listeAuswahl.Any())
                {
                    SolidColorBrush sb = new SolidColorBrush(Lavender);
                    label2.Foreground = sb;
                    label.Foreground = System.Windows.Media.Brushes.Lavender;
                    SolidColorBrush rb = new SolidColorBrush(Lavender);
                    RemoveAuswahl.Background = rb;
                }
            }
            lb_Auswahl.ItemsSource = null;
            lb_Auswahl.ItemsSource = listeAuswahl;
        }

        //___________________entfernt die Auswahl___________________________________________________
        //__________________________________________________________________________________________

        // um die Auswahl nach Bedarf zu Löschen 
        List<CheckBox> AlleCheckboxen = new List<CheckBox>();

        private void RemoveCheckBoxes_Click(object sender, RoutedEventArgs e)
        {
            if (lb_Auswahl.SelectedItems.Count == 0)
            {
                MessageBox.Show("     Zum löschen Auswahl markieren!     ");
                return;
            }

            IEnumerable<DokumentenTyp> selectedTypen = lb_Auswahl.SelectedItems.Cast<DokumentenTyp>();
            foreach (var selectedTyp in selectedTypen.ToList())
            {
                CheckBox selectedAuswahl = null;

                selectedAuswahl = AlleCheckboxen.Find(checkbox => checkbox.Content == selectedTyp);
                selectedAuswahl.IsChecked = false;
            }
            //DokumentenTyp selectedTyp = (DokumentenTyp)lb_Auswahl.SelectedItem;
        }
        //________________dynamische Anordnung der Elemente (Tab, GroupBox, CheckBox, etc.)_________ 
        //__________________________________________________________________________________________

        private void initFile()
        {
            XmlDocument doc = new XmlDocument();
            //Prüfung auf Valides XML dokument
            doc.Load(@"../config.xml");

            // erstellt das Dokument oberste Ebene
            XmlElement erstesElement = doc.DocumentElement; // Ruft das StammElement für das Dokument ab
            foreach (XmlNode tabPageXml in erstesElement.ChildNodes) // Ruft alle untergeordneten Knoten ab
            {
                // erstellt den Reiter nachfolgende Ebene
                TabItem tabItem = new TabItem(); // Iniziallisiert eine neue Instanz der TabItem-Klasse
                tabItem.Header = tabPageXml.Name;
                tabItem.Foreground = System.Windows.Media.Brushes.Navy;
                WrapPanel wrapPanel = new WrapPanel();
                wrapPanel.Orientation = Orientation.Horizontal;
                wrapPanel.HorizontalAlignment = HorizontalAlignment.Stretch;

                foreach (XmlNode groupBoxBezeichnungXml in tabPageXml.ChildNodes)
                {
                    // listBox beinhaltet groupBox, listBox2, checkBoxen 
                    ListBox listBox = new ListBox();
                    GroupBox groupBox = new GroupBox();
                    groupBox.Header = groupBoxBezeichnungXml.Name;
                    groupBox.Width = 262;
                    groupBox.Height = 226;
                    groupBox.Background = System.Windows.Media.Brushes.Gainsboro;
                    groupBox.Foreground = System.Windows.Media.Brushes.DarkBlue;

                    foreach (XmlNode checkBoxXml in groupBoxBezeichnungXml.ChildNodes)
                    {
                        ListBoxItem listBoxItem = new ListBoxItem();
                        CheckBox checkBoxControl = new CheckBox();
                        DokumentenTyp typ = new DokumentenTyp(checkBoxXml.Name, GetKategorieFromXml(checkBoxXml.InnerText)); // InnerText = Kategorie 
                        checkBoxControl.Content = typ;
                        checkBoxControl.Background = System.Windows.Media.Brushes.Lavender;

                        checkBoxControl.Checked += CheckBox_Checked;
                        checkBoxControl.Unchecked += CheckBox_Checked;
                        AlleCheckboxen.Add(checkBoxControl);
                        listBoxItem.Content = checkBoxControl;
                        listBox.Items.Add(listBoxItem);
                    }
                    groupBox.Content = listBox;
                    // Abstand der GroupBoxen zueinander
                    groupBox.Margin = new Thickness(29, 29, 0, 0);
                    wrapPanel.Children.Add(groupBox);
                }
                ScrollViewer scrollViewer = new ScrollViewer();
                scrollViewer.Background = System.Windows.Media.Brushes.Lavender;
                scrollViewer.Content = wrapPanel;
                tabItem.Content = scrollViewer;
                tcBehandlungen.Items.Add(tabItem);
            }
        }
        //________________________________DokumentenKategorie_______________________________________
        //__________________________________________________________________________________________
        private DokumentenKategorie GetKategorieFromXml(string xml)
        {
            switch (xml)
            {
                case "BGDOKU":
                    return DokumentenKategorie.BGDOKU;
                case "B_GE":
                    return DokumentenKategorie.B_GE;
                case "B_HA":
                    return DokumentenKategorie.B_HA;
                case "EINVER":
                    return DokumentenKategorie.EINVER;
                case "ESAVTI":
                    return DokumentenKategorie.ESAVTI;
                case "EXTAB":
                    return DokumentenKategorie.EXTAB;
                case "EXTAP":
                    return DokumentenKategorie.EXTAP;
                case "EXTBE":
                    return DokumentenKategorie.EXTBE;
                case "EXTMP":
                    return DokumentenKategorie.EXTMP;
                case "EXTUE":
                    return DokumentenKategorie.EXTUE;
                case "H_NR":
                    return DokumentenKategorie.H_NR;
                case "KONFPR":
                    return DokumentenKategorie.KONFPR;
                case "LUFU":
                    return DokumentenKategorie.LUFU;
                case "MDKDOK":
                    return DokumentenKategorie.MDKDOK;
                case "PATDOK":
                    return DokumentenKategorie.PATDOK;
                case "PATVER":
                    return DokumentenKategorie.PATVER;
                case "SAPO":
                    return DokumentenKategorie.SAPO;
                case "SAUFN":
                    return DokumentenKategorie.SAUFN;
                case "SCORES":
                    return DokumentenKategorie.SCORES;
                case "SCREEN":
                    return DokumentenKategorie.SCREEN;
                case "SDIA":
                    return DokumentenKategorie.SDIA;
                case "SKONSIL":
                    return DokumentenKategorie.SKONSIL;
                case "SLABOR":
                    return DokumentenKategorie.SLABOR;
                case "SNOT":
                    return DokumentenKategorie.SNOT;
                case "SOP":
                    return DokumentenKategorie.SOP;
                case "SPPAS":
                    return DokumentenKategorie.SPPAS;
                case "SSONST":
                    return DokumentenKategorie.SSONST;
                case "SSOZ":
                    return DokumentenKategorie.SSOZ;
                case "XXX":
                    return DokumentenKategorie.XXX;
                default:
                    // Wenn neue Kategorie hinzugefügt wird, welche nicht im Code zugewiesen ist dann switch case und enum erweitern!
                    throw new Exception("DokumentenKategorie konnte nicht zugeordnet werden");
            }
        }

        //____________________________________________________________________InfoBox_______________
        //__________________________________________________________________________________________
        private void InfoBox_Click(object sender, RoutedEventArgs e)
        {
            Window1 picInfo = new Window1();
            picInfo.Show();

            //picInfo.Close();
            //MessageBox.Show("                © by Daniel Serazin\n Augusta-Kranken-Anstalt Bochum gGmbH\n                        2019-2020 ");
        }
    }

    //_____________________DruckerAuswahl________________________________________________________
    //___________________________________________________________________________________________
    //private void cmbPrinterSelection_SelectionChanged(object sender, SelectionChangedEventArgs e)
    //{
    //    foreach (string printer in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
    //    {
    //        cmbPrinterSelection.Items.Add(printer);
    //    }
    //}





}

