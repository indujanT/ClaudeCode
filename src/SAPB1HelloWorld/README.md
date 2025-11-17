# SAP Business One - Sales Order to Delivery Add-on

A SAP Business One add-on that displays "Hello World" when the Add button is clicked in the Sales Order form, then automatically creates a Delivery Note with the same data using DI API.

## Overview

This C# add-on demonstrates how to:
- Connect to SAP Business One using both UI API and DI API
- Intercept form events (specifically the Sales Order form)
- Display messages when the Add button is clicked
- Automatically create Delivery Notes from Sales Orders using DI API
- Copy document data including header information and line items
- Link documents using base document references

## Prerequisites

Before running this add-on, ensure you have:

1. **SAP Business One Client** installed (version 9.3 or later recommended)
2. **SAP Business One DI API** installed
3. **SAP Business One UI API** installed
4. **.NET Framework 4.8** or later
5. **Visual Studio 2019** or later (for compilation)

## Project Structure

```
SAPB1HelloWorld/
├── Program.cs              # Main application with SAP B1 connection and event handling
├── SAPB1HelloWorld.csproj  # C# project file
├── App.config              # Application configuration
└── README.md               # This file
```

## Installation & Setup

### Step 1: Configure References

The project references SAP Business One DLLs. Update the paths in `SAPB1HelloWorld.csproj` if your SAP B1 installation is in a different location:

```xml
<Reference Include="SAPbobsCOM">
  <HintPath>C:\Program Files (x86)\SAP\SAP Business One DI API\SAPbobsCOM.dll</HintPath>
</Reference>

<Reference Include="SAPbouiCOM">
  <HintPath>C:\Program Files (x86)\SAP\SAP Business One UI API\SAPbouiCOM.dll</HintPath>
</Reference>
```

### Step 2: Build the Project

1. Open the solution in Visual Studio
2. Set the build configuration to **x86** (SAP B1 requires 32-bit)
3. Build the project (Build > Build Solution)

### Step 3: Register the Add-on (Optional)

For automatic startup with SAP Business One, you can register the add-on:

1. Open SAP Business One
2. Go to Administration > Add-Ons > Add-On Administration
3. Click "Register Add-On Identifier"
4. Browse and select your compiled executable
5. Configure startup settings

## Running the Add-on

### Method 1: Manual Launch (Recommended for Testing)

1. Start SAP Business One
2. Log in to your company database
3. Run the compiled executable (SAPB1HelloWorld.exe)
4. The add-on will connect to the running SAP B1 instance

### Method 2: Automatic Launch

If registered as per Step 3 above, the add-on will start automatically when SAP B1 starts.

## Testing the Add-on

1. Ensure the add-on is running (you should see "SAP B1 Add-on Started" in the console)
2. In SAP Business One, navigate to: **Sales - A/R > Sales Order**
3. Fill in the Sales Order details:
   - Select a Customer (CardCode)
   - Add one or more items with quantities
   - Fill in any other required fields
4. Click the **Add** button (or press Ctrl+A) to save the Sales Order
5. You should see:
   - "Hello World" message in the status bar and popup
   - Console output: "Add button clicked in Sales Order - Hello World displayed!"
   - The Sales Order will be saved normally
   - A status message: "Creating Delivery Note from Sales Order [DocEntry]..."
   - Console output showing the Delivery Note creation process
   - A success message: "Delivery Note [DocEntry] has been created successfully!"
6. Verify the Delivery Note was created:
   - Navigate to: **Sales - A/R > Delivery**
   - Find the newly created Delivery Note
   - Verify it contains the same items and quantities as the Sales Order
   - Check that the Delivery Note is linked to the Sales Order (base document)

## How It Works

### Workflow

1. **User clicks Add button** in Sales Order form
2. **"Hello World" message** is displayed (UI API)
3. **Sales Order is saved** by SAP B1
4. **FormDataEvent is triggered** after successful save
5. **Sales Order data is retrieved** using DI API
6. **Delivery Note is created** with copied data using DI API
7. **Documents are linked** via base document reference
8. **Success message** is displayed to user

### Key Components

1. **ConnectToSAP()**: Establishes connection to running SAP B1 instance using UI API and DI API
2. **SetupEventHandlers()**: Registers event listeners for item events and form data events
3. **SboApplication_ItemEvent()**: Event handler that intercepts button clicks and displays "Hello World"
4. **SboApplication_FormDataEvent()**: Event handler that intercepts document add events
5. **CreateDeliveryFromSalesOrder()**: DI API method that creates Delivery Note from Sales Order

### Event Detection

#### Button Click Detection (UI API)

The add-on detects the Sales Order Add button click by checking:
- **FormType = 139**: Sales Order form identifier
- **ItemUID = "1"**: Add button identifier
- **EventType = et_ITEM_PRESSED**: Button press event
- **BeforeAction = false**: After the action is processed

