using CameraPlusScriptMaker.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;


namespace CameraPlusScriptMaker
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window {


		public SettingsModel.Settings userSettings;

		public List<PathModel.Path> userPaths = new List<PathModel.Path>();

		public PathModel.Path loadedPath = new PathModel.Path();



		public MainWindow() {

			userSettings = SettingsModel.LoadSettings();

			InitializeComponent();

			ReloadPaths();


			







		}

		public void ReloadPaths() {
			userPaths = PathModel.LoadPaths();

			//Loops over files in paths folder and then adds them to the ListBox.
			List<string> pathsList = new List<string>();
			foreach(PathModel.Path path in userPaths) {
				pathsList.Add(path.name);
			}
			UIScriptsList.ItemsSource = pathsList;

		}

		private void UIScriptsList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e) {
			loadedPath = userPaths[UIScriptsList.SelectedIndex];
			UIKeyframes.SelectedIndex = -1;
			PopulateKeyframesUI();

		}



		public void PopulateKeyframesUI() {
			int i = 0;
			List<int> UIKeyframesList = new List<int>();
			foreach(PathModel.Keyframe keyframe in loadedPath.Movements) {
				UIKeyframesList.Add(i);
				i++;
			}
			UIKeyframes.ItemsSource = UIKeyframesList;
		}

		public void PopulateKeyframeValuesUI() {
			if (loadedPath == null)
				return;


			UIStartPosX.Text = loadedPath.Movements[UIKeyframes.SelectedIndex].StartPos.x.ToString();
			UIStartPosY.Text = loadedPath.Movements[UIKeyframes.SelectedIndex].StartPos.y.ToString();
			UIStartPosZ.Text = loadedPath.Movements[UIKeyframes.SelectedIndex].StartPos.z.ToString();

			UIStartRotX.Text = loadedPath.Movements[UIKeyframes.SelectedIndex].StartRot.x.ToString();
			UIStartRotY.Text = loadedPath.Movements[UIKeyframes.SelectedIndex].StartRot.y.ToString();
			UIStartRotZ.Text = loadedPath.Movements[UIKeyframes.SelectedIndex].StartRot.z.ToString();

			UIEndPosX.Text = loadedPath.Movements[UIKeyframes.SelectedIndex].EndPos.x.ToString();
			UIEndPosY.Text = loadedPath.Movements[UIKeyframes.SelectedIndex].EndPos.y.ToString();
			UIEndPosZ.Text = loadedPath.Movements[UIKeyframes.SelectedIndex].EndPos.z.ToString();
			  													
			UIEndRotX.Text = loadedPath.Movements[UIKeyframes.SelectedIndex].EndRot.x.ToString();
			UIEndRotY.Text = loadedPath.Movements[UIKeyframes.SelectedIndex].EndRot.y.ToString();
			UIEndRotZ.Text = loadedPath.Movements[UIKeyframes.SelectedIndex].EndRot.z.ToString();

			UIDuration.Text = loadedPath.Movements[UIKeyframes.SelectedIndex].Duration.ToString();
			UIDelay.Text = loadedPath.Movements[UIKeyframes.SelectedIndex].Delay.ToString();
			UIEaseTransition.IsChecked = loadedPath.Movements[UIKeyframes.SelectedIndex].EaseTransition;

		}

		private void UIApply_Click(object sender, RoutedEventArgs e) {

			loadedPath.Movements[UIKeyframes.SelectedIndex].StartPos.x = Convert.ToDouble(UIStartPosX.Text);
			loadedPath.Movements[UIKeyframes.SelectedIndex].StartPos.y = Convert.ToDouble(UIStartPosY.Text);
			loadedPath.Movements[UIKeyframes.SelectedIndex].StartPos.z = Convert.ToDouble(UIStartPosZ.Text);
			loadedPath.Movements[UIKeyframes.SelectedIndex].StartRot.x = Convert.ToDouble(UIStartRotX.Text);
			loadedPath.Movements[UIKeyframes.SelectedIndex].StartRot.y = Convert.ToDouble(UIStartRotY.Text);
			loadedPath.Movements[UIKeyframes.SelectedIndex].StartRot.z = Convert.ToDouble(UIStartRotZ.Text);

			loadedPath.Movements[UIKeyframes.SelectedIndex].EndPos.x = Convert.ToDouble(UIEndPosX.Text);
			loadedPath.Movements[UIKeyframes.SelectedIndex].EndPos.y = Convert.ToDouble(UIEndPosY.Text);
			loadedPath.Movements[UIKeyframes.SelectedIndex].EndPos.z = Convert.ToDouble(UIEndPosZ.Text);
			loadedPath.Movements[UIKeyframes.SelectedIndex].EndRot.x = Convert.ToDouble(UIEndRotX.Text);
			loadedPath.Movements[UIKeyframes.SelectedIndex].EndRot.y = Convert.ToDouble(UIEndRotY.Text);
			loadedPath.Movements[UIKeyframes.SelectedIndex].EndRot.z = Convert.ToDouble(UIEndRotZ.Text);

			loadedPath.Movements[UIKeyframes.SelectedIndex].Duration = Convert.ToDouble(UIDuration.Text);
			loadedPath.Movements[UIKeyframes.SelectedIndex].Delay = Convert.ToDouble(UIDelay.Text);
			loadedPath.Movements[UIKeyframes.SelectedIndex].EaseTransition = (bool)UIEaseTransition.IsChecked;

			string json = JsonConvert.SerializeObject(loadedPath);
			File.WriteAllText(Directory.GetCurrentDirectory() + @"\" + UIScriptsList.SelectedValue, json);

		}



		private void UIKeyframes_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e) {

			if (UIKeyframes.SelectedIndex >= 0)
				PopulateKeyframeValuesUI();
		}

		private void UIAddKeyframe_Click(object sender, RoutedEventArgs e) {
			if(loadedPath == null)
				return;

			PathModel.Keyframe keyframe = new PathModel.Keyframe();

			loadedPath.Movements.Add(keyframe);
			PopulateKeyframesUI();

		}

		private void UIRemove_Click(object sender, RoutedEventArgs e) {

			MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this keyframe?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
			if(result == MessageBoxResult.Yes) {
				loadedPath.Movements.RemoveAt(UIKeyframes.SelectedIndex);
				string json = JsonConvert.SerializeObject(loadedPath);
				File.WriteAllText(Directory.GetCurrentDirectory() + @"\" + UIScriptsList.SelectedValue, json);

				if (UIKeyframes.HasItems) {
					UIKeyframes.SelectedIndex = 0;
					PopulateKeyframesUI();
					
				}

			}

			
		}

		private void UINewPath_Click(object sender, RoutedEventArgs e) {
			if (UINewPathName.Text == "") {
				MessageBox.Show("You need to supply a name for the path.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}

			try {
				File.WriteAllText(Directory.GetCurrentDirectory() + @"\" + UINewPathName.Text + ".json", "{\n    \"Movements\": [\n\n    ]\n}");
				ReloadPaths();
			} catch {
				MessageBox.Show("An error occured while creating the file.\n" + e.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
			}
			


			
		}

	}
}
