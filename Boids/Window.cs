using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using Boids.Handlers;
using OpenTK.Input;

namespace Boids
{
	class Window : GameWindow
	{

		int vao, vbo;
		int mvp_id;
		int shader;

		float rotation = 0f;

		Matrix4 projection, view;

		float[] modelVertices = new float[]
		{
			 0.0f,  1.3f,  0.0f, // Triangle 1
			-1.0f, -1.0f,  1.0f,
			 1.0f, -1.0f,  1.0f,
			 0.0f,  1.3f,  0.0f, // Triangle 2
			 1.0f, -1.0f,  1.0f,
			 1.0f, -1.0f, -1.0f,
			 0.0f,  1.3f,  0.0f, // Triangle 3
			 1.0f, -1.0f, -1.0f,
			-1.0f, -1.0f, -1.0f,
			 0.0f,  1.3f,  0.0f, // Triangle 4
			-1.0f, -1.0f, -1.0f,
			-1.0f, -1.0f,  1.0f,
			// Connect bottom
			-1.0f, -1.0f, 1.0f, // bottom tri 1
			 1.0f, -1.0f, 1.0f,
			-1.0f, -1.0f, -1.0f,
			 1.0f, -1.0f, 1.0f, // bottom tri 2
			-1.0f, -1.0f, -1.0f,
			 1.0f, -1.0f, -1.0f
		};

		public Window(int width, int height) : base(width, height, GraphicsMode.Default, "", GameWindowFlags.FixedWindow)
		{
			VSync = VSyncMode.Adaptive;
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			GL.ClearColor(0.2f, 0.2f, 0.2f, 1.0f);
			GL.Enable(EnableCap.DepthTest);

			//! Load model for a boid
			vao = GL.GenVertexArray();
			GL.BindVertexArray(vao);


			vbo = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
			GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * modelVertices.Length, modelVertices, BufferUsageHint.DynamicDraw);
			
			GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
			GL.EnableVertexArrayAttrib(vao, 0);

			shader = ShaderHandler.LoadShader("vertex.shader", "fragment.shader");

			mvp_id = GL.GetUniformLocation(shader, "MVP");
		}

		protected override void OnUpdateFrame(FrameEventArgs e)
		{
			base.OnUpdateFrame(e);

			if (Keyboard.GetState().IsKeyDown(Key.Escape))
				Exit();
		}

		protected override void OnRenderFrame(FrameEventArgs e)
		{
			base.OnRenderFrame(e);
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

			projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(90f), (float)Width / (float)Height, 0.001f, 1000.0f);
			view = Matrix4.LookAt(new Vector3(0, 2, -3), Vector3.Zero, new Vector3(0, 1, 0));

			GL.UseProgram(shader);
			GL.BindVertexArray(vao);

			for (var i = -2; i <= 2; i += 2)
			{
				var model = Matrix4.Identity * Matrix4.CreateRotationY(rotation += 1f * (float)e.Time) * Matrix4.CreateTranslation(new Vector3((float)i * 2, 0, 0));
				var mvp = model * view * projection;
				GL.UniformMatrix4(mvp_id, false, ref mvp);

				GL.DrawArrays(PrimitiveType.Triangles, 0, 18);
			}


			SwapBuffers();
		}
	}
}
