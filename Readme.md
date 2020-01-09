# Link Controller

The link controller architecture is Microsoft Orleans. See [github](https://github.com/dotnet/orleans), and the [documentation](http://dotnet.github.io/orleans/Documentation/index.html) for more information.

## Concept

The concept of the Link Controller is to link 2 systems together. The Link Controller can also be used to convert the data models between 2 api endpoints.

### Link Controller Types

A PMS adapter will be a link controller type. They will be a single api endpoint for all hotels. Each hotel will have a unique identifier to determine how to route the messages it receives. e.g. Opera cloud will have a single opera cloud endpoint for all instances. Impala will be a single api for everyone. Htng will be a generic connection.

#### (TODO) Htng Connection _REST_

This will be a generic message format type that follows htng and works similar to Opera Cloud. We could tell a third party to implement an api that is htng defined and then create a htng link and it will just work.

The third party would either need to implement a queue of messages and the link controller would poll them to get the next message, or create a generic rest htng api.

### Outbound Links (Publish)

A user would setup a connection to an external api by giving it the endpoint of the target pms api. The configuration will have a hotel identification that is used to identify the hotel link. An ountbound message could be published to more than one location.

### Inbound Links (Subscribe)

A user would provide the endpoint to forward the messages from the given hotel. All messages are forwarded onto the endpoint after they have been mapped to the format required. Subscribers of inbound links can choose how they want to receive the messages. Each choice will need to be implemented for every pms type. See Opera Cloud for more information how this is done.

## Database setup

The database uses the script files in the SiloHost project to store the configuration into the database. These are created by Microsoft and are generic grain datastore tables and store the information as json. The databse can be most database types e.g. Postgres, SQL Server, Mongo etc. 

## How to run locally

The silohost is the core of the project and it needs to be running for any of the other projects to run. If you do not have this project running in the background then you will get errors.

- Open the project root folder in Visual Studio Code
- press F5 and it will run the silo host project
- Open the project in Visual Studio and set the project you want to run as the start up project and run

_note: Make sure you do not have the project open in another process and running as it needs to build the project before running_

Visual Studio has a free Community Edition version that is satisfactory for developing this product.

## General Folder Structure

The folder structure should follow conventions where possible. This will make it easier to find and organise as it grows. 

### Pms Adapters

New links (i.e. pms adapters) are named LinkController.[pmsType] 
They would normally be expected to have an interface project and an implementation project for the business logic. They will need to include api's to configuration and set up the inbound and outbound connections. This should be a rest interface using swagger ui.

### Service Extentions

The service extensions are for shared resources. I.e. there are helpers for wcf and orleans. In the future this would grow to include webhooks and polling libraries.
Ideally these could become nugets once they have been improved.