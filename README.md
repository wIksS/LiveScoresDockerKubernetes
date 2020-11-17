# Live scores project. [Docker, Kubernetes, RabbitMQ, GraphQL]



![Live scores diagram](/Diagrams/livescores.png)

## Microservices

-LiveScoresCrawlerSource
	- Created with a web crawler
	- Connects to rabbitmq and sends found data
	- Stateless and easily scalable
	
-LiveScoresAPI
	- Consumes games from rabbitmq
	- Serves queries about games using GraphQL
	- Writes and reads from SQL server (not scalable, preferrably we would have a distributed database and put everything there

## Used technologies

- ASP.NET Core
- RabbitMQ
  - Give us support of high income of messages
  - If a message cannot be processed it is not being lost
  - Helps us implement Pub/Sub pattern
- Docker
- Kubernetes
	- Helps us orchestrate our microservices
	- Can easily be hosted in google cloud, aws, etc.
	- We can easily span more istances of our microservices for scalability
- GraphQL
	- Helps us with flexible queries over games (very simplistic right now)
- HangFire
	- Used to retrigger crawling of sources
	- Used with an in memory database because the crawling intervals are 1 minute
	Can be easily changed to SQL db.
	
## Future work

- Hide all sensitive data. Use environment variables, hashing, etc.
- Checking for unique games is done in a horrible manner. It pull all of the data in memory and checks it (not scalable in any way)
	- a much better approach would be to use a distributed cache (redis for example) that would hold the games in the last two hours.
	This way we would be able to scale our data.
- SQL instance is only one and cannot be replicated. Use a distributed database like cassandra.
	- This is one of the bigger pittfalls in the application as it will limit scalability
- Configure rabbitmq to be distributed across nodes. Right now in the kubernetes config it spawns only 1 replica.
- Clean unneccessary usings, comments, code duplication all done in a hurry.
- Have fun with the project