using System;
using SAPbouiCOM;
using SAPbobsCOM;

namespace SAPB1HelloWorld
{
    /// <summary>
    /// SAP Business One Add-on to display "Hello World" when Add button is clicked in Sales Order
    /// </summary>
    class Program
    {
        private static SAPbouiCOM.Application SboApplication;
        private static SAPbobsCOM.Company SboCompany;

        static void Main(string[] args)
        {
            try
            {
                // Connect to SAP Business One Application
                ConnectToSAP();

                // Set up menu and event handlers
                SetupEventHandlers();

                // Keep the application running
                Console.WriteLine("SAP B1 Add-on Started. Press any key to exit...");
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                Console.ReadLine();
            }
            finally
            {
                // Clean up
                DisconnectFromSAP();
            }
        }

        /// <summary>
        /// Connects to SAP Business One Application using DI API
        /// </summary>
        private static void ConnectToSAP()
        {
            try
            {
                // Get SAP GUI API instance
                SAPbouiCOM.SboGuiApi SboGuiApi = new SAPbouiCOM.SboGuiApi();

                // Connect to a running SAP Business One application
                string sConnectionString = Environment.GetCommandLineArgs().GetValue(1).ToString();
                SboGuiApi.Connect(sConnectionString);
                SboApplication = SboGuiApi.GetApplication(-1);

                // Set up Company object
                SboCompany = (SAPbobsCOM.Company)SboApplication.Company.GetDICompany();

                Console.WriteLine("Successfully connected to SAP Business One");
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to connect to SAP B1: {ex.Message}");
            }
        }

        /// <summary>
        /// Sets up event handlers for Sales Order form
        /// </summary>
        private static void SetupEventHandlers()
        {
            // Subscribe to item events (button clicks, etc.)
            SboApplication.ItemEvent += new SAPbouiCOM._IApplicationEvents_ItemEventEventHandler(SboApplication_ItemEvent);

            Console.WriteLine("Event handlers registered successfully");
        }

        /// <summary>
        /// Event handler for SAP Business One item events
        /// </summary>
        private static void SboApplication_ItemEvent(string FormUID, ref SAPbouiCOM.ItemEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            try
            {
                // Check if this is a Sales Order form (Form Type: 139)
                // and the Add button (Item UID: 1) is clicked
                if (pVal.FormType == 139 &&
                    pVal.EventType == SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED &&
                    pVal.ItemUID == "1" &&
                    pVal.BeforeAction == false)
                {
                    // Display "Hello World" message
                    SboApplication.StatusBar.SetText("Hello World",
                        SAPbouiCOM.BoMessageTime.bmt_Short,
                        SAPbouiCOM.BoStatusBarMessageType.smt_Success);

                    // Also show as a message box
                    SboApplication.MessageBox("Hello World",
                        1,
                        "OK",
                        "",
                        "");

                    Console.WriteLine("Add button clicked in Sales Order - Hello World displayed!");
                }
            }
            catch (Exception ex)
            {
                SboApplication.StatusBar.SetText($"Error: {ex.Message}",
                    SAPbouiCOM.BoMessageTime.bmt_Short,
                    SAPbouiCOM.BoStatusBarMessageType.smt_Error);
            }
        }

        /// <summary>
        /// Disconnects from SAP Business One and releases resources
        /// </summary>
        private static void DisconnectFromSAP()
        {
            try
            {
                if (SboCompany != null && SboCompany.Connected)
                {
                    SboCompany.Disconnect();
                }

                // Release COM objects
                if (SboCompany != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(SboCompany);
                }

                if (SboApplication != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(SboApplication);
                }

                SboCompany = null;
                SboApplication = null;

                GC.Collect();
                GC.WaitForPendingFinalizers();

                Console.WriteLine("Disconnected from SAP Business One");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during disconnect: {ex.Message}");
            }
        }
    }
}
