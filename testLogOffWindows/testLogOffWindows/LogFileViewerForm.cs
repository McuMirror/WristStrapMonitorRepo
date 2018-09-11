using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.IO;   



namespace testLogOffWindows
{
    public partial class LogFileViewerForm : Form
    {
        string logFolder;
        public LogFileViewerForm(string vlogFolder)
        {
            InitializeComponent();
            logFolder = vlogFolder;
            this.WindowState = FormWindowState.Maximized;


            listView1.View = View.Details;
            listView1.GridLines = true;
            listView1.FullRowSelect = true;

            //Add column header
            listView1.Columns.Add("File Name", 200);
            listView1.Columns.Add("  ", 50);

            populateLV(logFolder);
           

        }

        private void populateLV(string directory)
        {
            listView1.Items.Clear();
            DirectoryInfo d = new DirectoryInfo(directory);//Assuming Test is your Folder
            FileInfo[] Files = d.GetFiles("*"); //Getting Text files
            string str = "";
            foreach (FileInfo file in Files)
            {
                string[] arr = new string[1];
                arr[0] = file.ToString();
                //string[] arr = new string[3];
                ListViewItem itm;
                itm = new ListViewItem(arr);
                listView1.Items.Add(itm);

            }



           
        


            
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

              ListView.SelectedIndexCollection indices = listView1.SelectedIndices;
              if (indices.Count > 0)
              {
                  int selectedRow = indices[0];
                  int totalRow = listView1.Items.Count;
                  if (totalRow > 0)
                  {

                      for (int a = 0; a < totalRow; a++)
                      {
                          listView1.Items[a].ForeColor = Color.Black;
                      }

                      listView1.Items[selectedRow].ForeColor = Color.Blue;
                  }

                  string filename = @"logFiles/" + listView1.Items[selectedRow].Text;
                  bool exists = System.IO.File.Exists(filename);
                  if (exists)
                  {
                      using (StreamReader sr = new StreamReader(filename))
                      {
                          // Read the stream to a string, and write the string to the console.
                          String line = sr.ReadToEnd();
                          richTextBox1.Text = line;
                      }

                  }

              }



          
        }

        private void selectReportBtn_Click(object sender, EventArgs e)
        {
              ListView.SelectedIndexCollection indices = listView1.SelectedIndices;
              if (indices.Count > 0)
              {
                  int selectedRow = indices[0];
              

                  string filename = @"logFiles/" + listView1.Items[selectedRow].Text;
                  bool exists = System.IO.File.Exists(filename);
                  if (exists)
                  {
                      using (StreamReader sr = new StreamReader(filename))
                      {
                          // Read the stream to a string, and write the string to the console.
                          String line = sr.ReadToEnd();
                          richTextBox1.Text = line;
                          saveFileDialog1.FileName = listView1.Items[selectedRow].Text;
                          saveFileDialog1.Filter = "Text (*.txt)|*.txt|Word Doc (*.doc)|*.doc";
                          saveFileDialog1.ShowDialog();

                      }

                  }

              }
              else
              {
                  MessageBox.Show("Please select at least one log file", "Error");
              }
            

        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            string fname = saveFileDialog1.FileName;
            // Write to the file name selected.
            // ... You can write the text from a TextBox instead of a string literal.
            File.WriteAllText(fname, richTextBox1.Text);

        }

        private void deleteLogBtn_Click(object sender, EventArgs e)
        {
            ListView.SelectedIndexCollection indices = listView1.SelectedIndices;
            if (indices.Count > 0)
            {
                int selectedRow = indices[0];
                string filename = @"logFiles/" + listView1.Items[selectedRow].Text;
                bool exists = System.IO.File.Exists(filename);
                if (exists)
                {
                    DialogResult result = MessageBox.Show("Do you want to delete?", "Confirmation", MessageBoxButtons.YesNoCancel);
                    if (result == DialogResult.Yes)
                    {
                        File.Delete(filename);
                    }
                   
                    populateLV(@"logFiles/");
                }
            }
            else
            {
                MessageBox.Show("Please select at least one log file to be delete", "Error");
            }
        }

    }
}
