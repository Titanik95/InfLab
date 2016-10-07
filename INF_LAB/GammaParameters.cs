using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace INF_LAB
{
	[Serializable]
	class GammaParameters
	{
		public int T0 { get; set; }
		public int A { get; set; }
		public int B { get; set; }
		public int C { get; set; }
	}
}
