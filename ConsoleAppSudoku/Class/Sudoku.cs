using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppSudoku.Class
{
    public class Sudoku : Dimensioni
    {
        private SuperCell[,] _matrix;

        #region proprietà
        private int NumeroSuperCelle { get; set; }

        public SuperCell this[int r, int c]
        {
            get
            {
                if(_matrix.ControlloDimensioni(r, c))
                    return _matrix[r, c];
                return null;
            }
            set
            {
                if (_matrix.ControlloDimensioni(r, c))
                    _matrix[r, c] = value;
            }
        }
        #endregion

        #region costruttori
        public Sudoku() : this (3) { }

        public Sudoku(int d) : base(d * d)
        {
            NumeroSuperCelle = d;

            _matrix = new SuperCell[d, d];

            for (int i = 0; i < d; i++)
                for (int j = 0; j < d; j++)
                    _matrix[i, j] = new SuperCell(d);

            int riga, colonna;
            for (int R = 0; R < d; R++)
                for (int C = 0; C < d; C++)
                    for (int r = 0; r < d; r++)
                        for (int c = 0; c < d; c++)
                        {
                            riga = R * d + r;
                            colonna = C * d + c;
                            _matrix[R, C][r, c] = new Cell(riga, colonna);
                        }
        }
        #endregion

        #region metodi
        public void AggiungiNumero(int r, int c, int val)
        {
            r--;
            c--;

            this[r / NumeroSuperCelle, c / NumeroSuperCelle][r % NumeroSuperCelle, c % NumeroSuperCelle].Valore = val;
        }

        #region risoluzione
        public void Risolvi()
        {
            Riempi();
            Semplifica();
        }

        private void Riempi()
        {
            for (int i = 0; i < NumeroCelle; i++)
                for (int j = 0; j < NumeroCelle; j++)
                {
                    Cell c = _matrix.OttieniCellaNonProtetta(i, j);
                    if (c.Valore == null)
                        for (int k = 1; k <= NumeroCelle; k++)
                            _matrix.AssegnaNumeri(c, k);
                }
        }

        private void Semplifica()
        {
            int continua;

            DateTime inizio = DateTime.Now, inizio2; // todo: cancellare
            do
            {
                continua = 0;
                for (int i = 0; i < NumeroCelle; i++)
                    for (int j = 0; j < NumeroCelle; j++)
                    {
                        Cell c = _matrix.OttieniCellaNonProtetta(i, j);
                        if (c.NumeriPossibili == 1 && c.Valore ==  null)
                        {
                            _matrix.UnicizzaCella(c);
                            continua++;
                        }
                        else if (c.NumeriPossibili > 1)
                        {
                            _matrix.ProvaUnicizzaCella(c);
                            continua++;
                        }
                    }
                inizio2 = DateTime.Now; //todo: cancellare

            } while (continua > 0 && (inizio2 - inizio).TotalMilliseconds < 1000); // todo: cancellare seconda condizione, quella del tempo
        }
        #endregion

        private string Format(string w, char f, string r, string e, string q)
        {
            string s = w;

            for (int i = 0; i < NumeroCelle; i++)
            {
                s += new string(f, NumeroCelle);

                if (i < NumeroCelle - 1)
                    if ((i + 1) % NumeroSuperCelle == 0)
                        s += q;
                    else
                        s += r;
            }

            s += e;
            s += "\n";

            return s;
        }

        public override string ToString()
        {
            string s = "";

            s += Format("╔", '═', "╦", "╗", "╦");

            for (int r = 0; r < NumeroCelle; r++) // righe esterne
            {
                s += "║";
                for (int c = 0; c < NumeroCelle; c++)
                {
                    s += _matrix.OttieniCellaNonProtetta(r, c).FormattaInStringa(NumeroCelle);
                    
                    if ((c + 1) % NumeroSuperCelle == 0)
                        s += "║";
                    else
                        s += "│";
                }
                s += "\n";

                if (r < NumeroCelle - 1)
                    if ((r + 1) % NumeroSuperCelle == 0)
                        s += Format("╟", '═', "╬", "╢", "╬");
                    else
                        s += Format("╟", '─', "┼", "╢", "╬");
            }
            s += Format("╚", '═', "╩", "╝", "╩");

            return s;
        }

        #endregion
    }
}
