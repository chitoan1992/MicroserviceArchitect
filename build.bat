docker network create my-net
docker run -d --name kong-database \
                --network my-net \
                -p 5432:5432 \
                -e "POSTGRES_USER=kong" \
                -e "POSTGRES_DB=kong" \
                postgres:alpine

docker run --rm \
    --network my-net \
    -e "KONG_DATABASE=postgres" \
    -e "KONG_PG_HOST=kong-database" \
    kong:latest kong migrations up

docker run -d --name kong \
    --network my-net \
    -e "KONG_DATABASE=postgres" \
    -e "KONG_PG_HOST=kong-database" \
    -e "KONG_PROXY_ACCESS_LOG=//dev/stdout" \
    -e "KONG_ADMIN_ACCESS_LOG=//dev/stdout" \
    -e "KONG_PROXY_ERROR_LOG=//dev/stderr" \
    -e "KONG_ADMIN_ERROR_LOG=//dev/stderr" \
    -e "KONG_ADMIN_LISTEN=0.0.0.0:8001" \
    -e "KONG_ADMIN_LISTEN_SSL=0.0.0.0:8444" \
    -p 8000:8000 \
    -p 8443:8443 \
    -p 8001:8001 \
    -p 8444:8444 \
    kong:latest

REM docker run -it debian bash -c 'exec env PATH=/home/app:$PATH bash'

docker container exec -u root kong bash -c "kong health & kong version"

REM KONG DASHBOARD
docker run --rm --network my-net -p 8080:8080 pgbi/kong-dashboard start --kong-url http://kong:8001

REM KONGA
docker run -d --name kongadb \
                --network my-net \
                -e "POSTGRES_USER=konga" \
                -e "POSTGRES_PASSWORD=k0ng@123-" \
                -e "POSTGRES_DB=konga_database" \
                postgres:alpine

docker run -d -p 1337:1337 \
             --name konga \
             --network my-net \
             -e "DB_ADAPTER=postgres" \
             -e "DB_HOST=kongadb" \
             -e "DB_USER=konga" \
             -e "DB_PASSWORD=k0ng@123-" \
             -e "DB_DATABASE=kongadb" \
             -e "NODE_ENV=development" \
             pantsel/konga


REM cd ../InventoryService
REM dotnet ef dbcontext scaffold "server=192.168.99.100,5433;user=sa;password=Pass@word;database=Inventory" Microsoft.EntityFrameworkCore.SqlServer -o Models -f
REM cd ../ShippingService
REM dotnet ef dbcontext scaffold "server=192.168.99.100,5433;user=sa;password=Pass@word;database=Shipping" Microsoft.EntityFrameworkCore.SqlServer -o Models -f
REM cd ../AccountingService
REM dotnet ef dbcontext scaffold "server=192.168.99.100,5433;user=sa;password=Pass@word;database=Accounting" Microsoft.EntityFrameworkCore.SqlServer -o Models -f

cd src
docker-compose -f docker-compose.yml -f docker-compose.override.yml build
docker-compose -f docker-compose.yml -f docker-compose.override.yml up

docker run -d --name sonarqube -p 9000:9000 -p 9092:9092 -e SONARQUBE_JDBC_USERNAME=sonar -e SONARQUBE_JDBC_PASSWORD=sonar -e SONARQUBE_JDBC_URL=jdbc:postgresql://localhost/sonar sonarqube