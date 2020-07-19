# Kickermat

The Kickermat is computer controlled foosball table.

## Getting Started

For the Kickermat solution to compile you need to install a couple of development tools. The easiest method is to use the package manager [Chocolatey](https://chocolatey.org/install).

#### Visual Studio

We use Visual Studio 2019 as our IDE, you can download the Community Edition from the official [website](https://visualstudio.microsoft.com/vs/). Make sure that you have installed both of the workloads *ASP.NET and web development* and *Desktop Development with C++*.

#### Angular & Node.js
The Kickermat is controlled via web frontend built with Angular and ASP.NET Core. Therefore install [Node.js](https://nodejs.org/en/) and npm in a command window with elevated privileges:

```
choco install nodejs
```

After this install the Angular command line interface:

```
npm install -g @angular/cli
```

#### Camera Driver & SDK

To be able to use the Teledyne Dalsa Genie Nano-C1280 camera, install the camera [driver](https://www.teledynedalsa.com/en/support/downloads-center/device-drivers/80/) and the adjacent [Sapera LT SDK](https://www.teledynedalsa.com/en/products/imaging/vision-software/sapera-lt/download/). You will be asked to login, try to use ```noone``` as username and ```nopassword``` as password. If the credentials do not work you can check [BugMeNot](http://bugmenot.com/view/teledynedalsa.com) for alternative credentials or just create an account. After the installation make sure that the environment variable ```%SAPERADIR%``` got set to your Sapera LT SDK folder. Otherwise the solution will not build as the libraries cannot be found.

## Installation & Development

If you start the Kickermat for the first time, you might need to manually download the missing dependencies with the following commands:

```
cd kicker\webapp\ClientApp
npm install
```

To avoid a restart of the Angular frontend each time the solution is compiled, it has to be launched seperately (for details refer to the [documentation](https://docs.microsoft.com/en-us/aspnet/core/client-side/spa/angular?view=aspnetcore-2.2&tabs=visual-studio#run-ng-serve-independently)):

```
ng serve --open
```