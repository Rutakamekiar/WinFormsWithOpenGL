using System;
using System.Drawing;
using System.Windows.Forms;
using SharpGL;
using SharpGL.Enumerations;

// для работы с библиотекой OpenGL
using Tao.OpenGl;

// для работы с библиотекой FreeGLUT
using Tao.FreeGlut;

namespace Project6
{
    public partial class Form1 : Form
    {
        private Color _figureColor = Color.GreenYellow;
        private Color _lightColor = Color.White;

        public Form1()
        {
            InitializeComponent();
            textToDraw.VisibleChanged += (sender, args) => labelTextToDraw.Visible = textToDraw.Visible;
            Glut.glutInit(); // инициализация Glut
            Glut.glutInitDisplayMode(Glut.GLUT_RGB | Glut.GLUT_DOUBLE | Glut.GLUT_DEPTH); // устанвока начального режима отображения
        }

        private void Lighting()
        {
            float[] lightPosition = { 0.5f, 0.5f, 0.5f, 1.0f }; // Координаты источника света
            Gl.glPolygonMode(Gl.GL_FRONT, Gl.GL_FILL); // Заливка
            Gl.glShadeModel(Gl.GL_SMOOTH); // Вывод с интерполяцией цветов
            Gl.glEnable(Gl.GL_LIGHTING); // Включаю освещенность
            Gl.glLightfv(Gl.GL_LIGHT0, Gl.GL_POSITION, lightPosition);
            Gl.glLightfv(Gl.GL_LIGHT0, Gl.GL_AMBIENT, new[] { Convert.ToSingle(_lightColor.R) / 255, Convert.ToSingle(_lightColor.G) / 255, Convert.ToSingle(_lightColor.B) / 255 }); // Рассеивание
            Gl.glEnable(Gl.GL_LIGHT0); // Включаем в уравнение освещенности источник GL_LIGHT0
            Gl.glMaterialfv(Gl.GL_FRONT, Gl.GL_DIFFUSE, new[] { Convert.ToSingle(_figureColor.R) / 255, Convert.ToSingle(_figureColor.G) / 255, Convert.ToSingle(_figureColor.B) / 255 });
        }

        public void RotationGlut() // процедура поворота для 3D фигур
        {
            Gl.glTranslated(tbTrX.Value, tbTrY.Value, tbTrZ.Value); // перенос по Х,Y,Z
            if (chBRotX.Checked) // для автоматического вращения по Х
            {
                if (tbRotX.Value + 1 > 360)
                    tbRotX.Value = -360;
                Gl.glRotated(tbRotX.Value += 1, 1.0f, 0.0f, 0.0f);
            }
            else
                Gl.glRotated(tbRotX.Value, 1.0f, 0.0f, 0.0f); // для вращения по трекбару по Х
            if (chBRotY.Checked)// для автоматического вращения по Y
            {
                if (tbRotY.Value + 1 > 360)
                    tbRotY.Value = -360;
                Gl.glRotated(tbRotY.Value += 1, 0.0f, 1.0f, 0.0f);
            }
            else
                Gl.glRotated(tbRotY.Value, 0.0f, 1.0f, 0.0f); // для вращения по трекбару по Y
            if (chBRotZ.Checked)// для вращения по трекбару по Z
            {
                if (tbRotZ.Value + 1 > 360)
                    tbRotZ.Value = -360;
                Gl.glRotated(tbRotZ.Value += 1, 0.0f, 0.0f, 1.0f);
            }
            else
                Gl.glRotated(tbRotZ.Value, 0.0f, 0.0f, 1.0f); // для вращения по трекбару по Z

            XRtText.Text = tbRotX.Value.ToString(); //отображение градусов и единиц переноса на label
            YRtText.Text = tbRotY.Value.ToString();
            ZRtText.Text = tbRotZ.Value.ToString();
            XTrText.Text = tbTrX.Value.ToString();
            YTrText.Text = tbTrY.Value.ToString();
            ZTrText.Text = tbTrZ.Value.ToString();
        }

        private void DrawTorus()
        {
            //рисование тора с помощью библиотеки FreeGlut
            Gl.glDisable(Gl.GL_BLEND);
            Gl.glColor3f(Convert.ToSingle(_figureColor.R) / 255, Convert.ToSingle(_figureColor.G) / 255, Convert.ToSingle(_figureColor.B) / 255);//цвет фигуры без освещения

            Gl.glPushMatrix();// сохраняет текущие координаты
            Gl.glTranslated(0, 0, -5);// перенос по Z

            RotationGlut();// поворот

            if (Wire.Checked)
                Glut.glutWireTorus(0.3, 0.65, 16, 200);// сеточный режим
            else
                Glut.glutSolidTorus(0.3, 0.65, 16, 200);//режим с заливкой

            Gl.glPopMatrix();
            Gl.glDisable(Gl.GL_LIGHTING); // пересчитываем освещение
            Gl.glFlush(); // довыполнение предыдущих команд
        }

