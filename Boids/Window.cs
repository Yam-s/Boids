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
		int vao, vbo, _ModelMatrixArrayVBO;
		int view_id, projection_id;
		int shader;

		Matrix4 projection, view;

		Flock flock = new Flock(500);

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
			GL.DepthFunc(DepthFunction.Less);

			//! Load model for a boid
			vao = GL.GenVertexArray();
			GL.BindVertexArray(vao);

			vbo = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
			GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * modelVertices.Length, modelVertices, BufferUsageHint.DynamicDraw);
			
			GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
			GL.EnableVertexArrayAttrib(vao, 0);


			//! Set up matrix instanced arrays
			_ModelMatrixArrayVBO = GL.GenBuffer();
			GL.BindBuffer(BufferTarget.ArrayBuffer, _ModelMatrixArrayVBO);
			GL.BufferData(BufferTarget.ArrayBuffer, flock.Boids.Count * (Vector4.SizeInBytes * 4), IntPtr.Zero, BufferUsageHint.DynamicDraw);

			GL.EnableVertexArrayAttrib(vao, 1);
			GL.VertexAttribPointer(1, 4, VertexAttribPointerType.Float, false, 4 * Vector4.SizeInBytes, 0);
			GL.EnableVertexArrayAttrib(vao, 2);
			GL.VertexAttribPointer(2, 4, VertexAttribPointerType.Float, false, 4 * Vector4.SizeInBytes, Vector4.SizeInBytes);
			GL.EnableVertexArrayAttrib(vao, 3);
			GL.VertexAttribPointer(3, 4, VertexAttribPointerType.Float, false, 4 * Vector4.SizeInBytes, Vector4.SizeInBytes * 2);
			GL.EnableVertexArrayAttrib(vao, 4);
			GL.VertexAttribPointer(4, 4, VertexAttribPointerType.Float, false, 4 * Vector4.SizeInBytes, Vector4.SizeInBytes * 3);

			GL.VertexAttribDivisor(1, 1);
			GL.VertexAttribDivisor(2, 1);
			GL.VertexAttribDivisor(3, 1);
			GL.VertexAttribDivisor(4, 1);

			shader = ShaderHandler.LoadShader("vertex.shader", "fragment.shader");

			view_id = GL.GetUniformLocation(shader, "view");
			projection_id = GL.GetUniformLocation(shader, "projection");
		}

		protected override void OnUpdateFrame(FrameEventArgs e)
		{
			base.OnUpdateFrame(e);

			// Update boids
			var ModelMatrices = flock.Update((float)e.Time);

			GL.BufferSubData(BufferTarget.ArrayBuffer, (IntPtr)0, flock.Boids.Count * (Vector4.SizeInBytes * 4), ModelMatrices.ToArray());

			if (Keyboard.GetState().IsKeyDown(Key.Escape))
				Exit();
		}

		protected override void OnRenderFrame(FrameEventArgs e)
		{
			base.OnRenderFrame(e);
			this.Title = this.RenderFrequency.ToString();
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

			projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(60f), (float)Width / (float)Height, 0.1f, 100.0f);
			view = Matrix4.LookAt(new Vector3(0, 7, -25), Vector3.Zero, new Vector3(0, 1, 0));

			GL.UseProgram(shader);
			GL.BindVertexArray(vao);
			
			GL.UniformMatrix4(view_id, false, ref view);
			GL.UniformMatrix4(projection_id, false, ref projection);
			GL.DrawArraysInstanced(PrimitiveType.Triangles, 0, 18, flock.Boids.Count);

			SwapBuffers();
		}
	}
}
