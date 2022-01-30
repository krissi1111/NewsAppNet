# NewsAppNet


This is the backend of my newsapp project. <br />
It is meant to be run in conjunction with the frontend [here](https://github.com/krissi1111/newsapp-redux)

The project is set up to run using Docker and can be run using the docker-compose.yml file seen below.

The backend is set up to run using a database that is only supplied through docker using the MSSQL image so running the backend independently doesn't really work.

To run the project save the docker-compose.yml file on your computer and then call 

### `docker-compose -f (your path here)\docker-compose.yml up`

assuming you have docker installed.

The frontend can then be viewed at http://localhost:3000/ in your browser.

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

