using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace otobusbiletotomasyonu
{
    class DB
    {
        public static SqlConnection baglanti;
        //Baglanti adında bir bağlantı oluşturdum

        public static bool baglanti_kontrol()
        {
            try
            {
                baglanti = new SqlConnection("Data Source=DESKTOP-CRJ0R5U\\SQLEXPRESS;Initial Catalog=otobusbiletsatisvt;Integrated Security=True");
                return true;
                //Veritabanına bağlanırsa baglanti_kontrol fonksiyonu "true" değeri gönderecek
            }

            catch (Exception)
            {
                return false;
                //Veritabanına bağlanamazsa "false" değeri dönecek
            }
        }
    }
}
