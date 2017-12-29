using DAL;
using DomainEntity.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KahveciLINQ
{
    public partial class AylikRapor : Form
    {
        public AylikRapor()
        {
            InitializeComponent();
        }

        private void AylikRapor_Load(object sender, EventArgs e)
        {
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "MM/yyyy";

            AylikRaporViewModel ay = new AylikRaporViewModel();
            RaporGetir(DateTime.Today);

        }
        KahveContext db = new KahveContext();
        public void RaporGetir(DateTime secilenGun)
        {
            AylikRaporViewModel rapor = new AylikRaporViewModel();
            try
            {

                rapor.ToplamSatısTutari = db.Siparisler.Where(x => DbFunctions.TruncateTime(x.Tarih).Value.Month == secilenGun.Month && DbFunctions.TruncateTime(x.Tarih).Value.Year == secilenGun.Year).Sum(x => x.SiparistekiUrunler.Sum(a => a.Tutar));
                //rapor.ToplamSatısTutari = (from x in db.Siparisler select x).Sum(x => x.SiparistekiUrunler.Sum(a => a.Tutar));
                rapor.ToplamSatilanUrunSayisi = db.Siparisler.Where(x => DbFunctions.TruncateTime(x.Tarih).Value.Month == secilenGun.Month && DbFunctions.TruncateTime(x.Tarih).Value.Year == secilenGun.Year).Sum(x => x.SiparistekiUrunler.Sum(a => a.Miktar));
                //select k.KulaniciAdi, Count(*) as SiparisSayisi, Sum (sd.tutar) from Kullanıcıs k
                //ınner join Siparis s
                //on k.KullaniciID=s.EkleyenKullaniciID
                //Inner joın SiparisDetay sd
                //On s.siparisID=sd.SiparisID
                //Group by k.KulaniciAdi
                rapor.KullaniciBasiSatislar = (from sd in db.Siparisler
                                               where DbFunctions.TruncateTime(sd.Tarih).Value.Month == secilenGun.Month && DbFunctions.TruncateTime(sd.Tarih).Value.Year == secilenGun.Year
                                               join k in db.Kullanicilar on sd.KaydedenKullaniciID equals k.KullaniciID
                                               group sd by k.KullaniciAdi into yeni
                                               select new KullaniciSatisViewModel
                                               {
                                                   KullaniciAdi = yeni.Key,
                                                   ToplamSatisTutari = yeni.Sum(x => x.SiparistekiUrunler.Sum(A => A.Tutar))
                                               }).ToList();
            }
            catch { }
            label2.Text = rapor.ToplamSatilanUrunSayisi.ToString("C");
            label3.Text = rapor.ToplamSatısTutari.ToString("C");
            dataGridView1.DataSource = null;
            dataGridView2.DataSource = null;
            dataGridView1.DataSource = rapor.KullaniciBasiSatislar;
            dataGridView2.DataSource = rapor.UrunBasiSatislar;
        }

    }
}
