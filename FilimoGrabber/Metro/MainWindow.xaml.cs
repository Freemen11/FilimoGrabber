using MahApps.Metro.Controls;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace Metro
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        bool cont;
        public string result;
        public string pagecode;
        string source;
        string start;

        List<string> s = new List<string>();
        List<string> links = new List<string>();

        public MainWindow()
        {

            InitializeComponent();
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            #region getchars
            s.Add("ا");
            s.Add("ب");
            s.Add("پ");
            s.Add("ت");
            s.Add("س");
            s.Add("ج");
            s.Add("چ");
            s.Add("ح");
            s.Add("خ");
            s.Add("د");
            s.Add("ذ");
            s.Add("ر");
            s.Add("ز");
            s.Add("ژ");
            s.Add("س");
            s.Add("ش");
            s.Add("ص");
            s.Add("ض");
            s.Add("ط");
            s.Add("ظ");
            s.Add("ع");
            s.Add("غ");
            s.Add("ف");
            s.Add("ق");
            s.Add("ک");
            s.Add("گ");
            s.Add("ل");
            s.Add("م");
            s.Add("ن");
            s.Add("و");
            s.Add("ه");
            s.Add("ی");
            #endregion
        }


        private void btngrab_Click(object sender, RoutedEventArgs e)
        {
            txtresult.Text = "";
           
            try
            {
                string path = System.AppDomain.CurrentDomain.BaseDirectory + @"\Links.txt";
                using (WebClient client = new WebClient()) // WebClient class inherits IDisposable
                {
                    txtresult.Text = "";
                    client.Encoding = Encoding.UTF8;
                    string line;
                    if (File.Exists(path))
                    {
                        if (File.ReadAllText(path) != "" )
                        {
                            Mouse.OverrideCursor = Cursors.Wait;
                            System.IO.StreamReader file = new System.IO.StreamReader(path);
                            while ((line = file.ReadLine()) != null)
                            {
                                links.Add(line);
                            }

                            foreach (string link in links)
                            {

                                pagecode = client.DownloadString(link);
                                source = pagecode;

                                start = Regex.Replace(source, "(<span class=\"comment-name\" title=\")[^\"]+(\">)", "$1REPLACE$2");



                                Regex regex = new Regex("<span class=\"comment - name\" title=\"REPLACE\">(.*)</span>");
                                var list1 = regex.Matches(start);

                                var r = new Regex("(<span class=\"comment-name\" title=\"REPLACE\")(.*?)+(</span>)", RegexOptions.Singleline);
                                foreach (Match m in r.Matches(start))
                                {
                                    try
                                    {
                                        cont = false;
                                        StringBuilder builder = new StringBuilder(m.ToString());
                                        builder.Replace("<span class=\"comment-name\" title=\"REPLACE\">", "");
                                        builder.Replace("</span>", "");
                                        string end = builder.ToString();
                                        foreach (string a in s)
                                        {
                                            if (end.Contains(a))
                                            {
                                                cont = true;
                                            }
                                        }

                                        if (cont == false)
                                        {
                                            txtresult.Text += builder + Environment.NewLine;
                                        }
                                        else
                                        {
                                            continue;
                                        }
                                    }
                                    finally
                                    {
                                        Mouse.OverrideCursor = Cursors.Arrow;
                                        txtresult.Cursor = Cursors.IBeam;
                                        btnabout.Cursor = Cursors.Hand;
                                        btngrab.Cursor = Cursors.Hand;
                                        btnnewpostlink.Cursor = Cursors.Hand;
                                        btnsave.Cursor = Cursors.Hand;
                                    }
                                }
                            }
                        }
                    else
                        {
                            MessageBox.Show("Links File Empty Add Some Post Links ", "Sorry", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Links File Not Found Program Will Automaticly Create it PLZ Add links to it", "Sorry", MessageBoxButton.OK, MessageBoxImage.Information);
                        File.Create(path);
                        Mouse.OverrideCursor = Cursors.Arrow;
                        txtresult.Cursor = Cursors.IBeam;
                        btnabout.Cursor = Cursors.Hand;
                        btngrab.Cursor = Cursors.Hand;
                        btnnewpostlink.Cursor = Cursors.Hand;
                        btnsave.Cursor = Cursors.Hand;
                    }
                }
                }
            catch { }
        }   
            
        


        private void btnnewpostlink_Click(object sender, RoutedEventArgs e)
        {
            string path = System.AppDomain.CurrentDomain.BaseDirectory + @"\Links.txt";
            if (File.Exists(path))
            {
                Process.Start("notepad.exe", path);
            }
            else
            {
                MessageBox.Show("Links File Not Found Program Will Automaticly Create it PLZ Add links to it", "Sorry", MessageBoxButton.OK, MessageBoxImage.Error);
                File.Create(path);               
            }
        }

        private void btnsave_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog()
            {
                Filter = "Text Files(*.txt)|*.txt|All(*.*)|*"
            };

            if (dialog.ShowDialog() == true)
            {
                File.WriteAllText(dialog.FileName, txtresult.Text);
            }
        }

        private void btnabout_Click(object sender, RoutedEventArgs e)
        {
            Window w = new About();
            w.ShowDialog();
        }
    }
}
