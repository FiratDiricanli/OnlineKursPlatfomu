using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Online_Kurs_Platformu
{
    public class Egitmen
    {
        [DisplayName("Eğitmen Adı")]
        public string ad { get; set; }

        [DisplayName("Uzmanlık Alanı")]
        public string uzmanlik { get; set; }

        public override string ToString() => ad;
    }

    public class Ogrenci
    {
        [DisplayName("Öğrenci ID")]
        public int ogrenci_id { get; set; }

        [DisplayName("Ad Soyad")]
        public string ad { get; set; }

        [DisplayName("E-Posta Adresi")]
        public string email { get; set; }

        [Browsable(false)]
        public List<Kurs> kayitli_kurslar { get; set; } = new List<Kurs>();

        public List<Kurs> kurs_listesi()
        {
            return kayitli_kurslar;
        }

        public override string ToString() => ad;
    }

    public class Kurs
    {
        [DisplayName("Kurs ID")]
        public int kurs_id { get; set; }

        [DisplayName("Kurs Adı")]
        public string kurs_adi { get; set; }

        [DisplayName("Eğitmen")]
        public string egitmen { get; set; }

        [DisplayName("Kalan Kontenjan")]
        public int kontenjan { get; set; }

        [Browsable(false)]
        public List<Ogrenci> sinif_listesi { get; set; } = new List<Ogrenci>();

        public bool ogrenci_kaydet(Ogrenci ogrenci)
        {
            if (kontenjan > 0 && !sinif_listesi.Contains(ogrenci))
            {
                sinif_listesi.Add(ogrenci);
                ogrenci.kayitli_kurslar.Add(this);
                kontenjan--;
                return true;
            }
            return false;
        }

        public override string ToString() => kurs_adi;
    }

    public class KayitIslemi
    {
        [DisplayName("Kayıt ID")]
        public int islem_id { get; set; }

        [DisplayName("Öğrenci")]
        public string ogrenci_ad { get; set; }

        [DisplayName("Kurs Adı")]
        public string kurs_ad { get; set; }

        [DisplayName("İşlem Tarihi")]
        public string tarih { get; set; }
    }

    public partial class Form1 : Form
    {
        List<Egitmen> egitmenler = new List<Egitmen>();
        List<Kurs> kurslar = new List<Kurs>();
        List<Ogrenci> ogrenciler = new List<Ogrenci>();
        List<KayitIslemi> kayitGecmisi = new List<KayitIslemi>();

        int ogrenciSayac = 2001;
        int kursSayac = 101;
        int islemSayac = 50001;

        TabControl sekmeler;
        TabPage sekmeKurs, sekmeOgrenci, sekmeKayit;
        DataGridView dgvKurslar, dgvOgrenciler, dgvKayitlar;
        ListBox lstOgrencininKurslari;
        ComboBox cmbEgitmenSec, cmbOgrenciSec, cmbKursSec;
        TextBox txtKursAd, txtKontenjan, txtOgrenciAd, txtOgrenciEmail;

        public Form1()
        {
            this.Text = "Online Kurs Platformu Yönetim Sistemi - 2300005412 Fırat Diricanlı";
            this.Size = new Size(1150, 750);
            this.StartPosition = FormStartPosition.CenterScreen;

            SistemVerileriniHazirla();
            ArayuzuInsaEt();
        }

        private void SistemVerileriniHazirla()
        {
            egitmenler.Add(new Egitmen { ad = "Prof. Dr. Erdem Yücesan", uzmanlik = "Yazılım Mühendisliği" });
            egitmenler.Add(new Egitmen { ad = "Prof. Dr. Ayşe Şahin", uzmanlik = "Veri Bilimi" });
            egitmenler.Add(new Egitmen { ad = "Prof. Dr. Engin Demiroğ", uzmanlik = "Siber Güvenlik" });
            egitmenler.Add(new Egitmen { ad = "Prof. Dr. Selim Akın", uzmanlik = "UI/UX Tasarımı" });
            egitmenler.Add(new Egitmen { ad = "Prof. Dr. Caner Yıldırım", uzmanlik = "Mobil Uygulama Geliştirme" });

            kurslar.Add(new Kurs { kurs_id = 101, kurs_adi = "Nesne Tabanlı Programlama", egitmen = egitmenler[0].ad, kontenjan = 30 });
            kurslar.Add(new Kurs { kurs_id = 102, kurs_adi = "Python ile Makine Öğrenmesi", egitmen = egitmenler[1].ad, kontenjan = 25 });
            kurslar.Add(new Kurs { kurs_id = 103, kurs_adi = "Etik Hacker Eğitimi", egitmen = egitmenler[2].ad, kontenjan = 20 });
            kurslar.Add(new Kurs { kurs_id = 104, kurs_adi = "İleri Seviye UI/UX Tasarımı", egitmen = egitmenler[3].ad, kontenjan = 15 });
            kurslar.Add(new Kurs { kurs_id = 105, kurs_adi = "Flutter ile İleri Seviye Mobil Uygulama Geliştirme", egitmen = egitmenler[4].ad, kontenjan = 40 });

            ogrenciler.Add(new Ogrenci { ogrenci_id = 2000, ad = "Fırat Diricanlı", email = "firat@ogr.edu.tr" });
        }

        private void ArayuzuInsaEt()
        {
            sekmeler = new TabControl { Dock = DockStyle.Fill, Font = new Font("Segoe UI", 10, FontStyle.Bold) };

            sekmeKurs = new TabPage("Kurs Yönetimi");
            sekmeOgrenci = new TabPage("Öğrenci İşlemleri");
            sekmeKayit = new TabPage("Kayıt ve Raporlama");

            Panel pnlKurs = new Panel { Dock = DockStyle.Top, Height = 90, BackColor = Color.WhiteSmoke };
            txtKursAd = new TextBox { Location = new Point(20, 30), Width = 200, PlaceholderText = "Eklenecek Kurs Adı" };

            // DropDownStyle.DropDown yapıldı, böylece kullanıcı hem seçebilir hem metin girebilir.
            cmbEgitmenSec = new ComboBox { Location = new Point(240, 28), Width = 180, DropDownStyle = ComboBoxStyle.DropDown };

            txtKontenjan = new TextBox { Location = new Point(440, 30), Width = 100, PlaceholderText = "Kontenjan" };
            Button btnKursEkle = new Button { Text = "KURS OLUŞTUR", Location = new Point(560, 28), Size = new Size(160, 32), BackColor = Color.SteelBlue, ForeColor = Color.White };
            btnKursEkle.Click += (s, e) => KursKaydet();

            dgvKurslar = TabloOlustur();
            pnlKurs.Controls.AddRange(new Control[] { txtKursAd, cmbEgitmenSec, txtKontenjan, btnKursEkle });
            sekmeKurs.Controls.Add(dgvKurslar);
            sekmeKurs.Controls.Add(pnlKurs);

            Panel pnlOgrenci = new Panel { Dock = DockStyle.Top, Height = 90, BackColor = Color.WhiteSmoke };
            txtOgrenciAd = new TextBox { Location = new Point(20, 30), Width = 200, PlaceholderText = "Öğrenci Ad Soyad" };
            txtOgrenciEmail = new TextBox { Location = new Point(240, 30), Width = 200, PlaceholderText = "Kurumsal E-Posta" };
            Button btnOgrenciEkle = new Button { Text = "ÖĞRENCİ KAYDET", Location = new Point(460, 28), Size = new Size(180, 32), BackColor = Color.SeaGreen, ForeColor = Color.White };
            btnOgrenciEkle.Click += (s, e) => OgrenciKaydet();

            dgvOgrenciler = TabloOlustur();
            pnlOgrenci.Controls.AddRange(new Control[] { txtOgrenciAd, txtOgrenciEmail, btnOgrenciEkle });
            sekmeOgrenci.Controls.Add(dgvOgrenciler);
            sekmeOgrenci.Controls.Add(pnlOgrenci);

            Panel pnlKayit = new Panel { Dock = DockStyle.Top, Height = 180, BackColor = Color.WhiteSmoke };

            Label l1 = new Label { Text = "Öğrenci Seçimi:", Location = new Point(20, 20), AutoSize = true };
            cmbOgrenciSec = new ComboBox { Location = new Point(140, 18), Width = 250, DropDownStyle = ComboBoxStyle.DropDownList };
            cmbOgrenciSec.SelectedIndexChanged += OgrenciSecimiDegisti;

            Label l2 = new Label { Text = "Kurs Seçimi:", Location = new Point(20, 60), AutoSize = true };
            cmbKursSec = new ComboBox { Location = new Point(140, 58), Width = 250, DropDownStyle = ComboBoxStyle.DropDownList };

            Button btnKayitYap = new Button { Text = "SEÇİLİ KURSA KAYIT YAP", Location = new Point(140, 100), Size = new Size(250, 45), BackColor = Color.Indigo, ForeColor = Color.White };
            btnKayitYap.Click += (s, e) => KursaKayitIslemi();

            Label l3 = new Label { Text = "Seçili Öğrencinin Aldığı Kurslar:", Location = new Point(450, 20), AutoSize = true, ForeColor = Color.DarkSlateGray };
            lstOgrencininKurslari = new ListBox { Location = new Point(450, 45), Size = new Size(350, 100) };

            dgvKayitlar = TabloOlustur();
            pnlKayit.Controls.AddRange(new Control[] { l1, cmbOgrenciSec, l2, cmbKursSec, btnKayitYap, l3, lstOgrencininKurslari });
            sekmeKayit.Controls.Add(dgvKayitlar);
            sekmeKayit.Controls.Add(pnlKayit);

            sekmeler.TabPages.AddRange(new TabPage[] { sekmeKurs, sekmeOgrenci, sekmeKayit });
            this.Controls.Add(sekmeler);

            TablolariGuncelle();
        }

        private DataGridView TabloOlustur()
        {
            return new DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = Color.White,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                AllowUserToAddRows = false,
                ReadOnly = true,
                RowHeadersVisible = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };
        }

        private void TablolariGuncelle()
        {
            dgvKurslar.DataSource = null; dgvKurslar.DataSource = kurslar.ToList();
            dgvOgrenciler.DataSource = null; dgvOgrenciler.DataSource = ogrenciler.ToList();
            dgvKayitlar.DataSource = null; dgvKayitlar.DataSource = kayitGecmisi.ToList();

            cmbOgrenciSec.Items.Clear();
            cmbKursSec.Items.Clear();
            cmbEgitmenSec.Items.Clear();

            foreach (var o in ogrenciler) cmbOgrenciSec.Items.Add(o);
            foreach (var k in kurslar) cmbKursSec.Items.Add(k);
            foreach (var e in egitmenler) cmbEgitmenSec.Items.Add(e);
        }

        private void KursKaydet()
        {
            string egitmenMetni = cmbEgitmenSec.Text;

            if (!string.IsNullOrWhiteSpace(txtKursAd.Text) && !string.IsNullOrWhiteSpace(egitmenMetni) && int.TryParse(txtKontenjan.Text, out int kont))
            {
               
                Egitmen secilenEgitmen = egitmenler.FirstOrDefault(e => e.ad.Equals(egitmenMetni, StringComparison.OrdinalIgnoreCase));

                if (secilenEgitmen == null)
                {
                    secilenEgitmen = new Egitmen { ad = egitmenMetni, uzmanlik = "Belirtilmedi" };
                    egitmenler.Add(secilenEgitmen);
                }

                kurslar.Add(new Kurs
                {
                    kurs_id = kursSayac++,
                    kurs_adi = txtKursAd.Text,
                    egitmen = secilenEgitmen.ad,
                    kontenjan = kont
                });

                txtKursAd.Clear(); txtKontenjan.Clear(); cmbEgitmenSec.Text = "";
                TablolariGuncelle();
                MessageBox.Show("Yeni kurs başarıyla sisteme tanımlanmıştır.", "İşlem Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Lütfen geçerli bir kurs adı, eğitmen bilgisi ve sayısal bir kontenjan değeri giriniz.", "Eksik Veri", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void OgrenciKaydet()
        {
            if (!string.IsNullOrWhiteSpace(txtOgrenciAd.Text) && !string.IsNullOrWhiteSpace(txtOgrenciEmail.Text))
            {
                ogrenciler.Add(new Ogrenci
                {
                    ogrenci_id = ogrenciSayac++,
                    ad = txtOgrenciAd.Text,
                    email = txtOgrenciEmail.Text
                });

                txtOgrenciAd.Clear(); txtOgrenciEmail.Clear();
                TablolariGuncelle();
                MessageBox.Show("Öğrenci kaydı başarıyla oluşturulmuştur.", "İşlem Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Öğrenci adı ve e-posta alanları boş bırakılamaz.", "Eksik Veri", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void KursaKayitIslemi()
        {
            if (cmbOgrenciSec.SelectedItem is Ogrenci seciliOgrenci && cmbKursSec.SelectedItem is Kurs seciliKurs)
            {
                if (seciliKurs.sinif_listesi.Contains(seciliOgrenci))
                {
                    MessageBox.Show("Bu öğrenci seçili kursa zaten kayıtlı durumdadır.", "Mükerrer Kayıt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                bool kayitBasarili = seciliKurs.ogrenci_kaydet(seciliOgrenci);

                if (kayitBasarili)
                {
                    kayitGecmisi.Add(new KayitIslemi
                    {
                        islem_id = islemSayac++,
                        ogrenci_ad = seciliOgrenci.ad,
                        kurs_ad = seciliKurs.kurs_adi,
                        tarih = DateTime.Now.ToString("dd.MM.yyyy HH:mm")
                    });

                    TablolariGuncelle();
                    OgrenciSecimiDegisti(null, null);
                    MessageBox.Show("Öğrencinin kurs kaydı başarıyla tamamlanmış ve kontenjan güncellenmiştir.", "Kayıt Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Seçili kursun kontenjanı dolduğu için kayıt işlemi gerçekleştirilememiştir.", "Kontenjan Dolu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Kayıt işlemi için lütfen bir öğrenci ve kurs seçimi yapınız.", "Seçim Yapılmadı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void OgrenciSecimiDegisti(object sender, EventArgs e)
        {
            lstOgrencininKurslari.Items.Clear();
            if (cmbOgrenciSec.SelectedItem is Ogrenci seciliOgrenci)
            {
                List<Kurs> alinanKurslar = seciliOgrenci.kurs_listesi();
                foreach (var kurs in alinanKurslar)
                {
                    lstOgrencininKurslari.Items.Add(kurs.kurs_adi);
                }

                if (alinanKurslar.Count == 0)
                {
                    lstOgrencininKurslari.Items.Add("Kayıtlı olunan bir kurs bulunmamaktadır.");
                }
            }
        }
    }
}