using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace AdvanceTDL
{
    public class infoSK : IComparable<infoSK>
    {
        private string id;
        private string TenSK;
        private string MotaSK;
        private string IsPast;
        private string IsRemind;
        private int time2remind;

        public infoSK(string id, string TenSK, string MotaSK,
            DateTime date, string IsPast, string IsRemind)
        {
            this.id = id;
            this.TenSK = TenSK;
            this.MotaSK = MotaSK;
            this.getDate = date;
            this.IsPast = IsPast;
            this.IsRemind = IsRemind;
            this.time2remind = 0;
            this.GetLoop = "0";
        }
        public infoSK(string id, string TenSK, string MotaSK,
            DateTime date, string IsPast, string IsRemind, int time2remind, string isLoop)
        {
            this.id = id;
            this.TenSK = TenSK;
            this.MotaSK = MotaSK;
            this.getDate = date;
            this.IsPast = IsPast;
            this.IsRemind = IsRemind;
            this.time2remind = time2remind;
            this.GetLoop = isLoop;
        }

        public DateTime getDate { get; set; }
        int IComparable<infoSK>.CompareTo(infoSK other)
        {
            return DateTime.Compare(this.getDate, other.getDate);
        }

        public string GetLoop { get; }

        public int ThoiGianDemNguoc()
        {
            DateTime d = getDate.AddMinutes(MainWindow.TIME_REMIND[time2remind]);
            return (int)((d.Ticks - DateTime.Now.Ticks) / ((long)MainWindow.myConsts.TICKS_PER_SECOND * 10));
        }

        public void MakeLoop()
        {
            getDate = getDate.AddDays(7);
            DateTime d = getDate.AddMinutes(MainWindow.TIME_REMIND[time2remind]);
            string newLine = string.Format(MainWindow.strDataFormat, id, TenSK, MotaSK,
                getDate.DayOfWeek.ToString(), getDate.Day,
                getDate.Month, getDate.Year, d.Hour, d.Minute,
                0, IsRemind, time2remind, GetLoop);

            StringBuilder sb = new StringBuilder();
            string[] lines = File.ReadAllLines("data.csv");
            foreach(string line in lines)
            {
                sb.AppendLine(line);
            }
            sb.AppendLine(newLine);
            File.WriteAllText("data.csv", sb.ToString());
            string[] ids = File.ReadAllLines("id.csv");
            int newId = int.Parse(ids[0]) + 1;
            File.WriteAllText("id.csv", newId + "");
        }

        public override string ToString()
        {
            return string.Format(MainWindow.strDataFormat, id, TenSK, MotaSK,
                getDate.DayOfWeek.ToString(), getDate.Day,
                getDate.Month, getDate.Year, getDate.Hour, getDate.Minute,
                IsPast, IsRemind, time2remind, GetLoop);
        }

        public string GetInfor()
        {
            return "Tên sự kiện: " + TenSK + "\n" + "Vị trí: " + MotaSK + "\n";
        }
    }
}
