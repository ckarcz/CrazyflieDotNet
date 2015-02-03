NEW WAY:
Get this Zadig USB Tool:
http://zadig.akeo.ie/

Install the driver for the crazy radio usb dongle for libusb-win32.

NOTE the vendor and device IDs! This library assumes vendorID=0x1915, productID=0x7777!
You use these IDs like so:
crazyradioDrivers = CrazyradioDriver.GetCrazyradios();
or 
crazyradioDrivers = CrazyradioDriver.GetCrazyradios(vendorID, productID);
or
var crazyRadiosRegDeviceList = UsbDevice.AllDevices.FindAll(new UsbDeviceFinder(CrazyradioDeviceId.VendorId, CrazyradioDeviceId.ProductId));
then provide one of UsbDevices to the constructor of CrazyradioDriver...Above two methods to that for you.

Open the run box (Windows Key+R) and type devmgmt.msc and hit enter. Check device manager devices under libusb-win32 devices.
You should see the crazy radio usb dongle listed there.

Done!




OLD WAY:
Get the libusb-win32 library from sourceforge:
http://sourceforge.net/projects/libusb-win32/

Uncompress the downloaded file and in the bin folder, there should be an inf wizard executable.

Plug in your crazyradio dongle and open the wizard.

Navigate the inf wizard application, selecting the crazyradio dongle and generating a generic libusb driver for windows for the device.

NOTE the vendor and device IDs! This library assumes vendorID=0x1915, productID=0x7777!
You use these IDs like so:
crazyradioDrivers = CrazyradioDriver.GetCrazyradios();
or 
crazyradioDrivers = CrazyradioDriver.GetCrazyradios(vendorID, productID);
or
var crazyRadiosRegDeviceList = UsbDevice.AllDevices.FindAll(new UsbDeviceFinder(CrazyradioDeviceId.VendorId, CrazyradioDeviceId.ProductId));
then provide one of UsbDevices to the constructor of CrazyradioDriver...Above two methods to that for you.

Install the driver for the dongle via the Wizard, or the normal way you would install a device driver in Windows.

You may need to configure windows to install the unsigned driver (OR use above new method to create and install a signed driver).

Open the run box (Windows Key+R) and type devmgmt.msc and hit enter. Check device manager devices under libusb-win32 devices.
You should see the crazy radio usb dongle listed there.

Done!
