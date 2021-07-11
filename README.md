# CBDuetFileUpload

CamBam PlugIn to allow .nc files to be uploaded to Duet 3D control boards.

To use, copy file <DuetFileUpload.dll> to the CamBam plugins folder.

The PlugIn adds a menu item "Upload to Duet CNC" to CamBam's Tools menu.

Clicking this prompts for a g-code file to be generated for the current project and then opens a dialog box.

![Dialog](/DuetFileUploadHMI.png)

#### Filename   
This is the filename of the g-code file as it will be stored in Duet's /gcodes folder. 

#### Duet 3D URL
Enter the network location of the Duet here, using either the mDNS name of the Duet board (e.g. http://cnc.local) or its IP address (e.g. http://192.168.0.2).

#### Upload
Click to start uploading the file to the Duet. Status updates will be shown and once the upload has completed the dialog will close.

![Success](/DuetFileUploadHMISuccess.png)


### Notes
Part of this plugin were derived from the excellent cb2cm plugin: https://github.com/jkmnt/cb2cm.

This plugin is for Windows only.

