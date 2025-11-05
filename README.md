(Inizia a copiare da qui)

Demo: Gestione Separatori Culturali in C# (.NET)
Questo progetto è un'applicazione console C# che dimostra i problemi comuni (e le loro soluzioni) legati alla localizzazione e internazionalizzazione (i18n) durante la lettura e scrittura di file CSV.

In particolare, si concentra su come la diversa rappresentazione dei numeri decimali (es. 1.80 vs 1,80) a seconda delle impostazioni regionali del sistema operativo possa portare a crash (FormatException) o, peggio, a una corruzione silenziosa dei dati.

🎯 Il Problema: Il "Dilemma della Virgola"
Il nucleo della demo simula uno scenario molto comune:

Impostazione della Cultura: Il programma viene forzato a girare in un contesto culturale "italiano" (it-IT), dove il separatore decimale è la virgola (,) e il separatore delle migliaia è il punto (.).

Generazione di un File "Estero": Viene generato un file CSV (anagrafica.csv) come farebbe un computer con cultura en-US (americana), usando il punto (.) per i decimali (es. Mario;Rossi;1.80).

Tentativo di Lettura (Il Fallimento): L'applicazione (che gira come it-IT) tenta di rileggere il file:

Il metodo float.Parse("1.80") viene eseguito.

Il parser it-IT non va in crash (come ci si potrebbe aspettare).

Interpreta il . come separatore delle migliaia e legge il valore 1.80 come 180.

Questo crea una corruzione silenziosa dei dati: il programma crede di aver letto correttamente un'altezza di 180 metri.

💡 La Soluzione: CultureInfo
La demo mostra come prevenire sia i crash (FormatException) sia la corruzione dei dati specificando esplicitamente quale cultura utilizzare durante la conversione di numeri.

La soluzione consiste nell'utilizzare gli overload dei metodi Parse e ToString che accettano un IFormatProvider (come un oggetto CultureInfo).

Ci sono due approcci principali dimostrati:

CultureInfo.CurrentCulture (Accesso al Sistema Operativo) Si accede alle impostazioni del sistema operativo (Thread.CurrentThread.CurrentCulture) e si usano quelle, sia in scrittura (float.ToString(cultura)) che in lettura (float.Parse(stringa, cultura)). Questo garantisce che il programma sia coerente con le aspettative dell'utente su quel PC.

CultureInfo.InvariantCulture (Soluzione Raccomandata) Si utilizza una cultura "invariante" (che è uno standard tecnico basato su en-US, usando sempre il punto). Questo è il metodo più robusto per lo scambio di file, poiché garantisce che il file sia leggibile e scrivibile correttamente su qualsiasi computer, indipendentemente dalle sue impostazioni regionali.

🚀 Come Eseguire la Demo
Clona il repository:

Bash

git clone [URL_DEL_TUO_REPO]
Apri la soluzione (.sln) in Visual Studio.

Avvia il progetto (premendo F5 o il pulsante "Play").

Osserva l'output della console.

L'output è diviso in due fasi:

FASE 1 (ROSSO): Mostra la generazione del file "sbagliato" e la successiva corruzione dei dati durante la rilettura.

FASE 2 (VERDE): Mostra la generazione e la lettura corrette utilizzando la CultureInfo appropriata.

Nota: Se il programma si blocca immediatamente con una System.IO.IOException (file in uso), assicurati di aver chiuso il file anagrafica.csv in Excel o altri editor di testo.

🔑 Concetti Chiave Dimostrati
Gestione della classe CultureInfo.

Lettura delle impostazioni regionali del sistema (Thread.CurrentThread.CurrentCulture).

Differenza tra separatori decimali (, vs .) e separatori di migliaia.

Pericolo della corruzione silenziosa dei dati.

Uso di float.Parse(stringa, cultura).

Uso di float.ToString(cultura).

Importanza di CultureInfo.InvariantCulture per l'interscambio di dati.

(Fine)
