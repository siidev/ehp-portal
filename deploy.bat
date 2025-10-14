@echo off
echo Building and deploying SSOPortalX...

echo Stopping existing containers...
docker-compose down

echo Building new image...
docker-compose build --no-cache

echo Starting application...
docker-compose up -d

echo Checking container status...
docker-compose ps

echo Deployment completed!
echo Application should be available at: http://localhost:8080

echo Showing logs (Ctrl+C to exit)...
docker-compose logs -f