        private void DrawCone()
        {
            //рисование конуса с помощью библиотеки FreeGlut
            Gl.glDisable(Gl.GL_BLEND);
            Gl.glColor3f(Convert.ToSingle(_figureColor.R) / 255, Convert.ToSingle(_figureColor.G) / 255, Convert.ToSingle(_figureColor.B) / 255);//цвет фигуры без освещения
            Gl.glPushMatrix();// сохраняет текущие координаты
            Gl.glTranslated(0, -0.3f, -2); // перенос по Y,Z
            RotationGlut();// поворот
            if (Wire.Checked)
                Glut.glutWireCone(0.2, 0.75, 16, 8);// сеточный режим
            else
                Glut.glutSolidCone(0.2, 0.75, 16, 8);//режим с заливкой

            Gl.glPopMatrix();
            Gl.glDisable(Gl.GL_LIGHTING); // пересчитываем освещение
            Gl.glFlush(); // довыполнение предыдущих команд
        }

        private void DrawCylinder()
        {
            //рисование цилиндра с помощью библиотеки FreeGlut
            Gl.glDisable(Gl.GL_BLEND);
            Gl.glColor3f(Convert.ToSingle(_figureColor.R) / 255, Convert.ToSingle(_figureColor.G) / 255, Convert.ToSingle(_figureColor.B) / 255);//цвет фигуры без освещения

            Gl.glPushMatrix();// сохраняет текущие координаты
            Gl.glTranslated(0, -0.3f, -2);// перенос по Y,Z
            RotationGlut();// поворот

            if (Wire.Checked)
                Glut.glutWireCylinder(0.2, 0.75, 16, 16);// сеточный режим
            else
                Glut.glutSolidCylinder(0.2, 0.75, 16, 16);//режим с заливкой

            Gl.glPopMatrix();
            Gl.glDisable(Gl.GL_LIGHTING); // пересчитываем освещение
            Gl.glFlush(); // довыполнение предыдущих команд
        }

        private void DrawIcosahedron()
        {
            //рисование икосаэдра с помощью библиотеки FreeGlut
            Gl.glDisable(Gl.GL_BLEND);
            Gl.glColor3f(Convert.ToSingle(_figureColor.R) / 255, Convert.ToSingle(_figureColor.G) / 255, Convert.ToSingle(_figureColor.B) / 255);//цвет фигуры без освещения

            Gl.glPushMatrix();// сохраняет текущие координаты
            Gl.glTranslated(0, 0, -4f);// перенос по Z

            RotationGlut();// поворот

            if (Wire.Checked)
                Glut.glutWireIcosahedron();// сеточный режим
            else
                Glut.glutSolidIcosahedron();//режим с заливкой

            Gl.glPopMatrix();
            Gl.glDisable(Gl.GL_LIGHTING); // пересчитываем освещение
            Gl.glFlush(); // довыполнение предыдущих команд
        }

        private void DrawDodecahedron()
        {
            //рисование додекаэдра с помощью библиотеки FreeGlut
            Gl.glDisable(Gl.GL_BLEND);
            Gl.glColor3f(Convert.ToSingle(_figureColor.R) / 255, Convert.ToSingle(_figureColor.G) / 255, Convert.ToSingle(_figureColor.B) / 255);//цвет фигуры без освещения

            Gl.glPushMatrix();// сохраняет текущие координаты
            Gl.glTranslated(0, 0, -6);// перенос по Z

            RotationGlut();// поворот

            if (Wire.Checked)
                Glut.glutWireDodecahedron();// сеточный режим
            else
                Glut.glutSolidDodecahedron();//режим с заливкой

            Gl.glPopMatrix();//возвращение к старым координатам
            Gl.glDisable(Gl.GL_LIGHTING); // пересчитываем освещение
            Gl.glFlush(); // довыполнение предыдущих команд
        }

