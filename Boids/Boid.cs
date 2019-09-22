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
		public float ViewDistance = 2f;

		public float rotation;
		float RotateSpeed = (float)Program.RANDOM.NextDouble();

		public Matrix4 model;

		public Boid()
		{
			Position = new Vector3(0, 0, 0);
			Velocity = new Vector3((float)Program.RANDOM.NextDouble() * 2.0f - 1.0f, (float)Program.RANDOM.NextDouble() * 2.0f - 1.0f, (float)Program.RANDOM.NextDouble() * 2.0f - 1.0f).Normalized();
			Speed = 3.0f;
		}

		public void Update(float deltaTime, Flock flock)
		{
			model = Matrix4.CreateScale(0.25f) * Matrix4.CreateTranslation(Position);

			Bounds();

			if (flock.Boids != null && flock.Boids.Count > 0)
			{
				//! Separation

				//! Alignment
				Vector3 dir = new Vector3(0, 0, 0);
				int BoidsInRange = 0;
				foreach (var boid in flock.Boids)
				{
					if (boid == this)
						continue;

					if ((boid.Position - Position).Length < ViewDistance)
					{
						dir += boid.Velocity;
						BoidsInRange++;
					}
				}
				if (BoidsInRange > 0)
				{
					dir /= BoidsInRange;
					dir = dir.Normalized();
					dir -= Velocity * 0.025f;
				}

				Velocity += dir;

				//! Cohesion
			}
			Position += Velocity.Normalized() * Speed * deltaTime;
		}

		private void Bounds()
		{
			if (Position.X > 10f)
			{
				Position.X = -10f;
			}
			else if (Position.X < -10f)
			{
				Position.X = 10f;
			}

			if (Position.Y > 10f)
			{
				Position.Y = -10f;
			}
			else if (Position.Y < -10f)
			{
				Position.Y = 10f;
			}

			if (Position.Z > 10f)
			{
				Position.Z = -10f;
			}
			else if (Position.Z < -10f)
			{
				Position.Z = 10f;
			}
		}

	}
}
