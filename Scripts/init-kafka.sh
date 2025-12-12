#!/bin/bash

set -e

echo "Creating Kafka topics..."

KAFKA_BROKER="kafka:9092"

# Function to create topic if it doesn't exist
create_topic() {
    local topic_name=$1
    local partitions=${2:-1}
    local replication_factor=${3:-1}

    kafka-topics --bootstrap-server $KAFKA_BROKER --create \
        --topic $topic_name \
        --partitions $partitions \
        --replication-factor $replication_factor \
        --if-not-exists 2>/dev/null || true
    
    echo "Topic '$topic_name' ready"
}

# Create required topics
create_topic "notifications" 3 1
create_topic "book-metrics" 3 1
create_topic "user-metrics" 3 1

echo "All Kafka topics created successfully!"