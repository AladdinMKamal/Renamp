﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Renamp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("-This application changes any c# project's name.. including all  properties and namespace.\n\n" + "-The default projects' directory is\n" + @"   ' C:\Users\XXX\Documents\Visual Studio 2015\Projects\ '" + "\n-The new project's name can only contain letters, numbers, - and _. \n\n -Please close all directories before renaming.", "About", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsLetterOrDigit(e.KeyChar) && e.KeyChar != '_' && e.KeyChar != '-' && e.KeyChar != (char)8)
            {
                e.Handled = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Trim() == "" || textBox2.Text.Trim() == "" || textBox3.Text.Trim() == "" && checkBox1.Checked == true)
            {
                MessageBox.Show("Please fill all the fields.");
                return;
            }
            else if (textBox1.Text == textBox2.Text)
            {
                MessageBox.Show("Please change new project name.");
            }
            else if (textBox2.Text.Contains(" "))
            {
                MessageBox.Show("New project name can't contain spaces, please try again.");

            }

            String newName = textBox2.Text;
            String oldName1 = textBox1.Text;
            String oldName2 = "";
            if (oldName1.Contains(' '))
            {
                oldName2 = oldName1;
                oldName2 = oldName2.Replace(' ', '_');
            }

            String oldPath;
            String newPath;
            String Backup;

            if (textBox3.Text.Trim() != "")
            {
                oldPath = textBox3.Text + @"\" + oldName1 + @"\";
                newPath = textBox3.Text + @"\" + newName + @"\";
                Backup = textBox3.Text + @"\Renamp Backup\" + oldName1 + @"\";
                String oldPathx = oldPath + oldName1 + @"\";
                if (!Directory.Exists(oldPathx))
                {
                    MessageBox.Show("Incorrect project name or directory, please try again.");
                    return;
                }
            }
            else
            {
                oldPath = @"C:\Users\" + Environment.UserName + @"\Documents\Visual Studio 2015\Projects\" + oldName1 + @"\";
                newPath = @"C:\Users\" + Environment.UserName + @"\Documents\Visual Studio 2015\Projects\" + newName + @"\";
                Backup = @"C:\Users\" + Environment.UserName + @"\Documents\Visual Studio 2015\Projects\Renamp Backup\" + oldName1 + @"\";
                if (!Directory.Exists(oldPath))
                {
                    MessageBox.Show("Incorrect project name, please try again.");
                    return;
                }
            }

            foreach (string dirPath in Directory.GetDirectories(oldPath, "*",
                SearchOption.AllDirectories))
                Directory.CreateDirectory(dirPath.Replace(oldPath, Backup));

            foreach (string newPath1 in Directory.GetFiles(oldPath, "*.*",
                SearchOption.AllDirectories))
                File.Copy(newPath1, newPath1.Replace(oldPath, Backup), true);

            Directory.Move(oldPath, newPath);

            System.IO.File.Move(newPath + oldName1 + ".sln", newPath + newName + ".sln");

            fileEditor(newPath + newName + ".sln", oldName1, newName, oldName2);

            oldPath = newPath + oldName1 + @"\";
            newPath += newName + @"\";
            Directory.Move(oldPath, newPath);

            if (Directory.Exists(newPath + @"bin\"))
                DeleteDirectory(newPath + @"bin\");
            if (Directory.Exists(newPath + @"obj\"))
                DeleteDirectory(newPath + @"obj\");

            System.IO.File.Move(newPath + oldName1 + ".csproj", newPath + newName + ".csproj");
            fileEditor(newPath + newName + ".csproj", oldName1, newName, oldName2);

            String[] files = System.IO.Directory.GetFiles(newPath, "*.cs");
            foreach (String c in files)
            {
                fileEditor(c, oldName1, newName, oldName2);
            }
            files = System.IO.Directory.GetFiles(newPath, "*.xaml");
            foreach (String c in files)
            {
                fileEditor(c, oldName1, newName, oldName2);
            }

            newPath = newPath + @"Properties\";
            files = System.IO.Directory.GetFiles(newPath, "*.cs");

            foreach (String c in files)
            {
                fileEditor(c, oldName1, newName, oldName2);
            }

            DialogResult result = MessageBox.Show("Project name has been successfully changed.\nDo you want to change another project name ?", "SUCCESS!!", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (result == DialogResult.No)
            {
                Application.Exit();
            }
            else if (result == DialogResult.Yes)
            {
                textBox1.Text = "";
                textBox2.Text = "";
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                this.ClientSize = new System.Drawing.Size(395, 180);
                button1.Location = new System.Drawing.Point(200, 140);
                textBox3.Visible = true;
            }
            else
            {
                this.ClientSize = new System.Drawing.Size(395, 150);
                button1.Location = new System.Drawing.Point(200, 110);
                textBox3.Visible = false;
                textBox3.Text = "";

            }
        }
        public static void DeleteDirectory(string target_dir)
        {
            string[] files = Directory.GetFiles(target_dir);
            string[] dirs = Directory.GetDirectories(target_dir);

            foreach (string file in files)
            {
                File.SetAttributes(file, FileAttributes.Normal);
                File.Delete(file);
            }

            foreach (string dir in dirs)
            {
                DeleteDirectory(dir);
            }

            Directory.Delete(target_dir, false);
        }

        public static void fileEditor(String path, String oldN1, String newN, String oldN2)
        {
            StreamReader reader = new StreamReader(path);
            string input = reader.ReadToEnd();
            reader.Close();
            input = input.Replace(oldN1, newN);
            if (oldN2 != "")
                input = input.Replace(oldN2, newN);
            StreamWriter writer = new StreamWriter(path);
            writer.Write(input);
            writer.Close();
        }
    }
}
