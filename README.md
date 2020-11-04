# DokumentenScan
Desktopanwendung zur dezentralen Erstellung von Barcodeetiketten.

Der Funktionsumfang der Software beinhaltet folgende Punkte:
+	Schnellerer Zugriff des Programms durch Einbindung in die Patientenfallakte des Abkürzungsverzeichnis
+ Implementierung eines Warenkorb-Systems zur Stapelverarbeitung von Barcodes
+	Vereinfachte Administration durch Konfigurationsdateien

Durch diese Erweiterungen soll eine zeitlich effizientere Arbeitsweise der Anwender und der Systemadministration gewährleistet werden. 
+ Die Daten können über festgelegte Optionen ausgewählt und als Barcode ausgegeben werden.
+ Die Oberfläche zur Erstellung des Barcodes ist im Informationszentrum integriert und wird innerhalb der klinischen Dokumente 
über Auswahl der Vorlagen aufgerufen.
+ Durch diese Anwendung wird Zeitersparnis, Kostenersparnis und gesteigerte Effizienz(schnellerer Aufruf der Anwendung) erreicht. 
Das Programm ist leicht Erweiterbar durch einen dynamischen Aufbau mit Hilfe von XML.config(Programm gliedert sich zur Laufzeit). 
Mehrere Barcode Aufträge sind in einem durchlauf realisierbar.

Dieses Programm, wird zentral mit zusätzlichen Übergabeparametern, welche aus Vorname, Nachname, geb.Datum und Patientennummer bestehen gestartet. 
Dafür ist eine eingerichtete Datenbank vorgesehen, welche die Patientendaten und die Art der gewählten Dokumentation speichert sowie abfragt. 
Die Anwendung wird auf einem Server abgelegt, welcher für jeden Client erreichbar ist. 
Um Barcodes erstellen zu können, wird der verwendete EtikettenDrucker so angesprochen, dass eine Geräteauswahl vor jedem Druck zu vermieden ist. 
Zur Druckeridentifizierung wird ein bestimmtes Merkmal verwendet, welches über einen Kommentar als Bsp. LABETI in den Geräteeigenschaften des EtiDruckers abgespeichert. 
Der Druck des Barcodes wird an den dafür vorkonfigurierten Netzwerkdrucker, der über ein Anmeldeskript festgelegt ist, gesendet. 
