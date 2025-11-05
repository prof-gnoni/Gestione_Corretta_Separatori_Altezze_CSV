public class Persona
{
    // Proprietà
    public string Nome { get; set; }
    public string Cognome { get; set; }
    public float Altezza { get; set; }

    // Costruttore per creare un nuovo oggetto Persona
    public Persona(string nome, string cognome, float altezza)
    {
        Nome = nome;
        Cognome = cognome;
        Altezza = altezza;
    }

    // Aggiungiamo un override di ToString() per vederlo bene
    // (Il 'record' lo faceva in automatico, qui lo facciamo noi)
    public override string ToString()
    {
        return $"{Nome} {Cognome} -> Altezza: {Altezza}m";
    }
}