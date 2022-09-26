using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using SharpGL;
using SharpGL.SceneGraph.Lighting;
using SharpGL.SceneGraph.Primitives;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

namespace OpenGL_lesson_CSharp
{
    public partial class SharpGLForm : Form
    {
        float rotation = 0.0f;
        float AngleX = 0, AngleY = 0;
        double POSX = 2, POSY = 0, POSZ = 0;

        const float Rad = 3.14f / 180f;

        const float AngleDl = 1;

        int[] worldX = new int[0]; // Значение вокселей
        int[] worldY = new int[0];
        int[] worldZ = new int[0];

        public SharpGLForm()
        {
            InitializeComponent();

            AdditionWorld(
                new int[] { 0, 10, 10, 0 }, // контур по Х
                new int[] { 0, 0,  10, 10 }, // контур по У
                4, // экструзия
                new int[] { 0, 0, 0 }, // Кручение вокселей
                new int[] { 0, 0, 0 }, // Перемещение
                1, // тип экструзии

                ref worldX, ref worldY, ref worldZ
            );

            AdditionWorld(
                new int[] { 0, 10, 10 }, // контур по Х
                new int[] { 10, 0, 10 }, // контур по У
                3, // экструзия
                new int[] { 0, 0, 0 }, // Кручение вокселей
                new int[] { 0, 0, 0 }, // Перемещение
                0, // тип экструзии

                ref worldX, ref worldY, ref worldZ
            );
        }






        private void openGLControl_OpenGLDraw(object sender, RenderEventArgs e)
        {
            //  Возьмём OpenGL объект
            OpenGL gl = openGLControl.OpenGL;

            //  Очищаем буфер цвета и глубины 
            gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);
            
            //  Загружаем единичную матрицу
            gl.LoadIdentity();

            // Сдвигаем перо вправо от центра и вглубь экрана, но уже дальше
            //gl.Translate(0.0f, 0.0f, -10.0f);
            gl.Translate(0, 0, 0);

            //  Указываем оси вращения (x, y, z)
            gl.Rotate(rotation, 0.0f, 1.0f, 0.0f);


            for (int i = 0; i < worldX.GetLength(0); i++)
            {
                float size = 1.0f;
                float[] coordinates = new float[3] { worldX[i], worldY[i], worldZ[i] };
                //рисуем куб
                Cub(gl, coordinates, size);
            }

            // Контроль полной отрисовки следующего изображения
            gl.Flush();

            //rotation -= 1.0f;
        }

        // Эту функцию используем для создания вокселя заданог id
        void Cub(OpenGL gl, float[] coordinates, float size)
        {
            // рисуем куб
            //gl.Begin(OpenGL.GL_TRIANGLES);
            gl.Begin(OpenGL.GL_QUADS);

            // Top
            gl.Color(0.0f, 1.0f, 0.0f);
            gl.Vertex(size + coordinates[0], size + coordinates[1], -size + coordinates[2]);
            gl.Vertex(-size + coordinates[0], size + coordinates[1], -size + coordinates[2]);
            gl.Vertex(-size + coordinates[0], size + coordinates[1], size + coordinates[2]);
            gl.Vertex(size + coordinates[0], size + coordinates[1], size + coordinates[2]);
            // Bottom
            gl.Color(1.0f, 0.5f, 0.0f);
            gl.Vertex(size + coordinates[0], -size + coordinates[1], size + coordinates[2]);
            gl.Vertex(-size + coordinates[0], -size + coordinates[1], size + coordinates[2]);
            gl.Vertex(-size + coordinates[0], -size + coordinates[1], -size + coordinates[2]);
            gl.Vertex(size + coordinates[0], -size + coordinates[1], -size + coordinates[2]);
            // Front
            gl.Color(1.0f, 0.0f, 0.0f);
            gl.Vertex(size + coordinates[0], size + coordinates[1], size + coordinates[2]);
            gl.Vertex(-size + coordinates[0], size + coordinates[1], size + coordinates[2]);
            gl.Vertex(-size + coordinates[0], -size + coordinates[1], size + coordinates[2]);
            gl.Vertex(size + coordinates[0], -size + coordinates[1], size + coordinates[2]);
            // Back
            gl.Color(1.0f, 1.0f, 0.0f);
            gl.Vertex(size + coordinates[0], -size + coordinates[1], -size + coordinates[2]);
            gl.Vertex(-size + coordinates[0], -size + coordinates[1], -size + coordinates[2]);
            gl.Vertex(-size + coordinates[0], size + coordinates[1], -size + coordinates[2]);
            gl.Vertex(size + coordinates[0], size + coordinates[1], -size + coordinates[2]);
            // Left
            gl.Color(0.0f, 0.0f, 1.0f);
            gl.Vertex(-size + coordinates[0], size + coordinates[1], size + coordinates[2]);
            gl.Vertex(-size + coordinates[0], size + coordinates[1], -size + coordinates[2]);
            gl.Vertex(-size + coordinates[0], -size + coordinates[1], -size + coordinates[2]);
            gl.Vertex(-size + coordinates[0], -size + coordinates[1], size + coordinates[2]);
            // Right
            gl.Color(1.0f, 0.0f, 1.0f);
            gl.Vertex(size + coordinates[0], size + coordinates[1], -size + coordinates[2]);
            gl.Vertex(size + coordinates[0], size + coordinates[1], size + coordinates[2]);
            gl.Vertex(size + coordinates[0], -size + coordinates[1], size + coordinates[2]);
            gl.Vertex(size + coordinates[0], -size + coordinates[1], -size + coordinates[2]);

            gl.End();
        }

