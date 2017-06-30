using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckupExec.Models
{
    public class BEServer
    {
        public string Name { get; set; }

        private string be_id;
        public string Id
        {
            get
            {
                return be_id;
            }
            set
            {
                be_id = value;
            }
        }

        public int BackupExecServerType { get; set; }

        public Boolean IsPrivateCloudServer { get; set; }

        public string Description { get; set; }

        public Boolean IsLocal { get; set; }

        //*switch/enum?
        public int SsoType { get; set; }

        public int Status { get; set; }
        //*

        public string Version { get; set; }

        public ServerInformation ServerInformation { get; set; }

        private string si_id;
        public string ServerInformationId
        {
            get
            {
                return si_id;
            }
            set
            {
                si_id = value;
            }
        }
    }
}
