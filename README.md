<div align="center">
![logo](readme_src/filmotekalogo.png)

&nbsp;

# **Filmoteka**

&nbsp;

##### Strona, której zadaniem jest umożliwić użytkownikom wybór idealnego filmu na wieczór. Każdy z użytkowników może dodawać filmy, komentarze i oceny do wybranej pozycji. Strona przyjemna i łatwa w odbiorze.

&nbsp;

&nbsp;

### **Adres strony**
http://filmotekaa-001-site1.atempurl.com/


</div>


&nbsp;

&nbsp;

&nbsp;

&nbsp;


<div align="right">

#### Autorzy &emsp;&emsp;&emsp;&nbsp;&nbsp;
##### Szymon Cieśla  &emsp;&emsp;&nbsp;
##### Szymon Zielonka  &emsp;

</div>


## Spis treści:
1. ##### [Work Breakdown Structure](#work-breakdown-structure)
2. ##### [Harmonogram](#harmonogram)
3. ##### [Specyfikacja wymagań biznesowch](#specyfikacja-wymagan-biznesowch)
    1. ##### [Poznaj Filmoteke!](#poznaj-filmoteke) 
    2. ##### [Wymagania funkcjonalne](#wymagania-funkcjonalne)
    3. ##### [Wymagania niefunkcjonalne](#wymagania-niefunkcjonalne)
    4. ##### [Opis ograniczeń systemu](#opis-ograniczen-systemu)
4. ##### [Dokumentacja techniczna](#dokumentacja-techniczna)
    1. ##### [Architektura systemu](#architektura-systemu)
    2. ##### [Struktura bazy danych](#struktura-bazy-danych)
    3. ##### [Diagram UML](#diagram-uml)
    4. ##### [Instalacja środowiska developerskiego](#instalacja-srodowiska-developerskiego) 
    5. ##### [Grafiki koncepcyjne](#grafiki-koncepcyjne)
5. ##### [Dokumentacja użytkownika](#dokumentacja-uzytkownika)
    1. ##### [Funkcjonalność Gościa](#funkcjonalność-goscia)
    2. ##### [Funkcjonalność Uzytkownika](#funkcjonalność-uzytkownika)
    3. ##### [Funkcjonalność Administratora](#funkcjonalność-administratora)


&nbsp;

## Work Breakdown Structure

&nbsp;

<div align="center">

#### Szymon Cieśla - Developer
#### Szymon Zielonka - Developer

&nbsp;

##### Spotkania 2 razy w tygodniu, czas pracy w przedziale od 2-5h.

</div>

&nbsp;

## Harmonogram

<div align="center">

&nbsp;

| Data zajęć | Opis realizowanych zagadnień |
| --- | --- |
| 05.10.2022 | Wypełnienie karty zadań |
| 12.10.2022 | Integracja z systemem GitLab |
| 19.10.2022 | Praca nad utworzeniem i hostingiem bazy danych, projekt diagramu UML, stworzenie wstępnych modeli oraz prace nad dokumentacją |
| 26.10.2022 | Projekt i implementacja kontrolerów oraz widoków filmów |
| 02.11.2022/09.11.2022 | Tworzenie widoków, implementacja modułu rejestracji i logowania |
| 16.11.2022 | Implementacja modułu odpowiedzialnego za dodawanie komentarzy do filmów i serialów oraz modułu odpowiedzialnego za ich oceny w systemie gwiazdkowym |
| 23.11.2022 | Utworzenie modułu do oglądania filmów oraz seriali |
| 30.11.2022 |Stworzenie modułu wyszukiwania i sortowania filmów. Utworzenie biblioteki dodanych filmów przez użytkownika oraz widoku strony głównej (projekt – 90% ukończenia, dokumentacja – 40%) |
| 07.12.2022 | Finalizacja pracy nad projektem i hostowanie aplikacji (projekt - 100%, dokumentacja 100%) |
| 14.12.2022 | Poprawa dokumentacji (projekt - 100%, dokumentacja 100%)  |
| 21.12.2022 | Prace konserwacyjne |
| 04.01.2023 | Prace konserwacyjne |
| 11.01.2023 | Prezentacje końcowych wersji projektów, oddanie dokumentacji |
| 18.01.2023 | Końcowe zaliczenie projektu |
</div>

#

# Specyfikacja wymagań biznesowch

## Poznaj Filmoteke

Szukasz idealnego filmu na wieczór? Myślisz że obejrzałeś już wszystkie filmy i seriale. Oto rozwiązanie dla Ciebie!  
**Filmoteka** jest to serwis internetowy który na celu ma ułatwić Tobie, naszemu użytkownikowi wybór idealnego filmu na wieczór na podstawie komentarzy oraz ocen innych użytkowników.

![obraz.png](readme_src/widok-glowny.png)

## Wymagania funkcjonalne

Dodawanie pozycji przez zalogowanych użytkowników. Zalogowani użytkownicymogą również oceniać inne pozycje oraz je komentować. Uprawenienia osób nie zalogowanych kończą się na przeglądaniu pozycji innych użytkowników. Strona umożliwia sortowanie po gatunkach oraz wyszukiwanie po tytule filmu. Administrator ma dostęp do wykresów statystycznych oraz posiada możliwość edycji każdego filmu i komentarza w bazie danych.

## Wymagania niefunkcjonalne

Aplikacja aplikacja ma być dostępna w systemie 24/7. Bezpieczeństwo danych użytkowników. Kontakt z administatorami przez email. Niezawodność i wydajność.

## Opis ograniczeń systemu

Największym i najbardziej poważnym ograniczeniem naszego systemu są prawa autorskie do filmów, seriali oraz ich elementów czyt. np. grafiki komercyjne. Strona powinna być prowadzona w taki sposób, by w jak najlepszy sposób nie konfliktować z prawem.


