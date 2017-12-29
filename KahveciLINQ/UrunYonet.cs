using DAL;
using DomainEntity.Models;
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
    public partial class UrunYonet : Form
    {
        public UrunYonet()
        {
            InitializeComponent();
        }
        KahveContext db = new KahveContext();
        Urun u = new Urun();
        
        private void button1_Click(object sender, EventArgs e)
        {//ekle
            var duzenlenecek = db.Urunler.Find(ID);
            if (duzenlenecek!=null)
            {
                duzenlenecek.UrunAdi = textBox1.Text;
                duzenlenecek.Fiyat = Convert.ToDecimal(textBox2.Text);
                db.Entry(duzenlenecek).State = System.Data.Entity.EntityState.Modified;
                
            }
            else
            {
                   u.UrunAdi = textBox1.Text;
            u.Fiyat = Convert.ToInt32(textBox2.Text);
            db.Urunler.Add(u);
              
            }
          
            db.SaveChanges(); 
            textBox1.Clear();
            textBox2.Clear();
            ListeYenile();
        SiparisEkran s = (SiparisEkran)Application.OpenForms["SiparisEkran"];
                s.Yenile();
        
        }
        private void UrunYonet_Load(object sender, EventArgs e)
        {
            ListeYenile();
        }
        public void ListeYenile()
        {
            listBox1.DataSource = null;
            listBox1.DataSource = db.Urunler.OrderBy(x=>x.UrunAdi).ToList();
            listBox1.DisplayMember = "UrunAdi";
        }

        private void button2_Click(object sender, EventArgs e)
        {//sil
            db.Urunler.Remove((Urun)listBox1.SelectedItem);
            db.SaveChanges();
            ListeYenile();
            SiparisEkran s = (SiparisEkran)Application.OpenForms["SiparisEkran"];
            s.Yenile();
        }
        int ID;
       private void button3_Click(object sender, EventArgs e)
        {//düzenle buton
            Urun secili =(Urun)listBox1.SelectedItem;
         
         ID = secili.UrunID;
            textBox1.Text = secili.UrunAdi;

            textBox2.Text = secili.Fiyat.ToString() ;
        }
    }
}
