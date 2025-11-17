using System;
using SAPbouiCOM;
using SAPbobsCOM;

namespace SAPB1HelloWorld
{
    /// <summary>
    /// SAP Business One Add-on to display "Hello World" when Add button is clicked in Sales Order,
    /// then automatically create a Delivery Note with the same data using DI API
    /// </summary>
    class Program
    {
        private static SAPbouiCOM.Application SboApplication;
        private static SAPbobsCOM.Company SboCompany;
        private static bool isProcessing = false; // Flag to prevent recursive processing

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

            // Subscribe to form data events (document add, update, etc.)
            SboApplication.FormDataEvent += new SAPbouiCOM._IApplicationEvents_FormDataEventEventHandler(SboApplication_FormDataEvent);

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
        /// Event handler for SAP Business One form data events (document add, update, etc.)
        /// </summary>
        private static void SboApplication_FormDataEvent(ref SAPbouiCOM.BusinessObjectInfo pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            try
            {
                // Check if this is a Sales Order (Object Type: 17) being added successfully
                if (pVal.FormTypeEx == "139" && // Sales Order form
                    pVal.EventType == SAPbouiCOM.BoEventTypes.et_FORM_DATA_ADD &&
                    pVal.BeforeAction == false &&
                    pVal.ActionSuccess == true &&
                    !isProcessing)
                {
                    // Get the document entry (DocEntry) of the newly created Sales Order
                    string docEntry = pVal.ObjectKey;

                    Console.WriteLine($"Sales Order {docEntry} created successfully. Creating Delivery Note...");

                    SboApplication.StatusBar.SetText($"Creating Delivery Note from Sales Order {docEntry}...",
                        SAPbouiCOM.BoMessageTime.bmt_Short,
                        SAPbouiCOM.BoStatusBarMessageType.smt_Warning);

                    // Create Delivery Note using DI API
                    CreateDeliveryFromSalesOrder(docEntry);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"FormDataEvent Error: {ex.Message}");
                SboApplication.StatusBar.SetText($"Error in FormDataEvent: {ex.Message}",
                    SAPbouiCOM.BoMessageTime.bmt_Long,
                    SAPbouiCOM.BoStatusBarMessageType.smt_Error);
            }
        }

        /// <summary>
        /// Creates a Delivery Note based on a Sales Order using DI API
        /// </summary>
        /// <param name="salesOrderDocEntry">The DocEntry of the Sales Order</param>
        private static void CreateDeliveryFromSalesOrder(string salesOrderDocEntry)
        {
            SAPbobsCOM.Documents oSalesOrder = null;
            SAPbobsCOM.Documents oDelivery = null;

            try
            {
                // Set processing flag to prevent recursive events
                isProcessing = true;

                // Check if company is connected
                if (SboCompany == null || !SboCompany.Connected)
                {
                    throw new Exception("Not connected to SAP Business One company");
                }

                // Get the Sales Order document
                oSalesOrder = (SAPbobsCOM.Documents)SboCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oOrders);

                if (!oSalesOrder.GetByKey(Convert.ToInt32(salesOrderDocEntry)))
                {
                    throw new Exception($"Sales Order {salesOrderDocEntry} not found");
                }

                Console.WriteLine($"Retrieved Sales Order {salesOrderDocEntry} - Customer: {oSalesOrder.CardCode}");

                // Create new Delivery document
                oDelivery = (SAPbobsCOM.Documents)SboCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oDeliveryNotes);

                // Copy header information from Sales Order to Delivery
                oDelivery.CardCode = oSalesOrder.CardCode;
                oDelivery.CardName = oSalesOrder.CardName;
                oDelivery.DocDate = DateTime.Now;
                oDelivery.DocDueDate = DateTime.Now;
                oDelivery.Comments = $"Auto-created from Sales Order {salesOrderDocEntry}";
                oDelivery.NumAtCard = oSalesOrder.NumAtCard;
                oDelivery.SalesPersonCode = oSalesOrder.SalesPersonCode;

                // Copy address information
                oDelivery.Address = oSalesOrder.Address;
                oDelivery.Address2 = oSalesOrder.Address2;
                oDelivery.ShipToCode = oSalesOrder.ShipToCode;
                oDelivery.PayToCode = oSalesOrder.PayToCode;

                // Copy line items from Sales Order to Delivery
                for (int i = 0; i < oSalesOrder.Lines.Count; i++)
                {
                    oSalesOrder.Lines.SetCurrentLine(i);

                    if (i > 0)
                    {
                        oDelivery.Lines.Add();
                        oDelivery.Lines.SetCurrentLine(i);
                    }

                    // Copy item details
                    oDelivery.Lines.ItemCode = oSalesOrder.Lines.ItemCode;
                    oDelivery.Lines.ItemDescription = oSalesOrder.Lines.ItemDescription;
                    oDelivery.Lines.Quantity = oSalesOrder.Lines.Quantity;
                    oDelivery.Lines.Price = oSalesOrder.Lines.Price;
                    oDelivery.Lines.Currency = oSalesOrder.Lines.Currency;
                    oDelivery.Lines.DiscountPercent = oSalesOrder.Lines.DiscountPercent;
                    oDelivery.Lines.TaxCode = oSalesOrder.Lines.TaxCode;
                    oDelivery.Lines.WarehouseCode = oSalesOrder.Lines.WarehouseCode;

                    // Link to the Sales Order (base document)
                    oDelivery.Lines.BaseType = 17; // 17 = Sales Order
                    oDelivery.Lines.BaseEntry = Convert.ToInt32(salesOrderDocEntry);
                    oDelivery.Lines.BaseLine = i;

                    Console.WriteLine($"  Line {i + 1}: {oDelivery.Lines.ItemCode} - Qty: {oDelivery.Lines.Quantity}");
                }

                // Add the Delivery Note
                int result = oDelivery.Add();

                if (result != 0)
                {
                    string errorMessage = SboCompany.GetLastErrorDescription();
                    throw new Exception($"Failed to create Delivery Note: {errorMessage}");
                }

                // Get the newly created Delivery Note DocEntry
                string newDeliveryDocEntry = SboCompany.GetNewObjectKey();

                Console.WriteLine($"Delivery Note {newDeliveryDocEntry} created successfully!");

                SboApplication.StatusBar.SetText($"Delivery Note {newDeliveryDocEntry} created successfully from Sales Order {salesOrderDocEntry}",
                    SAPbouiCOM.BoMessageTime.bmt_Medium,
                    SAPbouiCOM.BoStatusBarMessageType.smt_Success);

                // Show success message
                SboApplication.MessageBox($"Delivery Note {newDeliveryDocEntry} has been created successfully!",
                    1,
                    "OK",
                    "",
                    "");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating Delivery Note: {ex.Message}");
                SboApplication.StatusBar.SetText($"Error creating Delivery: {ex.Message}",
                    SAPbouiCOM.BoMessageTime.bmt_Long,
                    SAPbouiCOM.BoStatusBarMessageType.smt_Error);

                SboApplication.MessageBox($"Failed to create Delivery Note: {ex.Message}",
                    1,
                    "OK",
                    "",
                    "");
            }
            finally
            {
                // Release COM objects
                if (oSalesOrder != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(oSalesOrder);
                    oSalesOrder = null;
                }

                if (oDelivery != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(oDelivery);
                    oDelivery = null;
                }

                // Reset processing flag
                isProcessing = false;

                GC.Collect();
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
