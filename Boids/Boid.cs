﻿using System;
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
		float RotateSpeed = (float)Program.RANDOM.NextDouble();

		public Matrix4 model;

		public Boid()
		{
			Position = new Vector3(0, 0, 0);
			Velocity = new Vector3((float)Program.RANDOM.NextDouble() * 2.0f - 1.0f, (float)Program.RANDOM.NextDouble() * 2.0f - 1.0f, (float)Program.RANDOM.NextDouble() * 2.0f - 1.0f).Normalized();
			Speed = (float)Program.RANDOM.NextDouble() * 3.0f;
		}

		public void Update(float deltaTime)
		{
			model = Matrix4.CreateScale(0.25f) * Matrix4.CreateRotationX(rotation += 2f * RotateSpeed * deltaTime) * Matrix4.CreateTranslation(Position);
			//model = Matrix4.Identity;
			Position += Velocity * Speed * deltaTime;
		}

	}
}
