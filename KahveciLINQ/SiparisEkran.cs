using DAL;
using DomainEntity.Models;
using DomainEntity.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KahveciLINQ
{
    public partial class SiparisEkran : Form
    {
        public SiparisEkran()
        {
            InitializeComponent();
        }
        KahveContext db = new KahveContext();
        private void SiparisEkran_Load(object sender, EventArgs e)
        {

            Yenile();
            
        }
        public void Yenile()
        {  //strongly typed (list, int, string)
           //weakly typed (object)
            db = new KahveContext();
            List<Urun> tumUrunler = db.Urunler.OrderBy(x => x.Fiyat).ToList();
            flowLayoutPanel1.AutoScroll = true;
            flowLayoutPanel1.Controls.Clear();

            foreach (Urun item in tumUrunler)
            {
                Button b = new Button();
                b.Width = 180;
                b.Height = 90;
                b.Text = item.UrunAdi + Environment.NewLine + item.Fiyat;
                b.Name = "Urun_" + item.UrunID;
                b.Click += UrunuSipariseEkle;
                //hangiButon = b
                //e = new EventArgs
                flowLayoutPanel1.Controls.Add(b);
            }
        }
        List<Sepet> sepet = new List<Sepet>();

        public void UrunuSipariseEkle(object hangiButon, EventArgs e)
        {
            Button tiklananButton = (Button)hangiButon;
            int UrunID = Convert.ToInt32(tiklananButton.Name.Replace("Urun_", ""));
            Urun secilenUrun = db.Urunler.Find(UrunID);

            var sepetteki = sepet.Find(x => x.UrunID == UrunID);
            if (sepetteki == null)
                sepet.Add(new Sepet()
                {
                    UrunID = UrunID,
                    UrunAdi = secilenUrun.UrunAdi,
                    BirimFiyat = secilenUrun.Fiyat,
                    Miktar = 1
                });
            else
                sepetteki.Miktar++;
            SiparisDetayYenile();
        }

        private void SiparisDetayYenile()
        {
            listBox1.DataSource = null;
            listBox2.DataSource = null;
            listBox3.DataSource = null;

            listBox1.DataSource = sepet;
            listBox2.DataSource = sepet;
            listBox3.DataSource = sepet;

            listBox1.ValueMember = "UrunID";
            listBox1.DisplayMember = "UrunAdi";
            listBox2.DisplayMember = "Miktar";
            listBox3.DisplayMember = "ToplamFiyat";

            label1.Text = "Toplam: " + sepet.Sum(x => x.ToplamFiyat).ToString("C");
        }

        private void button1_Click(object sender, EventArgs e)
        { //sil butonu
            int secilenID = (int)listBox1.SelectedValue;
            var silinecek = sepet.Find(x => x.UrunID == secilenID);
            silinecek.Miktar--;
            if (silinecek.Miktar == 0)
                sepet.Remove(silinecek);
            SiparisDetayYenile();
        }

        private void button2_Click(object sender, EventArgs e)
        { //temizle
            sepet.Clear();
            SiparisDetayYenile();
        }

        private void button3_Click(object sender, EventArgs e)
        { //ödeme
            Siparis siparis = new Siparis();
            siparis.KaydedenKullaniciID = 1;
            siparis.SiparistekiUrunler = new List<SiparisDetay>();
            foreach (Sepet item in sepet)
            {
                SiparisDetay sd = new SiparisDetay();
                sd.Miktar = item.Miktar;
                sd.Tutar = item.ToplamFiyat;
                sd.UrunID = item.UrunID;
                siparis.SiparistekiUrunler.Add(sd);
            }
            db.Siparisler.Add(siparis);
            db.SaveChanges();
            button2.PerformClick(); //temizle butonu

        }

        private void günlükRaporToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new GunlukRapor().Show();
        }

        private void aylıkRaporToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new AylikRapor().Show();
        }

        private void ürünleriYönetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new UrunYonet().Show();
        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
        public void ButonSil()
        {

        }
    }
}
