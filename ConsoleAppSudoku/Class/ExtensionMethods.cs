using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppSudoku.Class
{
    public static class ExtensionMethods
    {
        public static void UnicizzaCella(this SuperCell[,] mat, Cell cell)
        {
            cell.Remove();

            mat.LiberaRigaColonna(cell);

            mat.OttieniSuperCellaNonProtetta(cell.Riga, cell.Colonna).Clear(cell.Valore);
        }

        private static void LiberaRigaColonna(this SuperCell[,] mat, Cell cell)
        {
            for (int i = 0; i < Math.Pow(mat.GetLength(0), 2); i++)
            {
                if (i != cell.Colonna)
                    mat.OttieniCellaNonProtetta(cell.Riga, i).Remove(cell.Valore);

                if (i != cell.Riga)
                    mat.OttieniCellaNonProtetta(i, cell.Colonna).Remove(cell.Valore);
            }
        }

        public static void ProvaUnicizzaCella(this SuperCell[,] mat, Cell cell)
        {
            int r = cell.Riga, c = cell.Colonna;
            SuperCell superCell = mat.OttieniSuperCellaNonProtetta(r, c);

            foreach (int i in cell)
                if (superCell.ContainsTuttiEccetto(i, r, c))
                {
                    cell.ValoreTrovato(i);

                    mat.LiberaRigaColonna(cell);

                    return;
                }
        }

        public static void AssegnaNumeri(this SuperCell[,] mat, Cell cell, int n)
        {
            if (ControlloAntiOrario(mat, cell, n) 
                && 
                mat.OttieniSuperCellaNonProtetta(cell.Riga, cell.Colonna)
                   .ContainsNoti(n))
                cell.Add(n);
        }

        private static bool ControlloAntiOrario(SuperCell[,] mat, Cell cell, int n)
        {
            int i, lunghezza = (int)Math.Pow(mat.GetLength(0), 2);

            // orizzontale
            for (i = 0; i < cell.Colonna; i++)
                if (mat.OttieniCellaNonProtetta(cell.Riga, i).Valore == n)
                    return false;

            for (i = cell.Colonna + 1; i < lunghezza; i++)
                if (mat.OttieniCellaNonProtetta(cell.Riga, i).Valore == n)
                    return false;

            // verticale
            for (i = 0; i < cell.Riga; i++)
                if (mat.OttieniCellaNonProtetta(i, cell.Colonna).Valore == n)
                    return false;

            for (i = cell.Riga + 1; i < lunghezza; i++)
                if (mat.OttieniCellaNonProtetta(i, cell.Colonna).Valore == n)
                    return false;

            return true;
        }

        #region piccoli
        public static bool ContainsNoti(this SuperCell mat, int v)
        {
            foreach(Cell c in mat)
                if (c.Valore == v) return false;
            return true;
        }

        public static bool ContainsTuttiEccetto(this SuperCell mat, int? v, int r, int c)
        {
            foreach (Cell cell in mat)
                if (cell.Contains(v) && !(cell.Riga == r && cell.Colonna == c))
                    return false;
            return true;
        }

        public static void Clear(this SuperCell mat, int? v)
        {
            for (int r = 0; r < mat.NumeroCelle; r++)
                for (int c = 0; c < mat.NumeroCelle; c++)
                {
                    Cell cell = mat[r, c];
                    if (cell.Contains(v))
                        cell.Remove(v);
                }
        }

        public static bool ControlloDimensioni(this Cell[,] mat, int r, int c)
        {
            return Controllo(r, c, mat.GetLength(0), mat.GetLength(1));
        }

        public static bool ControlloDimensioni(this SuperCell[,] mat, int r, int c)
        {
            return Controllo(r, c, mat.GetLength(0), mat.GetLength(1));
        }

        private static bool Controllo(int riga, int colonna, int righe, int colonne)
        {
            if (riga < 0
                    ||
                    riga >= righe
                    ||
                    colonna < 0
                    ||
                    colonna >= colonne)
                return false;
            return true;
        }

        public static Cell OttieniCellaNonProtetta(this SuperCell[,] mat, int r, int c)
        {
            int n = mat.GetLength(0);
            return mat[r / n, c / n][r % n, c % n];
        }
        public static SuperCell OttieniSuperCellaNonProtetta(this SuperCell[,] mat, int r, int c)
        {
            int n = mat.GetLength(0);
            return mat[r / n, c / n];
        }
        #endregion
    }
}
