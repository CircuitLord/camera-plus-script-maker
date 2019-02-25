using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CameraPlusScriptMaker.Models
{
	public class PathModel { 


		public class Path {
			public List<Keyframe> Movements = new List<Keyframe>();
			public string name { get; set; }
		}

		//public class Movements {
		//	public List<Keyframe> keyframes = new List<Keyframe>();
		//}

		public class Keyframe {
			public Pos StartPos { get; set; } = new Pos();
			public Rot StartRot { get; set; } = new Rot();
			public Pos EndPos { get; set; } = new Pos();
			public Rot EndRot { get; set; } = new Rot();
			public double Duration { get; set; } = 4;
			public double Delay { get; set; } = 0;

			public bool EaseTransition { get; set; } = true;
		}

		public class Pos {
			public double x { get; set; } = 0;
			public double y { get; set; } = 0;
			public double z { get; set; } = 0;
		}
		public class Rot {
			public double x { get; set; } = 0;
			public double y { get; set; } = 0;
			public double z { get; set; } = 0;
		}


		public static List<Path> LoadPaths() {
			List<Path> userPaths = new List<Path>();

			DirectoryInfo d = new DirectoryInfo(Directory.GetCurrentDirectory());

			int i = 0;
			foreach(var file in d.GetFiles("*.json")) {
				if(file.Name == "settings.json")
					
					continue;

				Path path = new Path();
				JsonConvert.PopulateObject(File.ReadAllText(file.FullName), path);

				path.name = file.Name;

				userPaths.Add(path);
				
				i++;
			}
			Console.WriteLine(userPaths[0].Movements[0].StartRot.x);

			return userPaths;
		}




	}
}
