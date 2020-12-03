using System.IO;
using System.Windows.Forms;

namespace LemmaGen_Plugin
{
    internal partial class SettingsForm_LemmaGen : Form
    {


        #region Get and Set Options

        public string SelectedModel { get; set; }

       #endregion



        public SettingsForm_LemmaGen(string SelectedModel)
        {
            InitializeComponent();


            LemmatizerLanguageSelector.Items.AddRange(new string[] {
            "English",
            "čeština (Czech)",
            "Eesti (Estonian)",
            "فارسی (Persian)",
            "français (French)",
            "Magyar (Hungarian)",
            "Македонски (Macedonian)",
            "polski (Polish)",
            "Română (Romanian)",
            "Pyccĸий (Russian)",
            "Slovenčina (Slovak)",
            "Slovene",
            "Srpski / Српски (Serbian)",
            "Українська (Ukranian)",
            "Deutsch (German)",
            "italiano (Italian)",
            "Español (Spanish)"
            });

            
            try
            {
                LemmatizerLanguageSelector.SelectedIndex = LemmatizerLanguageSelector.FindStringExact(SelectedModel);
            }
            catch
            {
                LemmatizerLanguageSelector.SelectedIndex = 0;
            }


        }



       

        private void OKButton_Click(object sender, System.EventArgs e)
        {
            this.SelectedModel = LemmatizerLanguageSelector.SelectedItem.ToString();
            this.DialogResult = DialogResult.OK;
        }
    }
}
