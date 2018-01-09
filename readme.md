Frisbee Visualizer 2018
=======================

Frisbee Visualizer 2018 is a 3D motion analysis tool made for analyzing Frisbee throwing motion in various flying disc games.
Our tool requires Unity 2017.2.0f3 together with OptiTrack Flex cameras, the OptiTrack Motive software and OptiTrack Unity plugin.

For usage instructions refer to the main menu of our software. To get started, you need to calibrate the OptiTrack system with Motive and the calibration wand, and set the ground plane with Z+ pointing in the throw direction. Additionally, network streaming needs to be enabled inside Motive and set to Local Loopback, and a rigid body with an id of 1 should be set up inside Motive. To start our software, you can either use the standalone build or open our scene inside Unity and start playback. We recommend opening inside Unity if you need to customize low-level parameters (i.e. the number of frames used for speed calculation inside ThrowController), although there are some basic settings inside the start menu.

The main parts of our software are:
* Custom Unity C# scripts for:
  * controlling the camera (CameraChange.cs and parts of UIScript.cs)
  * reading the frisbee movement data stream (CustomRigidBody.cs for OptiTrack, DebugMover.cs for csv playback)
  * switching the program state automatically based on frisbee movement (ModeHandler.cs and ThrowController.cs)
  * recording motion, calculating speeds (ThrowController.cs)
  * playing back the motion at requested speed (ModeHandler.cs)
  * simulating the possible trajectory of the frisbee after recording ends (Prediction.cs)
  * calculating the accuracy of the throw and ending speed (ModeHandler.cs)
  * start menu and user settings (StartMenuScript.cs)
  * drawing the movement trail as dots and displaying the calculated speeds (TrailHandler.cs and TrailPieceScript.cs)
  * FrisbeeLocation.cs, which contains the definitions for a single motion data point
* A Unity scene that ties all of the scripts together and defines the user interface (Frisbee.unity). Inside Frisbee.unity, there are various GameObjects:
  * "Client - OptiTrack"/RecordingFrisbee contains CustomRigidBody, DebugMover and ThrowController
  * PlaybackFrisbee contains ModeHandler
  * Cameras/CameraController contains CameraChange
  * UI contains
    * Start Menu with StartMenuScript, for user settings and instructions
    * Permanent Canvas, for the floating UI text with Text and Dropdown objects
    * EventSystem to receive UI events
  * At the root level, there are additionally some lights, a ground plane, a green line representing the green zone used for accuracy calculation, and Trail/SimulationTrail which contain TrailHandlers
  
  Many of the low-level parameters are customizable from inside the editor. 