        // Эту функцию используем для задания некоторых значений по умолчанию
        private void openGLControl_OpenGLInitialized(object sender, EventArgs e)
        {
           //  Возьмём OpenGL объект
            OpenGL gl = openGLControl.OpenGL;

            //  Фоновый цвет по умолчанию (в данном случае цвет голубой)
            gl.ClearColor(0.1f, 0.5f, 1.0f, 0);
        }

        // Данная функция используется для преобразования изображения 
        // в объёмный вид с перспективой
        private void openGLControl_Resized(object sender, EventArgs e)
        {
            //  Возьмём OpenGL объект
            OpenGL gl = openGLControl.OpenGL;

            //  Зададим матрицу проекции
            gl.MatrixMode(OpenGL.GL_PROJECTION);

            //  Единичная матрица для последующих преобразований
            gl.LoadIdentity();

            //  Преобразование
            gl.Perspective(60.0f, (double)Width / (double)Height, 0.01, 100.0);

            //  Данная функция позволяет установить камеру и её положение
            var dX = Math.Sin(AngleX * Rad) * Math.Cos(AngleY * Rad);
            var dY = Math.Sin(AngleY * Rad);
            var dZ = Math.Cos(AngleX * Rad) * Math.Cos(AngleY * Rad);

            gl.LookAt(POSX, POSY, POSZ,    // Позиция самой камеры
                      POSX + dX,
                      POSY + dY,
                      POSZ + dZ,     // Направление, куда мы смотрим
                       0, 1, 0);    // Верх камеры

            //  Зададим модель отображения
            gl.MatrixMode(OpenGL.GL_MODELVIEW);

            Console.WriteLine(POSX + " " + POSY + " " + POSZ + $" {AngleX}");
        }

        bool b = false;
        int lX = -1, lY = -1;

        private void SharpGLForm_MouseMove(object sender, MouseEventArgs e)
        {
            if (b)
            {
                if (lX != -1)
                {
                    AngleX += (lX - e.X) / 5f;
                }
                if (lY != -1) AngleY += (lY - e.Y) / 5f;
                Console.WriteLine($"mouse {lX} {lY}");
                openGLControl_Resized(sender, e);
            }
            lX = e.X;
            lY = e.Y;
            /*
             
            if (b)
            {
                if (lX == -1)
                {
                    lX = e.X;
                    lY = e.Y;
                }
                else
                {
                    AngleX += (lX - e.X) / 5f;
                    AngleY += (lY - e.Y) / 5f;
                    Cursor.Position = CUR_POINT;
                    Console.WriteLine($"mouse {lX} {lY}");
                    openGLControl_Resized(sender, e);
                }
            }
             */
        }

        private void openGLControl_MouseDown(object sender, MouseEventArgs e) => b = true;
        private void openGLControl_MouseUp(object sender, MouseEventArgs e) => b = false;

