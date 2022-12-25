using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.IO;
using System.IO.Ports;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Guna.UI2.WinForms.Suite;

namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        SerialPort serialPort5;
        string port; 
        string[] batarya_veri;
        string[] sicaklik_veri;
        string veri;
        string konum_veri;
        string gerilim_veri; //toplam batarya gerilimi
        string enerji_veri;  //kalan enerji miktari
        //en yuksek sicaklik
        

        int cpt = 1;
        private void Form1_Load(object sender, EventArgs e)
        {
           // textBox1.ReadOnly= true;
           // sicakliktextBox1.ReadOnly = true;
           // hizTextBox1.ReadOnly= true;
           // MotortextBox2.ReadOnly= true;
            Control.CheckForIllegalCrossThreadCalls = false; //hata ayıklanırken çağrıların yakalanıp yakalanmadığını anlar
            string[] portlar = SerialPort.GetPortNames();

            foreach (string prt in portlar)
            {
                comboBox1.Items.Add(prt);
            }


            textBox1.Hide();

            gunaDataGridView1.Rows.Add(4);
            gunaDataGridView1.Rows[0].Cells[0].Value = Image.FromFile("C:\\Users\\AktasSule\\Downloads\\1.png");
            gunaDataGridView1.Rows[1].Cells[0].Value = Image.FromFile("C:\\Users\\AktasSule\\Downloads\\2.png");
            gunaDataGridView1.Rows[2].Cells[0].Value = Image.FromFile("C:\\Users\\AktasSule\\Downloads\\3.png");
            gunaDataGridView1.Rows[3].Cells[0].Value = Image.FromFile("C:\\Users\\AktasSule\\Downloads\\4.png");
        }

        private void gunaCircleButton1_Click(object sender, EventArgs e)
        {
            if (cpt < gunaDataGridView1.Rows.Count)
            {
                gunaPictureBox1_car1.Image = (Image)gunaDataGridView1.Rows[cpt - 1].Cells[0].Value;
                cpt--;
            }
            else 
            {
                cpt = 1;
            }
        }

        private void gunaCircleButton2_Click(object sender, EventArgs e)
        {
            if(cpt < gunaDataGridView1.Rows.Count)
            {
                gunaPictureBox1_car1.Image= (Image)gunaDataGridView1.Rows[cpt].Cells[0].Value;
                cpt++;
            }
            else
            {
                cpt = 1;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            timer1.Start();

            try
            {
                port = comboBox1.SelectedItem.ToString();
                serialPort5 = new SerialPort(port);
                serialPort5.DataReceived += new SerialDataReceivedEventHandler(serialPort1_DataReceived);
                /*Bu event ile seri portu dinleyerek porta veri geldiğinde gelen veriyi alarak *_veri değişkenlerine eşitleyeceğiz. */
                serialPort5.Open();
                comboBox1.Text = port;
                comboBox1.Enabled = false;
                button1.Enabled = false;

            }
            catch
            {
             /* "try{}" butona tıklandığında gerçekleşmesini istediğimiz kod satırlarını; 
                “catch{}” ise bir hata türü ile karşılaşıldığında gerçekleşmesini istediğimiz komutları barındırıyor.*/
            }

            label13.Show();
            textBox2.Show();
            textBox1.Show();
            MotortextBox2.Show();
           
        }
        private void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {

            string zaman = DateTime.Now.ToString();

            textBox5.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox2.Clear();
            label13.Text= " ";

            veri = serialPort5.ReadLine(); // serialPort üstünden gelen veriyi okuması için

            if (veri.StartsWith("*")) // string tipinde bir değişkenin içeriğiyle başlayıp başlamadığı belirlenir.
            {

                string[] okuma = veri.Split('*');

                 /* konum_veri = okuma[1];
                   hiz_veri = okuma[5];
                   enerji_veri = okuma[3];
                   sicaklik_veri = okuma[2];
                   gerilim_veri = okuma[3];

                   */


                string SOC = okuma[26];
                string SOH = okuma[27];
                string hiz_veri = okuma[28];

                textBox1.Text += zaman;
               
                for (int i = 1; i < 20; i++)
                {
                    textBox5.Text += okuma[i];
                }
                for (int i = 20; i < 26; i++)
                {
                    label13.Text += okuma[i];
                }

                textBox3.Text = okuma[26];
                textBox4.Text = okuma[27];
                textBox2.Text = okuma[28];
                
                for (int i = 1; i < 29; i++)
                {
                    textBox1.Text += okuma[i];
                }

            }

            //String dosya_yolu = @"C:\Users\AktasSule\Desktop\yaz.txt";
            //FileStream fs = new FileStream(dosya_yolu, FileMode.OpenOrCreate, FileAccess.Write);

            //StreamWriter yaz = new StreamWriter(fs);
            //yaz.WriteLine(textBox1.Text);


            /*   textBox1.Text += DateTime.Now.ToString() + "        " + veri + "\n";                        //Gelen veriyi textBox içine güncel zaman ile ekle
                 string filelocation = @"C:\Users\AktasSule\Desktop\";                                       //Dosyanın kaydedileceği konumu belirliyoruz
                 string filename = "yaz.txt";                                                                //Kaydedilecek dosyanın ismi
                 System.IO.File.WriteAllText(filelocation + filename, "Zaman\t\t\tDeğer\n" + textBox1.Text); //Dosya konumuna textBox1 üstündeki verilerden oluşan text dosyamızı kaydediyoruz
                 MessageBox.Show("Dosya başarıyla kaydedildi", "Mesaj"); */

            System.IO.File.WriteAllText(@"C:\Users\AktasSule\Desktop\yaz.txt", textBox1.Text); //dosyaya yazdirma 
            textBox1.Select(textBox1.TextLength, 0);
            textBox1.ScrollToCaret();
        }


        int zaman2 = 1;
        private void timer1_Tick(object sender, EventArgs e)
        {
            label7.Text= DateTime.Now.ToLongTimeString();
           
            timer1.Interval= 100;
            gunaProgressBar1.Value= zaman2;
            zaman2++;
            if(zaman2 > 100)
            {
                timer1.Stop();
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            serialPort5.Close();         //Seri Portu kapa
            button3.Enabled = false;     //Durdurma butonunu pasif hale getir
            button1.Enabled = true;      //Başlatma butonunu aktif hale getir
        }


        private void button3_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            label13.Text = " ";
            textBox2.Clear();
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {
            timer1.Start();
            label7.Text = DateTime.Now.ToLongTimeString();
            label7.Text = DateTime.Now.ToLongDateString();
        }

        private void enbuyuksicakliktextBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