        private void DrawRhombicDodecahedron()
        {
            //рисование ромбического додекаэдра с помощью библиотеки FreeGlut
            Gl.glDisable(Gl.GL_BLEND);
            Gl.glColor3f(Convert.ToSingle(_figureColor.R) / 255, Convert.ToSingle(_figureColor.G) / 255, Convert.ToSingle(_figureColor.B) / 255);//цвет фигуры без освещения

            Gl.glPushMatrix();// сохраняет текущие координаты
            Gl.glTranslated(0, 0, -6);// перенос по Z

            RotationGlut();// поворот

            if (Wire.Checked)
                Glut.glutWireRhombicDodecahedron();// сеточный режим
            else
                Glut.glutSolidRhombicDodecahedron();//режим с заливкой

            Gl.glPopMatrix();//возвращение к старым координатам
            Gl.glDisable(Gl.GL_LIGHTING); // пересчитываем освещение
            Gl.glFlush(); // довыполнение предыдущих команд
        }

        private void DrawOctahedron()
        {
            //рисование октаэдра с помощью библиотеки FreeGlut
            Gl.glDisable(Gl.GL_BLEND);
            Gl.glColor3f(Convert.ToSingle(_figureColor.R) / 255, Convert.ToSingle(_figureColor.G) / 255, Convert.ToSingle(_figureColor.B) / 255);//цвет фигуры без освещения

            Gl.glPushMatrix();// сохраняет текущие координаты
            Gl.glTranslated(0, 0, -3);// перенос по Z

            RotationGlut();// поворот

            if (Wire.Checked)
                Glut.glutWireOctahedron();// сеточный режим
            else
                Glut.glutSolidOctahedron();//режим с заливкой

            Gl.glPopMatrix();//возвращение к старым координатам
            Gl.glDisable(Gl.GL_LIGHTING); // пересчитываем освещение
            Gl.glFlush(); // довыполнение предыдущих команд
        }

        private void DrawAxis()
        {
            //выключение освещения
            Gl.glDisable(Gl.GL_LIGHT0);
            Gl.glDisable(Gl.GL_LIGHTING);

            Gl.glClear(Gl.GL_COLOR_BUFFER_BIT | Gl.GL_DEPTH_BUFFER_BIT); // очистка буфера
            Gl.glLoadIdentity();
            Gl.glPushMatrix(); // сохраняет текущие координаты
            Gl.glTranslated(0f, 0f, -6f); // перенос по Z
            // сглаживание линий
            Gl.glEnable(Gl.GL_LINE_SMOOTH);
            Gl.glHint(Gl.GL_LINE_SMOOTH_HINT, Gl.GL_NICEST);
            Gl.glEnable(Gl.GL_POINT_SMOOTH);
            Gl.glHint(Gl.GL_POINT_SMOOTH_HINT, Gl.GL_NICEST);
            Gl.glEnable(Gl.GL_BLEND);
            Gl.glBlendFunc(Gl.GL_SRC_ALPHA, Gl.GL_ONE_MINUS_SRC_ALPHA);

            RotationGlut();// вращение

            Gl.glRotatef(0, 1, 0, 0);
            Gl.glRotatef(0, 0, 1, 0);
            Gl.glRotatef(30, 0, 0, 1); // поворот на 30 градусов по Z

            Gl.glBegin(Gl.GL_LINES); //рисуем линии

            Gl.glColor3f(0.67f, 0.65f, 1f); //цвет оси Х
            Gl.glVertex3f(10, 0, 0);
            Gl.glVertex3f(-10, 0, 0);// Ось X

            Gl.glColor3f(0.68f, 0.95f, 0.72f);//цвет оси Y
            Gl.glVertex3f(0, 10, 0);
            Gl.glVertex3f(0, -10, 0);//Ось Y

            Gl.glColor3f(0.84f, 0.58f, 0.58f);//цвет оси Z
            Gl.glVertex3f(0, 0, -10);
            Gl.glVertex3f(0, 0, 0);//Ось Z

            Gl.glColor3f(0.84f, 0.58f, 0.58f);//цвет оси Z
            Gl.glVertex3f(0, 0, 0);
            Gl.glVertex3f(0, 0, 10);//Ось Z
            Gl.glEnd(); // конец отрисовки

            Gl.glPopMatrix();//возвращение к старым координатам
            Gl.glFlush();// довыполнение предыдущих команд
            //включение освещения
            if (!Light.Checked) // если не стоит галочка - выключение освещения
            {
                Gl.glDisable(Gl.GL_LIGHT0);
                Gl.glDisable(Gl.GL_LIGHTING);
            }
            else
            {
                Gl.glEnable(Gl.GL_LIGHTING);
                Gl.glEnable(Gl.GL_LIGHT0);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            tbRotX.Value = 0;
            tbRotY.Value = 0;
            tbRotZ.Value = 0;
            chBRotX.Checked = false;
            chBRotY.Checked = false;
            chBRotZ.Checked = false;
            tbTrX.Value = 0;
            tbTrY.Value = 0;
            tbTrZ.Value = 0;
            if (tabControl1.SelectedTab.Text == "3D")
            {
                if (comboBox1.SelectedItem.Equals("Конус") || comboBox1.SelectedItem.Equals("Цилиндр"))
                {
                    Gl.glTranslated(0f, -0.3f, 0f);
                    tbRotX.Value = -90;
                }
            }
        }

        private void openGLControl1_OpenGLDraw(object sender, RenderEventArgs args)
        {
            // Очистка экрана и буфера глубин
            openGLControl1.OpenGL.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);
            openGLControl1.OpenGL.LoadIdentity(); // reset view
            if (Axes.Checked)
            {
                DrawAxis(); // отображение осей
            }

            if (Light.Checked)
            {
                Lighting(); // использование света
            }
            else
            {
                Gl.glDisable(Gl.GL_LIGHT0);
                Gl.glDisable(Gl.GL_LIGHTING);
            }

            if (tabControl1.SelectedTab.Text == "2D")
            {
                Axes.Checked = false;

                switch (comboBox2.Text) //2D
                {
                    case "Текст":
                        if (!textToDraw.Visible)
                        {
                            textToDraw.Visible = true;
                        }
                        DrawText();
                        break;
                    case "Квадрат":
                        if (textToDraw.Visible)
                        {
                            textToDraw.Visible = false;
                        }
                        DrawSquare();
                        break;
                    case "Треугольник":
                        if (textToDraw.Visible)
                        {
                            textToDraw.Visible = false;
                        }
                        DrawTriangle();
                        break;
                }
            }
            else
            {
                switch (comboBox1.Text)
                {
                    case "Тор":
                        DrawTorus();
                        break;
                    case "Цилиндр":
                        DrawCylinder();
                        break;
                    case "Ромб. додeкаэдр":
                        DrawRhombicDodecahedron();
                        break;
                    case "Икосаэдр":
                        DrawIcosahedron();
                        break;
                    case "Октаэдр":
                        DrawOctahedron();
                        break;
                    case "Додeкаэдр":
                        DrawDodecahedron();
                        break;
                    case "Конус":
                        DrawCone();
                        break;
                    case "Куб":
                        DrawCube();
                        break;
                    case "Сфера":
                        DrawSphere();
                        break;
                    case "Тетраэдр":
                        DrawTetrahedron();
                        break;
                    case "Чайник":
                        DrawTeapot();
                        openGLControl1.Invalidate();
                        break;
                }

                openGLControl1.Invalidate();
            }
        }

