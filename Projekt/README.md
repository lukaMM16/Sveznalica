# Sveznalica – Web aplikacija za kvizove

## Opis projekta

Sveznalica je web aplikacija razvijena u ASP.NET MVC tehnologiji koja omogućuje korisnicima igranje kvizova, natjecanje s drugim korisnicima, stvaranje timova i pregled rezultata kroz rang liste.

Aplikacija podržava dvije razine korisničkih prava:

 Administrator
 Registrirani korisnik

Administrator upravlja sadržajem aplikacije (kvizovi, pitanja, kategorije), dok korisnici mogu igrati kvizove, slati izazove i sudjelovati u timovima.



## Tehnologije

Projekt koristi sljedeće tehnologije:

 ASP.NET MVC (.NET Framework)
 C#
 MySQL baza podataka
 Repository pattern arhitektura
 Bootstrap 5 (UI)
 Session-based autentikacija



## Funkcionalnosti

### Registracija i autentikacija

Korisnici mogu:

 registrirati račun
 prijaviti se u sustav
 uređivati profil
 postaviti avatar URL



### Igranje kvizova

Korisnik može:

 odabrati kviz po kategoriji
 igrati kviz
 dobiti rezultat nakon završetka
 pregledati povijest svojih rezultata



### Rang lista

Sustav prikazuje:

 najbolje rezultate po kvizu
 top igrače
 osobne rezultate korisnika



### Challenge sustav

Korisnici mogu:

 poslati izazov drugom korisniku
 prihvatiti izazov
 igrati isti kviz kao protivnik



### Timovi

Korisnici mogu:

 kreirati tim
 dodavati članove
 pregledavati članove tima



### Administratorske funkcionalnosti

Administrator može:

 dodavati kategorije
 uređivati kategorije
 dodavati kvizove
 uređivati kvizove
 dodavati pitanja
 uređivati pitanja
 brisati sadržaj



## Struktura projekta

Projekt koristi MVC arhitekturu:

Controllers/
Models/
Views/
Repositories/
ViewModels/

Repository pattern koristi se za pristup bazi podataka.



## Baza podataka

Glavne tablice:

users
categories
quizzes
questions
answer_options
quiz_attempts
teams
team_members
challenges



## Pokretanje projekta

1. Pokrenuti MySQL server
2. Importirati bazu podataka
3. Otvoriti projekt u Visual Studio
4. Pokrenuti aplikaciju pomoću IIS Express

---

## Autor

Ime: Luka Mikac
Studij: Računarstvo – Međimursko veleučilište u Čakovcu
