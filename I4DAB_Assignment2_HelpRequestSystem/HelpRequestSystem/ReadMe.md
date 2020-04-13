# How to start application

Open the Visual Studio solution named I4DAB_Assignment2_HelpRequestSystem and set the "HelpRequestSystem" 
project as your startup project. Before you can run the program, you have to configure it to your personal 
docker environment. In the Data/HrsContext.cs file change the variables "username" and "password" in the 
OnConfiguring method to your match your personal environment settings. If your SQL Server setup is very 
different from ours with Docker you might have to edit the connection string in the line below the password
variable.

Now you can hit CTRL+F5 to build and run the project, if you use Visual Studio.

# How to use the application

Follow the menu in the terminal window.