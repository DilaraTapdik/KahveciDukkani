using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntity.ViewModels
{public class RaporViewModel
    {
        public decimal ToplamSatısTutari { get; set; }
        public int ToplamSatilanUrunSayisi { get; set; }
        public List<KullaniciSatisViewModel> KullaniciBasiSatislar { get; set; }
        public List<UrunBasiSatisViewModel> UrunBasiSatislar { get; set; }
    }
  public  class GunlukRaporViewModel:RaporViewModel
    {
     
    }
    public class AylikRaporViewModel:RaporViewModel
    {

    }
    public class KullaniciSatisViewModel
    {[Browsable (false)]
        public int KullaniciID { get; set; }
        public string KullaniciAdi { get; set; }
        public decimal ToplamSatisTutari { get; set; }

    }
    public class UrunBasiSatisViewModel
    {
        [Browsable(false)]
        public int UrunID { get; set; }
        public string UrunAdi { get; set; }
        public int Adet { get; set; }
    }
}