        private void openGLControl_KeyDown(object sender, KeyEventArgs e)
        {
            double dX, dY, dZ;
            switch (e.KeyCode)
            {
                case Keys.W:
                    dX = Math.Sin(AngleX * Rad) * Math.Cos(AngleY * Rad);
                    dY = Math.Sin(AngleY * Rad);
                    dZ = Math.Cos(AngleX * Rad) * Math.Cos(AngleY * Rad);
                    POSX += dX;
                    POSY += dY;
                    POSZ += dZ;
                    break;
                case Keys.S:
                    dX = Math.Sin(AngleX * Rad) * Math.Cos(AngleY * Rad);
                    dY = Math.Sin(AngleY * Rad);
                    dZ = Math.Cos(AngleX * Rad) * Math.Cos(AngleY * Rad);
                    POSX -= dX;
                    POSY -= dY;
                    POSZ -= dZ;
                    break;

                case Keys.A:
                    dX = Math.Sin((AngleX + 90) * Rad) * Math.Cos(AngleY * Rad);
                    dZ = Math.Cos((AngleX + 90) * Rad) * Math.Cos(AngleY * Rad);
                    POSX += dX;
                    POSZ += dZ;
                    break;
                case Keys.D:
                    dX = Math.Sin((AngleX - 90) * Rad) * Math.Cos(AngleY * Rad);
                    dZ = Math.Cos((AngleX - 90) * Rad) * Math.Cos(AngleY * Rad);
                    POSX += dX;
                    POSZ += dZ;
                    break;
                case Keys.Q:
                    dX = Math.Sin(AngleX * Rad) * Math.Cos((AngleY - 90) * Rad);
                    dY = Math.Sin((AngleY - 90) * Rad);
                    dZ = Math.Cos(AngleX * Rad) * Math.Cos((AngleY - 90) * Rad);
                    POSX += dX;
                    POSY += dY;
                    POSZ += dZ;
                    break;
                case Keys.E:
                    dX = Math.Sin(AngleX * Rad) * Math.Cos((AngleY + 90) * Rad);
                    dY = Math.Sin((AngleY + 90) * Rad);
                    dZ = Math.Cos(AngleX * Rad) * Math.Cos((AngleY + 90) * Rad);
                    POSX += dX;
                    POSY += dY;
                    POSZ += dZ;
                    break;
            }
            //openGLControl.Invalidate();
            openGLControl_Resized(sender, e);
        }





        //-----------------------------------------------------------------------+

        bool Dot(int x, int y, int[] CircuitX, int[] CircuitY)
        {
            int npol = CircuitX.GetLength(0);
            int j = npol - 1;
            bool c = false;
            for (var i = 0; i < npol; i++)
            {
                if ((((CircuitY[i] <= y) && (y < CircuitY[j])) || ((CircuitY[j] <= y) && (y < CircuitY[i]))) &&
                (x > (CircuitX[j] - CircuitX[i]) * (y - CircuitY[i]) / (CircuitY[j] - CircuitY[i]) + CircuitX[i]))
                {
                    c = !c;
                }
                j = i;
            }
            return c;
        } // Точка принадлежности

        int[] Dimensions(int[] CircuitX, int[] CircuitY)
        {
            int[] MinMax = new int[] { int.MaxValue, int.MaxValue, int.MinValue, int.MinValue };

            for (int i = 0; i < CircuitX.GetLength(0); i++)
            {
                if (MinMax[0] > CircuitX[i])
                {
                    MinMax[0] = CircuitX[i];
                }
                if (MinMax[2] < CircuitX[i])
                {
                    MinMax[2] = CircuitX[i];
                }

                if (MinMax[1] > CircuitY[i])
                {
                    MinMax[1] = CircuitY[i];
                }
                if (MinMax[3] < CircuitY[i])
                {
                    MinMax[3] = CircuitY[i];
                }
            }
            return MinMax;
        } // Точки контура мин. и макс.

        void Extrusion(ref int[] ExtruderX, ref int[] ExtruderY, ref int[] ExtruderZ, int HeightExtruder, int[] CircuitX, int[] CircuitY)
        {
            int[] dimensions = Dimensions(CircuitX, CircuitY);

            for (int x = dimensions[0]; x < dimensions[2]; x++)
            {
                for (int y = dimensions[1]; y < dimensions[3]; y++)
                {
                    if (Dot(x, y, CircuitX, CircuitY))
                    {
                        for (int z = 0; z < HeightExtruder; z++)
                        {
                            Array.Resize(ref ExtruderX, ExtruderX.Length + 1);
                            ExtruderX[ExtruderX.Length - 1] = x;

                            Array.Resize(ref ExtruderY, ExtruderY.Length + 1);
                            ExtruderY[ExtruderY.Length - 1] = y;

                            Array.Resize(ref ExtruderZ, ExtruderZ.Length + 1);
                            ExtruderZ[ExtruderZ.Length - 1] = z;
                        }
                    }
                }
            }
        }