&nbsp;

# Dokumentacja techniczna

## Architektura systemu

![struktura_bazy_danych.png](readme_src/architektura.png)

Powyższy diagram ukazuje całe zaplecze technologiczne Filmoteki. Wykorzystujemy składowe ASP.NET Core Architecture. ASP.NET pozwala na budowanie wydajnych, wieloplatformowych aplikacji internetowych. Wzorce takie jak MVC i wbudowana obsługa Dependency Injection pozwalają na budowanie aplikacji, które są łatwiejsze w utrzymaniu.

&nbsp;

## Struktura bazy danych

![struktura_bazy_danych.png](readme_src/diagrambazydanych.png)

Diagram ten przedstawia strukturę naszej bazy danych. Jak można zauważyć składa się ona z 10 tabel.

&nbsp;

## Diagram UML
![obraz.png](readme_src/diagramuml.png)
Prezentujemy Diagram UML naszego dzieła.

## Instalacja środowiska developerskiego

1. Pobierz Visual Studio 22 z strony [Microsoftu](https://visualstudio.microsoft.com/pl/).
2. Zainstaluj Visual Studio wybierając następno wybrane moduły: ![obraz.png](readme_src/obraz.png)
3. Po pomyślnej instalacji programu do edycji kodu sklonuj repozytorium korzystając z tego linku:

```
https://gitlab.com/TestMyVeins/filmoteka.git
```
4. Środowisko jest gotowe do wprowadzania zmian

&nbsp;
## Grafiki koncepcyjne
![obraz.png](readme_src/conception/Screenshot1.png)
![obraz.png](readme_src/conception/Screenshot2.png)
![obraz.png](readme_src/conception/Screenshot3.png)
![obraz.png](readme_src/conception/Screenshot4.png)
![obraz.png](readme_src/conception/Screenshot5.png)
![obraz.png](readme_src/conception/Screenshot6.png)
![obraz.png](readme_src/conception/Screenshot7.png)
![obraz.png](readme_src/conception/Screenshot8.png)
&nbsp;

# Dokumentacja użytkownika
## Funkcjonalność Gościa

Przy wejściu na naszą stronę pojawiamy się na stronie głównej:

![obraz.png](readme_src/quest/1.png)

**Gość(*osoba niezalogowana*) może podjąć następujące działania:**
 
  - Stworzyć nowe konto: ![obraz.png](readme_src/quest/2.png)
  - Zalogować się na istniejące konto: ![obraz.png](readme_src/quest/3.png)
  - Przypomnieć hasło do istniejącego konta: ![obraz.png](readme_src/quest/4.png)
  - Ponowić wysłanie linku aktywacyjnego do konta: ![obraz.png](readme_src/quest/5.png)
  - Przeglądać dodane filmy: ![obraz.png](readme_src/quest/6.png)
  - Przeglądać dodane seriale: ![obraz.png](readme_src/quest/7.png)
  - Korzystać z wyszukiwania pozycji: ![obraz.png](readme_src/quest/8.png)
  - Wyświetlać pozycje filmów i seriali: ![obraz.png](readme_src/quest/9.png)

## Funkcjonalność Uzytkownika

**Użytkownik jest rozszerzeniem roli Gościa, posiada on dodatkowe uprawnienia, przez które może dodatkowo:**

  - Oceniać pozycje filmów i seriali. Ocena może być wydana tylko raz i nie może być potem zmieniona: ![obraz.png](readme_src/quest/10.png)
  - Dodawać komentarze do filmów i seriali: ![obraz.png](readme_src/quest/11.png)![obraz.png](readme_src/quest/12.png)
  - Dodawać linki do hostingów z filmem, by umożliwić oglądanie filmów użytkownikom: ![obraz.png](readme_src/quest/13.png)
  - Oglądać filmy i seriale: ![obraz.png](readme_src/quest/14.png)
  - Dodawać nowe pozycje do filmów i seriali: ![obraz.png](readme_src/quest/15.png)![obraz.png](readme_src/quest/16.png)
  - Przeglądać i śledzić dodane przez siebie pozycje: ![obraz.png](readme_src/quest/17.png)
   - Wylogować się: ![obraz.png](readme_src/quest/18.png)
  
## Funkcjonalność Administratora

**Administrator jest rozszerzeniem roli Użytkownika, posiada on dodatkowe uprawnienia**

Przy zalogowaniu się na użytkownika z rolą Administratora pojawia się panel administratora:

![obraz.png](readme_src/quest/19.png)
  
Z tego miejsca administrator może wykonać następujące działania:
  - Zatwierdzać dodane filmy i seriale: ![obraz.png](readme_src/quest/20.png)
  - Anulować dodane filmy i seriale: ![obraz.png](readme_src/quest/21.png)
  - Przeglądać statystyki filmów: ![obraz.png](readme_src/quest/22.png)
  *Wykres ilustrujący ile zostało dodanych filmów w poszczególnych miesiącach*
  ![obraz.png](readme_src/quest/23.png)
  *Wykres ilustrujący ile filmów poszczególnych gatunków zostało dodanych w wybranym miesiącu*
  ![obraz.png](readme_src/quest/24.png)
  *Wykres ilustrujący oceny filmów wybranego gatunku*

  - Przeglądać statystyki seriali: 
  ![obraz.png](readme_src/quest/25.png)
  *Wykres ilustrujący ile zostało dodanych seriali w poszczególnych miesiącach*
  ![obraz.png](readme_src/quest/26.png)
  *Wykres ilustrujący ile seriali poszczególnych gatunków zostało dodanych w wybranym miesiącu*
  ![obraz.png](readme_src/quest/27.png)
  *Wykres ilustrujący oceny seriali wybranego gatunku*




#
