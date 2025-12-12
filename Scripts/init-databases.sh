#!/bin/bash

set -e

echo "Initializing databases..."

# Create databases if they don't exist
psql -v ON_ERROR_STOP=1 --username "postgres" <<-EOSQL
    SELECT 'CREATE DATABASE auth_db' WHERE NOT EXISTS (SELECT 1 FROM pg_database WHERE datname = 'auth_db')\gexec
    SELECT 'CREATE DATABASE user_db' WHERE NOT EXISTS (SELECT 1 FROM pg_database WHERE datname = 'user_db')\gexec
    SELECT 'CREATE DATABASE notification_db' WHERE NOT EXISTS (SELECT 1 FROM pg_database WHERE datname = 'notification_db')\gexec
    SELECT 'CREATE DATABASE book_db' WHERE NOT EXISTS (SELECT 1 FROM pg_database WHERE datname = 'book_db')\gexec
EOSQL

echo "Databases initialized successfully!"