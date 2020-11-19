using System;
using System.IO;
using System.Text;

namespace AdvanceTDL
{
    public class infoSK : IComparable<infoSK>
    {
        private string id;
        private string IsPast;
        private string IsRemind;
        private int time2remind;
        private string typeOfLoop;
        private string numOfLoop;

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
            typeOfLoop = "0";
            numOfLoop = "0";
        }
        public infoSK(string id, string TenSK, string MotaSK,
            DateTime date, string IsPast, string IsRemind, int time2remind, string isLoop, string typeOfLoop, string numOfLoop)
        {
            this.id = id;
            this.TenSK = TenSK;
            this.MotaSK = MotaSK;
            this.getDate = date;
            this.IsPast = IsPast;
            this.IsRemind = IsRemind;
            this.time2remind = time2remind;
            this.GetLoop = isLoop;
            this.typeOfLoop = typeOfLoop;
            this.numOfLoop = numOfLoop;
        }

        public int NumOfLoop{
            get { return int.Parse(numOfLoop); }
        }

        public DateTime getDate { get; set; }
        int IComparable<infoSK>.CompareTo(infoSK other)
        {
            return DateTime.Compare(this.getDate, other.getDate);
        }

        public string TenSK { get; }
        public string getTime()
        {
            return "Ngày: " + getDate.ToLongDateString() + "\n" + "Giờ: " + getDate.ToLongTimeString();
        }
        public string MotaSK { get; }
        public string GetLoop { get; }

        public int ThoiGianDemNguoc()
        {
            DateTime d = getDate.AddMinutes(MainWindow.TIME_REMIND[time2remind]);
            return (int)((d.Ticks - DateTime.Now.Ticks) / ((long)MainWindow.myConsts.TICKS_PER_SECOND * 10));
        }

        public bool MakeLoop()
        {
            if (numOfLoop.Equals("0")) return false;
            switch (typeOfLoop)
            {
                case "0":
                    getDate = getDate.AddDays(7);
                    break;
                case "1":
                    getDate = getDate.AddMonths(1);
                    break;
                case "2":
                    getDate = getDate.AddYears(1);
                    break;
                default:
                    getDate = getDate.AddDays(7);
                    break;
            }
            numOfLoop = "" + (int.Parse(numOfLoop) - 1);

            DateTime d = getDate.AddMinutes(MainWindow.TIME_REMIND[time2remind]);
            string newLine = string.Format(MainWindow.strDataFormat, id, TenSK, MotaSK,
                getDate.DayOfWeek.ToString(), getDate.Day,
                getDate.Month, getDate.Year, d.Hour, d.Minute,
                0, IsRemind, time2remind, GetLoop, typeOfLoop, numOfLoop);

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
            return true;
        }

        public override string ToString()
        {
            return string.Format(MainWindow.strDataFormat, id, TenSK, MotaSK,
                getDate.DayOfWeek.ToString(), getDate.Day,
                getDate.Month, getDate.Year, getDate.Hour, getDate.Minute,
                IsPast, IsRemind, time2remind, GetLoop, typeOfLoop, numOfLoop);
        }

        public string GetInfor()
        {
            return "Tên sự kiện: " + TenSK + "\n" + "Vị trí: " + MotaSK + "\n";
        }
    }
}
