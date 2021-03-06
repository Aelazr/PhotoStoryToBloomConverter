﻿using System;
using System.Collections.Generic;
using System.Windows.Forms;
using PhotoStoryToBloomConverter.PS3Model;
using System.IO;

namespace PhotoStoryToBloomConverter
{
    public partial class MainScreen : Form
    {
        private PhotoStoryProject photoStoryProject;
	    private IList<string> extractedText; 

        public MainScreen()
        {
            InitializeComponent();
        }

        private void selectProjectXmlButton_Click(object sender, EventArgs e)
        {
            var fileDialog = new OpenFileDialog
            {
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                Filter = "xml files (*.xml)|*.xml",
                RestoreDirectory = true
            };

            if (fileDialog.ShowDialog() != DialogResult.OK) return;
            var projectPath = fileDialog.FileName;

            photoStoryProject = Ps3AndBloomSerializer.DeserializePhotoStoryXml(projectPath);

            photoStoryProjectTextBox.Text = projectPath;

	        var bookName = photoStoryProject.GetProjectName();

            if (projectNameTextBox.Text.Length == 0 && bookName.Length > 0)
				projectNameTextBox.Text = bookName;
        }

        private void selectCollectionButton_Click(object sender, EventArgs e)
        {
            var fileDialog = new OpenFileDialog
            {
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                Filter = "Bloom Collections (*.bloomLibrary *.bloomCollection)|*.bloomLibrary;*.bloomCollection",
                RestoreDirectory = true
            };

            if (fileDialog.ShowDialog() != DialogResult.OK) return;

            bloomCollectionTextBox.Text = fileDialog.FileName;
        }

        private void selectBloomExeButton_Clicked(object sender, EventArgs e)
        {
            var bloomPath = GetBloomExePathFromDialog();
            if(bloomPath != null)
                bloomExeTextBox.Text = bloomPath;
        }

        public static string GetBloomExePathFromDialog()
        {
            var fileDialog = new OpenFileDialog
            {
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                Filter = "Bloom Executable (*.exe)|*.exe",
                RestoreDirectory = true
            };

            if (fileDialog.ShowDialog() != DialogResult.OK) return null;
            return fileDialog.FileName;
        }

        private void selectWordDocButton_Click(object sender, EventArgs e)
        {
            var fileDialog = new OpenFileDialog
            {
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                Filter = ".docx files (*.docx)|*.docx",
                RestoreDirectory = true
            };

            if (fileDialog.ShowDialog() != DialogResult.OK)
                return;

            var path = fileDialog.FileName;

            IList<string> text;
            if (!TextExtractor.TryExtractText(path, out text))
            {
                MessageBox.Show("There was a problem extracting text from this file. Please try a different one or continue without text.");
                wordDocTextBox.Text = string.Empty;
                return;
            }
            extractedText = text;

            wordDocTextBox.Text = path;
        }

        private void convertProjectButton_Click(object sender, EventArgs e)
        {
			var projectXmlPath = photoStoryProjectTextBox.Text;
            var bloomCollectionPath = bloomCollectionTextBox.Text;
            var projectName = projectNameTextBox.Text.Trim();
            var textPath = wordDocTextBox.Text;
            var bloomExePath = bloomCollectionTextBox.Text;

		    Program.Convert(projectXmlPath, Path.GetDirectoryName(bloomCollectionPath), projectName, textPath, bloomExePath, photoStoryProject, extractedText);

            var result = MessageBox.Show("Success! Convert another project?", "Photo Story to Bloom Converter", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                photoStoryProject = null;
                photoStoryProjectTextBox.Text = "";
                projectNameTextBox.Text = "";
            }
            else
                Close();
        }

		private void batchLabel_linkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			using (var dlg = new BatchConversionDlg())
			{
				if (dlg.ShowDialog(this) == DialogResult.OK)
					Close();
			}
		}
    }
}
