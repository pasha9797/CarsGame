using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace CarsGame
{
    public partial class Form1 : Form
    {
        Bitmap bitmap;
        Graphics gScreen;
        Graphics gBitmap;
        Rectangle r;
        public int width, height;
        public System.Timers.Timer updateState;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            GoFullscreen();
            gScreen = this.CreateGraphics();
            bitmap = new Bitmap(ClientRectangle.Width, ClientRectangle.Height);
            gBitmap = Graphics.FromImage(bitmap);
            r = ClientRectangle;
            width = r.Width;
            height = r.Height;
            updateState = new System.Timers.Timer(C.UpdateInterval);
            updateState.Elapsed += UpdateState;
            updateState.AutoReset = true;
            updateState.Start();
            Field.CreateRoadsAndCrossways();
        }

        private void GoFullscreen()
        {
            this.WindowState = FormWindowState.Normal;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Bounds = Screen.PrimaryScreen.Bounds;
        }

        public void UpdateState(Object source, ElapsedEventArgs e)
        {
            Field.MainUpdate();
            try
            {
                Field.Draw(gBitmap);
                gScreen.DrawImage(bitmap, r);
            }
            catch { }
            

   
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            Field.CheckFixLight(e.X, e.Y);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Field.LightsInspection();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            updateState.Stop();
            Application.Exit();
        }
    }
}
