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
+ Mit dieser Anwendung sollen Zeitersparnis, Kostenersparnis und gesteigerte Effizienz (schnellerer Aufruf der Anwendung, 
leicht Erweiterbar durch dynamischen Aufbau der Anwendung über XML.config (Programm gliedert sich zur Laufzeit), 
mehrere Barcode Aufträge in einem durchlauf realisierbar) erreicht werden.

Dieses Programm, wird zentral mit zusätzlichen Übergabeparametern, welche aus Vorname, Nachname,geb.Datum und Patientennummer bestehen gestartet. 
Dafür ist eine eingerichtete Datenbank vorgesehen, um die Patientendaten und die Art der gewählten Dokumentation zu speichern, sowie abzufragen. 
Die Anwendung soll auf einem Server abgelegt werden auf das jeder Client zugreifen kann. 
Um Barcodes erstellen zu können soll der verwendete EtikettenDrucker so angesprochen werden, dass eine Geräteauswahl vor jedem Druck vermieden wird. 
Dazu wird ein bestimmtes Merkmal, welches über einen Kommentar als Bsp. LABETI abgespeichert ist und der Druckeridentifizierung dient, verwendet werden. 
Der Druck des Barcodes wird an den dafür vorkonfigurierten Netzwerkdrucker gesendet, welcher über das Anmeldeskript festgelegt wird. 
