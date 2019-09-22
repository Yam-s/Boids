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
		public float TurnSpeed = 0.05f;

		public Matrix4 model;

		public Boid()
		{
			Position = new Vector3(0, 0, 0);
			Velocity = new Vector3((float)Program.RANDOM.NextDouble() * 2.0f - 1.0f, (float)Program.RANDOM.NextDouble() * 2.0f - 1.0f, (float)Program.RANDOM.NextDouble() * 2.0f - 1.0f).Normalized();
			Speed = 6.0f;
		}

		public void Update(float deltaTime, Flock flock)
		{
			model = Matrix4.CreateScale(0.125f) * Matrix4.CreateTranslation(Position);

			Bounds();

			if (flock.Boids != null && flock.Boids.Count > 0)
			{
				//! Separation
				var dir = new Vector3(0, 0, 0);
				int BoidsInRange = 0;
				foreach (var boid in flock.Boids)
				{
					if (boid == this)
						continue;

					var d = (boid.Position - Position).Length;
					if (d < ViewDistance)
					{
						var avoidDir = Position - boid.Position;
						avoidDir /= d;
						dir += avoidDir;
						BoidsInRange++;
					}
				}
				if (BoidsInRange > 0)
				{
					dir /= BoidsInRange;
					dir = dir.Normalized();
					dir -= Velocity * TurnSpeed;
				}

				Velocity += dir;


				//! Alignment
				dir = new Vector3(0, 0, 0);
				BoidsInRange = 0;
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
					dir -= Velocity * TurnSpeed;
				}

				Velocity += dir;

				//! Cohesion
				dir = new Vector3(0, 0, 0);
				var pos = new Vector3(0, 0, 0);
				BoidsInRange = 0;
				foreach (var boid in flock.Boids)
				{
					if (boid == this)
						continue;

					if ((boid.Position - Position).Length < ViewDistance)
					{
						dir += boid.Position;
						BoidsInRange++;
					}
				}
				if (BoidsInRange > 0)
				{
					pos /= BoidsInRange;
					dir = (pos - Position).Normalized();
					dir -= Velocity * TurnSpeed;
				}

				Velocity += dir;

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
