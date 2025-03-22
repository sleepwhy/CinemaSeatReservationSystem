using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsAppH6
{
    public partial class FormMain : Form
    {

        public FormMain()
        {
            InitializeComponent();
            label1.Visible = false;
            label2.Visible = false;

            this.FormBorderStyle = FormBorderStyle.FixedDialog; 
            this.MaximizeBox = false;  
            this.MinimizeBox = false;  
        }

        public void ChangeBackgroundColor(Color newColor)
        {
            this.BackColor = newColor;
            this.Invalidate();  
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            using (LinearGradientBrush brush = new LinearGradientBrush(this.ClientRectangle, Color.Blue, Color.LightSkyBlue, 45F))
            {
                e.Graphics.FillRectangle(brush, this.ClientRectangle);  
            }
        }


        List<string> secilenKoltuklar = new List<string>();





        private void SinemaKoltuklariOlustur(int sutunSayisi, int satirSayisi, string bosKonumlar,
                            string satilmisKoltuklar)
        {
            var satilmisKoltuks = satilmisKoltuklar.Split(',');
            var bosKoltuks = bosKonumlar.Split(',');

            List<string> bosKoltuklar = bosKoltuks.ToList();


            for (int i = 0; i < bosKoltuks.Length; i++)
            {
                if (bosKoltuks[i].Contains("-"))
                {
                    var alan = bosKoltuks[i].Split('-');

                    char baslangicHarf = alan[0][0];
                    char bitisHarf = alan[1][0];
                    int harfFarki = Math.Abs(baslangicHarf - bitisHarf) + 1;

                    int baslangicSayi = Convert.ToInt32(alan[0].Substring(1));
                    int bitisSayi = Convert.ToInt32(alan[1].Substring(1));

                    for (int j = 0; j < harfFarki; j++)
                    {
                        for (int k = baslangicSayi; k <= bitisSayi; k++)
                        {
                            string yeniKoltuk = ((char)(baslangicHarf + j)).ToString() + k.ToString();
                            bosKoltuklar.Add(yeniKoltuk);
                        }
                    }
                }
            }


            for (int i = 0; i < satirSayisi; i++)
            {
                for (int j = 0; j < sutunSayisi; j++)
                {
                    string koltukAdi = (char)(65 + i) + (j+1).ToString();

                    if (!bosKoltuklar.Contains(koltukAdi))
                    {
                        Button btn = new Button();
                        btn.Size = new Size(50, 50);
                        btn.Text = koltukAdi;
                        btn.Location = new Point(55 * j, 55 * i);
                        if (satilmisKoltuks.Contains(koltukAdi))
                        {
                            btn.Enabled = false;

                        }
                        else
                        {
                            btn.BackColor = Color.LightGreen;
                            btn.MouseEnter += new EventHandler(buttonEnter);
                            btn.MouseLeave += new EventHandler(buttonLeave);
                            btn.MouseClick += new MouseEventHandler(buttonSecildi);
                            btn.KeyDown += new KeyEventHandler(buttonKey);
                        }
                        btn.Image = Properties.Resources.seat;
                        btn.TextAlign = ContentAlignment.BottomCenter;
                        btn.ImageAlign = ContentAlignment.TopCenter;


                        this.Controls.Add(btn);


                    }
                    ChangeBackgroundColor(Color.LightYellow);
                }
            }

            label1.Location = new Point(55 * sutunSayisi, 55);
            label2.Location = new Point(55 * sutunSayisi, 85);

            ogeleriGorunmezYap();
        }
        
        private void ogeleriGorunmezYap()
        {
            label1.Visible = true;
            button2.Visible = false;
            label5.Visible = false;
            radioButton1.Visible = false;
            radioButton2.Visible = false;

            if(txtSilinecekKoltuklar != null)
            {
                Controls.Remove(txtSilinecekKoltuklar);
                txtSilinecekKoltuklar.Dispose();
            }
            if(dataGridView != null)
            {
                Controls.Remove(dataGridView);
                dataGridView.Dispose();
            }
            if(btnEkle != null)
            {
                Controls.Remove(btnEkle);
                btnEkle.Dispose();
            }

        }

        private void buttonEnter(object sender, EventArgs e)
        {
            Button tiklananButton = (Button)sender;


            if (!secilenKoltuklar.Contains(tiklananButton.Text))
            {
                tiklananButton.BackColor = Color.Blue;
            }
            
        }

        private void buttonLeave(object sender , EventArgs e)
        {
            Button tiklananButton = (Button)sender;
            if (!secilenKoltuklar.Contains(tiklananButton.Text))
            {
                tiklananButton.BackColor = Color.LightGreen;
            }
            
        }

        private void buttonSecildi(object sender, MouseEventArgs e)
        {
            Button tiklananButton = (Button)sender;

            if(tiklananButton.BackColor == Color.LightGreen || tiklananButton.BackColor == Color.Blue)
            {
                tiklananButton.BackColor = Color.Red;
                secilenKoltuklar.Add(tiklananButton.Text);
            }
            else
            {
                tiklananButton.BackColor = Color.LightGreen;
                secilenKoltuklar.Remove(tiklananButton.Text);
            }
            secilenKoltuklariGuncelle();
        }

        private void buttonKey(object sender, KeyEventArgs e)
        {
            Button tiklananButton = (Button)sender;

            if(e.KeyCode == Keys.Space && !secilenKoltuklar.Contains(tiklananButton.Text))
            {
                secilenKoltuklar.Add(tiklananButton.Text);
                tiklananButton.BackColor = Color.Red;
            }
            else if(e.KeyCode == Keys.Space && secilenKoltuklar.Contains(tiklananButton.Text))
            {
                secilenKoltuklar.Remove(tiklananButton.Text);
                tiklananButton.BackColor = Color.LightGreen;
            }
            secilenKoltuklariGuncelle();
        }

        private void secilenKoltuklariGuncelle()
        {
            label1.Visible = true;
            label2.Visible = true;

            label2.Text = "";
            for(int i = 0;i < secilenKoltuklar.Count; i++)
            {
                label2.Text += secilenKoltuklar[i] + ", ";
                if((i+1) % 4 == 0) { label2.Text += "\n"; }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                int satirSayi = Convert.ToInt32(textSatirSayi.Text);
                int sutunSayi = Convert.ToInt32(textSutunSayi.Text);


                if(satirSayi == 0 || sutunSayi == 0)
                {
                    MessageBox.Show("Lütfen Geçerli Sayılar giriniz!");
                }
                else if(satirSayi > 18 || sutunSayi > 31)
                {
                    MessageBox.Show("Lütfen Daha Düşük Rakamlar Giriniz!\n" +
                        "Satır Sayısı(MAX) = 18\n" +
                        "Sütun Sayısı(MAX) = 31");
                }
                else if (satirSayi < 3 || sutunSayi < 3)
                {
                    MessageBox.Show("Lütfen Daha Büyük Rakamlar Giriniz\n" +
                        "Satır Sayısı(MAX) = 18\n" +
                        "Sütun Sayısı(MAX) = 31");
                }
                else
                {

                    label3.Visible = false;
                    label4.Visible = false;
                    textSatirSayi.Visible = false;
                    textSutunSayi.Visible = false;
                    List<string> silinecekKoltuklar = new List<string>();
                    if (dataGridView != null)
                    {
                        foreach (DataGridViewRow row in dataGridView.Rows)
                        {
                            if (!row.IsNewRow)
                            {
                                silinecekKoltuklar.Add(row.Cells["Silinecekler"].Value?.ToString());
                            }
                        }
                    }
                    else
                    {
                        silinecekKoltuklar.Add(" ");
                    }



                    string silinecekler = string.Join(",", silinecekKoltuklar);

                    SinemaKoltuklariOlustur(sutunSayi, satirSayi, silinecekler, "A1,A2,F5,F6,G9,G10,G11,H1,H12,A11,A12");
                    
                    if(sutunSayi > 20  || satirSayi > 15)
                    {
                        WindowState = FormWindowState.Maximized;
                    }
                    else
                    {
                        Height = satirSayi * 55 + 55;
                        Width = sutunSayi * 55 + 250;
                    }
                }
                    
            }
            catch(FormatException)
            {
                MessageBox.Show("Lütfen Sadece Rakam Giriniz!");
            }
            catch(Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }



            
        }

        DataGridView dataGridView;
        Button btnEkle;
        private void dataGridViewiOlustur()
        {
            dataGridView = new DataGridView();
            dataGridView.Size = new Size(140, 250);
            dataGridView.Location = new Point(320, 20);
            dataGridView.Columns.Add("Silinecekler", "Silinecek Koltuklar");
            dataGridView.ScrollBars = ScrollBars.Vertical;
            dataGridView.RowHeadersVisible = false;
            dataGridView.ColumnHeadersDefaultCellStyle.Font = new Font("Arial", 14, FontStyle.Bold);
            dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            Controls.Add(dataGridView);
            
        }

        private TextBox txtSilinecekKoltuklar;
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                txtSilinecekKoltuklar = new TextBox();
                txtSilinecekKoltuklar.Location = new Point(180, 200);

                this.Controls.Add(txtSilinecekKoltuklar);
                button2.Top += 50;
                dataGridViewiOlustur();
                Size = new Size(this.Width + 100, this.Height);


                btnEkle = new Button();
                btnEkle.Location = new Point(90, 199);
                btnEkle.Text = "Ekle";
                ekleButonuOzellikleriniAyarla();
                Controls.Add(btnEkle);
            }
            ChangeBackgroundColor(Color.White);
        }

        private void ekleButonuOzellikleriniAyarla()
        {
            btnEkle.Click += new EventHandler(ekleButonuTiklandi);
        }

        private void ekleButonuTiklandi(object sender, EventArgs e)
        {
            if(!string.IsNullOrEmpty(txtSilinecekKoltuklar.Text))
            {
                dataGridView.Rows.Add(txtSilinecekKoltuklar.Text);
                txtSilinecekKoltuklar.Clear();
            }

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked && txtSilinecekKoltuklar != null && btnEkle != null)
            {
                this.Controls.Remove(txtSilinecekKoltuklar);
                txtSilinecekKoltuklar.Dispose();
                button2.Top -= 50;

                Controls.Remove(btnEkle);
                btnEkle.Dispose();

                this.Controls.Remove(dataGridView);
                dataGridView.Dispose();
                Size = new Size(this.Width - 100, this.Height);


            }
            ChangeBackgroundColor(Color.White);
        }
    }
}
