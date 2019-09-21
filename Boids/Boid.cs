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
		Vector3 Position;
		Vector3 Velocity;
		float Speed;

		public Boid()
		{
			Position = new Vector3(0, 0, 0);
			Velocity = new Vector3(0, 1, 0);
			Speed = 10;
		}
	}
}
