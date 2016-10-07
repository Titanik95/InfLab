using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INF_LAB
{
	class Main
	{
		string alphabetSubs, alphabetTrans;
		int mSubs, mTrans;


		public Main()
		{
			alphabetSubs = "012";
			alphabetTrans = "АБВГДЕЁЖЗИКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯабвгдеёжзийклмнопрстуфхцчшщъыьэюя";

			mSubs = 3;
			mTrans = 16;
		}

        public List<string> Encrypt(string str, Method method)
        {
            switch (method)
            {
                case Method.Substitution:
                    return new List<string> { Encryption.EncryptSubstitution(str, alphabetSubs, mSubs) };
				case Method.Transposition:
					return new List<string> { Encryption.EncryptTransposition(str, alphabetTrans, mTrans) };
				case Method.Linear:
					return Encryption.EncryptLinear(str, new GammaParameters() { A = 9, B = 256, C = 49, T0 = 24 });
            }
            return null;
        }

        public string Decrypt(string str, Method method, string keyFilePath)
        {
            switch (method)
            {
                case Method.Substitution:
                    return Encryption.DecryptSubstitution(str, keyFilePath, alphabetSubs, mSubs);
				case Method.Transposition:
					return Encryption.DecryptTransposition(str, keyFilePath, alphabetTrans, mTrans);
				case Method.Linear:
					return Encryption.DecryptLinear(str, keyFilePath);
            }
            return "";
        }
    }
}
