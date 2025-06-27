# 🛰️ Proxy Server

---

## 📌 Opis projekta

Sistem simulira rad pametnih uređaja koji šalju analogna ili digitalna merenja serveru. 
Komunikacija između klijenta i servera odvija se preko proxy servera, koji kešira i optimizuje pristup podacima.

---

## 🧩 Komponente sistema

### 🟢 **Device**
- Generiše i automatski šalje merenja na svakih 5 minuta

### 🔵 **Server**
- Centralna instanca koja prima i čuva podatke
- Omogućava čitanje podataka na osnovu zahteva
- Svaki događaj evidentira u log fajl (`server-log.txt`)

### 🟣 **Proxy**
- Posreduje između klijenta i servera
- Kešira poslednje preuzete podatke
- Proverava da li su lokalni podaci ažurni
- Briše neaktivne keširane podatke posle 24h

### 🟡 **Client**
- Pristupa podacima putem proxy-ja
- Korisnički interfejs zasnovan na komandnoj liniji
- Omogućava:
  - Čitanje svih merenja za određeni uređaj
  - Čitanje poslednje vrednosti za određeni uređaj
  - Čitanje poslednjih vrednosti za sve uređaje
  - Čitanje analognih ili digitalnih merenja

---
## ✅ Testiranje

- Testirani su modeli, kao i logika unutar servisnih slojeva
  
