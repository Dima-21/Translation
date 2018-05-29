using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Translation
{
    public partial class Form1 : Form
    {
        List<Dictionary> dictionary = new List<Dictionary>();

        public Form1()
        {
            InitializeComponent();

            if (File.Exists("Data.translation"))
            {
                using (var fs = new FileStream("Data.translation", FileMode.Open))
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    dictionary = bf.Deserialize(fs) as List<Dictionary>;
                }
            }
            UpdateText();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textRussian.Text != String.Empty && textEnglish.Text != String.Empty)
                AddTranslation();
            cEn_SelectedValueChanged(sender, e);
            textRussian.Text = String.Empty;
            textEnglish.Text = String.Empty;
            Serialize();
            UpdateText();
        }

        private void UpdateText()
        {
            cEn.Items.Clear();
            cRu.Items.Clear();

            foreach (var item in dictionary)
            {
                cEn.Items.Add(item.En);
                cRu.Items.AddRange(item.Ru.ToArray());
            }

        }
        private void AddTranslation()
        {
            StringBuilder ru = new StringBuilder(textRussian.Text.ToLower());
            StringBuilder en = new StringBuilder(textEnglish.Text.ToLower());
            ru[0] = Char.ToUpper(ru[0]);
            en[0] = Char.ToUpper(en[0]);
            foreach (var item in dictionary)
            {
                if (item.Ru.IndexOf(ru.ToString()) >= 0 && item.En.IndexOf(en.ToString()) >= 0)
                {
                    MessageBox.Show("Перевод уже существует!");
                    return;
                }
                if (item.En.IndexOf(en.ToString()) >= 0)
                {
                    item.AddTranslation(ru.ToString(), en.ToString());
                    return;
                }
            }
            dictionary.Add(new Dictionary(ru.ToString(), en.ToString()));

        }

        private void text_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar) || Char.IsPunctuation(e.KeyChar))
                e.Handled = true;
        }

        private void cEn_SelectedValueChanged(object sender, EventArgs e)
        {
            listBox1.Items.Clear();

            foreach (var item in dictionary)
            {
                if (item.En == cEn.Text)
                    listBox1.Items.AddRange(item.Ru.ToArray());

            }
        }

        private void cRu_SelectedValueChanged(object sender, EventArgs e)
        {
            listBox2.Items.Clear();
            foreach (var item in dictionary)
            {
                if (item.Ru.IndexOf(cRu.Text) >= 0)
                    listBox2.Items.Add(item.En);
            }
        }

        private void Serialize()
        {
            using (var fs = new FileStream("Data.translation", FileMode.OpenOrCreate))
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(fs, dictionary);
            }
        }

        private void bRemove1_Click(object sender, EventArgs e)
        {
            foreach (var item in dictionary)
            {
                if (item.Ru.IndexOf(listBox1.Text) >= 0)
                {
                    if (item.Ru.Count == listBox1.SelectedItems.Count)
                        dictionary.Remove(item);
                    else
                        foreach (string item1 in listBox1.SelectedItems)
                            item.Ru.Remove(item1);
                    Serialize();
                    cEn_SelectedValueChanged(sender, e);
                    cRu_SelectedValueChanged(sender, e);
                    UpdateText();
                    return;
                }
            }
        }

        private void bRemove2_Click(object sender, EventArgs e)
        {

            foreach (var item in dictionary)
            {
                if (listBox2.Text == item.En)
                {
                    foreach (var item1 in listBox2.SelectedItems)
                    {
                        dictionary.Remove(item);
                    }
                    Serialize();
                    cRu_SelectedValueChanged(sender, e);
                    cEn_SelectedValueChanged(sender, e);
                    return;
                }
            }

        }
    }
}
