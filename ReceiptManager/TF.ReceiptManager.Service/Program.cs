
#region Copyright

/* Copyright 2011 (c), ThoughtFocus.
   
  All rights are reserved.  Reproduction or transmission in whole or in part, in any form or by any means, electronic, mechanical or 
  otherwise, is prohibited without the prior written consent of the copyright owner.
 
  Author(s): KRISHNA NSS
  File Name: Program.cs
  Description: This class is the main entry point for the Receipt Manager service.
  Date Created : 01-Jun-2011
  Revision History: 
  */

#endregion

#region Namespaces

using System.ServiceProcess;

#endregion

namespace TF.ReceiptManager.Service
{
    #region Program

    /// <summary>
    /// This class is the main entry point for the Receipt Manager service.
    /// </summary>
    static class Program
    {
        #region Main

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
			{ 
				new ReceiptService() 
			};
            ServiceBase.Run(ServicesToRun);
        }

        #endregion
    }

    #endregion
}