        void AxesRotation(ref int[] ExtruderX, ref int[] ExtruderY, ref int[] ExtruderZ, int[] Corners)
        {
            for (int move = 0; move < ExtruderX.GetLength(0); move++)
            {
                int[] dot = new int[] { ExtruderX[move], ExtruderY[move], ExtruderZ[move] };
                dot = MX(dot, Corners[0]);
                dot = MY(dot, Corners[1]);
                dot = MZ(dot, Corners[2]);

                ExtruderX[move] = dot[0];
                ExtruderY[move] = dot[1];
                ExtruderZ[move] = dot[2];
            }
        }

        void MovingPoints(ref int[] ExtruderX, ref int[] ExtruderY, ref int[] ExtruderZ, int[] Moving)
        {
            for (int moving = 0; moving < ExtruderX.GetLength(0); moving++)
            {
                ExtruderX[moving] += Moving[0];
                ExtruderY[moving] += Moving[1];
                ExtruderZ[moving] += Moving[2];
            }
        }


        void AdditionWorld(
                int[] CircuitX, int[] CircuitY, int Height, int[] Corners, int[] Moving, int Type,
                ref int[] worldX, ref int[] worldY, ref int[] worldZ
        )
        {
            int[] ExtruderX = new int[0]; // Значение вокселей
            int[] ExtruderY = new int[0];
            int[] ExtruderZ = new int[0];

            Extrusion(ref ExtruderX, ref ExtruderY, ref ExtruderZ, Height, CircuitX, CircuitY);
            AxesRotation(ref ExtruderX, ref ExtruderY, ref ExtruderZ, Corners);
            MovingPoints(ref ExtruderX, ref ExtruderY, ref ExtruderZ, Moving);

            for (int i = 0; i < ExtruderX.GetLength(0); i++)
            {
                bool Existence = true;
                for (int MATW = 0; MATW < worldX.GetLength(0); MATW++)
                {
                    if (
                        ExtruderX[i] == worldX[MATW] &&
                        ExtruderY[i] == worldY[MATW] &&
                        ExtruderZ[i] == worldZ[MATW]
                        )
                    {
                        Existence = false;
                        if (Type == 0)
                        {
                            Delete(ref worldX, MATW);
                            Delete(ref worldY, MATW);
                            Delete(ref worldZ, MATW);
                        }
                        break;
                    }
                }
                if (Existence && (Type != 0))
                {
                    Array.Resize(ref worldX, worldX.Length + 1);
                    worldX[worldX.Length - 1] = ExtruderX[i];

                    Array.Resize(ref worldY, worldY.Length + 1);
                    worldY[worldY.Length - 1] = ExtruderY[i];

                    Array.Resize(ref worldZ, worldZ.Length + 1);
                    worldZ[worldZ.Length - 1] = ExtruderZ[i];
                }
            }
        }


        //----------------------------------------------------------

        void Delete(ref int[] array, int index)
        {
            if (index < array.Length && index >= 0)
            {
                int[] array2 = new int[array.Length - 1];
                for (int i = 0, j = 0; i < array.Length; i++)
                {
                    if (i == index) continue;
                    array2[j++] = array[i];
                }
                array = array2;
            }
        } // Удаление по индексу

        //----------------------------------------------------------

        int[] MX(int[] A, int angle)
        {
            int[] B = new int[3];
            double q = angle * (Math.PI / 180);

            B[0] = A[0];
            B[1] = (int)(A[1] * Math.Cos(q) + A[2] * Math.Sin(q));
            B[2] = (int)((-1) * A[1] * Math.Sin(q) + A[2] * Math.Cos(q));

            return B;
        }
        int[] MY(int[] A, int angle)
        {
            int[] B = new int[3];
            double q = angle * (Math.PI / 180);

            B[0] = (int)(A[0] * Math.Cos(q) + A[2] * Math.Sin(q));
            B[1] = A[1];
            B[2] = (int)((-1) * A[0] * Math.Sin(q) + A[2] * Math.Cos(q));

            return B;
        }
        int[] MZ(int[] A, int angle)
        {
            int[] B = new int[3];
            double q = angle * (Math.PI / 180);

            B[0] = (int)(A[0] * Math.Cos(q) - A[1] * Math.Sin(q));
            B[1] = (int)(A[0] * Math.Sin(q) + A[1] * Math.Cos(q));
            B[2] = A[2];

            return B;
        }

        //----------------------------------------------------------

    }
}