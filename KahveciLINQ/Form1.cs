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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Kullanici : ref eklemek için
            KahveContext db = new KahveContext();
            Kullanici k = db.Kullanicilar.Where(x => x.KullaniciAdi == textBox1.Text && x.Sifre == textBox2.Text).FirstOrDefault();
            if (k == null)
                MessageBox.Show("Hatalı giriş");
            else
            {
                new SiparisEkran().Show();
                this.Hide();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //button focusu alır. formun hala tuşları dinleyebilmesi için
            this.KeyPreview = true;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                button1.PerformClick();
        }
    }
}
