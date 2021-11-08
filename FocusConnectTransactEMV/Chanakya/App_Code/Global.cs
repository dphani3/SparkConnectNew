
#region Copyright

/* Copyright 2010 (c), ThoughtFocus.
   
  All rights are reserved.  Reproduction or transmission in whole or in part, in any form or by any means, electronic, mechanical or 
  otherwise, is prohibited without the prior written consent of the copyright owner.
 
  Author(s): KRISHNA NSS
  File Name: Global.cs
  Description: Defines the methods, properties, and events that are common to all application objects in a FocusConnect web application. 
               It primarily creates the custom performance counters for the FocusConnect on application start.
  Date Created : 17-Sep-2010
  Revision History: 
  */

#endregion

#region namespaces

using System;
using System.Diagnostics;
using System.Web;

#endregion

#region Global

/// <summary>
/// Defines the methods, properties, and events that are common to all application objects in a FocusConnect web application. It primarily 
/// creates the custom performance counters for the FocusConnect on application start.
/// </summary>
public class Global : HttpApplication
{

#if LOAD_TEST

        ////FocusConnect custom performance object.
        //const string CONNECT_TRANSACTIONS_CATEGORY = "FocusConnect";

        ////FocusConnect custom performance counter for TPS(Transactions Per Second).
        //const string CONNECT_TPS = "TPS";

        ////FocusConnect custom performance counter for Active Transactions.
        //const string CONNECT_ACTIVE_TRANSACTIONS = "ActiveTransactions";        

        ////FocusConnect custom performance counter for Failed Transactions.
        //const string CONNECT_FAILED_TRANSACTIONS = "FailedTransactions";

        ////FocusConnect custom performance counter for Transaction Time.
        //const string CONNECT_TRANSACTION_TIME = "TransactionTime";

        ////FocusConnect custom performance counter for Transaction Time Base.
        //const string CONNECT_TRANSACTION_TIME_BASE = "TransactionTimeBase";

        ////FocusConnect custom performance counter object for TPS(Transactions Per Second).
        //private PerformanceCounter tpsCounter = null;

        ////FocusConnect custom performance counter object for Active Transactions.
        //private PerformanceCounter activeTransactionsCounter = null;

        ////FocusConnect custom performance counter object for Failed Transactions.
        //private PerformanceCounter failedTransactionsCounter = null;

        ////FocusConnect custom performance counter object for Transaction Time.
        //private PerformanceCounter transactionTimeCounter = null;

        ////FocusConnect custom performance counter object for Transaction Time Base.
        //private PerformanceCounter transactionTimeBaseCounter = null;

#endif

    #region Global Constructor

    /// <summary>
    /// Constructor for Global class.
    /// </summary>
    public Global()
    {
    }

    #endregion

    #region Application_Start

    /// <summary>
    /// Code that runs when an application is started.
    /// </summary>
    /// <param name="sender">sender.</param>
    /// <param name="e">Event Arguments.</param>
    void Application_Start(object sender, EventArgs e)
    {

#if LOAD_TEST

        ////Initialize the FocusConnect TPS performance counter object and store it in Application state.
        //tpsCounter = new PerformanceCounter(CONNECT_TRANSACTIONS_CATEGORY, CONNECT_TPS, false);
        //Application["tpsCounter"] = tpsCounter;
        //tpsCounter.RawValue = 0;

        ////Initialize the FocusConnect Active Transactions performance counter object and store it in Application state.
        //activeTransactionsCounter = new PerformanceCounter(CONNECT_TRANSACTIONS_CATEGORY, CONNECT_ACTIVE_TRANSACTIONS, false);
        //Application["activeTransactionsCounter"] = activeTransactionsCounter;
        //activeTransactionsCounter.RawValue = 0;

        ////Initialize the FocusConnect Failed Transactions performance counter object and store it in Application state.
        //failedTransactionsCounter = new PerformanceCounter(CONNECT_TRANSACTIONS_CATEGORY, CONNECT_FAILED_TRANSACTIONS, false);
        //Application["failedTransactionsCounter"] = failedTransactionsCounter;
        //failedTransactionsCounter.RawValue = 0;

        ////Initialize the FocusConnect Transaction Time performance counter object and store it in Application state.
        //transactionTimeCounter = new PerformanceCounter(CONNECT_TRANSACTIONS_CATEGORY, CONNECT_TRANSACTION_TIME, false);
        //Application["transactionTimeCounter"] = transactionTimeCounter;
        //transactionTimeCounter.RawValue = 0;

        ////Initialize the FocusConnect Transaction Time Base performance counter object and store it in Application state.
        //transactionTimeBaseCounter = new PerformanceCounter(CONNECT_TRANSACTIONS_CATEGORY, CONNECT_TRANSACTION_TIME_BASE, false);
        //Application["transactionTimeBaseCounter"] = transactionTimeBaseCounter;
        //transactionTimeBaseCounter.RawValue = 0;
#endif

    }

    #endregion

    #region Application_End

    /// <summary>
    /// Code that runs when an application ends.
    /// </summary>
    /// <param name="sender">sender.</param>
    /// <param name="e">Event Arguments.</param>
    void Application_End(object sender, EventArgs e)
    {

#if LOAD_TEST

        ////Dispose the TPS(Transactions Per Second) performance counter object.
        //if (tpsCounter != null)
        //{
        //    tpsCounter.Close();
        //    tpsCounter.Dispose();
        //    tpsCounter = null;
        //}

        ////Dispose the Active Transactions performance counter object.
        //if (activeTransactionsCounter != null)
        //{
        //    activeTransactionsCounter.Close();
        //    activeTransactionsCounter.Dispose();
        //    activeTransactionsCounter = null;
        //}

        ////Dispose the Failed Transactions performance counter object.
        //if (failedTransactionsCounter != null)
        //{
        //    failedTransactionsCounter.Close();
        //    failedTransactionsCounter.Dispose();
        //    failedTransactionsCounter = null;
        //}

        ////Dispose the Transaction Time performance counter object.
        //if (transactionTimeCounter != null)
        //{
        //    transactionTimeCounter.Close();
        //    transactionTimeCounter.Dispose();
        //    transactionTimeCounter = null;
        //}

        ////Dispose the Transaction Time Base performance counter object.
        //if (transactionTimeBaseCounter != null)
        //{
        //    transactionTimeBaseCounter.Close();
        //    transactionTimeBaseCounter.Dispose();
        //    transactionTimeBaseCounter = null;
        //}
#endif

    }

    #endregion

    #region Application_Error

    /// <summary>
    /// Code that runs when an unhandled error occurs.
    /// </summary>
    /// <param name="sender">sender.</param>
    /// <param name="e">Event Arguments.</param>
    void Application_Error(object sender, EventArgs e)
    {

    }

    #endregion

    #region Session_Start

    /// <summary>
    /// Code that runs when a new session is started.
    /// </summary>
    /// <param name="sender">sender.</param>
    /// <param name="e">Event Arguments.</param>
    void Session_Start(object sender, EventArgs e)
    {

    }

    #endregion

    #region Session_End

    /// <summary>
    /// Code that runs when a session ends. 
    /// </summary>
    /// <param name="sender">sender.</param>
    /// <param name="e">Event Arguments.</param>
    void Session_End(object sender, EventArgs e)
    {
        // Note: The Session_End event is raised only when the sessionstate mode
        // is set to InProc in the Web.config file. If session mode is set to StateServer 
        // or SQLServer, the event is not raised.
    }

    #endregion
}

#endregion
