# Athena
Athena is the API that allows to check the availability of the product at the certain retailers' website.
The APi perodically checks the availability of the certain products. 
The results are stored temporarly in redis to reduce the database workload.
## Data access
The data access for the development environment utilises the Docker image of the SQLServer. 
The prerequisities for this to work is the Docker installed either as the Docker Desktop on Windows or the Linux Engine for Docker.
The image used ius the clean image of the Microsoft SQL Server that can be downloaded and run via the command: 
> docker run -e 'ACCPT_EULA=Y' -e 'SA_PASSWORD=zaq1@WSX' -p 11443:1433 -d mcr.microsoft.com/mssql/server:2019-latest

The default password for the instance of the database on the docker image is zaq1@WSX. The database is set up on port 11443 of the local machine.

## Cache definition
The Athena utilises the Redis Cache to reduce the load on the database. The Redis Cache used for the development purposes can be launched by the following command.
> docker run --name athena_cache -d redis

This command sets up the redis cache that is later accessed by the API to retrieve the current availability of the certain product.