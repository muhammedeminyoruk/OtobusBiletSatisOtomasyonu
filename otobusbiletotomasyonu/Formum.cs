using MetroFramework.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
namespace otobusbiletotomasyonu
{
    public partial class Formum : MetroForm
    {
        public Formum()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (DB.baglanti_kontrol())
            {
                rezervasyon_satis_gerceklesme_zamani();
                tarihi_gecmis_seferleri_vtden_sil();
                metroTabControl1.SelectedIndex = 0;
                cerceve_sakla();
                groupBox7.Visible = false;
                this.Text = "Otobüs Bilet Satış Otomasyonu";
                pictureBox3.Visible = false;
                pictureBox4.Visible = false;
                pictureBox5.Visible = false;
                pictureBox6.Visible = false;
                pictureBox7.Image = Properties.Resources.duzenle;
            }
            else
            {
                MetroFramework.MetroMessageBox.Show(this, "\n \t Lütfen Bağlantınızı Kontrol Edip Tekrar Deneyiniz..", "Bağlantı Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public DataTable dtable = new DataTable();
        
        public void rezervasyon_tarihi_gecmisse_sil()
        {
            try
            {
                DateTime tarih = DateTime.Now;
                if (DB.baglanti_kontrol())
                {
                    DB.baglanti.Open();
                    SqlCommand komut = new SqlCommand("select bilet_id,satismirezervasyonmu,sefer_tarihi from Biletler INNER JOIN seferler on seferler.sefer_id = Biletler.sefer_id", DB.baglanti);
                    SqlDataReader sdr = komut.ExecuteReader();

                    while (sdr.Read())
                    {
                        string biletin_idsi = sdr["bilet_id"].ToString();
                        string rezervasyon_mu_bak = sdr["satismirezervasyonmu"].ToString();
                        DateTime str1 = Convert.ToDateTime(sdr["sefer_tarihi"].ToString());
                        TimeSpan tspan = str1 - tarih;
                        if (rezervasyon_mu_bak == "Rezervasyon")
                        {
                            if (tspan.Days < Convert.ToUInt32(rezervasyon_satis_gerceklesme_zamani_degiskeni))
                            {
                                if (DB.baglanti_kontrol())
                                {
                                    DB.baglanti.Open();
                                    string sorgu = "DELETE FROM Biletler WHERE bilet_id = '" + biletin_idsi + "'";
                                    SqlCommand myComm = new SqlCommand(sorgu, DB.baglanti);
                                    myComm.ExecuteReader();
                                    DB.baglanti.Close();
                                }
                                else
                                {
                                    MetroFramework.MetroMessageBox.Show(this, "\n \t Lütfen Bağlantınızı Kontrol Edin..", "Bağlantı Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                        }
                    }
                }
                else
                {
                    MetroFramework.MetroMessageBox.Show(this, "\n \t Lütfen Bağlantınızı Kontrol Edin..", "Bağlantı Hatası", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch
            {
                return;
            }
        }
        public void tarihi_gecmis_seferleri_vtden_sil()
        {
            try
            {
                if (DB.baglanti_kontrol())
                {
                    DB.baglanti.Open();
                    string sorgu = "DELETE FROM seferler WHERE sefer_tarihi < '" + dateTimePicker3.Value.Date.ToString("yyyy-MM-dd") + "'";
                    SqlCommand myComm = new SqlCommand(sorgu, DB.baglanti);
                    myComm.ExecuteReader();
                    DB.baglanti.Close();
                }
                else
                {
                    MetroFramework.MetroMessageBox.Show(this, "\n \t Lütfen Bağlantınızı Kontrol Edin..", "Bağlantı Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch
            {
                return;
            }
          
        }
        public void cerceve_sakla()
        {
            Rectangle rect = new Rectangle(
                       metroTabPage1.Left,
                       metroTabPage1.Top,
                       metroTabPage1.Width,
                       metroTabPage1.Height);
            metroTabControl1.Region = new Region(rect);

            Rectangle rect1 = new Rectangle(
                       tabPage1.Left,
                       tabPage1.Top,
                       tabPage1.Width,
                       tabPage1.Height);
            tabControl1.Region = new Region(rect1);

        }
        

        private void metroButton1_Click(object sender, EventArgs e)
        {
            metroTabControl1.SelectedIndex = 1;
        }

        private void metroButton4_Click(object sender, EventArgs e)
        {
            metroTabControl1.SelectedIndex = 0;
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            textBox9.Clear();
            textBox10.Clear();
            kul_adi_kontrol();
            tabControl1.SelectedIndex = 1;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            textBox9.Clear();
            textBox10.Clear();
            kul_adi_kontrol();
            tabControl1.SelectedIndex = 2;
        }

        private void button47_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 0;
        }
        public bool ekleme = true;
        public DataTable dt = new DataTable();
        private void txt_kullaniciAdi_TextChanged(object sender, EventArgs e)
        {
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["kullanici_adi"].ToString().ToUpper().Trim() == txt_kullaniciAdi.Text.ToUpper() && txt_kullaniciAdi.Text != "")
                    {
                        label16.Visible = true;
                        label16.Text = "Kullanılıyor !";
                        ekleme = false;

                        i = dt.Rows.Count;
                    }
                    else
                    {
                        label16.Visible = false;
                        ekleme = true;
                    }
                }
            }
        }
        public void kul_adi_kontrol()
        {
            try
            {
                if (DB.baglanti_kontrol())
                {

                    DB.baglanti.Open();
                    SqlCommand komut = new SqlCommand("select * from Musteriler", DB.baglanti);
                    SqlDataAdapter da = new SqlDataAdapter(komut);
                    da.Fill(dt);
                    DB.baglanti.Close();
                }
                else
                {
                    MetroFramework.MetroMessageBox.Show(this, "\n \t Lütfen Bağlantınızı Kontrol Edin..", "Bağlantı Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch
            {
                return;
            }
        }

        private void txt_SifreTekrar_TextChanged(object sender, EventArgs e)
        {
            if(txt_Sifre.Text != "" && txt_SifreTekrar.Text !="")
            {
                if (txt_Sifre.Text != txt_SifreTekrar.Text)
                {
                    label17.Visible = true;
                    label17.Text = "Şifre Uyuşmuyor !";
                }
                else
                {
                    label17.Visible = false;
                }
            }
        }
        private void button46_Click(object sender, EventArgs e)
        {
            try
            {
                if (txt_Adi.Text != "" && txt_Soyadi.Text != "" && txt_kullaniciAdi.Text != "" && txt_Sifre.Text != "" && txt_SifreTekrar.Text != "" && txt_Email.Text != "" && txt_Telefon.Text != "" && dtp_Dogum.Text != DateTime.Now.ToString() && comboBox4.Text != "")
                {
                    if(txt_Sifre.Text == txt_SifreTekrar.Text)
                    {
                        if (DB.baglanti_kontrol())
                        {
                            if(ekleme)
                            {

                                SqlCommand komut1 = new SqlCommand("INSERT INTO Musteriler(ad,soyad,kullanici_adi,sifre,email,telefon,dogum_tarihi,cinsiyet) VALUES ('" + txt_Adi.Text + "','" + txt_Soyadi.Text + "','" + txt_kullaniciAdi.Text + "','" + txt_Sifre.Text + "','" + txt_Email.Text + "','" + txt_Telefon.Text + "','" + dtp_Dogum.Value.Date.ToString("yyyy-MM-dd") + "','" + comboBox4.Text + "')", DB.baglanti);
                                DB.baglanti.Open();
                                komut1.ExecuteNonQuery();
                                label53.Visible = true;
                                label53.Text = "Kayıt işleminiz gerçekleşmiştir.";
                                kulllanici_giris_bosalt();
                                DB.baglanti.Close();

                            }
                            else
                            {
                                MetroFramework.MetroMessageBox.Show(this, "\n Lütfen Kullanıcı Adını Tekrar Deneyiniz..", "Kullanıcı Adı Kullanılıyor", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                        else
                        {
                            MetroFramework.MetroMessageBox.Show(this, "\n \t Lütfen Bağlantınızı Kontrol Edin..", "Bağlantı Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MetroFramework.MetroMessageBox.Show(this, "\n Lütfen Şifrelerin Aynı Olduğundan Emin Olun..", "Şifre Uyuşmazlığı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                   
                }
                else
                {
                    MetroFramework.MetroMessageBox.Show(this, "\n Lütfen Boş Alan Bırakmayınız..", "Boş Alan Hatası", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch
            {
                return;
            }
                
        }
        public void kulllanici_giris_bosalt()
        {
            foreach(Control ctrl in groupBox5.Controls)
            {
                if(ctrl is TextBox)
                {
                    ctrl.Text = "";
                }
                if(ctrl is ComboBox)
                {
                    ctrl.Text = "";
                }
                if(ctrl is DateTimePicker)
                {
                    ctrl.Text = DateTime.Now.ToString();
                }
            }
        }
        public string musteri_id;
        public string ad = "";
        public string soyad = "";
        public string satismirezervasyonmu = "Boş";
        public string yetki = "";
        public string kullanicinin_adi = "";
        private void button44_Click(object sender, EventArgs e)
        {
            try
            {
                dateTimePicker1.MinDate = DateTime.Now;
                if (textBox10.Text != "" && textBox9.Text != "")
                {
                    if (DB.baglanti_kontrol())
                    {
                        DB.baglanti.Open();
                        SqlParameter prm1 = new SqlParameter("@P1", textBox9.Text);
                        SqlParameter prm2 = new SqlParameter("@P2", textBox10.Text);
                        string komut = "select * FROM Musteriler WHERE kullanici_adi=@P1 and sifre=@P2";
                        SqlCommand cmd = new SqlCommand(komut, DB.baglanti);

                        cmd.Parameters.Add(prm1);
                        cmd.Parameters.Add(prm2);
                        SqlDataAdapter da = new SqlDataAdapter(cmd);

                        DataTable dt = new DataTable();

                        da.Fill(dt);

                        if (dt.Rows.Count > 0)
                        {
                            musteri_id = Convert.ToString(dt.Rows[0]["musteri_id"]);
                            kullanicinin_adi = Convert.ToString(dt.Rows[0]["kullanici_adi"]);
                            ad = Convert.ToString(dt.Rows[0]["ad"]);
                            soyad = Convert.ToString(dt.Rows[0]["soyad"]);
                            yetki = Convert.ToString(dt.Rows[0]["yetki"]);
                            label15.Visible = true;
                            if (yetki == "Admin")
                            {
                                sefer_düzenle_grid_doldur();
                                metroTabControl1.SelectedIndex = 2;
                                metroTabControl2.SelectedIndex = 0;
                                label15.Text = "Hoşgeldiniz " + "  " + ad + "  " + soyad;
                                this.Text = "Yönetici Paneli";
                                pictureBox6.Visible = true;
                                dateTimePicker2.MinDate = DateTime.Now;
                                rezervasyon_satis_gerceklesme_zamani();
                            }
                            else if (yetki == "kullanıcı")
                            {

                                label15.Text = "Hoşgeldiniz " + "  " + ad + "  " + soyad;
                                this.Text = "Otobüs Bilet Satış Otomasyonu";
                                tabControl1.SelectedIndex = 3;
                                pictureBox3.Visible = true;
                                pictureBox4.Visible = true;
                                pictureBox5.Visible = true;
                                pictureBox6.Visible = true;
                                pictureBox1.Visible = true;
                                pictureBox8.Visible = false;
                            }
                            textBox9.Clear();
                            textBox10.Clear();
                        }
                        else
                        {
                            MetroFramework.MetroMessageBox.Show(this, "\n Kullanıcı Bulunamadı..", "Kullanıcı Adınız Veya Şifreniz Hatalı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    else
                    {
                        MetroFramework.MetroMessageBox.Show(this, "\n \t Lütfen Bağlantınızı Kontrol Edin..", "Bağlantı Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MetroFramework.MetroMessageBox.Show(this, "\n Lütfen Giriş Bilgileriniz Kontrol Ediniz..", "Boş Alan Hatası", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch
            {
                return;
            }
        }
        public string rezervasyon_satis_gerceklesme_zamani_degiskeni = "";
        public void rezervasyon_satis_gerceklesme_zamani()
        {
            try
            {
                if (DB.baglanti_kontrol())
                {
                    DB.baglanti.Open();
                    string komut = "select kac_gun FROM Rezervasyonislemzamani";
                    SqlCommand cmd = new SqlCommand(komut, DB.baglanti);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();

                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        rezervasyon_satis_gerceklesme_zamani_degiskeni = Convert.ToString(dt.Rows[0]["kac_gun"]);
                        maskedTextBox8.Text = rezervasyon_satis_gerceklesme_zamani_degiskeni;
                    }
                    DB.baglanti.Close();
                }
            }
            catch
            {
                return;
            }
        }
        public void sefer_düzenle_grid_doldur()
        {
           
            try
            {
                if (DB.baglanti_kontrol())
                {
                    DB.baglanti.Open();
                    string sorgu = "SELECT sefer_id,sefer_tarihi as 'Sefer Tarihi',S_kalkis_saati as 'Kalkış Saati',S_varis_saati as 'Varış Saati',S_kalkis_yeri as 'Kalkış Yeri',S_varis_yeri as 'Varış Yeri',arac_marka as 'Araç Markası',arac_plaka as 'Araç Plakası',bilet_tutari as 'Bilet Ücreti',sofor_adi as 'Şoför Adı',sofor_soyadi as 'Şoför Soyadı',muavin_adi as 'Muavin Adı',muavin_soyadi as 'Muavin Soyadı' FROM seferler where sefer_tarihi >='" + DateTime.Now.ToString("yyyy-MM-dd")+"' ";
                    SqlCommand komut = new SqlCommand(sorgu, DB.baglanti);
                    SqlDataAdapter da = new SqlDataAdapter(komut);
                    DataTable sefer_dt = new DataTable();
                    da.Fill(sefer_dt);
                    dataGridView2.DataSource = sefer_dt;
                    dataGridView2.Columns[0].Visible = false;
                    DB.baglanti.Close();
                }
                else
                {
                    MetroFramework.MetroMessageBox.Show(this, "\n \t Lütfen Bağlantınızı Kontrol Edin..", "Bağlantı Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            catch
            {
                return;
            }
        }
        public bool kontrol = true;
        private void button45_Click(object sender, EventArgs e)
        {
            try
            {
                if(sifre_unuttum)
                {
                    if (textBox12.Text != "")
                    {
                        if (textBox11.Text != "")
                        {
                            if (textBox12.Text == textBox13.Text)
                            {
                                if (kullanici_dogum_tar == dateTimePicker3.Value.ToString())
                                {

                                    if (DB.baglanti_kontrol())
                                    {
                                        DB.baglanti.Open();
                                        string sorgu = "update Musteriler set sifre='" + textBox12.Text + "' where kullanici_adi='" + textBox11.Text + "' and dogum_tarihi='" + dateTimePicker3.Value.ToString("yyyy-MM-dd") + "'";
                                        SqlCommand komut = new SqlCommand(sorgu, DB.baglanti);
                                        komut.ExecuteNonQuery();
                                        label18.Visible = true;
                                        label18.ForeColor = Color.Green;
                                        label18.Text = "Şifreniz Güncellenmiştir.";
                                        textBox11.Clear();
                                        textBox12.Clear();
                                        textBox13.Clear();
                                        dateTimePicker3.Text = DateTime.Now.ToString();
                                        DB.baglanti.Close();
                                    }
                                    else
                                    {
                                        MetroFramework.MetroMessageBox.Show(this, "\n \t Lütfen Bağlantınızı Kontrol Edin..", "Bağlantı Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                }
                                else
                                {
                                    MetroFramework.MetroMessageBox.Show(this, "\n Doğum Tarihinizi Kontrol Ediniz..", "Doğum Tarihi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }

                            }
                            else
                            {
                                MetroFramework.MetroMessageBox.Show(this, "\n Lütfen Bilgilerinizi Kontrol Ediniz..", "Yanlış Bilgi Girdiniz.", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                    }
                    else
                    {
                        MetroFramework.MetroMessageBox.Show(this, "\n  Şifre Alanları Boş Geçilemez", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    MetroFramework.MetroMessageBox.Show(this, "\n Kullanıcı Bulunamadı..", "Hata.", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
               
            }
            catch
            {
                return;
            }
        }

        private void textBox13_TextChanged(object sender, EventArgs e)
        {
            if (textBox12.Text != "" && textBox13.Text != "")
            {
                if (textBox12.Text != textBox13.Text)
                {
                    label19.Visible = true;
                    label19.Text = "Şifre Uyuşmuyor !";
                }
                else
                {
                    label19.Visible = false;
                }
            }
            
        }

        private void button48_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 0;
            label18.Text = "";
            label54.Text = "";
            label19.Text = "";
            textBox11.Clear();
            textBox12.Clear();
            textBox13.Clear();
            dateTimePicker3.Text = DateTime.Now.ToString();
        }


        private void comboBox1_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (DB.baglanti_kontrol())
                {
                    DB.baglanti.Open();
                    string sorgu = "Select DISTINCT S_kalkis_yeri from seferler ";
                    SqlDataAdapter adp = new SqlDataAdapter(sorgu, DB.baglanti);
                    DataSet ds = new DataSet();
                    adp.Fill(ds);
                    comboBox1.DisplayMember = "S_kalkis_yeri";
                    comboBox1.DataSource = ds.Tables[0];
                    DB.baglanti.Close();
                }
                else
                {
                    MetroFramework.MetroMessageBox.Show(this, "\n \t Lütfen Bağlantınızı Kontrol Edin..", "Bağlantı Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch
            {
                return;
            }
        }

        private void comboBox2_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (DB.baglanti_kontrol())
                {
                    DB.baglanti.Open();
                    string sorgu = "Select DISTINCT S_varis_yeri from seferler where S_kalkis_yeri = '" + comboBox1.Text + "' ";
                    SqlDataAdapter adp = new SqlDataAdapter(sorgu, DB.baglanti);
                    DataSet ds = new DataSet();
                    adp.Fill(ds);
                    comboBox2.DisplayMember = "S_varis_yeri";
                    comboBox2.DataSource = ds.Tables[0];
                    DB.baglanti.Close();
                }
                else
                {
                    MetroFramework.MetroMessageBox.Show(this, "\n \t Lütfen Bağlantınızı Kontrol Edin..", "Bağlantı Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch
            {
                return;
            }
        }
        public void satismirezervasyonmukontrol()
        {
            
            if (satinAl.Checked == true && rezervasyon.Checked == false)
            {
                satismirezervasyonmu = "Satış"; 
            }
            else if (satinAl.Checked == false && rezervasyon.Checked == true)
            {
                satismirezervasyonmu = "Rezervasyon";
            }
            else
            {
                satismirezervasyonmu = "Boş";
            }
        }
        private void metroButton1_Click_1(object sender, EventArgs e)
        {
            try
            {
               
                dataGridView1.ClearSelection();
                dataGridView1.DataSource = null;
                butonlar_enable("false");
                satismirezervasyonmukontrol();
                if (satismirezervasyonmu == "Rezervasyon")
                {
                    rezervasyon_tarihi_gecmisse_sil();
                }
                foreach (Control ctrl in groupBox3.Controls)
                {
                    if (ctrl is Button)
                    {
                        ctrl.BackColor = Color.White;
                    }

                }
                if (satinAl.Checked == false && rezervasyon.Checked == false)
                {
                    MetroFramework.MetroMessageBox.Show(this, "\n Lütfen Satış Veya Rezervasyon İşleminizi Belirtiniz.", "Uyarı..", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    if (DB.baglanti_kontrol())
                    {
                        DB.baglanti.Open();
                        string sorgu = "SELECT sefer_id,S_kalkis_saati as 'Kalkış Saati',S_varis_saati as 'Varış Saati',arac_marka as 'Araç Markası',arac_plaka as 'Araç Plakası',S_kalkis_yeri as 'Kalkış Yeri',S_varis_yeri as 'Varış Yeri',sefer_tarihi as 'Sefer Tarihi',bilet_tutari as 'Bilet Ücreti',sofor_adi as 'Şoför Adı',sofor_soyadi as 'Şoför Soyadı',muavin_adi as 'Muavin Adı',muavin_soyadi as 'Muavin Soyadı' FROM seferler where S_kalkis_yeri = '" + comboBox1.Text + "' and S_varis_yeri = '" + comboBox2.Text + "' and sefer_tarihi = '" + dateTimePicker1.Value.Date.ToString("yyyy-MM-dd") + "'";
                        SqlCommand komut = new SqlCommand(sorgu, DB.baglanti);
                        SqlDataAdapter da = new SqlDataAdapter(komut);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        dataGridView1.DataSource = dt;
                        dataGridView1.Columns[0].Visible = false;
                        DB.baglanti.Close();
                    }
                    else
                    {
                        MetroFramework.MetroMessageBox.Show(this, "\n \t Lütfen Bağlantınızı Kontrol Edin..", "Bağlantı Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    if (dataGridView1.Rows.Count > 0)
                    {
                        metroTabControl1.SelectedIndex = 1;
                    }
                    if (dataGridView1.Rows.Count == 0)
                    {
                        MetroFramework.MetroMessageBox.Show(this, "\n Üzgünüz.. \n Sefer Bulunamamıştır", "Bilgilendirme", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch
            {
                return;
            }
        }

        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dataGridView1.ClearSelection();
        }

        public string sefer_id = "";
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                bilgi.Visible = false;
                foreach (Control ctrl in groupBox3.Controls)
                {
                    if (ctrl is Button)
                    {
                        ctrl.BackColor = Color.White;
                    }

                }

                sefer_id = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                textBox4.Text = dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString();
                textBox5.Text = dataGridView1.Rows[e.RowIndex].Cells[6].Value.ToString();
                textBox6.Text = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
                textBox7.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
                textBox8.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
                textBox14.Text = dataGridView1.Rows[e.RowIndex].Cells[7].Value.ToString().Substring(0, 10);
                bilet_txb.Text = dataGridView1.Rows[e.RowIndex].Cells[8].Value.ToString();

                koltuklari_doldur();
                butonlar_enable("true");
                dolu_koltuklar();
            }
            catch
            {
                return;
            }
                
        }
        public void dolu_koltuklar()
        {
            foreach (Control ctrl in groupBox3.Controls)
            {
                if (ctrl is Button)
                {
                    if (ctrl.BackColor != Color.White)
                    {
                        ctrl.Enabled = false;
                    }
                }

            }
        }
        public void butonlar_enable(string secim)
        {
            if(secim == "true")
            {
                foreach (Control ctrl in groupBox3.Controls)
                {
                    if (ctrl is Button)
                    {
                        ctrl.Enabled = true;
                    }

                }
            }
            if(secim == "false")
            {
                foreach (Control ctrl in groupBox3.Controls)
                {
                    if (ctrl is Button)
                    {
                        ctrl.Enabled = false;
                    }

                }
            }
        }
        public string koltuk_renklendir = "",cinsiyet="", satışmırezervemi="";
        public void koltuklari_doldur()
        {
            try
            {
                if (DB.baglanti_kontrol())
                {
                    DB.baglanti.Open();
                    string sorgu = "select koltuk_no,cinsiyet,satismirezervasyonmu from Biletler where sefer_id = '" + sefer_id + "'";
                    SqlCommand komut = new SqlCommand(sorgu, DB.baglanti);
                    SqlDataReader okuyucu = komut.ExecuteReader();

                    while (okuyucu.Read())
                    {
                        koltuk_renklendir = okuyucu[0].ToString();
                        cinsiyet = okuyucu[1].ToString();
                        satışmırezervemi = okuyucu[2].ToString();
                        if (koltuk_renklendir != "" && cinsiyet != "")
                        {
                            foreach (Control ctrl in groupBox3.Controls)
                            {
                                if (ctrl is Button)
                                {
                                   if(ctrl.Text == koltuk_renklendir)
                                    {
                                            if (cinsiyet == "Erkek")
                                            {
                                                ctrl.BackColor = Color.DeepSkyBlue;
                                            }
                                            if (cinsiyet == "Bayan")
                                            {
                                                    ctrl.BackColor = Color.DeepPink;
                                            }
                                    }
                                }
                            
                            }

                        }
                    }
                    
                    okuyucu.Close();
                    DB.baglanti.Close();
                }
                else
                {
                    MetroFramework.MetroMessageBox.Show(this, "\n \t Lütfen Bağlantınızı Kontrol Edin..", "Bağlantı Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch
            {
                return;
            }
        }
        
        private void satinAl_CheckedChanged(object sender, EventArgs e)
        {
            if (rezervasyon.Checked == true)
            {
                satinAl.Checked = false;
            }
            
        }

        private void rezervasyon_CheckedChanged(object sender, EventArgs e)
        {
            if(satinAl.Checked == true)
            {
                rezervasyon.Checked = false;
            }
            else if(satinAl.Checked ==false && rezervasyon.Checked == true)
            {

                DateTime tarih = DateTime.Now;
                dateTimePicker1.MinDate = tarih.AddDays(3);
            }
            else if(rezervasyon.Checked == false)
            {

                dateTimePicker1.MinDate = DateTime.Now;
                dateTimePicker1.Value = DateTime.Now;
            }
        }

        private void metroButton4_Click_1(object sender, EventArgs e)
        {
            metroTabControl1.SelectedIndex = 0;
            foreach (Control ctrl in groupBox3.Controls)
            {
                if (ctrl is Button)
                {
                    ctrl.BackColor = Color.White;
                }

            }
        }
        public string koltuk_no = "bos",koltuk_no_tut="";
        public Button b;
        public int kac_koltuk = 0;

        private void button52_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 0;
            kulllanici_giris_bosalt();
            label53.Visible = false;
            label17.Visible = false;
        }

        private void metroButton2_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox1.Text != "" && textBox2.Text != "" && textBox3.Text !="")
                {
                    if (textBox15.Text !="")
                    {
                        if(satismirezervasyonmu == "Rezervasyon")
                        {
                            bilgi.Text = "Rezervasyon İşleminiz Gerçekleştirilmiştir.";
                            string gosterilecek_bilgi = "\n Biletinizi " + rezervasyon_satis_gerceklesme_zamani_degiskeni + " Gün İçinde Almalısınız Aksi Taktirde Biletiniz İptal Edilecektir.\n Onaylıyor musunuz ?";
                            DialogResult dialogResult = MetroFramework.MetroMessageBox.Show(this, gosterilecek_bilgi, "  Onay Kutucuğu", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                            if (dialogResult == DialogResult.Yes)
                            {
                                if (DB.baglanti_kontrol())
                                {

                                    SqlCommand komut1 = new SqlCommand("INSERT INTO Biletler(musteri_id,koltuk_no,satismirezervasyonmu,sefer_id,cinsiyet) VALUES ('" + musteri_id + "','" + koltuk_no + "','" + satismirezervasyonmu + "','" + sefer_id + "','" + yolcu_cinsiyet + "')", DB.baglanti);
                                    DB.baglanti.Open();
                                    komut1.ExecuteNonQuery();
                                    label24.Text = "";
                                    label25.Text = "";
                                    label26.Text = "";
                                    koltuklari_doldur();
                                    dolu_koltuklar();
                                    yolcu_bileri_sifirla();
                                    oturabilir_mi = false;
                                    bilgi.Visible = true;
                                    kac_koltuk = 0;
                                    DB.baglanti.Close();

                                }
                                else
                                {
                                    MetroFramework.MetroMessageBox.Show(this, "\n \t Lütfen Bağlantınızı Kontrol Edin..", "Bağlantı Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                        }
                        if (satismirezervasyonmu == "Satış")
                        { 
                            bilgi.Text = "Satış İşleminiz Gerçekleştirilmiştir.";
                            DialogResult dialogResult = MetroFramework.MetroMessageBox.Show(this, "\n Onaylıyor musunuz ?", "  Onay Kutucuğu", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                            if (dialogResult == DialogResult.Yes)
                            {
                                if (DB.baglanti_kontrol())
                                {
                                    SqlCommand komut1 = new SqlCommand("INSERT INTO Biletler(musteri_id,koltuk_no,satismirezervasyonmu,sefer_id,cinsiyet) VALUES ('" + musteri_id + "','" + koltuk_no + "','" + satismirezervasyonmu + "','" + sefer_id + "','" + yolcu_cinsiyet + "')", DB.baglanti);
                                    DB.baglanti.Open();
                                    komut1.ExecuteNonQuery();
                                    label24.Text = "";
                                    label25.Text = "";
                                    label26.Text = "";
                                    koltuklari_doldur();
                                    dolu_koltuklar();
                                    yolcu_bileri_sifirla();
                                    oturabilir_mi = false;
                                    bilgi.Visible = true;
                                    kac_koltuk = 0;
                                    DB.baglanti.Close();

                                }
                                else
                                {
                                    MetroFramework.MetroMessageBox.Show(this, "\n \t Lütfen Bağlantınızı Kontrol Edin..", "Bağlantı Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                        }
                    }
                    else
                    {
                        MetroFramework.MetroMessageBox.Show(this, "\n Lütfen Oturmak İstediğiniz Koltuk Numarasını Seçiniz.", "Koltuk Seçimi !", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                }
                else
                {
                    MetroFramework.MetroMessageBox.Show(this, "\n Lütfen Boş Alanları Kontrol Ediniz !", "Eksik Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    if (textBox1.Text == "")
                    {
                        label24.Visible = true;
                    }
                    if (textBox2.Text == "")
                    {
                        label25.Visible = true;
                    }
                    if (textBox3.Text == "")
                    {
                        label26.Visible = true;
                    }
                }
            }
            catch
            {
                return;
            }
        }
        public void yolcu_bileri_sifirla()
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox15.Clear();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            bilgi.Visible = false;
            b = (Button)sender;
            koltuk_no = b.Text;


            if (kac_koltuk < 1)
            {
                koltuk_no_tut = koltuk_no;

                groupBox7.Visible = true;
            }
            if(b.Text == koltuk_no_tut)
            {
                b.BackColor = Color.White;
                kac_koltuk = 0;
            }
            if (kac_koltuk >= 1)
            {
                MessageBox.Show("Sadece Bir Koltuk Seçebilirsiniz.");
                groupBox7.Visible = false;

            }
        }
        public string yolcu_cinsiyet = "";
        public bool oturabilir_mi = false;
        
        public string kullanici_dogum_tar = "";
        private void dateTimePicker3_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (sifre_unuttum)
                {
                    if (DB.baglanti_kontrol())
                    {

                        DB.baglanti.Open();
                        SqlCommand komut = new SqlCommand("select dogum_tarihi from Musteriler where kullanici_Adi = '" + textBox11.Text + "'", DB.baglanti);
                        SqlDataReader da = komut.ExecuteReader();
                        while (da.Read())
                        {
                            kullanici_dogum_tar = da["dogum_tarihi"].ToString();
                        }
                        DB.baglanti.Close();
                    }
                    else
                    {
                        MetroFramework.MetroMessageBox.Show(this, "\n \t Lütfen Bağlantınızı Kontrol Edin..", "Bağlantı Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                
            }
            catch
            {
                return;
            }
        }
        public bool sifre_unuttum = false;

        private void textBox11_TextChanged(object sender, EventArgs e)
        {
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["kullanici_adi"].ToString().ToUpper().Trim() == textBox11.Text.ToUpper())
                    {
                        sifre_unuttum = true;
                        label54.Visible = false;
                        i = dt.Rows.Count;
                    }
                    else
                    {
                        sifre_unuttum = false;
                        label54.Visible = true;
                    }
                }
            }
        }

        private void comboBox5_MouseClick(object sender, MouseEventArgs e)
        {
            if(comboBox3.Text!="")
            {
                for(int i=0; i < comboBox3.Items.Count;i++)
                {
                    if(comboBox3.Text != comboBox3.Items[i].ToString())
                    {
                        comboBox5.Items.Add(comboBox3.Items[i].ToString());
                    }
                }
                
            }
            else
            {
                MessageBox.Show("Kalkış Yerini Seçmeden Önce Varış Yerini Seçemezsiniz !");
            }
        }

        private void comboBox3_MouseClick(object sender, MouseEventArgs e)
        {
            comboBox5.Items.Clear();
            comboBox5.Text = "";
        }


        private void button47_Click_1(object sender, EventArgs e)
        {
            oturabilir_mi = false;
            int koltuk = Convert.ToUInt16(koltuk_no);
            groupBox7.Visible = false;
                if (b.Text == koltuk_no)
                {
                    if(kac_koltuk < 1)
                    {

                        yolcu_cinsiyet = "Erkek";
                        bay_bayan_kontrol_et(koltuk_no,yolcu_cinsiyet);
                        if(oturabilir_mi)
                        {
                            b.BackColor = Color.DeepSkyBlue;
                            textBox15.Text = yolcu_cinsiyet;
                            kac_koltuk++;
                        }
                        else
                        {
                            MessageBox.Show("Yan koltukta bayan yolcu bulunmaktadır !");
                        }
                        
                    }
                }
        }


        private void button49_Click(object sender, EventArgs e)
        {
            oturabilir_mi = false;
            groupBox7.Visible = false;
            if (b.Text == koltuk_no)
            {
                if (kac_koltuk < 1)
                {

                    yolcu_cinsiyet = "Bayan";
                    bay_bayan_kontrol_et(koltuk_no, yolcu_cinsiyet);
                    if (oturabilir_mi)
                    {
                        b.BackColor = Color.DeepPink;
                        textBox15.Text = yolcu_cinsiyet;
                        kac_koltuk++;
                    }
                    else
                    {
                        MessageBox.Show("Yan koltukta erkek yolcu bulunmaktadır !");
                    }
                }
            }
        }
        public void bay_bayan_kontrol_et(string koltuk_kontrolu,string yolcu_cinsiyet)
        {
            if (Convert.ToInt32(koltuk_kontrolu) % 2 == 1)
            {
                int koltuk_tut = Convert.ToInt32(koltuk_kontrolu) + 1;
                foreach(Control ctrl in groupBox3.Controls)
                {
                    if(ctrl is Button)
                    {
                        if(Convert.ToString(koltuk_tut) == ctrl.Text)
                        {
                            if(yolcu_cinsiyet == "Erkek")
                            {
                                if(ctrl.BackColor == Color.DeepSkyBlue)
                                {
                                    oturabilir_mi = true;
                                }
                                if(ctrl.BackColor == Color.White)
                                {
                                    oturabilir_mi = true;
                                }
                            }
                            else if (yolcu_cinsiyet == "Bayan")
                            {
                                if (ctrl.BackColor == Color.DeepPink)
                                {
                                    oturabilir_mi = true;
                                }
                                if (ctrl.BackColor == Color.White)
                                {
                                    oturabilir_mi = true;
                                }
                            }
                            else if (yolcu_cinsiyet == "Erkek")
                            {
                                if (ctrl.BackColor == Color.DeepPink)
                                {
                                    oturabilir_mi = false;
                                }
                                if (ctrl.BackColor == Color.White)
                                {
                                    oturabilir_mi = true;
                                }
                            }
                            else if (yolcu_cinsiyet == "Bayan")
                            {
                                if (ctrl.BackColor == Color.DeepSkyBlue)
                                {
                                    oturabilir_mi = false;
                                }
                                if (ctrl.BackColor == Color.White)
                                {
                                    oturabilir_mi = true;
                                }
                            }
                        }
                    }
                }
            }

            if (Convert.ToInt32(koltuk_kontrolu) % 2 == 0)
            {
                int koltuk_tut = Convert.ToInt32(koltuk_kontrolu) - 1;
                foreach (Control ctrl in groupBox3.Controls)
                {
                    if (ctrl is Button)
                    {
                        if (Convert.ToString(koltuk_tut) == ctrl.Text)
                        {
                            if (yolcu_cinsiyet == "Erkek")
                            {
                                if (ctrl.BackColor == Color.DeepSkyBlue)
                                {
                                    oturabilir_mi = true;
                                }
                                if (ctrl.BackColor == Color.White)
                                {
                                    oturabilir_mi = true;
                                }
                            }
                            else if (yolcu_cinsiyet == "Bayan")
                            {
                                if (ctrl.BackColor == Color.DeepPink)
                                {
                                    oturabilir_mi = true;
                                }
                                if (ctrl.BackColor == Color.White)
                                {
                                    oturabilir_mi = true;
                                }
                            }
                            else if (yolcu_cinsiyet == "Erkek")
                            {
                                if (ctrl.BackColor == Color.DeepPink)
                                {
                                    oturabilir_mi = false;
                                }
                                if (ctrl.BackColor == Color.White)
                                {
                                    oturabilir_mi = true;
                                }
                            }
                            else if (yolcu_cinsiyet == "Bayan")
                            {
                                if (ctrl.BackColor == Color.DeepSkyBlue)
                                {
                                    oturabilir_mi = false;
                                }
                                if (ctrl.BackColor == Color.White)
                                {
                                    oturabilir_mi = true;
                                }
                            }
                        }
                    }
                }
            }
        }
        private void button53_Click(object sender, EventArgs e)
        {
            try
            {
                sefer_ekle_arac_bilgileri();
                if (kontrol1)
                {
                    sefer_ekle_gorevli_personel_bilgileri();
                    if (kontrol2)
                    {
                        sefer_ekle_sefer_bilgileri();
                        if (kontrol3)
                        {
                            if (DB.baglanti_kontrol())
                            {
                                SqlCommand komut = new SqlCommand("INSERT INTO seferler(S_kalkis_yeri,S_varis_yeri,S_kalkis_saati,S_varis_saati,sefer_tarihi,bilet_tutari,sofor_adi,sofor_soyadi,muavin_adi,muavin_soyadi,arac_plaka,arac_marka) VALUES ('" + comboBox3.Text + "','" + comboBox5.Text + "','" + maskedTextBox1.Text + "','" + maskedTextBox2.Text + "','" + dateTimePicker2.Value.Date.ToString("yyyy-MM-dd") + "','" + maskedTextBox3.Text + "','" + textBox18.Text + "','" + textBox19.Text + "','" + textBox20.Text + "','" + textBox21.Text + "','" + textBox16.Text + "','" + textBox17.Text + "')", DB.baglanti);
                                DB.baglanti.Open();
                                komut.ExecuteNonQuery();
                                timer1.Start();
                                sefer_ekleme_ekrani_bosalt();
                                sefer_düzenle_grid_doldur();
                                DB.baglanti.Close();
                            }
                            else
                            {
                                MetroFramework.MetroMessageBox.Show(this, "\n \t Lütfen Bağlantınızı Kontrol Edin..", "Bağlantı Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Sefer bilgilerini eksiksiz girmelisiniz.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Görevli personel bilgilerini eksiksiz girmelisiniz.");
                    }

                }
                else
                {
                    MessageBox.Show("Araç bilgilerini eksiksiz girmelisiniz.");
                }

            }
            catch
            {
                return;
            }
        }
        public bool kontrol1 = false, kontrol2 = false, kontrol3 = false;
        public void sefer_ekle_arac_bilgileri()
        {
            if (textBox16.Text != "" && textBox17.Text != "")
            {
                kontrol1 = true;
            }
        }

        private void comboBox6_MouseClick(object sender, MouseEventArgs e)
        {
            if (comboBox7.Text != "")
            {
                for (int i = 0; i < comboBox7.Items.Count; i++)
                {
                    if (comboBox7.Text != comboBox7.Items[i].ToString())
                    {
                        comboBox6.Items.Add(comboBox7.Items[i].ToString());
                    }
                }

            }
            else
            {
                MessageBox.Show("Kalkış Yerini Seçmeden Önce Varış Yerini Seçemezsiniz !");
            }
        }

        private void comboBox7_MouseClick(object sender, MouseEventArgs e)
        {
            comboBox6.Items.Clear();
            comboBox6.Text = "";
        }

        private void button54_Click(object sender, EventArgs e)
        {
            try
            {
                if(dataGridView2.SelectedRows.Count > 0)
                {
                    sefer_düzenle_arac_bilgileri();
                    if (sefer_düzenle_kontrol1)
                    {
                        sefer_düzenle_gorevli_personel_bilgileri();
                        if (sefer_düzenle_kontrol2)
                        {
                            sefer_düzenle_sefer_bilgileri();
                            if (sefer_düzenle_kontrol3)
                            {
                                if (DB.baglanti_kontrol())
                                {
                                    DB.baglanti.Open();
                                    string sorgu = "update seferler set S_kalkis_yeri='" + comboBox7.Text + "',S_varis_yeri='" + comboBox6.Text + "',S_kalkis_saati='" + maskedTextBox6.Text + "',S_varis_saati='" + maskedTextBox5.Text + "',sefer_tarihi='" + dateTimePicker4.Value.Date.ToString("yyyy-MM-dd") + "',bilet_tutari='" + maskedTextBox4.Text + "',sofor_adi='" + textBox27.Text + "',sofor_soyadi='" + textBox26.Text + "' ,muavin_adi='" + textBox25.Text + "' ,muavin_soyadi='" + textBox24.Text + "' ,arac_plaka='" + textBox23.Text + "' ,arac_marka='" + textBox22.Text + "'  where sefer_id='" + sefer_duzenle_sefer_id + "' ";
                                    SqlCommand komut = new SqlCommand(sorgu, DB.baglanti);
                                    komut.ExecuteNonQuery();
                                    timer2.Start();
                                    sefer_düzenle_grid_doldur();
                                    sefer_düzenleme_ekrani_bosalt();
                                    DB.baglanti.Close();
                                }
                                else
                                {
                                    MetroFramework.MetroMessageBox.Show(this, "\n \t Lütfen Bağlantınızı Kontrol Edin..", "Bağlantı Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                            else
                            {
                                MessageBox.Show("Sefer bilgilerini kontrol ediniz.");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Görevli personel bilgilerini kontrol ediniz.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Araç bilgilerini kontrol ediniz.");
                    }
                }
                else
                {
                    MessageBox.Show("Düzenlemek istediğiniz seferi seçmelisiniz.");
                }
                
            }
            catch
            {
                return;
            }
        }

        public bool sefer_düzenle_kontrol1 = false, sefer_düzenle_kontrol2 = false, sefer_düzenle_kontrol3 = false;
        public void sefer_düzenle_arac_bilgileri()
        {
            if (textBox23.Text != "" && textBox22.Text != "")
            {
                sefer_düzenle_kontrol1 = true;
            }
        }
        public void sefer_düzenle_gorevli_personel_bilgileri()
        {
            if (textBox24.Text != "" && textBox25.Text != "" && textBox26.Text != "" && textBox27.Text != "")
            {
                sefer_düzenle_kontrol2 = true;
            }

        }
    
        public void sefer_düzenle_sefer_bilgileri()
        {
            if (comboBox7.Text != "" && comboBox6.Text != "" && maskedTextBox6.Text != "" && maskedTextBox5.Text != "" && maskedTextBox4.Text != "")
            {
                sefer_düzenle_kontrol3 = true;
            }
        }
        public void sefer_düzenleme_ekrani_bosalt()
        {
            foreach (Control ctrl in groupBox11.Controls)
            {
                if (ctrl is TextBox)
                {
                    ctrl.Text = "";
                }
            }
            foreach (Control ctrl1 in groupBox12.Controls)
            {
                if (ctrl1 is TextBox)
                {
                    ctrl1.Text = "";
                }
            }
            foreach (Control ctrl2 in groupBox13.Controls)
            {
                if (ctrl2 is MaskedTextBox)
                {
                    ctrl2.Text = "";
                }
                if (ctrl2 is ComboBox)
                {
                    ctrl2.Text = "";
                }
                if (ctrl2 is DateTimePicker)
                {
                    ctrl2.Text = DateTime.Now.ToString();
                }
            }
        }
        public string sefer_duzenle_sefer_id = "";
        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                dateTimePicker4.MinDate = new DateTime(1985, 6, 20);
                sefer_duzenle_sefer_id = dataGridView2.Rows[e.RowIndex].Cells[0].Value.ToString();
                comboBox7.Text = dataGridView2.Rows[e.RowIndex].Cells[4].Value.ToString();
                comboBox6.Text = dataGridView2.Rows[e.RowIndex].Cells[5].Value.ToString();
                maskedTextBox6.Text = dataGridView2.Rows[e.RowIndex].Cells[2].Value.ToString();
                maskedTextBox5.Text = dataGridView2.Rows[e.RowIndex].Cells[3].Value.ToString();
                maskedTextBox4.Text = dataGridView2.Rows[e.RowIndex].Cells[8].Value.ToString();
                dateTimePicker4.Text = dataGridView2.Rows[e.RowIndex].Cells[1].Value.ToString();
                textBox27.Text = dataGridView2.Rows[e.RowIndex].Cells[9].Value.ToString();
                textBox26.Text = dataGridView2.Rows[e.RowIndex].Cells[10].Value.ToString();
                textBox25.Text = dataGridView2.Rows[e.RowIndex].Cells[11].Value.ToString();
                textBox24.Text = dataGridView2.Rows[e.RowIndex].Cells[12].Value.ToString();
                textBox23.Text = dataGridView2.Rows[e.RowIndex].Cells[7].Value.ToString();
                textBox22.Text = dataGridView2.Rows[e.RowIndex].Cells[6].Value.ToString();
            }
            catch
            {
                return;
            }
        }

        private void dateTimePicker4_ValueChanged(object sender, EventArgs e)
        {

            dateTimePicker4.MinDate = DateTime.Now;
        }

        private void dataGridView2_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dataGridView2.ClearSelection();
        }

        private void button55_Click(object sender, EventArgs e)
        {
            try
            {

                if (dataGridView2.SelectedRows.Count > 0)
                {
                    DialogResult dialogResult = MetroFramework.MetroMessageBox.Show(this, "\n Sefer silinecektir onaylıyor musunuz ?", "Uyarı", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                    if (dialogResult == DialogResult.Yes)
                    {
                        if (DB.baglanti_kontrol())
                        {
                            DB.baglanti.Open();
                            string sorgu = "DELETE FROM seferler WHERE sefer_id = " + sefer_duzenle_sefer_id + "";
                            SqlCommand myComm = new SqlCommand(sorgu, DB.baglanti);
                            SqlDataReader MyReader;
                            MyReader = myComm.ExecuteReader();
                            timer6.Start();
                            sefer_düzenle_grid_doldur();
                            sefer_düzenleme_ekrani_bosalt();
                            DB.baglanti.Close();
                        }
                        else
                        {
                            MetroFramework.MetroMessageBox.Show(this, "\n \t Lütfen Bağlantınızı Kontrol Edin..", "Bağlantı Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else if (dialogResult == DialogResult.No)
                    {
                        dataGridView2.ClearSelection();
                        sefer_düzenleme_ekrani_bosalt();
                    }
                }
                else
                {
                    MessageBox.Show("Silmek istediğiniz seferi seçmelisiniz.");
                }
            }
            catch
            {
                return;
            }
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            metroTabControl1.SelectedIndex = 0;
            tabControl1.SelectedIndex = 0;
            pictureBox3.Visible = false;
            pictureBox4.Visible = false;
            pictureBox5.Visible = false;
            pictureBox6.Visible = false;
            label15.Visible = false;
            pictureBox1.Visible = false;
            pictureBox8.Visible = true;
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            biletleri_listele();
            metroTabControl1.SelectedIndex = 3;
        }
        public void biletleri_listele()
        {
            try
            {
                if (DB.baglanti_kontrol())
                {

                    DB.baglanti.Open();
                    string sorgu = "select Biletler.bilet_id,Seferler.sefer_tarihi as 'Sefer Tarihi',Seferler.S_kalkis_yeri as 'Kalkış Yeri',Seferler.S_varis_yeri as 'Varış Yeri',Seferler.S_kalkis_saati as 'Kalkış Saati',Seferler.S_varis_saati as 'Varış Saati',Biletler.koltuk_no as 'Koltuk No',Biletler.satismirezervasyonmu as 'Yapılan İşlem',Seferler.bilet_tutari as 'Bilet Tutarı',Seferler.sofor_adi as 'Şoför Adı',Seferler.sofor_soyadi as 'Şoför Soyadı',Seferler.muavin_adi as 'Muavin Adı',Seferler.muavin_soyadi as 'Muavin Soyadı',Seferler.arac_plaka as 'Araç Plakası',Seferler.arac_marka as 'Araç Markası'  from Biletler INNER JOIN seferler ON seferler.sefer_id = Biletler.sefer_id where musteri_id = '" + musteri_id + "'";
                    SqlCommand komut = new SqlCommand(sorgu, DB.baglanti);
                    SqlDataAdapter da = new SqlDataAdapter(komut);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridView3.DataSource = dt;
                    dataGridView3.Columns[0].Visible = false;
                    DB.baglanti.Close();

                }
                else
                {
                    MetroFramework.MetroMessageBox.Show(this, "\n \t Lütfen Bağlantınızı Kontrol Edin..", "Bağlantı Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch
            {
                return;
            }
        }
        private void pictureBox5_Click(object sender, EventArgs e)
        {
            kul_adi_kontrol();
            profil_duzenle_musteri_bilgileri_doldur();
            metroTabControl1.SelectedIndex = 4;
        }
        public void profil_duzenle_musteri_bilgileri_doldur()
        {
            try
            {
                if (DB.baglanti_kontrol())
                {
                    DB.baglanti.Open();
                    string komut = "select * FROM Musteriler WHERE musteri_id = '" + musteri_id + "'";
                    SqlCommand cmd = new SqlCommand(komut, DB.baglanti);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        textBox34.Text = Convert.ToString(dt.Rows[0]["ad"]);
                        textBox33.Text = Convert.ToString(dt.Rows[0]["soyad"]);
                        textBox32.Text = Convert.ToString(dt.Rows[0]["kullanici_adi"]);
                        textBox31.Text = Convert.ToString(dt.Rows[0]["sifre"]);
                        textBox30.Text = Convert.ToString(dt.Rows[0]["sifre"]);
                        textBox29.Text = Convert.ToString(dt.Rows[0]["email"]);
                        maskedTextBox7.Text = Convert.ToString(dt.Rows[0]["telefon"]);
                        dateTimePicker5.Text = Convert.ToString(dt.Rows[0]["dogum_tarihi"]);
                    }
                }
                else
                {
                    MetroFramework.MetroMessageBox.Show(this, "\n Kullanıcı Bulunamadı..", "Kullanıcı Adınız Veya Şifreniz Hatalı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch
            {
                return;
            }
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            metroTabControl1.SelectedIndex = 0;
            tabControl1.SelectedIndex = 3;
        }

        private void dataGridView3_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dataGridView3.ClearSelection();
        }
        public string bilet_sil_bilet_id = "";
        private void dataGridView3_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                bilet_sil_bilet_id = dataGridView3.Rows[e.RowIndex].Cells[0].Value.ToString();
            }
            catch
            {
                return;
            }
            
        }

        private void button56_Click(object sender, EventArgs e)
        {
            try
            {

                if (dataGridView3.SelectedRows.Count > 0)
                {
                    DialogResult dialogResult = MetroFramework.MetroMessageBox.Show(this, "\n Sefer silinecektir onaylıyor musunuz ?", "Uyarı", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                    if (dialogResult == DialogResult.Yes)
                    {
                        if (DB.baglanti_kontrol())
                        {
                            DB.baglanti.Open();
                            string sorgu = "DELETE FROM Biletler WHERE bilet_id = " + bilet_sil_bilet_id + "";
                            SqlCommand myComm = new SqlCommand(sorgu, DB.baglanti);
                            SqlDataReader MyReader;
                            MyReader = myComm.ExecuteReader();
                            timer3.Start();
                            biletleri_listele();
                            dataGridView3.ClearSelection();
                            DB.baglanti.Close();
                        }
                        else
                        {
                            MetroFramework.MetroMessageBox.Show(this, "\n \t Lütfen Bağlantınızı Kontrol Edin..", "Bağlantı Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else if (dialogResult == DialogResult.No)
                    {
                        dataGridView3.ClearSelection();
                    }
                }
                else
                {
                    MessageBox.Show("Silmek istediğiniz bileti seçmelisiniz.");
                }
            }
            catch
            {
                return;
            }
            
        }
        public int bilet_sil_tiksay = 0;
        private void timer3_Tick(object sender, EventArgs e)
        {
            bilet_sil_tiksay++;
            if (bilet_sil_tiksay <= 4)
            {
                label57.Visible = true;
                label57.Text = "Güncelleme işleminiz gerçekleşmiştir.";
            }
            else
            {
                label57.Visible = false;
                timer3.Stop();
                bilet_sil_tiksay = 0;
            }
        }

        private void linkLabel5_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                    DialogResult dialogResult = MetroFramework.MetroMessageBox.Show(this, "\n Üyeliğiniz silinecektir onaylıyor musunuz ?", "Uyarı", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                    if (dialogResult == DialogResult.Yes)
                    {
                        if (DB.baglanti_kontrol())
                        {
                            DB.baglanti.Open();
                            string sorgu = "DELETE FROM Musteriler WHERE musteri_id = " + musteri_id + "";
                            SqlCommand myComm = new SqlCommand(sorgu, DB.baglanti);
                            SqlDataReader MyReader;
                            MyReader = myComm.ExecuteReader();
                            MessageBox.Show("Aramızdan ayrıldığınız için üzgünüz. :(");
                            metroTabControl1.SelectedIndex = 0;
                            tabControl1.SelectedIndex = 0;
                            pictureBox3.Visible = false;
                            pictureBox4.Visible = false;
                            pictureBox5.Visible = false;
                            pictureBox6.Visible = false;
                            label15.Visible = false;
                            DB.baglanti.Close();
                        }
                        else
                         {
                             MetroFramework.MetroMessageBox.Show(this, "\n \t Lütfen Bağlantınızı Kontrol Edin..", "Bağlantı Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
                         }
                    }
            }
            catch
            {
                return;
            }
        }
        public int profil_düzenle_tiksay = 0;
        private void timer4_Tick(object sender, EventArgs e)
        {
            profil_düzenle_tiksay++;
            if (profil_düzenle_tiksay <= 4)
            {
                pictureBox7.Image = Properties.Resources.basarili;
            }
            else
            {
                pictureBox7.Image = Properties.Resources.duzenle;
                timer4.Stop();
                profil_düzenle_tiksay = 0;
            }
        }

        private void button57_Click(object sender, EventArgs e)
        {
            
            try
            {
                if (profil_güncelle_kontrol == true)
                {
                    profil_güncelle_bosluk_var_mi();
                    if (profil_güncellenecek_mi)
                    {
                        if (DB.baglanti_kontrol())
                        {
                            DB.baglanti.Open();
                            string sorgu = "update Musteriler set ad = '" + textBox34.Text + "' ,soyad = '" + textBox33.Text + "' ,kullanici_adi = '" + textBox32.Text + "' ,sifre = '" + textBox30.Text + "' ,email = '" + textBox29.Text + "' ,telefon = '" + maskedTextBox7.Text + "' ,dogum_tarihi = '" + dateTimePicker5.Value.Date.ToString("yyyy-MM-dd") + "'  where musteri_id = '" + musteri_id + "' ";
                            SqlCommand komut = new SqlCommand(sorgu, DB.baglanti);
                            komut.ExecuteNonQuery();
                            timer4.Start();
                            DB.baglanti.Close();
                        }
                        else
                        {
                            MetroFramework.MetroMessageBox.Show(this, "\n \t Lütfen Bağlantınızı Kontrol Edin..", "Bağlantı Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Boş alan kalmamalıdır.");
                    }
                    
                }
                else
                {
                    MessageBox.Show("Kullanıcı adı kullanılmaktadır !");
                }
            }
            catch
            {
                return;
            }
           
        }
        public bool profil_güncellenecek_mi = false;
        public void profil_güncelle_bosluk_var_mi()
        {
            if (textBox34.Text != "" && textBox33.Text != "" && textBox32.Text != "" && textBox31.Text != "" && textBox30.Text != "" && textBox29.Text != "" && maskedTextBox7.Text != "")
            {
                profil_güncellenecek_mi = true;
            }
        }
        public bool profil_güncelle_kontrol = true;
        private void textBox32_TextChanged(object sender, EventArgs e)
        {
            if (dt.Rows.Count > 0)
            {
                if(kullanicinin_adi != textBox32.Text)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows[i]["kullanici_adi"].ToString().ToUpper().Trim() == textBox32.Text.ToUpper() && textBox32.Text != "")
                        {
                            label70.Visible = true;
                            profil_güncelle_kontrol = false;

                            i = dt.Rows.Count;
                        }
                        else
                        {
                            label70.Visible = false;
                            profil_güncelle_kontrol = true;
                        }
                    }
                }
                
            }
        }

        private void textBox31_TextChanged(object sender, EventArgs e)
        {
            if (textBox30.Text != "" && textBox31.Text != "")
            {
                if (textBox30.Text != textBox31.Text)
                {
                    label61.Visible = true;
                }
                else
                {
                    label61.Visible = false;
                }
            }
        }

        private void textBox30_TextChanged(object sender, EventArgs e)
        {
            if (textBox30.Text != "" && textBox31.Text != "")
            {
                if (textBox30.Text != textBox31.Text)
                {
                    label61.Visible = true;
                }
                else
                {
                    label61.Visible = false;
                }
            }
        }

        private void textBox28_KeyUp(object sender, KeyEventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView2.Rows)
            {
                string s = row.Cells[1].Value.ToString();
                if (!s.StartsWith(textBox28.Text, true, null))
                {
                    CurrencyManager currencyManager1 = (CurrencyManager)BindingContext[dataGridView2.DataSource];
                    currencyManager1.SuspendBinding();
                    row.Visible = false;
                    currencyManager1.ResumeBinding();
                }
                else
                {
                    row.Visible = true;
                }
            }
        }

        private void textBox10_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button44_Click((object)sender, (EventArgs)e);
            }
        }
        public int sefer_sil_tiksay = 0;
        private void timer6_Tick(object sender, EventArgs e)
        {
            sefer_sil_tiksay++;
            if (sefer_sil_tiksay <= 4)
            {
                label71.Visible = true;
                label71.Text = "Silme işleminiz gerçekleşmiştir.";
            }
            else
            {
                label71.Visible = false;
                timer6.Stop();
                sefer_sil_tiksay = 0;
            }
           
        }

        private void textBox9_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button44_Click((object)sender, (EventArgs)e);
            }
        }

        private void txt_Adi_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button46_Click((object)sender, (EventArgs)e);
            }
        }

        private void textBox11_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button45_Click((object)sender, (EventArgs)e);
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                metroButton2_Click((object)sender, (EventArgs)e);
            }
        }

        private void textBox34_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button57_Click((object)sender, (EventArgs)e);
            }
        }

        private void button58_Click(object sender, EventArgs e)
        {
            try
            {
                if(maskedTextBox8.Text != "")
                {
                    if (DB.baglanti_kontrol())
                    {
                        DB.baglanti.Open();
                        string sorgu = "update Rezervasyonislemzamani set kac_gun='" + maskedTextBox8.Text + "' ";
                        SqlCommand komut = new SqlCommand(sorgu, DB.baglanti);
                        komut.ExecuteNonQuery();
                        timer5.Start();
                        DB.baglanti.Close();
                    }
                    else
                    {
                        MetroFramework.MetroMessageBox.Show(this, "\n \t Lütfen Bağlantınızı Kontrol Edin..", "Bağlantı Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MetroFramework.MetroMessageBox.Show(this, "\n Lütfen Kaç Saat Önce Bileti Alması Gerektiğini Belirtiniz !", "Eksik Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                
            }
            catch
            {
                return;
            }
        }
        public int rezervasyon_saat_onceligi = 0;
        private void timer5_Tick(object sender, EventArgs e)
        {
            rezervasyon_saat_onceligi++;
            if (rezervasyon_saat_onceligi <= 4)
            {
                label77.Visible = true;
                label77.Text = "Güncelleme işleminiz gerçekleşmiştir.";
            }
            else
            {
                label77.Visible = false;
                timer5.Stop();
                rezervasyon_saat_onceligi = 0;
            }
        }

        public int sefer_düzenle_tiksay = 0;
        private void timer2_Tick(object sender, EventArgs e)
        {
            sefer_düzenle_tiksay++;
            if (sefer_düzenle_tiksay <= 4)
            {
                label56.Visible = true;
                label56.Text = "Güncelleme işleminiz gerçekleşmiştir.";
            }
            else
            {
                label56.Visible = false;
                timer2.Stop();
                sefer_düzenle_tiksay = 0;
            }
        }

        public void sefer_ekle_gorevli_personel_bilgileri()
        {
            if (textBox18.Text != "" && textBox19.Text != "" && textBox20.Text != "" && textBox21.Text != "")
            {
                kontrol2 = true;
            }

        }
        public void sefer_ekle_sefer_bilgileri()
        {
            if (comboBox3.Text != "" && comboBox5.Text != "" && maskedTextBox1.Text != "" && maskedTextBox2.Text != "" && maskedTextBox3.Text != "")
            {
                kontrol3 = true;
            }
        }
        public int tik_say = 1;
        private void timer1_Tick(object sender, EventArgs e)
        {
            tik_say++;
            if(tik_say <= 4)
            {
                label55.Visible = true;
                label55.Text = "Kayıt işleminiz gerçekleşmiştir.";
            }
            else
            {
                label55.Visible = false;
                timer1.Stop();
                tik_say = 0;
            }
        }
        public void sefer_ekleme_ekrani_bosalt()
        {
            foreach (Control ctrl in groupBox8.Controls)
            {
                if (ctrl is TextBox)
                {
                    ctrl.Text = "";
                }
            }
            foreach (Control ctrl1 in groupBox9.Controls)
            {
                if (ctrl1 is TextBox)
                {
                    ctrl1.Text = "";
                }
            }
            foreach (Control ctrl2 in groupBox10.Controls)
            {
                if (ctrl2 is MaskedTextBox)
                {
                    ctrl2.Text = "";
                }
                if (ctrl2 is ComboBox)
                {
                    ctrl2.Text = "";
                }
                if (ctrl2 is DateTimePicker)
                {
                    ctrl2.Text = DateTime.Now.ToString();
                }
            }
        }



    }
}
