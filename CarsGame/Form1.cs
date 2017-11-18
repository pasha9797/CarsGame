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
        Model model;
        Bitmap bitmap;
        Graphics gScreen;
        Graphics gBitmap;
        Rectangle r;
        public System.Windows.Forms.Timer updateState;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            GoFullscreen();
            model = new Model(this.Size);
            gScreen = this.CreateGraphics();
            bitmap = new Bitmap(ClientRectangle.Width, ClientRectangle.Height);
            gBitmap = Graphics.FromImage(bitmap);
            r = ClientRectangle;
            updateState = new System.Windows.Forms.Timer();
            updateState.Interval = CarsGameLib.C.UpdateInterval;
            updateState.Tick += UpdateState;
            //updateState.AutoReset = true;
            updateState.Start();
        }

        private void GoFullscreen()
        {
            this.WindowState = FormWindowState.Normal;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Bounds = Screen.PrimaryScreen.Bounds;
        }

        public void UpdateState(Object sender, EventArgs e)
        {
            model.MainUpdate();
            model.Draw(gBitmap);
            gScreen.DrawImage(bitmap, r);
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            model.CheckFixLight(e.X, e.Y);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            model.LightsInspection();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            updateState.Stop();
            Application.Exit();
        }
    }
}
