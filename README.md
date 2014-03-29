CrazyflieDotNet
===============

Dot.NET libraries written in C# for Crazyflie Quadcopters and Crazyradios.

A Crazyflie is a tiny opensource picocopter/quadcopter that began as a side project by a bunch of engineers and grew to great internet acclaim via a HackADay posting in 2011 (http://hackaday.com/2011/04/29/mini-quadrocopter-is-crazy-awesome/).

They have since brought their Crazyflie into mass production. Check out their site here (http://www.bitcraze.se/);

This is currently a work in progress in my spare time.

Milestones:
- Working USB driver for Crazyradio USB dongle - COMPLETED
- Implement the Crazyradio Transfer Protocol (CRTP) - IN PROGRESS
- Front end design and development - NOT STARTED
- Cross platform support using Mono framework - IN PROGRESS

The C# CrazyradioDriver is completed and working. This library (CrazyflieDotNet.Crazyradio.dll) provides a type safe API for communication with the Crazyradio USB dongle with Crazyflies. The library and driver expose all available configurations for the USB dongle, including defaults. The API is quite nice and easy to understand and use, even for a beginner.

Currently I am working on the transfer protocol, which I took some time off of working on due to lack of information regarding the protocol available (on the Crazyflie wiki). I have found a C++ implementation of which is providing some help where the wiki lacks in information, or provides no information at all. I'm sure I'll be glancing at thier library if I get lost, but so far so good!
Check out their project here: https://github.com/fairlight1337/libcflie

More updates, commits, and hopefully a working protocol to come soon! :)

-Chris

// TODO - rest of readme
