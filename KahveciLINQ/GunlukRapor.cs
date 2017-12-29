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
    public partial class GunlukRapor : Form
    {
        public GunlukRapor()
        {
            InitializeComponent();
        }
        KahveContext db = new KahveContext();
        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            RaporGetir(dateTimePicker1.Value);

            int secilenID = db.Siparisler.Where(x => DbFunctions.TruncateTime(x.Tarih) == dateTimePicker1.Value.Date).Select(x => x.SiparisID).FirstOrDefault();
           
        }

        private void GunlukRapor_Load(object sender, EventArgs e)
        {
            RaporGetir(DateTime.Now);
        }
        public void RaporGetir(DateTime secilenGun)
        { 
            GunlukRaporViewModel rapor = new GunlukRaporViewModel();
            try
            {

           
            rapor.ToplamSatısTutari = db.Siparisler.Where(x => DbFunctions.TruncateTime(x.Tarih).Value.Month == secilenGun.Month && DbFunctions.TruncateTime(x.Tarih).Value.Year == secilenGun.Year && DbFunctions.TruncateTime(x.Tarih).Value.Day == secilenGun.Day).Sum(x => x.SiparistekiUrunler.Sum(a => a.Tutar));
            ////rapor.ToplamSatısTutari = (from x in db.Siparisler select x).Sum(x => x.SiparistekiUrunler.Sum(a => a.Tutar));
            rapor.ToplamSatilanUrunSayisi = db.Siparisler.Where(x => DbFunctions.TruncateTime(x.Tarih).Value.Month == secilenGun.Month && DbFunctions.TruncateTime(x.Tarih).Value.Year == secilenGun.Year).Sum(x => x.SiparistekiUrunler.Sum(a => a.Miktar));
            //select k.KulaniciAdi, Count(*) as SiparisSayisi, Sum (sd.tutar) from Kullanıcıs k
            //ınner join Siparis s
            //on k.KullaniciID=s.EkleyenKullaniciID
            //Inner joın SiparisDetay sd
            //On s.siparisID=sd.SiparisID
            //Group by k.KulaniciAdi
            rapor.KullaniciBasiSatislar = (from sd in db.Siparisler where DbFunctions.TruncateTime(sd.Tarih).Value.Month==secilenGun.Month && DbFunctions.TruncateTime(sd.Tarih).Value.Year == secilenGun.Year
                                           join k in db.Kullanicilar on sd.KaydedenKullaniciID equals k.KullaniciID
                                           group sd by k.KullaniciAdi into yeni
                                           select new KullaniciSatisViewModel
                                           {
                                               KullaniciAdi = yeni.Key,
                                               ToplamSatisTutari = yeni.Sum(x => x.SiparistekiUrunler.Sum(A => A.Tutar))
                                           }).ToList();
             //rapor.UrunBasiSatislar=(from sd in db.Siparisler where DbFunctions.TruncateTime(sd.Tarih).Value.Month == secilenGun.Month && DbFunctions.TruncateTime(sd.Tarih).Value.Year == secilenGun.Year join k in db.Urunler on sd.SiparistekiUrunler equals 
           
            label2.Text = rapor.ToplamSatilanUrunSayisi.ToString("C");
            label3.Text = rapor.ToplamSatısTutari.ToString("C");
            dataGridView1.DataSource = null;
            dataGridView2.DataSource = null;
            dataGridView1.DataSource = rapor.KullaniciBasiSatislar;
            dataGridView2.DataSource = rapor.UrunBasiSatislar; }
            catch 
            {
            }
        }
    }
}
