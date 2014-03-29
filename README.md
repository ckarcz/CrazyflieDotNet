CrazyflieDotNet
==============================
Dot.NET libraries written in C# for Crazyflie Quadcopters and Crazyradios.
The original open source software is written in Python.



About the CrazyFlie:
==============================
A Crazyflie is a tiny open source picocopter/quadcopter that began as a side project by a bunch of engineers and grew to great internet acclaim via a HackADay posting in 2011 (http://hackaday.com/2011/04/29/mini-quadrocopter-is-crazy-awesome/).

A lot of hard and a couple years later, they were able to bring their Crazyflie open source quadcopter into mass production. Check out their site here (http://www.bitcraze.se/).



Project:
==============================
This is currently a work in progress in my spare time.

Milestones:
- Working USB driver for Crazyradio USB dongle - COMPLETED
  - You are able to send the copter packets (byte array) and get an ACK packet back.
  Currently only testing pinging the copter as I am working on the remainder of the transfer protocol. 
- Implement the Crazyradio Transfer Protocol (CRTP) - IN PROGRESS
- Front end design and development - NOT STARTED
- Cross platform support using Mono framework - IN PROGRESS

The C# CrazyradioDriver is completed and working. This library (CrazyflieDotNet.Crazyradio.dll) provides a type safe API for communication with the Crazyradio USB dongle with Crazyflies. The library and driver expose all available configurations for the USB dongle, including defaults. The API is quite nice and easy to understand and use, even for a beginner.

Currently I am working on the transfer protocol.

More updates, commits, and hopefully a working protocol to come soon! :)

-Chris

// TODO - rest of readme
