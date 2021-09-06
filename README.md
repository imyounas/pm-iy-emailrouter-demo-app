# Demo Email Router App

## Overview
This is a Demo Email Router .NET Core 5 Solution. The project structure is based on 'Clean Architecture'. I have tried to touch all aspects of the problem, keeping in view the original purpose of this exercise. 

This project uses following 3rd party libraries: 
 - [RabbitMQ.Client](https://www.rabbitmq.com/dotnet.html) for managing Email messages in Queue.
 - [MediatR](https://github.com/jbogard/MediatR) for loose couping and managing Query and Commands from Controllers.
 - [Fluent Validation](https://fluentvalidation.net/) for DTO/ViewModel validations.
 - [Serilog](https://serilog.net/) for Logging.

Here is the Solution Structure
 - PM.IY.EmailRouterDemo => Main WEBAPI Project to entertain REST API Requests and it is also hosting a Background service to consume RabbitMQ messages. Ideally, this background service should be hosted in a separate solution but for this demo, I have added it into the main Project. 
 - ER.Domain => To manage the core domain of the application. 
 - ER.Application => To manage the main Application business logic, UseCases, Interfaces, DTOs/ViewModels  
 - ER.Infrastructure => To provide external services implementation like DB, Message Queue, Cache etc.
 - ER.Tests => To have some basic Unit and Integration Tests

## Implemented Solution
The problem scope itself is quite big, but considering it is a test exercise and description states to limit the scope and focus on communication/interface of different components/layers. So I have tried to touch main components and their communication till the actual delivery pipeline of Transactional & Bulk Emails.

Once the User hits REST API Endpoints, before accepting the Email delivery request, it tries to validate (data properties & headers) of the received DTO and that Sender ('From' Email Address) is verified against a Client account (dummy implementation). If the received Email object fails any of the checks , then its status is updated in DB (dummy implementation) and failed response is delivered to the Client. Otherwise, if everything is all right, then Email object is appended with some MetaData and sent to RabbitMQ Email Ingest Queue. 

Here my main thought behind ingesting to Queue is that after doing initial validation, we should push the message to the next phase for further processing and meanwhile send a response to the Client that his/her Email delivery requested has been accepted. 

Then we have Consumer/Subscriber to that Ingest Queue, as a background hosted service. Which should had been ideally hosted separately, in its own process.
This background service reads the Email Request and tries to run some business logic. For this Post-Processing, I am using the 'Chain of Responsibility' pattern to add pluggable business logic. On each step of chain, it adds/updates PostProcessed MetaData to the main EmailRequest object.  
 
After completion of this Post-Processing, it is checked if the Email Request is eligible for delivery or not (based on some dummy logic to check Reputation score, Terms&Conditions etc.)
If it is not eligable then it is added to FailedQueue otherwise depending upon its type (Transactional or Bulk) it is routed to the suitable Queue. 

![Solution Overview](https://github.com/imyounas/pm-iy-emailrouter-demo-app/blob/new-docker-compose/IY.EmailRouter.DemoSloution.png)

For Testing needs, I have added 2 Integration tests, where it will use the existing (configured RabbitMQ settings in the main project) Queue and try to send the message. And some Unit Tests to check that Email message validations are properly working. 

## Sclability & High Availability
I have used Visual Studio Docker Compose to run .NET Core and RabbitMQ in Linux Containers. To make this solution, horizontally scalable automatically and on-demand, we should use Kubernetes. We can scale the pods based on utilization or some other suitable metric. If we are already on AWS, then ECS could also be an option to have managed container orchestration service.

## Improvements
Many, there could be many. I have implemented this solution as per my understanding of the problem. The .NET Container is not published so we have to use Visual Studio Docker Tools to run the Docker Compose. For RabbitMQ Connection, we should use a better connection retry strategy with resilience and fault tolerance something like [Polly](https://github.com/App-vNext/Polly) library.
I could have added consumers/subscribers for Failed, Bulk and Transactional queues to add more depeth to the Solution, but I left it as its implementation would had been same as of Ingest queue consumer.  

## How to Run
Either use Visual Studio Docker Compose Tools to run the whole solution. Or you can run the solution separately in VS Code or Visual Studio and run RabbitMQ Docker Container by using this command:
  
  ```
   docker run -it --rm --name local-iy-rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:3-management
  ```
AppSettings file is configured to connect to RabbitMQ on 'localhost'. You can change its value or can also provide equivalent Environment variables against AppSettings variables as done in Docker Compose file.
