using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;
using OpenTK.Graphics.OpenGL4;


namespace Boids.Handlers
{
	public static class ShaderHandler
	{
		public static int LoadShader(string vs, string fs)
		{
			Assembly assembly = Assembly.GetExecutingAssembly();

			string vertexSource;
			using (var stream = new StreamReader(assembly.GetManifestResourceStream("Boids.Shaders." + vs)))
			{
				vertexSource = stream.ReadToEnd();
			}

			string fragmentSource;
			using (var stream = new StreamReader(assembly.GetManifestResourceStream("Boids.Shaders." + fs)))
			{
				fragmentSource = stream.ReadToEnd();
			}

			int vertexShader = GL.CreateShader(ShaderType.VertexShader);
			GL.ShaderSource(vertexShader, vertexSource);
			GL.CompileShader(vertexShader);

			int fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
			GL.ShaderSource(fragmentShader, fragmentSource);
			GL.CompileShader(fragmentShader);

			int shaderProgram = GL.CreateProgram();
			GL.AttachShader(shaderProgram, vertexShader);
			GL.AttachShader(shaderProgram, fragmentShader);

			Console.WriteLine(GL.GetShaderInfoLog(vertexShader));
			Console.WriteLine(GL.GetShaderInfoLog(fragmentShader));

			GL.LinkProgram(shaderProgram);

			GL.DetachShader(shaderProgram, vertexShader);
			GL.DetachShader(shaderProgram, fragmentShader);

			GL.DeleteShader(vertexShader);
			GL.DeleteShader(fragmentShader);

			return shaderProgram;
		}

		public static int LoadSingleShader(string shader, ShaderType shaderType)
		{
			Assembly assembly = Assembly.GetExecutingAssembly();

			string shaderSource;
			using (var stream = new StreamReader(assembly.GetManifestResourceStream("rays.Shaders." + shader)))
			{
				shaderSource = stream.ReadToEnd();
			}

			int shaderID = GL.CreateShader(shaderType);
			GL.ShaderSource(shaderID, shaderSource);
			GL.CompileShader(shaderID);

			int shaderProgram = GL.CreateProgram();
			GL.AttachShader(shaderProgram, shaderID);

			Console.WriteLine(GL.GetShaderInfoLog(shaderID));

			GL.LinkProgram(shaderProgram);
			GL.DetachShader(shaderProgram, shaderID);
			GL.DeleteShader(shaderID);
			return shaderProgram;
		}

	}
}
