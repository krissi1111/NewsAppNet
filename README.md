# NewsAppNet


This is the backend of my newsapp project. <br />
It is meant to be run in conjunction with the frontend [here](https://github.com/krissi1111/newsapp-redux)

The project is set up to run using Docker and can be run using the docker-compose.yml file seen below.



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

