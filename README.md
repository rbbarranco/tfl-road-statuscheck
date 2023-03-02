# TFL Road Status Checker
This is an implementation of TFL's Developer Coding Challenge

### Some notes on the challenge requirements
* Saw in this [post](https://techforum.tfl.gov.uk/t/not-able-to-find-app-id-and-key/1883/2) that the App_Id is no longer needed. My testing also shows that it is no longer required. But I kept it in the config file and I still append it in the requests done to the TFL Open Data's Road endpoint.

## Building, Publishing, Running the application

### Building the code
* Unzip the files to a folder (if using the provided zip file) or download the code from [github](https://github.com/rbbarranco/tfl-road-statuscheck). 
* Open the solution using Visual Studio. Make sure you have the latest .NET Framework/SDK installed.
* Make sure you have access to nuget as the packages might need to be restored on the first time you build the solution.

### Publishing the code
If preferred, you can publish the code to a specified folder location using Visual Studio
* Make sure the solution is building
* Select Build>Publish Selection
* Select Folder as Target
* Select Folder as Specific target
* Put in your preferred folder location and click Finish
* Make sure that the Configuration is set to Release and Target Runtime to Portable
* Click Publish

### Running and testing the code
1. First make sure that the application is properly configured with the correct App_Key
2. If you published your application, navigate to the folder where you published the application and open the appSettings.json file using a text editor., If not, open the appSettings.json file in Visual Studio
3. Put in your own App_key. There is no need to update App_Id as mentioned above.

   ![image](https://user-images.githubusercontent.com/21362502/222507673-6ad7da13-aa23-459d-a18d-29cf3fb3b5b3.png)

4. If updating the file using Visual Studio, make sure to rebuild the code.
5. Open Windows Commmand Prompt or Windows Powershell and navigate to the folder where the application executable (TFL.Road.StatusCheck.exe) resides.
6. If using Windows PowerShell, run the application using the command .\TFL.Road.StatusCheck.exe Road_Id e.g. 
   
   **.\TFL.Road.StatusCheck.exe A2**

   If using Window Command Prompt, run the application using the command TFL.Road.StatusCheck.exe Road_Id e.g. 
   
   **TFL.Road.StatusCheck.exe A2**

    The result will be displayed like
![image](https://user-images.githubusercontent.com/21362502/222509872-305708b7-f7f8-4c29-b34e-3586b040dd23.png) or
![image](https://user-images.githubusercontent.com/21362502/222510027-7c368c77-7b01-4fab-a841-ebce0b2abc5e.png)
 
7. You can also check the application exit code by using the command **echo $LASTEXITCODE** if using PowerShell or **echo %errorlevel%** if using Windows Command Prompt.

## Implementation Notes

### Structure
The solution is written following the Onion architecture
* TFL.Road.StatusCheck
   * The project is the User Interface layer
   * This is where I injected the dependencies
* 
  
