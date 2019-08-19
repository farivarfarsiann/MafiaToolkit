﻿using System;
using System.IO;
using ResourceTypes.Translokator;
using System.Windows.Forms;
using Utils.Lang;

namespace Mafia2Tool.Forms
{
    public partial class TranslokatorEditor : Form
    {
        private FileInfo file;
        private TranslokatorLoader translokator;
        private object clipboard;

        public TranslokatorEditor(FileInfo info)
        {
            InitializeComponent();
            file = info;
            Localise();
            LoadFile();
            Show();
        }

        private void Localise()
        {
            ExitButton.Text = Language.GetString("$EXIT");
            fileToolButton.Text = Language.GetString("$FILE");
            SaveToolButton.Text = Language.GetString("$SAVE");
            ReloadButton.Text = Language.GetString("$RELOAD");
            AddInstance.Text = Language.GetString("$ADD_INSTANCE");
            AddObject.Text = Language.GetString("$ADD_OBJECT");
            DeleteInstance.Text = Language.GetString("$DELETE_INSTANCE");
            DeleteObject.Text = Language.GetString("$DELETE_OBJECT");
            Text = Language.GetString("$TRANSLOKATOR_EDITOR");
            CopyButton.Text = Language.GetString("$COPY");
            PasteButton.Text = Language.GetString("$PASTE");
        }

        private void LoadFile()
        {
            translokator = new TranslokatorLoader(file);
            TranslokatorTree.Nodes.Clear();

            TreeNode headerData = new TreeNode("Header Data");
            headerData.Tag = translokator;

            TreeNode gridNode = new TreeNode("Grids");
            for (int i = 0; i < translokator.Grids.Length; i++)
            {
                Grid grid = translokator.Grids[i];
                TreeNode child = new TreeNode("Grid " + i);
                child.Tag = grid;
                gridNode.Nodes.Add(child);
            }
            TreeNode ogNode = new TreeNode("Objects Groups");
            for (int i = 0; i < translokator.ObjectGroups.Length; i++)
            {
                ObjectGroup objectGroup = translokator.ObjectGroups[i];
                TreeNode objectGroupNode = new TreeNode("Object Group " + i);
                objectGroupNode.Tag = objectGroup;

                for (int y = 0; y < objectGroup.Objects.Length; y++)
                {
                    ResourceTypes.Translokator.Object obj = objectGroup.Objects[y];
                    TreeNode objNode = new TreeNode(obj.Name);
                    objNode.Tag = obj;
                    objectGroupNode.Nodes.Add(objNode);

                    for (int x = 0; x < obj.Instances.Length; x++)
                    {
                        Instance instance = obj.Instances[x];
                        TreeNode instanceNode = new TreeNode(obj.Name + " " + x);
                        instanceNode.Tag = instance;
                        objNode.Nodes.Add(instanceNode);
                    }
                }

                ogNode.Nodes.Add(objectGroupNode);
            }
            TranslokatorTree.Nodes.Add(headerData);
            TranslokatorTree.Nodes.Add(gridNode);
            TranslokatorTree.Nodes.Add(ogNode);
        }

        private void SaveFile()
        {
            translokator.Grids = new Grid[TranslokatorTree.Nodes[1].GetNodeCount(false)];
            for (int i = 0; i < translokator.Grids.Length; i++)
            {
                Grid grid = (TranslokatorTree.Nodes[1].Nodes[i].Tag as Grid);
                translokator.Grids[i] = grid;
            }

            translokator.ObjectGroups = new ObjectGroup[TranslokatorTree.Nodes[2].GetNodeCount(false)];
            for (int i = 0; i < translokator.ObjectGroups.Length; i++)
            {
                ObjectGroup objectGroup = (TranslokatorTree.Nodes[2].Nodes[i].Tag as ObjectGroup);
                objectGroup.Objects = new ResourceTypes.Translokator.Object[TranslokatorTree.Nodes[2].Nodes[i].GetNodeCount(false)];
                objectGroup.NumObjects = objectGroup.Objects.Length;
                for (int y = 0; y < objectGroup.Objects.Length; y++)
                {
                    ResourceTypes.Translokator.Object obj = (TranslokatorTree.Nodes[2].Nodes[i].Nodes[y].Tag as ResourceTypes.Translokator.Object);
                    obj.Instances = new Instance[TranslokatorTree.Nodes[2].Nodes[i].Nodes[y].GetNodeCount(false)];
                    obj.NumInstances = obj.Instances.Length;
                    for (int z = 0; z < obj.Instances.Length; z++)
                    {
                        Instance instance = (TranslokatorTree.Nodes[2].Nodes[i].Nodes[y].Nodes[z].Tag as Instance);
                        obj.Instances[z] = instance;
                    }
                    objectGroup.Objects[y] = obj;
                }

                translokator.ObjectGroups[i] = objectGroup;
            }
            translokator.WriteToFile(file);
        }

