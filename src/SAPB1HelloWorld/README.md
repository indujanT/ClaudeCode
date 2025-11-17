# SAP Business One - Hello World Add-on

A simple SAP Business One add-on that displays "Hello World" when the Add button is clicked in the Sales Order form.

## Overview

This C# add-on demonstrates how to:
- Connect to SAP Business One using the UI API
- Intercept form events (specifically the Sales Order form)
- Display messages when the Add button is clicked

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
3. Click the **Add** button (or press Ctrl+A)
4. You should see:
   - "Hello World" message in the status bar at the bottom
   - A popup message box with "Hello World"
   - Console output: "Add button clicked in Sales Order - Hello World displayed!"

## How It Works

### Key Components

1. **ConnectToSAP()**: Establishes connection to running SAP B1 instance
2. **SetupEventHandlers()**: Registers event listener for item events
3. **SboApplication_ItemEvent()**: Event handler that intercepts button clicks

### Event Detection

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

## Customization

### Changing the Message

To customize the message, modify the event handler in `Program.cs`:

```csharp
SboApplication.StatusBar.SetText("Your Custom Message",
    SAPbouiCOM.BoMessageTime.bmt_Short,
    SAPbouiCOM.BoStatusBarMessageType.smt_Success);
```

### Intercepting Other Forms

To handle other forms, change the FormType number:
- Sales Order: 139
- Purchase Order: 142
- Invoice: 133
- Delivery: 140
- etc.

Refer to SAP Business One SDK documentation for complete form type list.

### Intercepting Other Buttons

To handle different buttons, change the ItemUID:
- Add button: "1"
- Find button: "2"
- OK button: "1"
- Cancel button: "2"
- Update button: "1"

## Troubleshooting

### Common Issues

**Issue: "Failed to connect to SAP B1"**
- Ensure SAP Business One is running before starting the add-on
- Check that you're passing the connection string parameter

**Issue: "Could not load file or assembly 'SAPbouiCOM'"**
- Verify SAP B1 UI API is installed
- Check that the DLL path in .csproj is correct
- Ensure platform target is set to x86

**Issue: Event not firing**
- Verify FormType is correct (139 for Sales Order)
- Check ItemUID is "1" for Add button
- Ensure BeforeAction is set to false for after-action events

**Issue: Add-on crashes when SAP B1 closes**
- This is normal; the add-on depends on SAP B1 being open
- Implement proper cleanup in DisconnectFromSAP()

## Development Tips

1. **Debug Mode**: Use Visual Studio debugger attached to the process
2. **Logging**: Add file logging for production deployments
3. **Error Handling**: Always wrap SAP B1 API calls in try-catch blocks
4. **Memory Management**: Release COM objects properly to avoid memory leaks
5. **BubbleEvent**: Set to false to prevent default SAP B1 behavior

## Additional Resources

- [SAP Business One SDK Documentation](https://help.sap.com/docs/SAP_BUSINESS_ONE_SDK)
- [SAP Business One UI API Reference](https://help.sap.com/viewer/product/SAP_BUSINESS_ONE_SDK)
- [SAP Community](https://community.sap.com/topics/business-one)

## License

This is a sample project for educational purposes.

## Support

For issues or questions related to this add-on, please refer to the SAP Business One SDK documentation or community forums.

---

**Note**: This add-on is a basic example. For production use, consider adding:
- Comprehensive error handling
- Logging functionality
- User configuration options
- Multi-language support
- Proper add-on registration and licensing
