using AlarmToWork.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AlarmToWork
{
    public partial class Form1 : Form
    {
        private System.Timers.Timer timer;
        private bool isConfirmed = false;
        private int notificationInterval = 15;
        public Form1()
        {
            InitializeComponent();
            this.MinimizeBox = false;
            this.MaximizeBox = false;

            // Lấy đường dẫn thư mục gốc của ứng dụng
            string appDirectory = AppDomain.CurrentDomain.BaseDirectory;

            // Xây dựng đường dẫn tương đối tới hình ảnh
            string relativePath = Path.Combine("Resources", "200w.gif");
            string imagePath = Path.Combine(appDirectory, relativePath);
            // Kiểm tra xem tập tin hình ảnh có tồn tại không
            if (System.IO.File.Exists(imagePath))
            {
                // Tạo một đối tượng hình ảnh từ đường dẫn
                Image image = Image.FromFile(imagePath);

                // Đặt hình ảnh vào PictureBox
                pictureBox1.Image = image;
                pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            }


            this.FormClosing += Form1_FormClosing;
 
            // Xây dựng đường dẫn tương đối tới tệp âm thanh
            string relativePathSound = Path.Combine("Resources", "PrincessVoice.wav");
            string soundPath = Path.Combine(appDirectory, relativePathSound);

            System.Media.SoundPlayer player = new System.Media.SoundPlayer(soundPath);
            player.Play();
            // Khởi tạo Timer và thiết lập các thuộc tính
            timer = new System.Timers.Timer();
            timer.Interval = notificationInterval * 60 * 1000; ; // 15 phút
            timer.Enabled = true;
            timer.AutoReset = true;
            timer.Elapsed += Timer_Elapsed;
        }

        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            timer.Stop(); // Tạm thời tắt Timer
            this.MinimizeBox = false;
            this.MaximizeBox = false;
            this.FormClosing += Form1_FormClosing;
            string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string relativePathSound = Path.Combine("Resources", "PrincessVoice.wav");
            string soundPath = Path.Combine(appDirectory, relativePathSound);

            System.Media.SoundPlayer player = new System.Media.SoundPlayer(soundPath);
            player.Play();
            ShowNotification();
            timer.Start(); // Bật lại Timer sau khi thông báo được hiển thị
        }

        private void ShowNotification()
        {
            // Hiển thị cửa sổ thông báo
            isConfirmed = false;
            this.Invoke(new Action(() =>
            {
                this.Show();
                this.WindowState = FormWindowState.Normal;
            }));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Ẩn cửa sổ khi người dùng nhấn nút Xác nhận
            isConfirmed = true;
            this.Hide();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Ngăn người dùng đóng cửa sổ
            if (!isConfirmed && e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Xử lý sự kiện Load Form (nếu cần)
            this.TopMost = true;
            int screenWidth = Screen.PrimaryScreen.Bounds.Width;
            int screenHeight = Screen.PrimaryScreen.Bounds.Height;
            int formWidth = this.Width;
            int formHeight = this.Height;

            int xPos = (screenWidth - formWidth) / 2;
            int yPos = (screenHeight - formHeight) / 2;

            this.Location = new System.Drawing.Point(xPos, yPos);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Kiểm tra xem giá trị nhập vào TextBox có hợp lệ không
            if (int.TryParse(textBox1.Text, out int intervalMinutes) && intervalMinutes > 0)
            {
                // Cài đặt thời gian lặp lại mới
                notificationInterval = intervalMinutes;
                timer.Interval = notificationInterval * 60 * 1000; // Đổi phút thành mili giây
                isConfirmed = true;
                this.Hide();
                tabControl1.SelectTab(tabPage1);
            }
        }
    }
}
