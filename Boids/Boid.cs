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
		public Vector3 Acceleration = new Vector3(0,0,0);
		public float ViewDistance = 2f;

		public float TurnSpeed = 1f;

		public float SeparationStrength = 0.125f;
		public float AlignmentStrength = 0.125f;
		public float CohesionStrength = 0.2f;

		public Matrix4 model;
		public Vector3 Up = new Vector3(0,1,0);

		public Boid()
		{
		}

		private void ApplyForce(Vector3 force)
		{
			Acceleration += force;
		}

		private Matrix4 CalculateRotation()
		{
			var rotAngle = (float)Math.Acos(Vector3.Dot(Up, Velocity.Normalized()));

			if (Math.Abs(rotAngle) < 0.001f)
				return Matrix4.Zero;

			var rotAxis = Vector3.Cross(Up, Velocity.Normalized());

			return Matrix4.CreateFromAxisAngle(rotAxis, rotAngle);
		}

		public void Update(float deltaTime, Flock flock)
		{
			var rotation = CalculateRotation();

			if (rotation != Matrix4.Zero)
				model = Matrix4.CreateScale(0.075f) * rotation * Matrix4.CreateTranslation(Position);

			Bounds();

			if (flock.Boids == null && flock.Boids.Count <= 1)
				return;

			var neighbours = new List<Boid>();
			foreach (var boid in flock.Boids)
			{
				if (boid == this)
					continue;

				if ((boid.Position - Position).Length < ViewDistance)
					neighbours.Add(boid);
			}

			Separate(neighbours);
			Align(neighbours);
			Cohesion(neighbours);

			Velocity += Acceleration;
			Position += Velocity.Normalized() * 2.0f * deltaTime;
			Acceleration = new Vector3(0, 0, 0);
		}

		private void Separate(List<Boid> neighbours)
		{
			var dir = new Vector3(0, 0, 0);
			foreach (var boid in neighbours)
			{
				if (boid == this)
					continue;

				var d = (boid.Position - Position).Length;
				if (d < 0.5f && d > 0)
				{
					dir = (Position - boid.Position).Normalized();
					dir /= d;
				}
			}

			ApplyForce(dir * SeparationStrength);
		}

		private void Align(List<Boid> neighbours)
		{
			var dir = new Vector3(0, 0, 0);
			int BoidsInRange = 0;
			foreach (var boid in neighbours)
			{
				if (boid == this)
					continue;
				var d = (boid.Position - Position).Length;
				if (d < ViewDistance && d > 0)
				{
					dir += boid.Velocity;
					BoidsInRange++;
				}
			}
			if (BoidsInRange > 0)
			{
				dir /= BoidsInRange;
				dir = dir.Normalized();
			}

			ApplyForce((dir - Velocity) * AlignmentStrength);
		}

		private void Cohesion(List<Boid> neighbours)
		{
			var dir = new Vector3(0, 0, 0);
			var avgPos = new Vector3(0, 0, 0);
			var BoidsInRange = 0;
			foreach (var boid in neighbours)
			{
				if (boid == this)
					continue;

				var d = (boid.Position - Position).Length;
				if (d < ViewDistance && d > 0)
				{
					avgPos += boid.Position;
					BoidsInRange++;
				}
			}
			if (BoidsInRange > 0)
			{
				avgPos /= BoidsInRange;
				dir = avgPos - Position;
			}

			ApplyForce(dir * CohesionStrength);
		}


		private void Bounds()
		{
			if (Position.X >= 5f)
			{
				Position.X = -5f;
			}
			else if (Position.X <= -5f)
			{
				Position.X = 5f;
			}

			if (Position.Y >= 5f)
			{
				Position.Y = -5f;
			}
			else if (Position.Y <= -5f)
			{
				Position.Y = 5f;
			}

			if (Position.Z >= 5f)
			{
				Position.Z = -5f;
			}
			else if (Position.Z <= -5f)
			{
				Position.Z = 5f;
			}
		}

	}
}
