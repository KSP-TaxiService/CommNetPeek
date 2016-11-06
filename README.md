#CommNet Peek

##Introduction

CommNet Peek is a debugging plugin for Kerbal Space Program that adds a button to every probe and command module. This little button launchs a dialog and displays some underlying information about the CommNet network. A player can read this CommNet data whenever s/he feels like during a play session.

**Work in progress**

Additionally, the plugin also implements the concept of a signal radio that obeys the phsyical rule of the light speed. Any command issued to an active vessel will be delayed before it can be in effect.

##Features

* Information:
    * Status of a vessel
    * Signal path between a vessel and its control source (either a remote pilot or a ground station)
    * Neighbour nodes of a vessel
    * Signal delay
    * RemoteTech 
    * Output of CommNet's debug info
* Technical details:
    * Name of a vessel or ground station
    * Signal strength
    * Distance in meters
    * Signal delay in seconds
    * Communication type
    * Orbit or ground location

##Issues

* Not yet

##Downloads

*[CommNet Peek](https://github.com/KSP-TaxiService/CommNetPeek/releases)
*[ModuleManager](http://forum.kerbalspaceprogram.com/index.php?/topic/50533-105-module-manager)

##Installation

Unzip the GameData folder into the main directory of your Kerbal Space Program

If you are upgrading, overwriting the existing files is sufficient.

##Acknowledgments

I would like to thank the contributors behind the RemoteTech mod for the codes and ideas used in one way or another:

*[RemoteTech](https://github.com/RemoteTechnologiesGroup/RemoteTech)

##License

GNU GENERAL PUBLIC LICENSE Version 2
TLDR: http://tldrlegal.com/license/gnu-general-public-license-v2