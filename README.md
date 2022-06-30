# NewsAppNet


This is the backend of my newsapp project. <br />
It is meant to be run in conjunction with the frontend [here](https://github.com/krissi1111/newsappretype)

To run the backend you can clone the repository and open with Visual Studio and you should be able to just run it.

On first run an Sqlite local database will be created in project directory and seeded with some data.

An admin user will be created and after running the frontend you can log in as admin with username: admin and password: Superadmin1
Alternatively you can register a new user.

The project is set up to run using Docker but the docker images are not up to date at the moment so ignore instructions below

### `Ignore below`

The project is set up to run using Docker and can be run using the docker-compose.yml file seen below.

To run the project save the docker-compose.yml file on your computer and then call 

### `docker-compose -f (your path here)\docker-compose.yml up`

using cmd, assuming you have docker installed.

The frontend can then be viewed at http://localhost:3000/ in your browser.

The database will be automatically seeded with some data including an admin account. The admin account can be accessed by logging in with username: admin and password: admin. Alternatively you can register your own account. 

docker-compose.yml
```

version: '3.4'

volumes:
    news:    

services:
    newsappdb:
        image: "mcr.microsoft.com/mssql/server:2019-latest"
        environment:
            ACCEPT_EULA: Y
            SA_PASSWORD: Password123
        ports:
            - 1433:1433
        volumes:
            - news:/var/opt/mssql

    newsappnet:
        image: stjani11/newsapp_backend:latest
        depends_on:
            - newsappdb
        ports:
            - 5000:80

    newsapp-redux:
        image: stjani11/newsapp_frontend:latest
        ports:
            - 3000:3000
        environment:
            - CHOKIDAR_USEPOLLING=true
            - REACT_APP_BACKEND_PORT=5000
```