        private void DrawText()
        {
            openGLControl1.OpenGL.DrawText(-1, 0, Convert.ToSingle(_figureColor.R) / 255, Convert.ToSingle(_figureColor.G) / 255, Convert.ToSingle(_figureColor.B) / 255, "", 70, textToDraw.Text, 0, 0);
        }

        private void DrawSquare()
        {
            openGLControl1.OpenGL.Translate(-1.5f, 0.0f, -7.0f);
            openGLControl1.OpenGL.Color(_figureColor.R, _figureColor.G, _figureColor.B);
            openGLControl1.OpenGL.Begin(BeginMode.Quads); // начало отрисовки

            openGLControl1.OpenGL.Vertex(1, 1);
            openGLControl1.OpenGL.Vertex(1, 0);
            openGLControl1.OpenGL.Vertex(2, 0);
            openGLControl1.OpenGL.Vertex(2, 1);
            openGLControl1.OpenGL.End();// конец отрисовки
            openGLControl1.OpenGL.Flush();// довыполнение предыдущих команд
        }

        public void DrawTriangle()
        {
            openGLControl1.OpenGL.Translate(1, -1f, -7.0f);
            openGLControl1.OpenGL.Color(_figureColor.R, _figureColor.G, _figureColor.B);
            openGLControl1.OpenGL.Begin(BeginMode.Triangles); // начало отрисовки
            openGLControl1.OpenGL.Vertex(-2, 0);
            openGLControl1.OpenGL.Vertex(0, 0);
            openGLControl1.OpenGL.Vertex(-1, 2);
            openGLControl1.OpenGL.End();// конец отрисовки
            openGLControl1.OpenGL.Flush();// довыполнение предыдущих команд
        }

