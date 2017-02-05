using System;
using System.Drawing;
using System.Windows.Forms;

namespace aknakereso_osztalyok
{
    internal class Mezo : Button
    {
        public bool Akna;
        public bool Felfedve;
        public int Sor { get; set; }
        public int Oszlop { get; set; }
        public int SzomszedSzam { get; set; }

        public Mezo(int sor, int oszlop)
        {
            Sor = sor;
            Oszlop = oszlop;
            Width = 25;
            Height = 25;
            Left = oszlop * 25;
            Top = sor * 25;
        }

        public int Felfed()
        {
            if (Akna)
            {
                BackColor = Color.Black;
                MessageBox.Show(@"Bumm!");
                return -1;
            }
            FlatStyle = FlatStyle.Flat;
            Felfedve = true;
            if (SzomszedSzam <= 0) return 0;
            Text = SzomszedSzam.ToString();
            return 1;
        }
    }

    internal class Jatek
    {
        private bool _vege;
        private readonly Mezo[,] _palya;
        private readonly int _szelesseg;
        private readonly int _magassag;
        private readonly int _aknaszam;
        private int _x;

        public Jatek(int szelesseg, int magassag, int aknaszam)
        {
            _szelesseg = szelesseg;
            _magassag = magassag;
            _aknaszam = aknaszam;
            
            _palya = new Mezo[magassag, szelesseg];
            for (var i = 0; i < magassag; i++)
            {
                for (var j = 0; j < szelesseg; j++)
                {
                    var m = new Mezo(i, j);
                    _palya[i, j] = m;
                    m.MouseDown += mezo_MouseDown;
                }
            }
            Aknasit(aknaszam);
        }

        private void Aknasit(int aknaszam)
        {
            var r = new Random();
            for (var i = 0; i < aknaszam; i++)
            {
                int sor;
                int oszlop;
                do
                {
                    sor = r.Next(_magassag);
                    oszlop = r.Next(_szelesseg);
                } while (_palya[sor, oszlop].Akna);
                _palya[sor, oszlop].Akna = true;
                SzomszedSzamoz(sor, oszlop);
            }

        }

        private void SzomszedSzamoz(int sor, int oszlop)
        {
            for (var i = sor-1; i <= sor+1; i++)
            {
                for (var j = oszlop-1; j <= oszlop+1; j++)
                {
                    if (i >= 0 && j >= 0 && i < _magassag && j < _szelesseg && !(i == sor && j == oszlop))
                    {
                        _palya[i, j].SzomszedSzam++;
                    }
                }
            }
        }

        private void mezo_MouseDown(object sender, MouseEventArgs e)
        {
            if (_vege) return;
            var m = (Mezo) sender;
            switch (e.Button)
            {
                case MouseButtons.Right:
                    if (m.Text == "")
                    {
                        if (_x >= _aknaszam) return;
                        m.Text = @"X";
                        _x++;
                        if (!Nyert()) return;
                        _vege = true;
                        MindentMutat();
                        MessageBox.Show(@"Grat, Staff Sergeant William James!");
                    } else
                    {
                        m.Text = "";
                        _x--;
                    }
                    break;
                case MouseButtons.Left:
                    Felfedes(m);
                    if (_vege)
                    {
                        MindentMutat();
                    }
                    break;
            }
        }

        private void Felfedes(Mezo m)
        {
            if (m.Felfedve) return;
            switch (m.Felfed())
            {
                case 0:
                    for (var i = m.Sor - 1; i <= m.Sor + 1; i++)
                    {
                        for (var j = m.Oszlop - 1; j <= m.Oszlop + 1; j++)
                        {
                            if (i >= 0 && j >= 0 && i < _szelesseg && j < _magassag && !(i == m.Sor && j == m.Oszlop))
                            {
                                Felfedes(_palya[i, j]);
                            }
                        }
                    }
                    break;
                case -1:
                    _vege = true;
                    break;
            }
        }

        public void Lerajzol(Panel p)
        {
            p.Width = _szelesseg*25;
            p.Height = _magassag*25;

            for (var i = 0; i < _magassag; i++)
            {
                for (var j = 0; j < _szelesseg; j++)
                {
                    p.Controls.Add(_palya[i, j]);
                }
            }
        }

        private bool Nyert()
        {
            for (var i = 0; i < _magassag; i++)
            {
                for (var j = 0; j < _szelesseg; j++)
                {
                    if (_palya[i, j].Akna && _palya[i, j].Text != @"X")
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private void MindentMutat()
        {
            for (var i = 0; i < _magassag; i++)
            {
                for (var j = 0; j < _szelesseg; j++)
                {
                    if (_palya[i, j].Akna)
                    {
                        _palya[i,j].BackColor = Color.Black;
                    }
                    else if(_palya[i, j].SzomszedSzam == 0)
                    {
                        _palya[i, j].FlatStyle = FlatStyle.Flat;
                    }
                    else
                    {
                        _palya[i, j].FlatStyle = FlatStyle.Flat;
                        _palya[i, j].Text = _palya[i, j].SzomszedSzam.ToString();
                    }
                }
            }
        }
    }
}