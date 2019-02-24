using Newtonsoft.Json;
using System.IO;

namespace CameraPlusScriptMaker.Models
{
	public class SettingsModel {


		public class Settings {
			public string scriptDir = null;
		}


		public static Settings LoadSettings() {
			Settings userSettings = new Settings();
			JsonConvert.PopulateObject(File.ReadAllText(Directory.GetCurrentDirectory() + @"\settings.json"), userSettings);
			return userSettings;
		}



	}

}
