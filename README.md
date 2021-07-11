# CBDuetFileUpload

CamBam PlugIn to allow .nc files to be uploaded to Duet 3D control boards.

To use, copy file <DuetFileUpload.dll> to the CamBam plugins folder.

The PlugIn adds a menu item "Upload to Duet CNC" to CamBam's Tools menu.

Clicking this prompts for a g-code file to be generated for the current project and then opens a dialog box.

![Dialog](/DuetFileUploadHMI.png)

##Filename   
This is the name of the g-code file as it will be stored in Duet's /gcodes folder.

##Duet 3D URL
This is the URL of the Duet and may be eentered either as an IP address, such as http://192.168.0.2 or the mDNS name of the Duet board, such as http://cnc.local.

Clicking the Upload button uploads the file to the Duet.



Part of this plugin were derived from the excellent cb2cm plugin: https://github.com/jkmnt/cb2cm
Windows only.

