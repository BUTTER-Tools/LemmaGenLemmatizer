using PluginContracts;
using System.Collections.Generic;
using System.Drawing;
using LemmaSharp;
using System.Windows.Forms;
using System;
using OutputHelperLib;
using System.Linq;


namespace LemmaGen_Plugin
{
    public class LemmaGen_Plugin : Plugin
    {

        public string[] InputType { get; } = { "Tokens" };
        public string OutputType { get; } = "Tokens";

        public bool InheritHeader { get; } = false;
        public Dictionary<int, string> OutputHeaderData { get; set; } = new Dictionary<int, string>() { };
        private LemmaSharp.LemmatizerPrebuiltCompact Lemmatizer { get; set; } = new LemmaSharp.LemmatizerPrebuiltCompact(LanguagePrebuilt.English);
        private string SelectedLemmatizerLanguage { get; set; } = "English";

        #region Plugin Details and Info

        public string PluginName { get; } = "LemmaGen v3";
        public string PluginType { get; } = "Lemmatizers";
        public string PluginVersion { get; } = "3.0.1";
        public string PluginAuthor { get; } = "Ryan L. Boyd (ryan@ryanboyd.io)";
        public string PluginDescription { get; } = "This plugin will lemmatize tokens that it receives from a tokenizer (e.g., the Twitter-Aware Tokenizer). Built on top of the LemmaGen pre-trained lemmatizers (v3):" + Environment.NewLine +
            "http://lemmatise.ijs.si/Software/Version3" + Environment.NewLine + Environment.NewLine +
            "Juršic, M., Mozetic, I., Erjavec, T., & Lavrac, N. (2010). Lemmagen: Multilingual lemmatisation with induced ripple-down rules. Journal of Universal Computer Science, 16(9), 1190–1214.";
        public bool TopLevel { get; } = false;
        public string PluginTutorial { get; } = "https://youtu.be/1FUzDnERlJc";

        public Icon GetPluginIcon
        {
            get
            {
                return Properties.Resources.icon;
            }
        }

        #endregion



        public void ChangeSettings()
        {

            using (var form = new SettingsForm_LemmaGen(SelectedLemmatizerLanguage))
            {


                form.Icon = Properties.Resources.icon;
                form.Text = PluginName;

                var result = form.ShowDialog();
                if (result == DialogResult.OK)
                {

                    SelectedLemmatizerLanguage = form.SelectedModel;
                }
            }


        }



       


        public Payload RunPlugin(Payload Input)
        {

            Payload pData = new Payload();
            pData.FileID = Input.FileID;
            pData.SegmentID = Input.SegmentID;


            for (int counter = 0; counter < Input.StringArrayList.Count; counter++)
            {

                string[] TextToLemmatize = Input.StringArrayList[counter];
                //make sure everything is lowercase
                TextToLemmatize = TextToLemmatize.Select(s => s.ToLowerInvariant()).ToArray();
                for (int i = 0; i < TextToLemmatize.Length; i++) TextToLemmatize[i] = Lemmatizer.Lemmatize(TextToLemmatize[i]);
                pData.StringArrayList.Add(TextToLemmatize);
                pData.SegmentNumber.Add(Input.SegmentNumber[counter]);
            }

            return (pData);

        }


        public void Initialize()
        {
            LemmatizerChooser chooser = new LemmatizerChooser();
            Lemmatizer = chooser.LemmaGenChoice(SelectedLemmatizerLanguage);
        }


        public bool InspectSettings()
        {
            return true;
        }

        public Payload FinishUp(Payload Input)
        {
            return (Input);
        }


        #region Import/Export Settings
        public void ImportSettings(Dictionary<string, string> SettingsDict)
        {
            SelectedLemmatizerLanguage = SettingsDict["SelectedLemmatizerLanguage"];
        }

        public Dictionary<string, string> ExportSettings(bool suppressWarnings)
        {
            Dictionary<string, string> SettingsDict = new Dictionary<string, string>();
            SettingsDict.Add("SelectedLemmatizerLanguage", SelectedLemmatizerLanguage);
            return (SettingsDict);
        }
        #endregion


    }
}
