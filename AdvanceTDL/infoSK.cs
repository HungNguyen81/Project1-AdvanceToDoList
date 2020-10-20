using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvanceTDL
{
    public class infoSK : IComparable<infoSK>
    {
        private string id;
        private string TenSK;
        private string MotaSK;
        private DateTime date;
        private string IsPast;
        private string IsRemind;
        public infoSK(string id, string TenSK, string MotaSK, DateTime date, string IsPast, string IsRemind)
        {
            this.id = id;
            this.TenSK = TenSK;
            this.MotaSK = MotaSK;
            this.date = date;
            this.IsPast = IsPast;
            this.IsRemind = IsRemind;
        }

        public DateTime Date
        {
            get { return date; }
        }
        int IComparable<infoSK>.CompareTo(infoSK other)
        {
            return DateTime.Compare(this.Date, other.Date);
        }

        public override string ToString()
        {
            return id + '\t' + TenSK + '\t' + MotaSK + '\t' 
                + date.DayOfWeek.ToString() + '\t' + date.Day + '\t' 
                + date.Month + '\t' + date.Year + '\t' + date.Hour + '\t' + date.Minute + '\t'
                + IsPast + '\t' + IsRemind;
        }

        public string GetInform()
        {
            return "Tên sự kiện: " + TenSK + "\n" + "Mô tả: " + MotaSK + "\n";
        }
    }
}
