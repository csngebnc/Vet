# Vet
Állatorvosi adatnyilvántartó és időpontfoglaló webalkalmazás REST API backend-del és SPA frontend-del.

## Felhasznált technológiák:
- Frontend: Angular
- Backend: ASP.NET Core, Entity Framework

## Specifikáció pontokba szedve:
- Lehetőség felhasználói fiókok regisztrálására, bejelentkezésre
- Felhasználók három jogosultsági szintjei:
  - általános felhasználló
  - állatorvos
  - tulajdonos (állatorvos)
- Regisztráció után a felhasználó általános jogosultsággal rendelkezik, ebből kinevezhető orvossá
- A felhasznasználó szerkesztheti profilját, önmagáról képet tölthet fel

### Felhasználó által végezhető műveletek:
- A felhasználó hozzáadhat hozzá tartozó állatokat
  - Az állatok hozzáadása egy űrlap segítésével történik, kép feltöltése az állatról opcionális
  - Véletlen hozzáadás esetén törlési lehetőség adott
  - Elpusztult állat esetén a hozzá tartozó adatok archiválhatók
- A már hozzáadott állatok adatait a felhasználó szerkesztheti, korábbi ismert oltásokat felvehet
- Az állat kórlapjait megtekintheti, PDF-ként letöltheti
- Az állatorvosi rendelő által megadott időpontok alapján a rendszerben időpontot foglalhat:
  - Állattól függetlenül
  - Állat számára

### Állatorvos által végezhető műveletek:
- Profilján jegyzetet rögzíthet
- Láthatja a soron következő időpontokat, ezekkel műveleteket végezhet:
  - Kórlap készítés időponthoz
  - Időponthoz tartozó adatok (felhasználó/állat) megtekintése
  - Időpont törlése
- Időpontot foglalhat felhasználók számára e-mail és/vagy állat megadásával
- Kórlapot készíthet, melyhez képeket is csatolhat
  - Nem regisztrált felhasználó esetén megadott e-mail alapján elérhető lesz a kórlap, ha a gazdi regisztrál
- Megtekintheti a pácienseket, közöttük szűréseket hajthat végre

### Tulajdonos (állatorvos) által végezhető műveletek:
- Felhasználói fiókok jogosultságát állíthatja (felhasználó/állatorvos)
- Beállíthatja az időpontfoglalás rendszerét
- A kórlapokhoz rögzíthető kezelésekhez új elemeket vehet fel, vagy szerkesztheti a már meglévőket

## Bővítési lehetőségek:
- Általános weboldal megvalósítása
  - Tartalomkezelő beépítése 
- Real-time üzenet funkció dolgozók és gazdik között
- Időpontok foglalásának kiegészítése
- Kórlap kezeléseinek kibővítése
  - Kategóriák
  - Árak, majd összesítő készítés
  



  

