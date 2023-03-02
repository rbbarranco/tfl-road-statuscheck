# TFL Road Status Checker
This is an implementation of TFL's Developer Coding Challenge

### Some notes on the challenge requirements
* Saw in this [post](https://techforum.tfl.gov.uk/t/not-able-to-find-app-id-and-key/1883/2) that the App_Id is no longer needed. My testing also shows that it is no longer required. But I kept it in the config file and I still append it in the requests done to the TFL Open Data's Road endpoint.

## Building, Publishing, Running the application

### Building the code
* Unzip the files to a folder (if using the provided zip file) or download the code from [github](https://github.com/rbbarranco/tfl-road-statuscheck). 
* Make sure you have the latest .NET Framework/SDK installed.
* Make sure you have access to nuget as the packages might need to be restored on the first time you build the solution.
* Open and build the solution using Visual Studio. 

### Publishing the code
If preferred, you can publish the code to a specified folder location using Visual Studio
* Make sure the solution is built
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
The solution is written following the Onion architecture. Since the solution doesn't really need a Domain layer, that was skipped.
* User Interface Layer
   * TFL.Road.StatusCheck
      * This is where dependencies were injected
      * Only reponsible for what will be displayed and what application code will be returned
* Application Layer
   * TFL.Road.StatusCheck.Application.Contracts
      * This is where the contracts are defined
      * The contracts here are only used for the communication between the User Interface layer and the Application layer
   * TFL.Road.StatusCheck.Application.Entities
      * This is where the entities are defined
      * The entities are only used for the communication between the Application layer and the Infrastructure layer
   * TFL.Road.StatusCheck.Application.Interfaces
      * This is where the interfaces are defined e.g. IRoadService, IRoadMapper, and others.
      * Since the solution is following the Onion architecture, this is where the interfaces that should be implemented by the Infrastructure layer are defined - IRoadRepository
   * TFL.Road.StatusCheck.Application
      * This is where the actual implementation of the services, mappers, and all other application layer classes are implemented.
      * The road service class acts only as a coordinator
* Infrastructure Layer
   * TFL.Road.StatusCheck.Infrastructure.TFLOpenData.DTO
      * This is where the DTO's are defined
      * The DTO's are the objects to which the responses from TFL's Open Data API are deserialised to
   * TFL.Road.StatusCheck.Infrastructure.TFLOpenData
      * This is where the implementation of the IRoadRepository defined in the application layer's interfaces project is done
      * A crude implementation of a client and serialiser are also done in separate classes to make the repository testable
* Tests
   * TFL.Road.StatusCheck.Tests
      * Contains all the unit tests for all projects in the solution
  
### Contracts
* Note that I've created separate input and output contracts. It is good practice to separate these two. 
* Although we're only expecting RoadId as the input, it is good practice to put this is in a "request" class. Mainly so we can easily separate the validation of requests in separate validator classes, as was done in this solution, and keep the service class logic free.

### Validator
* I've added a validator class here using FluentValidator. This validator only validates the "cleanliness" of the request, and not the actual business logic. E.g. required validations, length validations, invalid character validations, etc.

### Mappers
* In this solution, I've manually created mapper classes. But this can easily be done using any mapping library e.g. AutoMapper.

### Unit tests
* Notice that in a couple of classes, I've added the assembly attribute InternalsVisibleTo. 
   * Reason for doing this is because these methods don't need to be exposed publicly but it would be good to have unit tests against them.
   * Also the methods consuming these internal methods can be tested independently without having to do assertions related to the internal methods.
   * One way to avoid using this attribute is to extract these internal methods to a separate class and inject it, but this a bit of an overkill.

### Additional notes
More information regarding some details are in comments in the code
