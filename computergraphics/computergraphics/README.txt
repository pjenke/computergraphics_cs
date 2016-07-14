FRAMEWORK FÜR DAS WP "COMPUTERGRAFIK" - KURZANLEITUNG

1) Bedienung
- Rotation um Oben-Vektor: linke Maustaste gedrückt halten + horizontale Mausbewegung
- Rotation um Seiten-Vektor: linke Maustaste gedrückt halten + vertikale Mausbewegung
- Zoomen: rechte Maustaste gedrückt halten + vertikale Mausbewegung

2) Framework
- alle Klassen in "framework"
- "math" beinhaltet Funktionalität für mathematische Berechnungen
- "mesh" beinhaltet Basisklassen für Netz-Datenstrukturen
- "scenegraph" beinhaltet Klassen für Szenengraph

3) Entwicklung
- neue Klasse in "exercises", die von Scene erbt (Beispiel: Exercise 1)
- Animations-Timeout setzen (= Millisekunden zwischen zwei Aufrufen von timerTick()
  der Szenengraphen-Knoten
- Shader-Modus setzen: 
  - PHONG: Phong-Beleuchtungsmodell mit Oberflächenfarbe
  - TEXTURE: Phong-Beleuchtungsmodell mit Textur
  - NO_LIGHTING: nur Oberflächenfarbe, keine Beleuchtung
- eigene Knoten für Szenengraphen: Interface Node implementieren