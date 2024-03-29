using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;
using ConsoleAppSudoku.Class;

namespace ConsoleAppSudoku
{
    public class Program
    {
        #region Messaggi

        const string CHIUSURA = "Premere qualsiasi tasto per chiudere . . .",
                     DIMENSIONI = "Inserire la dimensione del tabellone: ",
                     TITOLO = "*** SUDOKU - SUDOKU - SUDOKU ***",
                     NUMERO = "Inserire il numero: ",
                     COLONNA = "Inserire la colonna: ",
                     RIGA = "Inserire la riga: ",
                     ERRORE_NUMERO = "Numero non idoneo, riprova!!!",
                     ERRORE_COLONNA = "Colonna non idonea, riprova!!!",
                     ERRORE_RIGA = "Riga non idonea, riprova!!!",
                     ERRORE_DIMENSIONI = "Dimensioni non idone, riprova!!!";

        #endregion
        private static int Conversione(string mess, string err, int d = 9)
        {
            int n;
            while (true)
            {
                Write(mess);
                if (int.TryParse(ReadLine(), out n) && n - 1 >= 0 && n - 1 < d)
                    return n;
                else 
                    WriteLine(mess);
            }
        }

        static void Main(string[] args)
        {
            Title = TITOLO;

            Sudoku sudoku = new Sudoku();

            int n = 0;
            bool ok = true;
            do
            {
                Write("Numero celle note: ");
                if (int.TryParse(ReadLine(), out n) && n > 0)
                    ok = false;
                else
                    WriteLine("Numero non idone, riprova!!!");
            } while (ok);

            // valori noti
            for (int i = 0; i < n; i++)
            {
                int num = Conversione(NUMERO, ERRORE_NUMERO);
                int r = Conversione(RIGA, ERRORE_RIGA);
                int c = Conversione(COLONNA, ERRORE_COLONNA);
                
                sudoku.AggiungiNumero(r, c, num);
            }

            DateTime inizio = DateTime.Now;
            sudoku.Risolvi();
            DateTime fine = DateTime.Now;

            WriteLine("Tempo esecuzione: " + (fine - inizio).ToString());
            Write(sudoku.ToString());

            // chiusura
            Write(CHIUSURA);
            ReadKey(true);
        }
    }
}