        private void TranslokatorTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (TranslokatorTree?.SelectedNode.Tag != null)
            {
                PropertyGrid.SelectedObject = TranslokatorTree?.SelectedNode.Tag;
            }
        }

        private void ReloadButton_Click(object sender, EventArgs e)
        {
            LoadFile();
        }

        private void SaveToolButton_Click(object sender, EventArgs e)
        {
            SaveFile();
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void TranslokatorContext_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            for (int i = 0; i != TranslokatorContext.Items.Count; i++)
                TranslokatorContext.Items[i].Visible = false;

            if (TranslokatorTree.SelectedNode != null && TranslokatorTree.SelectedNode.Tag != null)
            {
                if (TranslokatorTree.SelectedNode.Tag.GetType() == typeof(ResourceTypes.Translokator.Object))
                    TranslokatorContext.Items[0].Visible = true;
                if (TranslokatorTree.SelectedNode.Tag.GetType() == typeof(ObjectGroup))
                    TranslokatorContext.Items[1].Visible = true;
                if (TranslokatorTree.SelectedNode.Tag.GetType() == typeof(Instance))
                    TranslokatorContext.Items[2].Visible = true;
                if (TranslokatorTree.SelectedNode.Tag.GetType() == typeof(ResourceTypes.Translokator.Object))
                    TranslokatorContext.Items[3].Visible = true;

                if (TranslokatorTree.SelectedNode.Tag.GetType() == typeof(ResourceTypes.Translokator.Object) ||
                    TranslokatorTree.SelectedNode.Tag.GetType() == typeof(Instance))
                {
                    TranslokatorContext.Items[4].Visible = true;
                    TranslokatorContext.Items[5].Visible = true;
                }
            }

            bool nonVisible = true;
            for (int i = 0; i != TranslokatorContext.Items.Count; i++)
            {
                if (TranslokatorContext.Items[i].Visible)
                    nonVisible = false;
            }

            if(nonVisible)
                e.Cancel = true;
        }

        private void AddInstance_Click(object sender, EventArgs e)
        {
            ResourceTypes.Translokator.Object obj = (TranslokatorTree.SelectedNode.Tag as ResourceTypes.Translokator.Object);
            Instance instance = new Instance();
            TreeNode instanceNode = new TreeNode(obj.Name + " " + TranslokatorTree.SelectedNode.GetNodeCount(false));
            instanceNode.Tag = instance;
            TranslokatorTree.SelectedNode.Nodes.Add(instanceNode);
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                if (TranslokatorTree.SelectedNode != null && TranslokatorTree.SelectedNode.Tag != null)
                    TranslokatorTree.Nodes.Remove(TranslokatorTree.SelectedNode);
            }
        }

        private void AddObjectOnClick(object sender, EventArgs e)
        {
            ObjectGroup group = (TranslokatorTree.SelectedNode.Tag as ObjectGroup);
            ResourceTypes.Translokator.Object obj = new ResourceTypes.Translokator.Object();
            TreeNode instanceNode = new TreeNode(obj.Name + " " + TranslokatorTree.SelectedNode.GetNodeCount(false));
            instanceNode.Tag = obj;
            TranslokatorTree.SelectedNode.Nodes.Add(instanceNode);
        }

        private void DeleteInstance_Click(object sender, EventArgs e)
        {
            if (TranslokatorTree.SelectedNode != null && TranslokatorTree.SelectedNode.Tag != null)
                TranslokatorTree.Nodes.Remove(TranslokatorTree.SelectedNode);
        }

        private void DeleteObject_Click(object sender, EventArgs e)
        {
            if (TranslokatorTree.SelectedNode != null && TranslokatorTree.SelectedNode.Tag != null)
                TranslokatorTree.Nodes.Remove(TranslokatorTree.SelectedNode);
        }

        private void CopyButton_Click(object sender, EventArgs e)
        {
            if (TranslokatorTree.SelectedNode != null && TranslokatorTree.SelectedNode.Tag != null)
            {
                if(TranslokatorTree.SelectedNode.Tag is Instance || TranslokatorTree.SelectedNode.Tag is ResourceTypes.Translokator.Object)
                {
                    clipboard = TranslokatorTree.SelectedNode.Tag;
                }
            }
        }

        private void PasteButton_Click(object sender, EventArgs e)
        {
            var data = clipboard;
            if (data != null)
            {      
                if (TranslokatorTree.SelectedNode != null && TranslokatorTree.SelectedNode.Tag != null)
                {
                    if(TranslokatorTree.SelectedNode.Tag is Instance && data is Instance)
                        TranslokatorTree.SelectedNode.Tag = data;
                    if (TranslokatorTree.SelectedNode.Tag is ResourceTypes.Translokator.Object && data is ResourceTypes.Translokator.Object)
                        TranslokatorTree.SelectedNode.Tag = data;
                }
            }
            PropertyGrid.SelectedObject = TranslokatorTree?.SelectedNode.Tag;
        }
    }
}