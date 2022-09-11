# Congestion Tax Calculator

The implementation proposed, requires that the library for the congestion tax calculator be generated as a Nuget package and 
configure by the consumer. The library projects that should be compile and copy into the local folder are Congestion_Models 
and congestion-tax-calculator-netcore. For generate the libraries as nuget package, in the project properties, inside the package section, check the option Produce a package file during build operations, as can be seen in the next picture:

![alt text](https://github.com/camilofg/congestion-tax-calculator/blob/main/netcore/Images/GenerateNugetPackage.png?raw=true)

This will generate a file with the extension nupkg. The file generated has to be copied in the local Nuget packages repository. To find the route of the folder for nuget package repositories go to **"Tool/NuGet Package Manager/Package Manager Settings"** in the window opened, select Package Source. In the next picture can be seen the folder where the nupkg files should be copied.

![alt text](https://github.com/camilofg/congestion-tax-calculator/blob/main/netcore/Images/NugetFolder.png?raw=true)

After that the nuget packages should be able to be restored. In case of having issues at restoring, open the **Nuget Package Manager*, select the source of the folder (as can be seen in the picture) and reinstall by hand the newly created packages. 

![alt text](https://github.com/camilofg/congestion-tax-calculator/blob/main/netcore/Images/NugetPackageManager.png?raw=true)


# Configurations provided by the client

The main goal of exposing the library as a nuget package is the great advantage that anyone with access to the nuget repository can consume and configure with his own parameters, decoupling its use. 

In the implementation proposed, the Congestion Tax Calculator library is consumed by a Web Api. This consumer has to provide 5 parameters:

1. A **List of DateOnly** that will contain the holidays.

2. An object **RangesTax** that is included in the nuget Congestion_Models, and is used to define the different fares for each time of the day.

3. A **List of strings** that will contain the vehicle type that will be excluded of the toll payment.

4. An **int** that will represent the period of grace between the tolls.

5. An **int** that will represent the maximum amount that can be charge per day.


In the web api, the first three parameters are given by json files located in the ConfigFiles folder, and their own file name is referenced in the appsettings with the assembly name included (it can be refactored). The last 2 parameters are directly assign into the appsettings.


# Security

Even there were no security consideration the Web Api request a GUI that is also in the appsettings.json and compare the value given by the final user with this value and allows the request if it matches. Is implemented with a filter, that reads the context and retrieve the value as a header of the request.


# Notes

The implementation and design took a little bit more than 4 hours, it also includes the unit testing, and the present documentation. In the unit test there is room for improvements on how retrieve the data for the test. 
As a final consideration, this is a good use case to implement Azure functions that consumes the library and would be easier to scale. I'll continue with this implementation on my spare time in a different branch from the main of the current repository.