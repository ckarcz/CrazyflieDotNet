#CrazyflieDotNet
Dot.NET libraries written in C# for Crazyflie Quadcopters and Crazyradios.

######Builds:

| Branch      | Build                                                                                                                                |
|-------------|--------------------------------------------------------------------------------------------------------------------------------------|
| master      | [![Build Status](https://travis-ci.org/ckarcz/CrazyflieDotNet.svg?branch=master)](https://travis-ci.org/ckarcz/CrazyflieDotNet)      |
| integration | [![Build Status](https://travis-ci.org/ckarcz/CrazyflieDotNet.svg?branch=integration)](https://travis-ci.org/ckarcz/CrazyflieDotNet) |
| dev         | [![Build Status](https://travis-ci.org/ckarcz/CrazyflieDotNet.svg?branch=dev)](https://travis-ci.org/ckarcz/CrazyflieDotNet)         |

######Releases:
| Version |                                                                                                                    |
|---------|--------------------------------------------------------------------------------------------------------------------|
| 0.1     | [![Github Releases (by Release)](https://img.shields.io/github/downloads/ckarcz/CrazyflieDotNet/v0.1/total.svg)]() |

[![Issues](https://img.shields.io/github/issues/ckarcz/CrazyflieDotNet.svg)](https://github.com/ckarcz/CrazyflieDotNet/issues)

#About The CrazyFlie
The [Crazyflie](https://www.bitcraze.io/crazyflie/) is a tiny open source picocopter/quadcopter that began as a side project by a bunch of engineers and grew to great internet acclaim via a [HackADay posting in 2011](http://hackaday.com/2011/04/29/mini-quadrocopter-is-crazy-awesome).

A lot of hard and a couple years later, they were able to bring their Crazyflie open source quadcopter into mass production. 

[Check out their site here](http://www.bitcraze.se/).

* [Crazyflie Wiki](http://wiki.bitcraze.se/projects:crazyflie:index)

* [Crazyflie Documentation](http://wiki.bitcraze.se/doc:crazyflie:index)

* [Crazyradio Wiki](http://wiki.bitcraze.se/projects:crazyradio:index)

#About This Project
This is currently a work in progress in my spare time. Pull requests are welcome if good implementation and clean code!

##Milestones:
- Working .NET abstraction "driver" for Crazyradio USB dongle - COMPLETED
- Implement the Crazyradio Transfer Protocol (CRTP) - IN PROGRESS
  - Able to ping copter and send commander packages.
  - Working flight with wired in PS3 controller.
- Cross platform support using Mono framework - IN PROGRESS
- Front end design and development - NOT STARTED

The C# CrazyradioDriver is completed and working. This library (CrazyflieDotNet.Crazyradio.dll) provides a type safe API for communication with the Crazyradio USB dongle with Crazyflies. The library and "driver" expose all available configurations for the USB dongle, including defaults. The API is quite nice and easy to understand and use, even for a beginner.

#Development

##Windows

###Dev Environment:
1. Windows OS.
2. Visual Studio (with NuGet installed).
3. GitHub Windows client: https://windows.github.com/
4. Git for Windows libraries and tools: http://msysgit.github.io/
5. Open the GitHub Windows client and clone this repository to a folder specifically used for your GitHub clones/repos. (ex: C:\Dev\GitHub\...)
5. Open Visual Studio.
6. Tools > Options > Source Control > Microsoft Git Provider.
7. File > Open > Open From Source Control.
8. In Team Explorer Window: Local Git Repositories > Add. Browse to your GitHub repos folder (ex: C:\Dev\GitHub\).
9. Team Explorer should load all found git repos within that folder.
10. Double click this repo. Now you should see the solution(s) listed, which you can double click to open.
11. Go to Tools > Options > Text Editor > C# > Tabs > USE TABS!
12. If you use ReSharper, please load the shared dot settings file and use that for the solution! Use the provided clean to clean your files! Keep the code style the same! Once in a while the MASTER branch will be cleaned to ensure consistensy in style. Finally, please comment your code as done in already submitted files!

###Dev Standards:
1. Please attempt to follow the coding style throughout the solution including tabbing, spacing, and commenting.
2. ReSharper is recommended. Please use the supplied DotSettings to adhere to formatting/standards and use code cleanup regularly.

###OS Driver:
1. Get the Zadig USB Tool: http://zadig.akeo.ie/
2. Run the Zadig executable.
2. In the drop down, select the "Crazyradio USB Dongle".
3. Select "libusb-win32 (vx.x.x.x)" as the driver to use for the device.
4. Note the USB IDs. The default, assumed by this library, are: 1915 (vendor ID) and 7777 (product ID).
5. Click Install Driver.
6. Open Device Management (devmgmt.msc in run box).
7. Navigate to "libusb-win32 devices" to ensure that "Crazyradio USB Dongle" is listed.

#[License](license.txt)
```
MIT License

CrazyflieDotNet [https://github.com/ckarcz/CrazyflieDotNet]
Copyright (c) [2013] [Chris Karcz]

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
```