        private void button2_Click(object sender, EventArgs e)
        {
            openGLControl1.OpenGLDraw -= openGLControl1_OpenGLDraw;
            using (var colorDialog = new ColorDialog())
            {
                if (colorDialog.ShowDialog() == DialogResult.OK)
                {
                    _figureColor = colorDialog.Color;
                }
            }

            openGLControl1.OpenGLDraw += openGLControl1_OpenGLDraw;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openGLControl1.OpenGLDraw -= openGLControl1_OpenGLDraw;
            using (var colorDialog = new ColorDialog())
            {
                if (colorDialog.ShowDialog() == DialogResult.OK)
                {
                    _lightColor = colorDialog.Color;
                }
            }

            openGLControl1.OpenGLDraw += openGLControl1_OpenGLDraw;
        }

        private void DrawTeapot()
        {
            //рисование чайника с помощью библиотеки FreeGlut
            Gl.glDisable(Gl.GL_BLEND);
            Gl.glColor3f(Convert.ToSingle(_figureColor.R) / 255, Convert.ToSingle(_figureColor.G) / 255, Convert.ToSingle(_figureColor.B) / 255);//цвет фигуры без освещения

            Gl.glPushMatrix();// сохраняет текущие координаты
            Gl.glTranslated(0, 0, -5f); // перенос по Z

            RotationGlut();// поворот

            // рисуем чайник с помощью библиотеки FreeGLUT
            if (Wire.Checked)
                Glut.glutWireTeapot(1);// сеточный режим
            else
                Glut.glutSolidTeapot(1);//режим с заливкой

            Gl.glPopMatrix();
            Gl.glDisable(Gl.GL_LIGHTING); // пересчитываем освещение
            Gl.glFlush(); // довыполнение предыдущих команд
        }

        private void DrawCube()
        {
            //рисование куба с помощью библиотеки FreeGlut
            Gl.glDisable(Gl.GL_BLEND);
            Gl.glColor3f(Convert.ToSingle(_figureColor.R) / 255, Convert.ToSingle(_figureColor.G) / 255, Convert.ToSingle(_figureColor.B) / 255);//цвет фигуры без освещения

            Gl.glPushMatrix();// сохраняет текущие координаты
            Gl.glTranslated(0, 0, -5f); // перенос по Z
            RotationGlut();

            if (Wire.Checked)
                Glut.glutWireCube(1);// сеточный режим
            else
                Glut.glutSolidCube(1);//режим с заливкой

            Gl.glPopMatrix();
            Gl.glDisable(Gl.GL_LIGHTING); // Будем рассчитывать освещенность
            Gl.glFlush();
        }

        private void DrawSphere()
        {
            //рисование сферы с помощью библиотеки FreeGlut
            Gl.glDisable(Gl.GL_BLEND);
            Gl.glColor3f(Convert.ToSingle(_figureColor.R) / 255, Convert.ToSingle(_figureColor.G) / 255, Convert.ToSingle(_figureColor.B) / 255);//цвет фигуры без освещения

            Gl.glPushMatrix();// сохраняет текущие координаты
            Gl.glTranslated(0, 0, -7);// перенос по Z

            RotationGlut();// поворот

            if (Wire.Checked)
                Glut.glutWireSphere(2, 32, 32);// сеточный режим
            else
                Glut.glutSolidSphere(2, 32, 32);//режим с заливкой

            Gl.glPopMatrix();//возвращение к старым координатам
            Gl.glDisable(Gl.GL_LIGHTING); // пересчитываем освещение
            Gl.glFlush(); // довыполнение предыдущих команд
        }

        private void DrawTetrahedron()
        {
            //рисование тетраэдра с помощью библиотеки FreeGlut
            Gl.glDisable(Gl.GL_BLEND);
            Gl.glColor3f(Convert.ToSingle(_figureColor.R) / 255, Convert.ToSingle(_figureColor.G) / 255, Convert.ToSingle(_figureColor.B) / 255);//цвет фигуры без освещения

            Gl.glPushMatrix();// сохраняет текущие координаты
            Gl.glTranslated(0, 0, -3);// перенос по Z

            RotationGlut(); // поворот

            if (Wire.Checked)
                Glut.glutWireTetrahedron(); // сеточный режим
            else
                Glut.glutSolidTetrahedron(); //режим с заливкой

            Gl.glPopMatrix();
            Gl.glDisable(Gl.GL_LIGHTING); // пересчитываем освещение
            Gl.glFlush(); // довыполнение предыдущих команд
        }
    }
}