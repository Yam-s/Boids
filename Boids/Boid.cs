using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace Boids
{
	public class Boid
	{
		public Vector3 Position;
		public Vector3 Velocity;
		public float Speed;

		public float rotation;

		public Matrix4 model;

		public Boid()
		{
			Position = new Vector3(0, 0, 0);
			Velocity = new Vector3(0, 1, 0);
			Speed = (float)Program.RANDOM.NextDouble();
		}

		public void Update(float deltaTime)
		{
			model = Matrix4.Identity * Matrix4.CreateTranslation(Position) * Matrix4.CreateRotationX(rotation += 1f * Speed * deltaTime);
		}

	}
}
