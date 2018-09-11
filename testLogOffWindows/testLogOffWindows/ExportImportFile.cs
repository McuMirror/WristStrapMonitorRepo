using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace testLogOffWindows
{
    class ExportImportFile
    {

        ListView lv;

        public ExportImportFile()
        {
            lv = new ListView();

        }
        public static void StringArrayToFile(List<string> lstr,string fileName)
        {
            ListView slv2 = new ListView();
            ListViewItem slvi = new ListViewItem();
            foreach (string s in lstr)
            {
                slvi.SubItems.Add(s);
            }
            slv2.Items.Add(slvi);
            export2File(slv2, ",", fileName);
        }
        public static List<string> FileToStringArray(string fileName)
        {
            List<string> lstr = new List<string>();
            ListView slv = new ListView();

            bool exists = System.IO.File.Exists(fileName);
            if (exists)
            {
                importFromFile(slv, ",", fileName);
            }
           

            ListViewItem lvi = new ListViewItem();
            if(slv.Items.Count>0)
            {
                lvi = slv.Items[0];

                for (int a = 0; a < lvi.SubItems.Count; a++)
                {
                    lstr.Add(lvi.SubItems[a].Text);
                }

            }
         

            return lstr;
        }





        private static void importFromFile(ListView lv, string splitter, string filename)
        {

            List<string> list = new List<string>();
            using (StreamReader reader = new StreamReader(filename))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    list.Add(line); // Add to list.
                    Console.WriteLine(line); // Write to console.
                }
            }

            lv.Items.Clear();
            foreach (string nlist in list)
            {
                string[] arr = nlist.Split(',');

                int arrlen = arr.Length;
                string[] arrLast = arr[arrlen - 1].Split(';');
                arr[arrlen - 1] = arrLast[0];


                //string[] arr = new string[3];
                ListViewItem itm;
                itm = new ListViewItem(arr);
                lv.Items.Add(itm);
            }


        }
        private static void export2File(ListView lv, string splitter, string filename)
        {
            // string filename = "";
            if (filename != "")
            {
                using (StreamWriter sw = new StreamWriter(filename))
                {
                    foreach (ListViewItem item in lv.Items)
                    {
                        for (int i = 1; i < item.SubItems.Count; i++)
                        {
                            if (i == item.SubItems.Count - 1)
                            {
                                sw.WriteLine("{0};", item.SubItems[i].Text);
                            }
                            else
                            {
                                sw.Write("{0}{1}", item.SubItems[i].Text, splitter);
                            }
                        }
                    }
                }
            }
        }


    }
}
