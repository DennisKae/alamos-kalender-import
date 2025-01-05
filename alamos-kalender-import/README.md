# alamos-kalender-import

Anwendung zum massenhaften Import von Terminen aus einer Excel Datei in Kalender des Alamos FE2 Servers.

***Disclaimer:*** Es werden keine offiziellen Schnittstellen des Alamos FE2 Servers genutzt. Diese Anwendung kann also jederzeit durch ein FE2 Update kaputt gehen.

Zuletzt getestete FE2 Version: **2.34.118-STABLE**

## Systemvoraussetzungen

- .NET 8 Runtime
  - Installationsstatus prüfen: ``dotnet --list-runtimes`` in einer Konsole ausführen. 
  - Download: https://dotnet.microsoft.com/en-us/download/dotnet/

## Verwendung (nach Veröffentlichung als dotnet tool)

- Anwendung mit ``dotnet tool install alamos-kalender-import --global`` installieren.
- ``alamos-kalender-import`` in einer Konsole ausführen. 
- Alle benötigten Informationen werden interaktiv abgefragt.

### Excel-Vorlage

- Eine Vorlage kann [hier]() herunter geladen werden.
- Es wird immer das erste Tabellenblatt ausgewertet.
- In der ersten Zeile der Tabelle müssen die Überschriften stehen.
- Es können beliebige sonstige Spalten eingefügt werden. Nur die in der Vorlage gelb markierten Spalten sind zwingend notwendig.

## Fehleranalyse bei Netzwerkproblemen
- Wenn die Anwendung mit dem Parameter "--debug" gestartet wird, wird jede Serveranfrage und -Antwort ausgegeben.

## (Noch) nicht umgesetzte Features

- Veröffentlichung als dotnet tool
  - [Best Practices](https://learn.microsoft.com/en-us/nuget/create-packages/package-authoring-best-practices) berücksichtigen
- Validierung der Daten aus der Excel Datei.
- Individuelle Benachrichtigungen
  - Aktuell wird jeder Termin hart mit folgenden Benachrichtigungen angelegt:
    - Push Benachrichtigung 1 Tag vor dem Termin
    - Push Benachrichtigung 1 Stunde vor dem Termin
- Unterstützung verschiedener Termintypen
  - Aktuell werden alle Termine mit dem Typ "Übung" angelegt
- Ganztägige Termine und Termine, die über mehrere Tage gehen, unterstützen
  - Aktuell werden nur Termine unterstützt, die am gleichen Tag beginnen und enden.
- Login als Administrator

## Bekannte Probleme

- Der API Token, der für Anfragen an den FE2 Server genutzt wird, wird hart für 30 Sekunden gecached. Dies kann u.U. zu Problemen führen.
  - Hintergrund: Aktuell kann die Gültigkeitsdauer des API Tokens nicht bestimmt werden.