```csharp
if (pVal.FormType == 139 &&
    pVal.EventType == SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED &&
    pVal.ItemUID == "1" &&
    pVal.BeforeAction == false)
{
    // Display "Hello World"
}
```

#### Document Add Detection (UI API)

The add-on detects successful Sales Order creation:
- **FormTypeEx = "139"**: Sales Order form
- **EventType = et_FORM_DATA_ADD**: Document add event
- **BeforeAction = false**: After the document is saved
- **ActionSuccess = true**: Only if save was successful

```csharp
if (pVal.FormTypeEx == "139" &&
    pVal.EventType == SAPbouiCOM.BoEventTypes.et_FORM_DATA_ADD &&
    pVal.BeforeAction == false &&
    pVal.ActionSuccess == true)
{
    string docEntry = pVal.ObjectKey;
    CreateDeliveryFromSalesOrder(docEntry);
}
```

### DI API Document Creation

The `CreateDeliveryFromSalesOrder()` method:

1. **Retrieves the Sales Order** using DI API:
```csharp
SAPbobsCOM.Documents oSalesOrder = (SAPbobsCOM.Documents)
    SboCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oOrders);
oSalesOrder.GetByKey(Convert.ToInt32(salesOrderDocEntry));
```

2. **Creates a new Delivery Note**:
```csharp
SAPbobsCOM.Documents oDelivery = (SAPbobsCOM.Documents)
    SboCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oDeliveryNotes);
```

3. **Copies header information**:
   - Customer code and name
   - Addresses (ship-to, bill-to)
   - Sales person
   - Document dates
   - Customer reference number

4. **Copies all line items**:
   - Item codes and descriptions
   - Quantities and prices
   - Discounts and tax codes
   - Warehouse codes
   - Links each line to the source Sales Order line

5. **Links to base document**:
```csharp
oDelivery.Lines.BaseType = 17; // 17 = Sales Order
oDelivery.Lines.BaseEntry = salesOrderDocEntry;
oDelivery.Lines.BaseLine = lineNumber;
```

6. **Saves the Delivery Note**:
```csharp
int result = oDelivery.Add();
string newDeliveryDocEntry = SboCompany.GetNewObjectKey();
```

## Customization

### Changing the Message

To customize the message, modify the event handler in `Program.cs`:

```csharp
SboApplication.StatusBar.SetText("Your Custom Message",
    SAPbouiCOM.BoMessageTime.bmt_Short,
    SAPbouiCOM.BoStatusBarMessageType.smt_Success);
```

### Creating Different Document Types

To create different document types (e.g., Invoice instead of Delivery), change the BoObjectTypes:

**For A/R Invoice:**
```csharp
oDocument = (SAPbobsCOM.Documents)SboCompany.GetBusinessObject(
    SAPbobsCOM.BoObjectTypes.oInvoices);
```

**For A/R Credit Memo:**
```csharp
oDocument = (SAPbobsCOM.Documents)SboCompany.GetBusinessObject(
    SAPbobsCOM.BoObjectTypes.oCreditNotes);
```

**Common Document Object Types:**
- Sales Order: `oOrders` (17)
- Delivery Note: `oDeliveryNotes` (15)
- A/R Invoice: `oInvoices` (13)
- A/R Credit Memo: `oCreditNotes` (14)
- Purchase Order: `oPurchaseOrders` (22)
- Goods Receipt PO: `oPurchaseDeliveryNotes` (20)

### Intercepting Other Forms

To handle other forms, change the FormType number:
- Sales Order: 139
- Purchase Order: 142
- A/R Invoice: 133
- Delivery: 140
- Goods Receipt PO: 143
- etc.

Refer to SAP Business One SDK documentation for complete form type list.

### Intercepting Other Buttons

To handle different buttons, change the ItemUID:
- Add button: "1"
- Find button: "2"
- OK button: "1"
- Cancel button: "2"
- Update button: "1"

### Customizing Copied Fields

To copy additional fields or skip certain fields, modify the `CreateDeliveryFromSalesOrder()` method:

```csharp
// Add custom fields
oDelivery.UserFields.Fields.Item("U_CustomField").Value =
    oSalesOrder.UserFields.Fields.Item("U_CustomField").Value;

// Skip copying certain fields
// Simply don't include the assignment statement
```

## Troubleshooting

### Common Issues

**Issue: "Failed to connect to SAP B1"**
- Ensure SAP Business One is running before starting the add-on
- Check that you're passing the connection string parameter
- Verify the DI API is properly installed

**Issue: "Could not load file or assembly 'SAPbouiCOM' or 'SAPbobsCOM'"**
- Verify both SAP B1 UI API and DI API are installed
- Check that the DLL paths in .csproj are correct
- Ensure platform target is set to x86 (32-bit)

**Issue: "Not connected to SAP Business One company"**
- Ensure you're logged into a company database in SAP B1
- Check that the DI Company connection was successful
- Verify SboCompany.Connected returns true

