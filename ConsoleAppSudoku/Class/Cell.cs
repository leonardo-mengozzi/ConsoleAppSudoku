using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppSudoku.Class
{
    public class Cell : IEnumerable<int>
    {
        private List<int?> _valoriPossibili = new List<int?>();
        public int? Valore { get; set; }
        public int Colonna { get; set; }
        public int Riga { get; set; }

        #region metodi
        public bool Contains(int? val)
        {
            return _valoriPossibili.Contains(val) ? true : false;
        }

        public void Add(int val)
        {
            _valoriPossibili.Add(val);
        }

        public void Remove()
        {
            Valore = _valoriPossibili[0];
            _valoriPossibili.Clear();
        }

        public void Remove(int? val)
        {
            _valoriPossibili.Remove(val);
        }

        public void ValoreTrovato(int? i)
        {
            Valore = i;
            _valoriPossibili.Clear();
        }

        public string FormattaInStringa(int l)
        {
            string s;
            if (Valore != null)
                s = Valore.ToString();
            else
                s = string.Join("", _valoriPossibili.ToArray());

            for (int i = s.Length; i < l; i++)
                s += " ";

            return s;
        }

        public IEnumerator<int> GetEnumerator()
        {
            foreach(int i in _valoriPossibili)
                yield return i;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion

        public int NumeriPossibili
        {
            get
            {
                return _valoriPossibili.Count;
            }
        }

        #region costruttori
        public Cell(int r, int c, List<int?> val) : this(r, c)
        {
            _valoriPossibili = val;
        }
        public Cell(int r, int c, int n) : this(r, c)
        {
            Valore = n;
        }

        public Cell(int r, int c)
        {
            Riga = r;
            Colonna = c;
        }

        public Cell()
        {
            
        }
        #endregion
    }
}
