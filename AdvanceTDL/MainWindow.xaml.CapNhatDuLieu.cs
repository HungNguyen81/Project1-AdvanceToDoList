using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AdvanceTDL
{
    public partial class MainWindow
    {
        private void UpdateData(string id, int indexOfItem)
        {
            string[] lines = File.ReadAllLines("data.csv");
            if (lines.Length > 0)
            {
                StringBuilder sb = new StringBuilder();
                string temp = null;
                foreach (string line in lines)
                {
                    string[] data = line.Split(',');
                    switch (indexOfItem)
                    {
                        case (int)myConsts.I_PAST:
                            if (data[0].Equals(id))
                            {
                                data[9] = "1";
                                temp = string.Join(",", data);
                            }
                            break;
                        case (int)myConsts.UPDATE_MISSED:   //-2
                            if (data[0].Equals(id))
                            {
                                data[9] = "2";
                                temp = string.Join(",", data);
                            }
                            break;
                        // Kiểm tra sự kiện đã xảy ra;
                        case (int)myConsts.UPDATE_PASSED:   // -1   
                            DateTime date = new DateTime(int.Parse(data[6]), int.Parse(data[5]),
                                int.Parse(data[4]), int.Parse(data[7]), int.Parse(data[8]), 0);
                            if (data[9].Equals("0") && DateTime.Compare(DateTime.Now, date) >= 0)
                            {
                                data[9] = "1";
                                sb.AppendLine(string.Join(",", data));
                            }
                            else
                            {
                                sb.AppendLine(line);
                            }
                            break;
                    }
                }
                if (temp != null)
                {
                    sb.AppendLine(temp);
                    foreach (string line in lines)
                    {
                        string[] data = line.Split(',');
                        if (data[0].Equals(id) == false)
                        {
                            sb.AppendLine(line);
                        }
                    }
                    File.WriteAllText("data.csv", sb.ToString());
                    UpdateID();
                    if (indexOfItem != -1) UpdateEvents();
                }
                else if (indexOfItem == -1)
                {
                    File.WriteAllText("data.csv", sb.ToString());
                }
            }
        }
        private void UpdateID()
        {
            int num = 0;
            string[] lines = File.ReadAllLines("data.csv");
            string[] ids = File.ReadAllLines("id.csv");
            foreach (string id in ids)
            {
                num = int.Parse(id);
            }
            if (lines.Length > 1)
            {
                StringBuilder sb = new StringBuilder();
                int newId = 0;
                foreach (string line in lines)
                {
                    string[] data = line.Split(',');
                    if (newId < num)
                    {
                        data[0] = "" + newId;
                        newId++;
                        sb.AppendLine(string.Join(",", data));
                    }
                }
                File.WriteAllText("data.csv", sb.ToString());
                File.WriteAllText("id.csv", newId + "");
            }
        }
        private void SortEvent()
        {
            string[] lines = File.ReadAllLines("data.csv");
            if (lines.Length > 0)
            {
                List<infoSK> listSK = new List<infoSK>();

                StringBuilder sb = new StringBuilder();
                DateTime[] date = new DateTime[lines.Length];

                foreach (string line in lines)
                {
                    string[] data = line.Split(',');
                    int i = 0;

                    date[i] = new DateTime(int.Parse(data[6]), int.Parse(data[5]), int.Parse(data[4]), 
                        int.Parse(data[7]), int.Parse(data[8]), 0);

                    listSK.Add(new infoSK(data[0], data[1], data[2], date[i++], data[9], 
                        data[10], int.Parse(data[11]), data[12], int.Parse(data[13]), int.Parse(data[14])));
                }
                listSK.Sort();
                foreach (infoSK s in listSK)
                {
                    sb.AppendLine(s.ToString());
                }
                File.WriteAllText("data.csv", sb.ToString());
                UpdateID();
                isUpdated = false;
            }
        }
        public void UpdateEvents()
        {            
            if (pnlSuKien.Children != null) pnlSuKien.Children.Clear();

            UpdateData(null, (int)myConsts.UPDATE_PASSED);
            SortEvent();
            string[] lines = File.ReadAllLines("data.csv");
            if (lines.Length > 0)
            {
                foreach (string line in lines)
                {
                    string[] data = line.Split(',');
                    Add_Event(data[0], data[1], data[2], new DateTime(int.Parse(data[6]), int.Parse(data[5]), int.Parse(data[4])),
                        int.Parse(data[7]), int.Parse(data[8]), data[9], data[10], data[11], data[12], data[13], data[14], true);
                }
            }
            if (mGadget != null) mGadget.Update(pnlSuKien);
            isUpdated = true;            
        }
        private void StoreData(int id, string tenSK, string motaSK, DateTime date,
            string isPast, string isRemind, int time_to_remind, string isLoop, string typeOfLoop, string numOfLoop)
        {
            string newLine = string.Format(strDataFormat, id, tenSK, motaSK, date.DayOfWeek,
                date.Day, date.Month, date.Year, date.Hour, date.Minute,
                isPast, isRemind, time_to_remind, isLoop, typeOfLoop, numOfLoop);
            csv = new StringBuilder();
            csv.AppendLine(newLine);
            File.AppendAllText("data.csv", csv.ToString());
            File.WriteAllText("id.csv", (id + 1) + "");
            csv.Clear();
        }
    }
}