**Issue: Event not firing**
- Verify FormType is correct (139 for Sales Order)
- Check ItemUID is "1" for Add button
- Ensure BeforeAction is set to false for after-action events
- Check that ActionSuccess is true for FormDataEvent

**Issue: "Failed to create Delivery Note: [error message]"**
- Check the specific error message from SboCompany.GetLastErrorDescription()
- Common causes:
  - **Missing required fields**: Ensure all mandatory fields are populated
  - **Invalid warehouse**: Verify warehouse codes exist and are valid
  - **Invalid item code**: Check that all items exist in the item master
  - **Insufficient inventory**: Ensure there's enough stock in the warehouse
  - **Tax code issues**: Verify tax codes are valid for the document date
  - **User permissions**: Ensure the logged-in user has rights to create deliveries
  - **License limitations**: Check SAP B1 license allows creating deliveries

**Issue: "Sales Order [DocEntry] not found"**
- The Sales Order might not have been saved yet
- Check that pVal.ActionSuccess is true before processing
- Verify the DocEntry is being captured correctly

**Issue: Delivery created but not linked to Sales Order**
- Ensure BaseType, BaseEntry, and BaseLine are set correctly
- BaseType should be 17 for Sales Order
- BaseEntry should be the Sales Order DocEntry
- BaseLine should match the Sales Order line number (0-based)

**Issue: Add-on crashes when SAP B1 closes**
- This is normal; the add-on depends on SAP B1 being open
- Implement proper cleanup in DisconnectFromSAP()

**Issue: Duplicate deliveries being created**
- The isProcessing flag prevents recursive processing
- Verify the flag is being set and cleared properly
- Check that you're not triggering multiple FormDataEvent handlers

**Issue: Some line items missing in Delivery Note**
- Verify the loop iterates through all lines correctly
- Check that Lines.Add() is called for each line after the first
- Ensure SetCurrentLine() is called before accessing line properties

## Development Tips

### General Tips

1. **Debug Mode**: Use Visual Studio debugger attached to the process
   - Attach to the running add-on process for step-by-step debugging
   - Use breakpoints to inspect COM object properties

2. **Logging**: Add file logging for production deployments
   - Log all DI API operations and their results
   - Include timestamps and error details
   - Example: Use NLog or log4net for structured logging

3. **Error Handling**: Always wrap SAP B1 API calls in try-catch blocks
   - Get detailed error messages from `SboCompany.GetLastErrorDescription()`
   - Log the error code: `SboCompany.GetLastErrorCode()`

4. **Memory Management**: Release COM objects properly to avoid memory leaks
   - Always release COM objects in finally blocks
   - Call `GC.Collect()` after releasing objects
   - Use `Marshal.ReleaseComObject()` for all COM objects

5. **BubbleEvent**: Set to false to prevent default SAP B1 behavior
   - Set `BubbleEvent = false` in BeforeAction to cancel default behavior
   - Keep `BubbleEvent = true` to allow normal processing

### DI API Specific Tips

6. **Transaction Management**: For complex operations, use transactions
   ```csharp
   if (SboCompany.InTransaction)
       SboCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack);
   SboCompany.StartTransaction();
   // Perform operations
   if (success)
       SboCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_Commit);
   else
       SboCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack);
   ```

7. **Document Object Reuse**: Don't reuse document objects
   - Create a new object for each document
   - Release the object after use
   - Never call Add() twice on the same object

8. **Line Collection**: Remember line indexing is 0-based
   - First line doesn't need Lines.Add()
   - Call Lines.Add() before setting subsequent lines
   - Always call SetCurrentLine() before accessing properties

9. **Base Document Linking**: Set BaseType, BaseEntry, and BaseLine together
   - This creates proper document chains in SAP B1
   - Enables traceability and copy-to functionality
   - Required for proper inventory and financial tracking

10. **Testing with Test Company**: Always test with a test/demo database
    - Never test directly on production data
    - Verify all document creations work correctly
    - Check that documents appear correctly in SAP B1 UI

## Additional Resources

- [SAP Business One SDK Documentation](https://help.sap.com/docs/SAP_BUSINESS_ONE_SDK)
- [SAP Business One UI API Reference](https://help.sap.com/viewer/product/SAP_BUSINESS_ONE_SDK)
- [SAP Community](https://community.sap.com/topics/business-one)

## License

This is a sample project for educational purposes.

## Support

For issues or questions related to this add-on, please refer to the SAP Business One SDK documentation or community forums.

---

**Note**: This add-on is a demonstration example. For production use, consider adding:
- Comprehensive error handling and validation
- File-based logging functionality (e.g., using NLog or log4net)
- User configuration options (e.g., enable/disable auto-delivery creation)
- Multi-language support for messages
- Proper add-on registration and licensing
- User confirmation before creating documents
- Options to select which fields to copy
- Support for batch operations
- Approval workflow integration
- Email notifications on document creation
- Custom field mapping configuration
