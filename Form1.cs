using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BaiTapLon
{
    public partial class GamePlay_UI : Form
    {
        public GamePlay_UI()
        {
            InitializeComponent();
        }
        private System.Windows.Forms.Timer aTimer;
        SoundPlayer sp = new SoundPlayer();
        private int counter;// thời gian đếm
        List<List<PictureBox>> mt;
        int time = 300;//thời gian đếm ngược(mặc định=300s:độ khó dễ)
        int y = 10;
        private void Start_Click(object sender, EventArgs e)
        {
            textBox2.Text = "0";           
            //bộ đếm thời gian
            counter = time;//thời gian đếm ngược
            mt = new List<List<PictureBox>>();
            TABLE.Controls.Clear();//xóa trắng bảng chọn(tránh chồng dối tượng)
            aTimer = new System.Windows.Forms.Timer();//khởi tạo đối tượng đếm
            aTimer.Tick += new EventHandler(aTimer_Tick);
            aTimer.Interval = 1000; // thời gian đếm bằng 1s(1000ms)
            aTimer.Start();//bắt đầu bộ đếm
            textBox1.Text = counter.ToString();//chuyển tg đếm lên form qua textbox
            //-------------------------------------------------------------------------------------------------------
            //Sắp xếp ảnh ngẫu nhiên trong bảng chọn ảnh
            Random random = new Random();//biến tạo ngẫu nhiên
            int[] anh = new int[36];//mảng lưu hình
            int[] dem = new int[18];//mảng lưu bộ đếm số lần xh của từng hình
            int k = 0;
            for (int i = 0; i < 18; i++) dem[i] = 0;
            while (k < 36)
            {
                int x = random.Next(0, 17);//random 1 số từ 0-17
                if (dem[x] < 2)//nếu số lần xh của số vừa random nhỏ hơn 2 thì thực hiện công việc
                {
                    dem[x]++;//tăng số lần xh thêm 1
                    anh[k] = x + 1;//thêm số random vào mảng ảnh
                    k++;//tăng chỉ số của mảng
                }
                else
                    x = random.Next(0, 17);//random lại 1 số khác(số vừa random đã xh 2 lần)
                if (k > 20)//khi k lớn hơn 20 thực hiện việc sau
                {
                    for (int j = 0; j < 18; j++)//chạy số từ 0-17
                        if (dem[j] < 2)//nếu số lần xh nhỏ hơn 2 thì thêm vào mảng ảnh
                        {
                            dem[j]++;
                            anh[k] = j + 1;
                            k++;
                        }
                }
            }
            k = 0;          
            //thiết lập nhóm hình ảnh vào bảng
            PictureBox t1 = new PictureBox() { Width = 91, Height = 77, Location = new Point(3, 3) };//picture box gốc
            for (int i = 0; i < 6; i++)
            {
                mt.Add(new List<PictureBox>());//thêm picture box mới vào list
                for (int j = 0; j < 6; j++)
                {
                    //thiết lập các button và thuộc tính của từng picture box
                    PictureBox pb = new PictureBox()
                    {
                        Width = 91,//thiết lập width
                        Height = 77,//thiết lập height
                        
                        Location = new Point(t1.Location.X + i * Width, t1.Location.Y + j * Height),//thiết lập vị trí của button
                        BackgroundImageLayout = ImageLayout.Stretch,//định dạng ảnh nền
                        BackgroundImage = Image.FromFile(Application.StartupPath + "\\Resources\\" + Convert.ToString(anh[k]) + ".jpg"),//thiết lập ảnh nền(ảnh cần so sánh)
                        Image = Image.FromFile(Application.StartupPath + "\\Resources\\back.jpg"),//thiết lập ảnh che hình
                        Tag = anh[k]
                    };
                    k++;
                    pb.Click += Pb_Click;
                    TABLE.Controls.Add(pb);//thêm picture box đã thiết lập vào bảng chọn
                }
            }
        }
        Boolean kt = true;
        int m = 20, n = 21;
        PictureBox t2 = new PictureBox();
        PictureBox t3 = new PictureBox();
        //quy tắc trò chơi
        private void Pb_Click(object sender, EventArgs e)
        {
            PictureBox pb = sender as PictureBox;
            pb.Image = null;
            if (kt == true)
            {
                t2 = pb;
                kt = false;
            }
            else if(pb.Location != t2.Location)
            {
                t3 = pb;
                kt = false;
            }
            else
            {
                kt = false;
            }
            if ((kt==false) )
            {
                if (t2 != null && t3 != null)
                {
                    m = int.Parse(t2.Tag.ToString());
                    n = int.Parse(t3.Tag.ToString());
                    TimeBreak.Start();
                }            
            }           
        }
        private void aTimer_Tick(object sender, EventArgs e)
        {
            counter--;
            if (counter < 0)
                aTimer.Stop();
            else
                textBox1.Text = counter.ToString();

        }
        //nút exit
        private void Exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        //nút hướng dẫn
        private void button1_Click(object sender, EventArgs e)
        {
            string rule = "QUY TẮC TRÒ CHƠI:\n"
                +"Người chơi chọn hình trên bảng hình, nếu hình giống nhau sẽ biến mất,còn không sẽ bị che lại\n"
                +" Nhiệm vụ của ng chơi là làm biến mất tất cả hình trong thời gian giới hạn\n Chúc các bạn chơi game vui vẻ!!";
            MessageBox.Show(rule);
        }
        //__________________________________________________________________________________
        //nhóm phím chọn độ khó, sau khi chọn tự động bắt đầu game, độ khó mặc định là dễ
        private void dễToolStripMenuItem_Click(object sender, EventArgs e){time = 300;Start_Click(null,null);y = 10;}        
        private void vừaToolStripMenuItem_Click(object sender, EventArgs e){time = 200; Start_Click(null, null);y = 20;}
        private void khóToolStripMenuItem_Click(object sender, EventArgs e){time = 100; Start_Click(null, null);y = 40;}
        //show messbox báo đã thua do hết giờ, đồng thời xóa màn hình trò chơi
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (int.Parse(textBox1.Text) == 0)
            {
                aTimer.Stop();//ngừng bộ đếm khi tg bằng 0
                MessageBox.Show("Bạn Thua!!");
                TABLE.Controls.Clear();
            }
        }
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (int.Parse(textBox2.Text) == 360)
            {
                aTimer.Stop();
                textBox2.Text = Convert.ToString(int.Parse(textBox2.Text) + y * int.Parse(textBox1.Text));
                MessageBox.Show("Bạn Thắng!!\nĐiểm số của bạn là: " + textBox2.Text);
                TABLE.Controls.Clear();
            }
        }
        private void TimeBreak_Tick(object sender, EventArgs e)
        {
            if(m==n)
            {
                t2.BackgroundImage = null;
                t3.BackgroundImage = null;
                textBox2.Text = Convert.ToString(int.Parse(textBox2.Text) + 20);
                t2 = null;
                t3 = null;
                kt = true;
            }
            else
            {
                kt = true;
                t3.Image = Image.FromFile(Application.StartupPath + "\\Resources\\back.jpg");
                t2.Image = Image.FromFile(Application.StartupPath + "\\Resources\\back.jpg");
                t2 = null; t3 = null;
            }
            TimeBreak.Stop();           
        }
    }
    
}
