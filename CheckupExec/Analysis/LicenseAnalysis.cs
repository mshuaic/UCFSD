using CheckupExec.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckupExec.Analysis
{
    //license model

    public class LicenseAnalysis
    {
        //likely will be called straight from frontend forecast to tie the forecast with the user's licensing setup
        public LicenseAnalysis()
        {
            var editionInformationAsList = DataExtraction.EditionInformationController.GetEditionInformation();

            var editionInformation = editionInformationAsList.First();

            var serverEdition = (editionInformation == null) ? "" : editionInformation.Edition;

            if (serverEdition.Equals("Capacity Edition"))
            {
                //todo
            }
            else if (serverEdition.Equals("Capacity Edition Lite"))
            {
                //todo
            }

            //add to license model
            
        }
    }
}
