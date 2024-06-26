using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RozmieniarkaApp.Models
{
    public class TWStatusModel
    {
        public int IsTaxingEnabled { get; set; }
        public int IsCarWashWorking { get; set; }
        public TWStatusModel()
        {
            this.IsTaxingEnabled = -1;
            this.IsCarWashWorking = -1;
        }
    }
}
