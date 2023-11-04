using CommunityToolkit.Mvvm.ComponentModel;
using RozmieniarkaApp.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RozmieniarkaApp.Models
{
    /// <summary>
    /// Class containing rozmieniarka's status information.
    /// </summary>
    internal class MachineStatusModel
    {
        public Color isMachineOkColor;
        public Color isHopper5OkColor;
        public Color isHopper2OkColor;
        public Color isHopper1OkColor;
        public Color isCasetteOkColor;
        public Color isDoorOkColor;
        public Color isReaderOkColor;
        public int numberOfBanknotesAvailable;
        public void FillMachineStatusFromStatusQuery(string reply)
        {
            isMachineOkColor =
                reply[5] == '0' ? Colors.Green :
                    reply[5] == '1' ? Colors.Red :
                        reply[5] == '2' ? Colors.Yellow : Colors.Black;
            isHopper5OkColor = 
                reply[2] == '0' ? Colors.Green :
                    reply[2] == '1' ? Colors.Yellow :
                        reply[2] == '2' ? Colors.Red : Colors.Black;
            isHopper2OkColor =
                reply[1] == '0' ? Colors.Green :
                    reply[1] == '1' ? Colors.Yellow :
                        reply[1] == '2' ? Colors.Red : Colors.Black;
            isHopper1OkColor =
                reply[0] == '0' ? Colors.Green :
                    reply[0] == '1' ? Colors.Yellow :
                        reply[0] == '2' ? Colors.Red : Colors.Black;
            isCasetteOkColor =
                reply[6] == '0' ? Colors.Green :
                    reply[6] == '1' ? Colors.Red : Colors.Black;
            isDoorOkColor =
                reply[7] == '0' ? Colors.Green :
                    reply[7] == '1' ? Colors.Red : Colors.Black;
            isReaderOkColor =
                reply[8] == '0' ? Colors.Green :
                    reply[8] == '1' ? Colors.Red : Colors.Black;
            numberOfBanknotesAvailable = Convert.ToInt32(reply.Substring(3, 2));
        }
    }
}
