# Athena
Athena is the API that allows checking the availability of the product at certain retailers' website.
The API periodically checks the availability of particular products. 
The results are stored temporarily in Redis to reduce the database workload.
## Data access
The data access for the development environment utilises the Docker image of the SQLServer. 
The prerequisites for this to work is the Docker installed as Docker Desktop on Windows or the Linux Engine for Docker.
The image used in the clean image of the Microsoft SQL Server can be downloaded and run via the command: 
> docker run -e 'ACCPT_EULA=Y' -e 'SA_PASSWORD=zaq1@WSX' -p 11443:1433 -d mcr.microsoft.com/mssql/server:2019-latest

The default password for the instance of the database on the docker image is zaq1@WSX. The database is set up on port 11443 of the local machine.

## Cache definition
The Athena utilises the Redis Cache to reduce the load on the database. The following command can launch the Redis Cache used for development purposes.
> docker run --name athena_cache -d redis

This command sets up the Redis cache that is later accessed by the API to retrieve the current availability of the particular product.

This repository is a fork of a [stefangrosaru/Athena](https://github.com/stefangrosaru/Athena) repository. The initial idea has been created as a solution to the CIS015-3 unit Social and Professional Project Management at the University of Bedfordshire.