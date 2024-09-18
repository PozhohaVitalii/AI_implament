using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System;
using System.Windows.Forms;


namespace AI_implament
{
    internal class OpenGLControl : GLControl
    {
        private float angleX = 0.0f;
        private float angleY = 0.0f;
        private float angleZ = 0.0f;

        public OpenGLControl() : base()
        {
            this.Load += OpenGLControl_Load;
            this.Paint += OpenGLControl_Paint;
            this.Resize += OpenGLControl_Resize;
        }

        private void OpenGLControl_Load(object sender, EventArgs e)
        {
            GL.ClearColor(Color4.Black);
            SetupViewport();
        }

        private void OpenGLControl_Paint(object sender, PaintEventArgs e)
        {
            Render();
        }

        private void OpenGLControl_Resize(object sender, EventArgs e)
        {
            SetupViewport();
        }

        private void SetupViewport()
        {
            int width = this.Width;
            int height = this.Height;

            GL.Viewport(0, 0, width, height);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            Matrix4 perspective = Matrix4.CreatePerspectiveFieldOfView((float)Math.PI / 4, width / (float)height, 1.0f, 64.0f);
            GL.LoadMatrix(ref perspective);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
        }

        private void Render()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.LoadIdentity();

            // Встановлюємо камеру
            GL.Translate(0.0f, 0.0f, -10.0f);

            // Обертаємо систему координат
            GL.Rotate(angleX, 1.0, 0.0, 0.0); // Обертання навколо осі X
            GL.Rotate(angleY, 0.0, 1.0, 0.0); // Обертання навколо осі Y
            GL.Rotate(angleZ, 0.0, 0.0, 1.0); // Обертання навколо осі Z

            // Малюємо осі
            GL.Begin(PrimitiveType.Lines);

            // Вісь X червона
            GL.Color3(1.0, 0.0, 0.0);
            GL.Vertex3(0.0, 0.0, 0.0);
            GL.Vertex3(5.0, 0.0, 0.0);

            // Вісь Y зелена
            GL.Color3(0.0, 1.0, 0.0);
            GL.Vertex3(0.0, 0.0, 0.0);
            GL.Vertex3(0.0, 5.0, 0.0);

            // Вісь Z синя
            GL.Color3(0.0, 0.0, 1.0);
            GL.Vertex3(0.0, 0.0, 0.0);
            GL.Vertex3(0.0, 0.0, 5.0);

            GL.End();

            this.SwapBuffers();
        }

        public void RotateX(float angle)
        {
            angleX += angle;
            Invalidate(); // Перерисовуємо контрол
        }

        public void RotateY(float angle)
        {
            angleY += angle;
            Invalidate(); // Перерисовуємо контрол
        }

        public void RotateZ(float angle)
        {
            angleZ += angle;
            Invalidate(); // Перерисовуємо контрол
        }
    }

}
