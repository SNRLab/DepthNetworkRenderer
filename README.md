# DepthNetworkRenderer

Unity project for creating rendered images and cooresponding depth maps of the 
inside of airways.

It is not currently a polished standalone application, and most configuration 
has to be done from within the Unity editor.

Press 's' to start and stop recording renderings.

The renderings are stored as PNG files and the depth maps are stored as float16
EXR files. The depth in millimeters is stored in the red channel.
