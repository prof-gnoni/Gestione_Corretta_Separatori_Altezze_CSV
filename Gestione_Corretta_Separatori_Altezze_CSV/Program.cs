using System;
using System.Collections.Generic;
using System.Globalization; // FONDAMENTALE per la soluzione!
using System.IO;
using System.Text;
using System.Threading; // Per simulare la cultura italiana

public class Program
{
    // === IMPOSTAZIONI ===
    private static string percorsoFile = "anagrafica.csv";

    private static List<Persona> persone = new List<Persona>
    {
        new Persona("Mario", "Rossi", 1.80f),
        new Persona("Anna", "Verdi", 1.65f)
    };

    // === METODO PRINCIPALE ===
    public static void Main(string[] args)
    {
        Console.WriteLine("--- Demo Problema Separatore CSV (Virgola vs Punto) ---");

        // Forziamo la cultura "it-IT"
        Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("it-IT");
        Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("it-IT");

        Console.WriteLine($"\nSimulazione in corso con la cultura: {CultureInfo.CurrentCulture.Name}");
        Console.WriteLine($"Il separatore decimale di questa cultura è: '{CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator}'");
        Console.WriteLine(new string('-', 20));

        // --- 1. IL PROBLEMA ---
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("\nFASE 1: Genero il CSV in modo 'ingenuo' (sbagliato)...");
        Console.ResetColor();

        SalvaCsv_PROBLEMA();
        Console.WriteLine($"File '{percorsoFile}' creato.");
        Console.WriteLine("Contenuto del file:");
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine(File.ReadAllText(percorsoFile));
        Console.ResetColor();

        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("\nOra provo a ricaricare il file che ho appena scritto...");
        Console.ResetColor();

        CaricaCsv_PROBLEMA();

        Console.WriteLine(new string('-', 20));

        // --- 2. LA SOLUZIONE ---
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("\nFASE 2: Genero il CSV con la SOLUZIONE (CultureInfo)...");
        Console.ResetColor();

        SalvaCsv_SOLUZIONE_1(CultureInfo.CurrentCulture);

        Console.WriteLine($"File '{percorsoFile}' sovrascritto correttamente.");
        Console.WriteLine("Contenuto del file:");
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine(File.ReadAllText(percorsoFile));
        Console.ResetColor();

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("\nOra ricarico il file scritto correttamente...");
        Console.ResetColor();

        CaricaCsv_SOLUZIONE_1(CultureInfo.CurrentCulture);

        Console.WriteLine("\nDemo completata.");
        Console.ReadLine();
    }

    // --- METODI DEL PROBLEMA ---

    public static void SalvaCsv_PROBLEMA()
    {
        // Simula un file scritto con il PUNTO (es. da un PC americano)
        var culturaUSA = CultureInfo.GetCultureInfo("en-US");

        StringBuilder sb = new StringBuilder();
        sb.AppendLine("Nome;Cognome;Altezza");
        foreach (var p in persone)
        {
            // Forziamo la scrittura con il PUNTO
            string altezzaString = p.Altezza.ToString(culturaUSA);
            sb.AppendLine($"{p.Nome};{p.Cognome};{altezzaString}");
        }
        File.WriteAllText(percorsoFile, sb.ToString());
    }

    public static void CaricaCsv_PROBLEMA()
    {
        var righe = File.ReadAllLines(percorsoFile);

        for (int i = 1; i < righe.Length; i++)
        {
            try
            {
                string[] campi = righe[i].Split(';');
                string nome = campi[0];
                string cognome = campi[1];
                string altezzaString = campi[2]; // es. "1.80"

                float altezza = float.Parse(altezzaString); // <-- Non va in crash

                // --- AGGIUNGI QUESTA RIGA ---
                // Stampa l'altezza che CREDE di aver caricato
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine($"--> CORRUZIONE DATI: '{altezzaString}' è stato caricato come: {altezza}m");
                Console.ResetColor();
                // -----------------------------
            }
            catch (FormatException ex)
            {
                // QUESTA PARTE NON VIENE MAI ESEGUITA
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"--> CRASH durante il caricamento!");
                Console.ResetColor();
            }
        }
    }

    // --- METODI DELLA SOLUZIONE 1 (CurrentCulture) ---

    public static void SalvaCsv_SOLUZIONE_1(CultureInfo cultura)
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("Nome;Cognome;Altezza");
        foreach (var p in persone)
        {
            // Usiamo la cultura fornita (es. "it-IT") per scrivere "1,80"
            string altezzaString = p.Altezza.ToString(cultura);
            sb.AppendLine($"{p.Nome};{p.Cognome};{altezzaString}");
        }
        File.WriteAllText(percorsoFile, sb.ToString());
    }

    public static void CaricaCsv_SOLUZIONE_1(CultureInfo cultura)
    {
        var righe = File.ReadAllLines(percorsoFile);
        for (int i = 1; i < righe.Length; i++)
        {
            try
            {
                string[] campi = righe[i].Split(';');
                string nome = campi[0];
                string cognome = campi[1];
                string altezzaString = campi[2]; // es. "1,80"

                // SOLUZIONE: Usiamo la stessa cultura ("it-IT") per leggere.
                float altezza = float.Parse(altezzaString, cultura);

                // Ora creiamo l'oggetto Persona (non serve in questa demo,
                // ma potremmo aggiungerlo a una nuova lista)
                Persona p = new Persona(nome, cognome, altezza);

                Console.WriteLine($"CARICATO: {p.ToString()}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Errore inaspettato: {ex.Message}");
            }
        }
    }
}