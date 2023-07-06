using Microsoft.EntityFrameworkCore;
using ZleceniaAPI.Entities;

namespace ZleceniaAPI.Seeders
{
    public class CategorySeeder
    {
        private OferiaDbContext _dbContext;

        public CategorySeeder(OferiaDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Seed()
        {
            if (_dbContext.Database.CanConnect())
            {
                var pendingMigrations = _dbContext.Database.GetPendingMigrations();
                if(pendingMigrations != null && pendingMigrations.Any())
                {
                    _dbContext.Database.Migrate();
                }

                if (!_dbContext.Categories.Any())
                {
                    var categories = GetCategories();
                    _dbContext.Categories.AddRange(categories);
                    _dbContext.SaveChanges();
                }
            }
        }

        private IEnumerable<Category> GetCategories()
        {
            var categories = new List<Category>()
            {
                new Category()
                {
                    Name = "Budowa, Remont",
                    ChildCategories = new List<Category>()
                    {
                        new Category()
                        {
                            Name = "Budowa",
                            ChildCategories = new List<Category>()
                            {
                                new Category()
                                {
                                    Name = "Archeologia"
                                },
                                new Category()
                                {
                                    Name = "Architektura"
                                },
                                new Category()
                                {
                                    Name = "Architektura krajobrazu"
                                },
                                new Category()
                                {
                                    Name = "Betoniarstwo i zbrojenie"
                                },
                                new Category()
                                {
                                    Name = "Bramy i ogrodzenia"
                                },
                                new Category()
                                {
                                    Name = "Brukarstwo i kamieniarstwo"
                                },
                                new Category()
                                {
                                    Name = "Budownictwo telekomunikacyjne"
                                },
                                new Category()
                                {
                                    Name = "Ciesielstwo i stolarstwo"
                                },
                                new Category()
                                {
                                    Name = "Dachy, rynny, blacharstwo, dekarstwo"
                                },
                                new Category()
                                {
                                    Name = "Deweloperstwo"
                                },
                                new Category()
                                {
                                    Name = "Docieplanie budynków"
                                },
                                new Category()
                                {
                                    Name = "Domy prefabrykowane"
                                },
                                new Category()
                                {
                                    Name = "Domy z drewna"
                                },
                                new Category()
                                {
                                    Name = "Drogi i mosty"
                                },
                                new Category()
                                {
                                    Name = "Dźwigi, koparki, podnośniki"
                                },
                                new Category()
                                {
                                    Name = "Ekspertyzy, kosztorysy"
                                },
                                new Category()
                                {
                                    Name = "Fundamenty i wykopy"
                                },
                                new Category()
                                {
                                    Name = "Garaże"
                                },
                                new Category()
                                {
                                    Name = "Geodezja i kartografia"
                                },
                                new Category()
                                {
                                    Name = "Geologia"
                                },
                                new Category()
                                {
                                    Name = "Murarstwo i tynkarstwo"
                                },
                                new Category()
                                {
                                    Name = "Nadzór budowlany"
                                },
                                new Category()
                                {
                                    Name = "Place zabaw"
                                },
                                new Category()
                                {
                                    Name = "Prace porządkowe"
                                },
                                new Category()
                                {
                                    Name = "Prace wysokościowe"
                                },
                                new Category()
                                {
                                    Name = "Projekty, makiety, modele"
                                },
                                new Category()
                                {
                                    Name = "Studnie i odwierty"
                                },
                                new Category()
                                {
                                    Name = "Tarasy"
                                },
                                new Category()
                                {
                                    Name = "Usługi kompleksowe"
                                },
                                new Category()
                                {
                                    Name = "Wynajem sprzętu budowlanego"
                                },
                                new Category()
                                {
                                    Name = "Inne"
                                },
                            }
                        },
                        new Category()
                        {
                            Name = "Instalacje i naprawy",
                            ChildCategories = new List<Category>()
                            {
                                new Category()
                                {
                                    Name = "Alarmy, zabezpieczenia"
                                },
                                new Category()
                                {
                                    Name = "Anteny"
                                },
                                new Category()
                                {
                                    Name = "Domofony"
                                },
                                new Category()
                                {
                                    Name = "Ekologiczne i fotowoltaiczne"
                                },
                                new Category()
                                {
                                    Name = "Ekspertyzy, kosztorysy"
                                },
                                new Category()
                                {
                                    Name = "Elektryczne i oświetleniowe"
                                },
                                new Category()
                                {
                                    Name = "Grzewcze"
                                },
                                new Category()
                                {
                                    Name = "Hydrauliczne, wodomierze"
                                },
                                new Category()
                                {
                                    Name = "Klimatyzacja, wentylacja"
                                },
                                new Category()
                                {
                                    Name = "Odkurzacze centralne"
                                },
                                new Category()
                                {
                                    Name = "Wodno-kanalizacyjne"
                                },
                                new Category()
                                {
                                    Name = "Inne"
                                }
                            }
                        },
                        new Category()
                        {
                            Name = "Obsługa",
                            ChildCategories = new List<Category>()
                            {
                                new Category()
                                {
                                    Name = "Administracja domów"
                                },
                                new Category()
                                {
                                    Name = "Dezynfekcja, deratyzacja"
                                },
                                new Category()
                                {
                                    Name = "Konserwacja zabytków"
                                },
                                new Category()
                                {
                                    Name = "Ochrona, monitoring"
                                },
                                new Category()
                                {
                                    Name = "Oczyszczanie ścieków"
                                },
                                new Category()
                                {
                                    Name = "Odśnieżanie"
                                },
                                new Category()
                                {
                                    Name = "Osuszanie, odgrzybianie"
                                },
                                new Category()
                                {
                                    Name = "Pośrednictwo w formalnościach"
                                },
                                new Category()
                                {
                                    Name = "Świadectwa energetyczne"
                                },
                                new Category()
                                {
                                    Name = "Udogodnienia dla niepełnosprawnych"
                                },
                                new Category()
                                {
                                    Name = "Usługi kominiarskie"
                                },
                                new Category()
                                {
                                    Name = "Uzdatnianie wody"
                                },
                                new Category()
                                {
                                    Name = "Wyburzenia, rozbiórki"
                                },
                                new Category()
                                {
                                    Name = "Wywóz śmieci, gruzu"
                                },
                                new Category()
                                {
                                    Name = "Inne"
                                },
                            }
                        },
                        new Category()
                        {
                            Name = "Remonty i wykończenia",
                            ChildCategories = new List<Category>()
                            {
                                new Category()
                                {
                                    Name = "Adaptacje"
                                },
                                new Category()
                                {
                                    Name = "Armatura"
                                },
                                new Category()
                                {
                                    Name = "Boazerie"
                                },
                                new Category()
                                {
                                    Name = "Budowa, zabudowa balkonów"
                                },
                                new Category()
                                {
                                    Name = "Cyklinowanie"
                                },
                                new Category()
                                {
                                    Name = "Drzwi wewnętrzne"
                                },
                                new Category()
                                {
                                    Name = "Drzwi zewnętrzne"
                                },
                                new Category()
                                {
                                    Name = "Elewacje, ocieplenie"
                                },
                                new Category()
                                {
                                    Name = "Glazura i terakota"
                                },
                                new Category()
                                {
                                    Name = "Hydroizolacje"
                                },
                                new Category()
                                {
                                    Name = "Izolacje akustyczne"
                                },
                                new Category()
                                {
                                    Name = "Izolacje termiczne"
                                },
                                new Category()
                                {
                                    Name = "Kominki i kominy"
                                },
                                new Category()
                                {
                                    Name = "Kraty okienne, okiennice"
                                },
                                new Category()
                                {
                                    Name = "Malowanie"
                                },
                                new Category()
                                {
                                    Name = "Meble"
                                },
                                new Category()
                                {
                                    Name = "Okna, parapety"
                                },
                                new Category()
                                {
                                    Name = "Osuszanie budynków"
                                },
                                new Category()
                                {
                                    Name = "Parkiety, podłogi, posadzki"
                                },
                                new Category()
                                {
                                    Name = "Piaskowanie, antykorozja"
                                },
                                new Category()
                                {
                                    Name = "Prace wysokościowe"
                                },
                                new Category()
                                {
                                    Name = "Remonty kompleksowe"
                                },
                                new Category()
                                {
                                    Name = "Rolety, żaluzje, moskitiery, markizy"
                                },
                                new Category()
                                {
                                    Name = "Stolarstwo"
                                },
                                new Category()
                                {
                                    Name = "Sufity"
                                },
                                new Category()
                                {
                                    Name = "Szklarstwo"
                                },
                                new Category()
                                {
                                    Name = "Tapetowanie"
                                },
                                new Category()
                                {
                                    Name = "Tapicerstwo"
                                },
                                new Category()
                                {
                                    Name = "Inne"
                                }
                            }
                        },
                        new Category()
                        {
                            Name = "Wyposażenie wnętrz",
                            ChildCategories = new List<Category>()
                            {
                                new Category()
                                {
                                    Name = "Akwaria"
                                },
                                new Category()
                                {
                                    Name = "Architektura wnętrz"
                                },
                                new Category()
                                {
                                    Name = "Dywany i wykładziny"
                                },
                                new Category()
                                {
                                    Name = "Inteligentny dom"
                                },
                                new Category()
                                {
                                    Name = "Lustra"
                                },
                                new Category()
                                {
                                    Name = "Meble - montaż"
                                },
                                new Category()
                                {
                                    Name = "Meble na wymiar"
                                },
                                new Category()
                                {
                                    Name = "Rośliny domowe"
                                },
                                new Category()
                                {
                                    Name = "Sauny i łaźnie"
                                },
                                new Category()
                                {
                                    Name = "Schody i balustrady"
                                },
                                new Category()
                                {
                                    Name = "Sejfy, kasy pancerne"
                                },
                                new Category()
                                {
                                    Name = "Zasłony, firany, karnisze"
                                },
                                new Category()
                                {
                                    Name = "Inne"
                                },
                            }
                        },
                    }
                },
                new Category()
                {
                    Name = "Dzieci, zwierzęta, opieka",
                    ChildCategories = new List<Category>()
                    {
                        new Category()
                        {
                            Name = "Dla dzieci",
                            ChildCategories = new List<Category>()
                            {
                                new Category()
                                {
                                    Name = "Place zabaw"
                                },
                                new Category()
                                {
                                    Name = "Przedszkola"
                                },
                                new Category()
                                {
                                    Name = "Wypożyczanie strojów"
                                },
                                new Category()
                                {
                                    Name = "Występy artystyczne"
                                },
                                new Category()
                                {
                                    Name = "Żłobki"
                                },
                                new Category()
                                {
                                    Name = "Inne"
                                },
                            }
                        },
                        new Category()
                        {
                            Name = "Dla zwierząt",
                            ChildCategories = new List<Category>()
                            {
                                new Category()
                                {
                                    Name = "Budy na zamówienie"
                                },
                                new Category()
                                {
                                    Name = "Hotele dla zwierząt"
                                },
                                new Category()
                                {
                                    Name = "Reprodukcja"
                                },
                                new Category()
                                {
                                    Name = "Salony piękności"
                                },
                                new Category()
                                {
                                    Name = "Tresura, szkolenia"
                                },
                                new Category()
                                {
                                    Name = "Weterynarz"
                                },
                                new Category()
                                {
                                    Name = "Wystawy"
                                },
                            }
                           
                        },
                        new Category()
                        {
                            Name = "Opieka",
                            ChildCategories = new List<Category>()
                            {
                                new Category()
                                {
                                    Name = "Nad chorym/starszym"
                                },
                                new Category()
                                {
                                    Name = "Nad dzieckiem"
                                },
                                new Category()
                                {
                                    Name = "Nad zwierzakiem"
                                },
                            } 
                        }
                    }
                },
                new Category()
                {
                    Name = "Edukacja, szkolenia",
                    ChildCategories = new List<Category>
                    {
                        new Category()
                        {
                            Name = "Biblioteki, czytelnie"
                        },
                        new Category()
                        {
                            Name = "E-learning"
                        },
                        new Category()
                        {
                            Name = "Instruktorzy"
                        },
                        new Category()
                        {
                            Name = "Kursy, szkolenia",
                            ChildCategories = new List<Category>()
                            {
                                new Category()
                                {
                                    Name = "BHP"
                                },
                                new Category()
                                {
                                    Name = "Coaching"
                                },
                                new Category()
                                {
                                    Name = "Doskonalenie zawodowe"
                                },
                                new Category()
                                {
                                    Name = "Ekonomia"
                                },
                                new Category()
                                {
                                    Name = "Finanse"
                                },
                                new Category()
                                {
                                    Name = "Gastronomia"
                                },
                                new Category()
                                {
                                    Name = "Grafika i animacja"
                                },
                                new Category()
                                {
                                    Name = "Informatyka, komputerowe"
                                },
                                new Category()
                                {
                                    Name = "Interpersonalne"
                                },
                                new Category()
                                {
                                    Name = "Kadrowe"
                                },
                                new Category()
                                {
                                    Name = "Kosmetyka"
                                },
                                new Category()
                                {
                                    Name = "Księgowe"
                                },
                                new Category()
                                {
                                    Name = "Marketing, sprzedaż"
                                },
                                new Category()
                                {
                                    Name = "Maturalne"
                                },
                                new Category()
                                {
                                    Name = "Prawo"
                                },
                                new Category()
                                {
                                    Name = "Ratownicze"
                                },
                                new Category()
                                {
                                    Name = "Tańca"
                                },
                                new Category()
                                {
                                    Name = "Techniczne"
                                },
                                new Category()
                                {
                                    Name = "Zarządzanie"
                                },
                                new Category()
                                {
                                    Name = "Inne"
                                },
                            }
                        },
                        new Category()
                        {
                            Name = "Lekcje, korepetycje",
                            ChildCategories = new List<Category>()
                            {
                                new Category()
                                {
                                    Name = "Biologia"
                                },
                                new Category()
                                {
                                    Name = "Chemia"
                                },
                                new Category()
                                {
                                    Name = "Fizyka, astronomia"
                                },
                                new Category()
                                {
                                    Name = "Geografia"
                                },
                                new Category()
                                {
                                    Name = "Historia"
                                },
                                new Category()
                                {
                                    Name = "Informatyka"
                                },
                                new Category()
                                {
                                    Name = "Język angielski"
                                },
                                new Category()
                                {
                                    Name = "Język francuski"
                                },
                                new Category()
                                {
                                    Name = "Język hiszpański"
                                },
                                new Category()
                                {
                                    Name = "Język niemiecki"
                                },
                                new Category()
                                {
                                    Name = "Język polski"
                                },
                                new Category()
                                {
                                    Name = "Język rosyjski"
                                },
                                new Category()
                                {
                                    Name = "Języki obce"
                                },
                                new Category()
                                {
                                    Name = "Matematyka"
                                },
                                new Category()
                                {
                                    Name = "Inne"
                                },
                            }
                        },
                        new Category()
                        {
                            Name = "Nauka gry na instrumentach",
                            ChildCategories = new List<Category>()
                            {
                                new Category()
                                {
                                    Name = "Gitara"
                                },
                                new Category()
                                {
                                    Name = "Instrumenty dęte"
                                },
                                new Category()
                                {
                                    Name = "Instrumenty smyczkowe"
                                },
                                new Category()
                                {
                                    Name = "Perkusja"
                                },
                                new Category()
                                {
                                    Name = "Pianino, keyboard"
                                },
                            }
                        },
                        new Category()
                        {
                            Name = "Nauka jazdy",
                            ChildCategories = new List<Category>()
                            {
                                new Category()
                                {
                                    Name = "Kategoria A"
                                },
                                new Category()
                                {
                                    Name = "Kategoria B"
                                },
                                new Category()
                                {
                                    Name = "Pozostałe kategorie"
                                },
                                new Category()
                                {
                                    Name = "Kursy doszkalające"
                                }
                            }
                        },
                        new Category()
                        {
                            Name = "Nauka języków - szkoły, kursy",
                            ChildCategories = new List<Category>()
                            {
                                new Category()
                                {
                                    Name = "Angielski"
                                },
                                new Category()
                                {
                                    Name = "Francuski"
                                },
                                new Category()
                                {
                                    Name = "Hiszpański"
                                },
                                new Category()
                                {
                                    Name = "Niemiecki"
                                },
                                new Category()
                                {
                                    Name = "Obozy językowe"
                                },
                                new Category()
                                {
                                    Name = "Rosyjski"
                                },
                                new Category()
                                {
                                    Name = "Pozostałe języki"
                                }
                            }
                        },
                        new Category()
                        {
                            Name = "Przedszkola, zajęcia dla dzieci",
                            ChildCategories = new List<Category>()
                            {
                                new Category()
                                {
                                    Name = "Ceramika"
                                },
                                new Category()
                                {
                                    Name = "Gry i zabawy ruchowe"
                                },
                                new Category()
                                {
                                    Name = "Integracja sensoryczna"
                                },
                                new Category()
                                {
                                    Name = "Kluby malucha"
                                },
                                new Category()
                                {
                                    Name = "Plastyka"
                                },
                                new Category()
                                {
                                    Name = "Przedszkole"
                                },
                                new Category()
                                {
                                    Name = "Rytmika"
                                },
                                new Category()
                                {
                                    Name = "Szachy"
                                },
                                new Category()
                                {
                                    Name = "Zajęcia artystyczne"
                                },
                                new Category()
                                {
                                    Name = "Zajęcia językowe"
                                },
                                new Category()
                                {
                                    Name = "Zajęcia kulinarne"
                                },
                                new Category()
                                {
                                    Name = "Zajęcia logopedyczne"
                                },
                                new Category()
                                {
                                    Name = "Zajęcia muzyczne"
                                },
                                new Category()
                                {
                                    Name = "Zajęcia ogólnorozwojowe"
                                },
                                new Category()
                                {
                                    Name = "Zajęcia sportowe"
                                },
                                new Category()
                                {
                                    Name = "Zajęcia teatralne"
                                }
                            }
                        },
                        new Category()
                        {
                            Name = "Szkoły",
                            ChildCategories = new List<Category>()
                            {
                                new Category()
                                {
                                    Name = "Artystyczne"
                                },
                                new Category()
                                {
                                    Name = "Biznesu"
                                },
                                new Category()
                                {
                                    Name = "Gastronomiczne"
                                },
                                new Category()
                                {
                                    Name = "Gimnazja"
                                },
                                new Category()
                                {
                                    Name = "Kosmetyczne"
                                },
                                new Category()
                                {
                                    Name = "Licea"
                                },
                                new Category()
                                {
                                    Name = "Muzyczne"
                                },
                                new Category()
                                {
                                    Name = "Podstawowe"
                                },
                                new Category()
                                {
                                    Name = "Policealne"
                                },
                                new Category()
                                {
                                    Name = "Sportowe"
                                },
                                new Category()
                                {
                                    Name = "Wyższe"
                                },
                                new Category()
                                {
                                    Name = "Zawodowe"
                                },
                            }
                        },
                        new Category()
                        {
                            Name = "Inne"
                        },
                    }
                },
               new Category()
               {
                   Name = "Firma, Biuro"
               },
               new Category()
               {
                   Name = "Fotowoltaika"
               },
               new Category()
               {
                   Name = "Grafika, Multimedia"
               },
                new Category()
               {
                   Name = "Handel"
               },
               new Category()
               {
                   Name = "Kultura, Sztuka"
               },
               new Category()
               {
                   Name = "Marketing, Reklama"
               },
               new Category()
               {
                   Name = "Motoryzacja, Transport"
               },
               new Category()
               {
                   Name = "Naprawa, Serwis"
               },
               new Category()
               {
                   Name = "Prace Domowe, Ogród"
               },
               new Category()
               {
                   Name = "Prawo, Finanse"
               },
               new Category()
               {
                   Name = "Programowanie, IT"
               },
               new Category()
               {
                   Name = "Przemysł"
               },
               new Category()
               {
                   Name = "Rozrywka, Imprezy"
               },
               new Category()
               {
                   Name = "Rzemiosło, Fachowcy"
               },
               new Category()
               {
                   Name = "Sport, Turystyka"
               },
               new Category()
               {
                   Name = "Teksty, Tłumaczenia"
               },
               new Category()
               {
                   Name = "Zdrowie, Uroda"
               },
               new Category()
               {
                   Name = "Pozostałe"
               },
            };

            return categories;
        }
    }
}