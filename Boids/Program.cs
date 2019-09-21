using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace Boids
{
	class Program
	{
		public static int seed = 1;
		public static Random RANDOM = new Random(seed);
		static void Main()
		{
			using (var game = new Window(1280, 720))
			{
				game.Run(60);
			}
		}
	}
}
