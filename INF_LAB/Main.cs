using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INF_LAB
{
	class Main
	{
		public Main()
		{

		}

        public string Encrypt(string str, Method method)
        {
            switch (method)
            {
                case Method.Substitution:
                    return Encryption.EncriptSubstitution("123");
            }
            return "";
        }
	}
}
