using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Translation
{
    [Serializable]
    class Dictionary
    {
        public string En { get; set; }
        public List<string> Ru { get; set; }

        public Dictionary(string ru, string en)
        {
            Ru = new List<string>();
            Ru.Add(ru);
            En = en;
        }
        public void AddTranslation(string ru, string en)
        {
            if (Ru.IndexOf(ru) > 0 && En.IndexOf(en) < 0)
                En = en;
            else
                Ru.Add(ru);
        }
    }
}
