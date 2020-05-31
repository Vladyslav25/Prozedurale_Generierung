# Prozedurale_Generierung | 12.2019, 9 Monate

Was habe ich gelernt:
  - Prozedurale Generierung
  - Final State Maschine
  - ScriptabelObjects in Unity

Es war Aufgabe eine prozedural Generierte Umgebung zu erschaffen, auf welcher dann eine KI mit einer Final State Maschine nachvollziehbar interagieren soll.

In meiner Abgabe wird eine Stadt mit Stadtmauern prozedural generiert. In dieser Stadt laufen dann Zombies (grüne Kapseln) herum. Wenn der Spieler von diesen gesehen wird greifen sie ihn an. Bei Kontakt verliert man.
Wenn der Spieler es aber schafft das Schwert in der Stadt zu finden, dann sterben die Zombies bei Kontakt. Doch sobald er das Schwert aufgehoben hat, laufen die Zombies weg.

Generierung:
Es gibt eine Plane unter dem Boden, welche die maximale Größe der Stadt bestimmt. Per Seed wird dann ein zufälliger Punkt auf diesem Areal als Stadtmitte ausgesucht. Daraufhin wird die Größe der Stadt bestimmt, da die Mitte zu allen vier Seiten denselben Abstand zum Ende der Stadt haben soll. Die Größe wissend kann man jetzt vom Mittelpunkt aus die Ecken der Stadt bestimmen. An diesen Ecken werden Türme platziert. Dann wir vom einem Turm aus Teil für Teil die Mauer gebaut bis zu Hälfte der Strecke. Denn an diese Position wird das Tor erstellt. Dann wird von der gegenüberliegenden Seite an die Mauer weiter fertig gebaut. Dies wird für die restlichen 3 Seiten der Stadt wiederholt.
Drinnen werden dann vom Zentrum aus Kreise bestimmt, welche das Zentrum als Mittelpunkt und gleich steigende Radien haben. Auf diesen Kreisen wird dann eine per Seed zufällige Position ausgewählt, wenn die Position nicht zu nah an einem anderen Gebäude ist, wird dort ein Gebäude platziert. Die Art des Gebäudes ist abhängig davon wie weit weg es vom Mittelpunkt ist. Dies wird dann für die angegebene Anzahl (Gebäude Pro Kreis) wiederholt bis alle Kreise eine Position besitzen. Dazu wird dann per Seed eine zufällige Rotation des Gebäudes gewählt.



KI:
![alt text](https://github.com/Vladyslav25/Prozedurale_Generierung/blob/master/FSM_Prozeduarale%20Generierung.jpg)
