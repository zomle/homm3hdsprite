# HOMM3 HD sprite to Stream Avatars sprite sheet converter

Purpose of the app is to help with exporting sprite sheets from Heroes of Might and Magic 3 HD assets. 
The tool can load image data and sprite information from Homm3 HD .pak and .lod files as well, and allows the user to adjust the 
individual frames before exporting a sprite sheet that could be loaded in Stream Avatars.

## Download

The program can be downloaded from here (H3HDSpriteGen_xyz.zip): https://github.com/zomle/homm3hdsprite/releases 

## Getting Started

The program requires no installation whatsoever, it can be extracted and started, as long as the prerequisites are met.

### Prerequisites

The program uses .NET 5.0 and WPF, so the [.NET 5.0 Runtime](https://dotnet.microsoft.com/download/dotnet/5.0) 
is required: [*.NET 5.0 Runtime Installer*](https://dotnet.microsoft.com/download/dotnet/thank-you/runtime-5.0.5-windows-x64-installer).

### How to use

After starting the program, load the Homm3 HD asset files found in the data subdirectory:

![Main screen](https://github.com/zomle/homm3hdsprite/raw/master/Resources/scr01_load.png)

Select the creature you want to export:

![Select creature](https://github.com/zomle/homm3hdsprite/raw/master/Resources/scr03_select.png)

All the available animations for the selected creature and their individual frames can be reviewed:

![Individual animations](https://github.com/zomle/homm3hdsprite/raw/master/Resources/scr04_sprites1.png)

The program uses sprite information extracted from the homm3 asset to adjust the frames, but it's usually not pixel perfect. 
In some cases it's completely off, so usually some manual adjustment is needed.
Each frame can be individually adjusted by 1 or 5 pixels at a time. It's possible the pause/resume the animation preview and 
step through the frames manually for easier adjustment:

![Individual adjustments](https://github.com/zomle/homm3hdsprite/raw/master/Resources/scr04_sprites2.png)

It's also possible to adjust the scaling of the image, so the spritesheet fits a certain size. The frame that will be used in 
the sprite sheet can be also increased or decreased. If part of the image crosses the frame border, it will be cut off during sprite sheet export:

![Individual adjustments](https://github.com/zomle/homm3hdsprite/raw/master/Resources/scr04_sprites3.png)

For easier adjustment, a helper center line can be enabled. This won't be included in the export, it's only there to help with 
the adjustment. The background of the frame can be changed, in some cases it's easier to see the animation in front of a dark or light color:

![Adjustment helper](https://github.com/zomle/homm3hdsprite/raw/master/Resources/scr05_adjust.png)

The Stream Avatars(SA) avatars have 5 mandatory actions (idle, run, sit, stand, jump), and some optional, out of which we are 
interested in one (attack). The tool automatically assigns the HOMM3 animations to the SA actions, and allows each actions to be 
previewed. Even though the animations might look fine on their own, they might be misaligned compared to each other. It's possible 
to adjust each frame at the same time, so when you toggle through idle/run/sit/etc. they look aligned.

![SA animations](https://github.com/zomle/homm3hdsprite/raw/master/Resources/scr06_saanimations1.png)

It's possible to assign any HOMM3 animation to any SA action:

![HOMM3 animation to SA action](https://github.com/zomle/homm3hdsprite/raw/master/Resources/scr07_saanimationhdsprite.png)

Frame size can also be adjusted here:

![SA animations frame size](https://github.com/zomle/homm3hdsprite/raw/master/Resources/scr06_saanimations2.png)

After the animations are properly aligned, the tool can try to determine the minimum frame size that would fit all frames. It 
usually works, but in case of some images it doesn't, in which case manual frame size adjustment is suggested.

![SA automatic frame size](https://github.com/zomle/homm3hdsprite/raw/master/Resources/scr08_saadjust.png)

When the animations and frames are aligned, the frame size is set up, the sprite sheet can be exported. The exported sprite sheet 
will not contain the dashed center line, or a background for the frames (it will be transparent):

![SA sheet export](https://github.com/zomle/homm3hdsprite/raw/master/Resources/scr09_export.png)

Along with the sprite sheet, a text file will also be exported, which contains information regarding the generated sprite sheet, 
so if necessary it can be reproduced later, or the information can be used when importing it to Stream Avatars:

![Export result](https://github.com/zomle/homm3hdsprite/raw/master/Resources/scr10_readysheet.png)

**Suggestions:**

* It's unnecessary to adjust all HOMM3 animations. After loading a creature, assign the correct HOMM3 animation sequences to the 
Stream Avatars actions and fix only those 6 animations that are actually used.
* There is a memory leak somewhere, that causes the program to start using a lot of memory after loading several creatures. Sometimes 
it's a good idea to restart the tool.

### Known Issues

* **Error handling.** A lot of errors are dealt with, but it can be crashed by opening invalid .lod/.pak files for examples. Also 
testing was not extensive by any means, so there might be circumstances that causes crashes. "Normal" usage should work fine.

* **Memory leak.** Loading the animations involve loading several large (but downscaled) images, and displaying them causes seemingly 
unnecessarily large memory usage. The memory used should be freed up when another creature is loaded, but after some testing I suspect 
WPF caches images even after they are not needed, which causes a memory leak. Restarting the tool when the memory usage is too big fixes 
the issue, so I haven't spent more time trying to fix this.

* **Layout issues on small resolution.** Some parts of the UI look misaligned in very small resolution. 

* ???

## Development/Contribution

This is a hobby project of mine, and as such I have limited time to work on it, hence the reason why it's made open-source.
Feel free to report issues, suggest features, create pull requests, etc. I'll try to deal with everything in a timely 
manner :)

### Development environment

* I used *Visual Studio 2019 Community Edition* for development and WPF on *.NET 5.0*. 
* I haven't tested it on earlier versions, but might work fine.

## Authors

* **zomle** - *Initial work* - https://github.com/zomle

## License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details.

## Acknowledgments

* Thanks [specialiste](https://www.twitch.tv/specialiste/) for the inspiration. 
