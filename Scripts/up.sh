#!/bin/bash

set -e

echo "Starting Docker containers..."

docker-compose -f docker-compose.yml up -d

echo "Waiting for services to be healthy..."
sleep 10

echo "All services started successfully!"
echo ""
echo "Services URLs:"
echo "  - Nginx Reverse Proxy: http://localhost"
echo "  - Auth Service: http://localhost:5001"
echo "  - User Service: http://localhost:5002"
echo "  - PostgreSQL: localhost:5432"
echo "  - Kafka: localhost:9092"
echo "  - Prometheus: http://localhost:9090"
echo "  - Grafana: http://localhost:3000"
echo "  - ClickHouse: localhost:8123"
echo ""
echo "Default credentials:"
echo "  - PostgreSQL: postgres/postgres"
echo "  - Grafana: admin/admin"