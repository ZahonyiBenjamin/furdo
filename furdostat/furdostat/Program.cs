using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace furdostat
{
    internal class Program
    {
        static List<Adatok> adatok = new List<Adatok>();
        static string[] f = File.ReadAllLines("furdoadat.txt");
        static void Main(string[] args)
        {
            feladat1();
            feladat2();
            feladat3();
            feladat4();
            feladat5();
            feladat6();
            feladat7();

            Console.ReadKey();
        }

        private static void feladat7()
        {
            Console.WriteLine("7. feladat");
            Dictionary<string, int> statisztika = new Dictionary<string, int>();
            statisztika["Uszoda"] = 0;
            statisztika["Szaunák"] = 0;
            statisztika["Gyógyvizes medencék"] = 0;
            statisztika["Strand"] = 0;
            int volt_azonosito_uszoda = 0;
            int volt_azonosito_szauna = 0;
            int volt_azonosito_gyogyviz = 0;
            int volt_azonosito_strand = 0;

            foreach (var item in adatok)
            {
                if (item.furdoreszleg == 1 && item.kibelepes == 0 && item.azonosito != volt_azonosito_uszoda)
                {
                    statisztika["Uszoda"] += 1;
                    volt_azonosito_uszoda = item.azonosito;
                }
                else if (item.furdoreszleg == 2 && item.kibelepes == 0 && item.azonosito != volt_azonosito_szauna)
                {
                    statisztika["Szaunák"] += 1;
                    volt_azonosito_szauna = item.azonosito;
                }
                else if (item.furdoreszleg == 3 && item.kibelepes == 0 && item.azonosito != volt_azonosito_gyogyviz)
                {
                    statisztika["Gyógyvizes medencék"] += 1;
                    volt_azonosito_gyogyviz = item.azonosito;
                }
                else if (item.furdoreszleg == 4 && item.kibelepes == 0 && item.azonosito != volt_azonosito_strand)
                {
                    statisztika["Strand"] += 1;
                    volt_azonosito_strand = item.azonosito;
                }
            }

            foreach (var item in statisztika)
            {
                Console.WriteLine($"{item.Key}: {item.Value}");
            }
        }

        private static void feladat6()
        {
            List<string> kimenet = new List<string>();
            DateTime belepes = adatok[0].ido;
            DateTime kilepes = adatok[0].ido;
            TimeSpan kulonbseg = TimeSpan.MinValue;
            DateTime osszes_kulonbseg = DateTime.MinValue;
            string sz = "";

            for (int i = 1; i < adatok.Count; i++)
            {
                if (adatok[i].azonosito != adatok[i - 1].azonosito)//megnézem új vendég következik-e
                {
                    if (osszes_kulonbseg > DateTime.MinValue)//ha igen és volt az előző a szaunában akkor hozzáadom a kimenet listához
                    {
                        sz = $"{adatok[i - 1].azonosito} {osszes_kulonbseg.ToString("HH:mm:ss")}";
                        kimenet.Add(sz);
                    }
                    sz = "";//az adatokat lenullázom
                    osszes_kulonbseg = DateTime.MinValue;
                }
                else
                {
                    if (adatok[i].furdoreszleg == 2 && adatok[i].kibelepes == 0)//megnézem a vendég belépett-e a szaunába
                    {
                        //ha igen, a kilépés és a belépés közti különbséget hozzáadom egy összkülönbséghez, amit csak akkor nullázok ha új vendég jön
                        belepes = adatok[i].ido;
                        kilepes = adatok[i + 1].ido;
                        kulonbseg = kilepes - belepes;
                        osszes_kulonbseg += kulonbseg;
                    }
                }
            }

            File.WriteAllLines("szauna.txt", kimenet);
        }

        private static void feladat5()
        {
            Console.WriteLine("5. feladat");

            Dictionary<string,int> statisztika = new Dictionary<string,int>();
            statisztika["6-9 óra"] = 0;
            statisztika["9-16 óra"] = 0;
            statisztika["16-20 óra"] = 0;

            foreach (var item in adatok)
            {
                if (item.kibelepes == 1 && item.furdoreszleg == 0)
                {
                    if (item.ora >= 6 && item.ora < 9)
                    {
                        statisztika["6-9 óra"] += 1;
                    }
                    else if (item.ora >= 9 && item.ora < 16)
                    {
                        statisztika["9-16 óra"] += 1;
                    }
                    else if (item.ora >= 16 && item.ora < 20)
                    {
                        statisztika["16-20 óra"] += 1;
                    }
                }
            }

            foreach (var item in statisztika)
            {
                Console.WriteLine($"{item.Key} között {item.Value} vendég");
            }

            Console.WriteLine();
        }

        private static void feladat4()
        {
            Console.WriteLine("4. feladat");

            DateTime belepes = adatok[0].ido;//belépés ideje
            DateTime kilepes;//kilépés ideje
            TimeSpan kulonbseg;//egy embernél a be- és kilépés ideje közti különbség
            TimeSpan max_kulonbseg = TimeSpan.MinValue;//a legnagyobb különbség, a MinValue csak kezdőérték, mivel kell valami
            int max_azonosito = 0;//a legtöbb időt eltöltő vendég azonosítója
            for (int i = 1; i < adatok.Count; i++)
            {
                if (adatok[i].azonosito == adatok[i - 1].azonosito && adatok[i - 1].furdoreszleg == 0 && adatok[i - 1].kibelepes == 1)//ellenőrzöm hogy ugyanaz az ember-e, illetve hogy kilépett az öltözőből, azért i-1 mivel azt nézem hogy az előző ugyanaz-e, az első alkalom amikor igen a második adatnál fog előjönni, így egyel viszább kell menni 
                {
                    belepes = adatok[i - 1].ido;
                }
                if (adatok[i].azonosito == adatok[i - 1].azonosito && adatok[i].furdoreszleg == 0 && adatok[i].kibelepes == 0)//ugyanaz mint az előző, csak az öltözőbe va belépést ellenőrzöm, valamint itt már az i kell, nem az i-1, mivel az i lesz az utolsó és jelenlegi adat
                {
                    kilepes = adatok[i].ido;
                    kulonbseg = kilepes - belepes;
                    if (max_kulonbseg < kulonbseg)
                    {
                        max_kulonbseg = kulonbseg;
                        max_azonosito = adatok[i].azonosito;
                    }
                }
            }
            Console.WriteLine($"A legtöbb időt eltöltő vendég:\n{max_azonosito}. vendég {max_kulonbseg}");

            Console.WriteLine();
        }

        private static void feladat3()
        {
            Console.WriteLine("3. feladat");

            int egy_reszleg_db = 0; //hány ember volt csak 1 részlegen
            int szemely = 1; //azt nézi, hogy ugyanaz az ember-e
            for (int i = 1; i < adatok.Count; i++)
            {
                if (adatok[i].azonosito == adatok[i - 1].azonosito)
                {
                    szemely++;//ha a 2 azonosító ugyanaz, növelem 1-el
                }
                else
                {
                    if (szemely == 4)//ha pont 4 mielőtt új ember jön, akkor csak 1 részlegen volt, mivel öltözőből ki, részlegbe be, részlegből ki és öltözőbe be
                    {
                        egy_reszleg_db++;
                    }
                    szemely = 1;//visszaállítom egyre hogy újra számoljak
                }
            }
            Console.WriteLine($"A fürdőben {egy_reszleg_db} vendég járt csak egy részlegen.");

            Console.WriteLine();
        }

        private static void feladat2()
        {
            Console.WriteLine("2. feladat");

            foreach (var item in adatok)
            {
                if (adatok[0].azonosito == item.azonosito && item.kibelepes == 1 && item.furdoreszleg == 0)
                {
                    Console.WriteLine($"Az első vendég {item.ido.ToString("HH:mm:ss")}-kor lépett ki az öltözőből.");
                }
                if (adatok[adatok.Count() - 1].azonosito == item.azonosito && item.kibelepes == 1 && item.furdoreszleg == 0)
                {
                    Console.WriteLine($"Az utolsó vendég {item.ido.ToString("HH:mm:ss")}-kor lépett ki az öltözőből.");
                }
            }

            Console.WriteLine();
        }

        private static void feladat1()
        {
            foreach (var item in f)
            {
                adatok.Add(new Adatok(item));
            }
        }
    }
}
