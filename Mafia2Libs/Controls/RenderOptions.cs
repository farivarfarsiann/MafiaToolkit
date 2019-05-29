﻿using System;
using System.Windows.Forms;
using Utils.Lang;
using Utils.Settings;

namespace Forms.OptionControls
{
    public partial class RenderOptions : UserControl
    {
        public RenderOptions()
        {
            InitializeComponent();
            Localise();
            LoadSettings();
        }

        private void Localise()
        {
            RenderGroup.Text = Language.GetString("$RENDER_OPTIONS");
            ScreenFarLabel.Text = Language.GetString("$RENDER_SCREENFAR");
            ScreenNearLabel.Text = Language.GetString("$RENDER_SCREENEAR");
            TexLabel.Text = Language.GetString("$TEXTURE_DIRECTORY");
            CameraSpeedLabel.Text = Language.GetString("$RENDER_CAMERASPEED");
            TexBrowser.Description = Language.GetString("$SELECT_TEX_FOLDER");
            ExperimentalBox.Text = Language.GetString("$ENABLE_EXPERIMENTAL");
        }
        
        /// <summary>
        /// Read Settings from INI and populate controls.
        /// </summary>
        private void LoadSettings()
        {
            ScreenFarUpDown.Value = Math.Min(Convert.ToInt16(ToolkitSettings.ScreenDepth), ScreenFarUpDown.Maximum);
            ScreenNearUpDown.Value = Math.Min(Convert.ToInt16(ToolkitSettings.ScreenNear), ScreenNearUpDown.Maximum);
            CameraSpeedUpDown.Value = Math.Min((decimal)ToolkitSettings.CameraSpeed, CameraSpeedUpDown.Maximum);
            TexDirectoryBox.Text = ToolkitSettings.TexturePath;
            ExperimentalBox.Checked = ToolkitSettings.Experimental;
        }

        private void ScreenDepth_Changed(object sender, EventArgs e)
        {
            ToolkitSettings.ScreenDepth = Convert.ToSingle(ScreenFarUpDown.Value);
            ToolkitSettings.WriteKey("ScreenDepth", "ModelViewer", ToolkitSettings.ScreenDepth.ToString());
        }

        private void ScreenNear_Changed(object sender, EventArgs e)
        {
            ToolkitSettings.ScreenNear = Convert.ToSingle(ScreenNearUpDown.Value);
            ToolkitSettings.WriteKey("ScreenNear", "ModelViewer", ToolkitSettings.ScreenNear.ToString());
        }

        private void CameraSpeedUpDown_Changed(object sender, EventArgs e)
        {
            ToolkitSettings.CameraSpeed = Convert.ToSingle(CameraSpeedUpDown.Value);
            ToolkitSettings.WriteKey("CameraSpeed", "ModelViewer", ToolkitSettings.CameraSpeed.ToString());
        }

        private void TexDirectoryBox_TextChanged(object sender, EventArgs e)
        {
            ToolkitSettings.TexturePath = TexDirectoryBox.Text;
            ToolkitSettings.WriteKey("TexturePath", "ModelViewer", ToolkitSettings.TexturePath);
        }

        private void BrowseButton_Click(object sender, EventArgs e)
        {
            TexBrowser.SelectedPath = "";
            if (TexBrowser.ShowDialog() == DialogResult.OK)
            {
                TexDirectoryBox.Text = TexBrowser.SelectedPath;
                TexDirectoryBox_TextChanged(null, null);
            }
            else return;
        }

        private void ExperimentalBox_CheckedChanged(object sender, EventArgs e)
        {
            ToolkitSettings.Experimental = ExperimentalBox.Checked;
            ToolkitSettings.WriteKey("EnableExperimental", "ModelViewer", ToolkitSettings.Experimental.ToString());
        }
    }
}
