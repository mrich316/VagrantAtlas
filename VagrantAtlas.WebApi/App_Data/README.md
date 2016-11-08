# Folder used to store boxes.

Change Startup.cs and ajust VagrantAtlas.WebApi.wpp.targets
if you don't want to save anything or to change the default destination.

Please leave this README in ~/App_Data. Its sole presence is to force msdeploy
and msbuild targets to create this folder.

The targets defined in VagrantAtlas.WebApi.wpp.targets also set read/write acl
to allow boxes.json to be saved in ~/App_Data by default.
