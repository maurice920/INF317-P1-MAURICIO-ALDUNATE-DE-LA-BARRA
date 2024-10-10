using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace Ejercicio9
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Image = new Bitmap(openFileDialog.FileName);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                // imagen 1
                Bitmap bmp1 = new Bitmap(pictureBox1.Image);
                // convertimos a escalas grises
                Bitmap ImagenGris = ConvertirEscalaGris(bmp1);
                // aplicamos el operador Sobel para la deteccion de bordes
                Bitmap ImagenBord = OperSobel(ImagenGris);
                // Mostramos la imagen resultante
                pictureBox2.Image = ImagenBord;
            }
            else
            {
                MessageBox.Show("Por favor, suba una imagen!!!");
            }
        }
        private Bitmap ConvertirEscalaGris(Bitmap original)
        {
            Bitmap ImGris = new Bitmap(original.Width, original.Height);

            for (int y = 0; y < original.Height; y++)
            {
                for (int x = 0; x < original.Width; x++)
                {

                    Color orCol = original.GetPixel(x, y);
                    int escalaGris = (int)((orCol.R * 0.3) + (orCol.G * 0.59) + (orCol.B * 0.11));
                    Color grisCol = Color.FromArgb(escalaGris, escalaGris, escalaGris);
                    ImGris.SetPixel(x, y, grisCol);

                }
            }
            return ImGris;
        }
        private Bitmap OperSobel(Bitmap imagenGris)
        {
            Bitmap imageBord = new Bitmap(imagenGris.Width, imagenGris.Height);

            int[,] gx = new int[,]
            {
                { -1, 0, 1 },
                { -2, 0, 2 },
                { -1, 0, 1 }
            };
            int[,] gy = new int[,]
            {
                { -1, -2, -1},
                { 0, 0, 0 },
                { 1, 2, 1 }
            };
            for (int y = 1; y < imagenGris.Height - 1; y++)
            {
                for (int x = 1; x < imagenGris.Width - 1; x++)
                {
                    int pixelX = (
                        (gx[0, 0] * imagenGris.GetPixel(x - 1, y - 1).R) + (gx[0, 1] * imagenGris.GetPixel(x, y - 1).R) + (gx[0, 2] * imagenGris.GetPixel(x + 1, y - 1).R) +
                        (gx[1, 0] * imagenGris.GetPixel(x - 1, y).R) + (gx[1, 1] * imagenGris.GetPixel(x, y).R) + (gx[1, 2] * imagenGris.GetPixel(x + 1, y).R) +
                        (gx[2, 0] * imagenGris.GetPixel(x - 1, y + 1).R) + (gx[2, 1] * imagenGris.GetPixel(x, y + 1).R) + (gx[2, 2] * imagenGris.GetPixel(x + 1, y + 1).R)
                    );

                    int pixelY = (
                        (gy[0, 0] * imagenGris.GetPixel(x - 1, y - 1).R) + (gy[0, 1] * imagenGris.GetPixel(x, y - 1).R) + (gy[0, 2] * imagenGris.GetPixel(x + 1, y - 1).R) +
                        (gy[1, 0] * imagenGris.GetPixel(x - 1, y).R) + (gy[1, 1] * imagenGris.GetPixel(x, y).R) + (gy[1, 2] * imagenGris.GetPixel(x + 1, y).R) +
                        (gy[2, 0] * imagenGris.GetPixel(x - 1, y + 1).R) + (gy[2, 1] * imagenGris.GetPixel(x, y + 1).R) + (gy[2, 2] * imagenGris.GetPixel(x + 1, y + 1).R)
                    );

                    int magnitude = (int)Math.Sqrt((pixelX * pixelX) + (pixelY * pixelY));
                    magnitude = Math.Min(255, Math.Max(0, magnitude));

                    imageBord.SetPixel(x, y, Color.FromArgb(magnitude, magnitude, magnitude));
                }
            }

            return imageBord;
        }
    }
}
