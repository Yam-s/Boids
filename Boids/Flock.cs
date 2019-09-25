using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;


namespace Boids
{
	public class Flock
	{
		public List<Boid> Boids = new List<Boid>();
		// Array to store all boids' model matrices
		// Will be updated and used to draw all boids using instanced arrays
		public List<Matrix4> ModelMatrices = new List<Matrix4>();

		int FlockSize = 0;

		public Flock(int flockSize = 100)
		{
			FlockSize = flockSize;
			Boids.Clear();
			for (var i = 1; i <= FlockSize; i++)
			{
				var boid = new Boid();
				boid.Position = new Vector3(
					(float)Program.RANDOM.NextDouble() * (-5 - 5) + 5,
					(float)Program.RANDOM.NextDouble() * (-5 - 5) + 5,
					(float)Program.RANDOM.NextDouble() * (-5 - 5) + 5);

				boid.Velocity = new Vector3(
					Program.RANDOM.Next(2) * 2 - 1,
					Program.RANDOM.Next(2) * 2 - 1,
					Program.RANDOM.Next(2) * 2 - 1);
				Boids.Add(boid);
			}
		}

		public List<Matrix4> Update(float deltaTime)
		{
			var ModelMatrices = new List<Matrix4>();
			foreach (var boid in Boids)
			{
				boid.Update(deltaTime, this);
				ModelMatrices.Add(boid.model);
			}

			return ModelMatrices;
		}
	}
}
