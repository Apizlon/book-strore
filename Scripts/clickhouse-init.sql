CREATE DATABASE IF NOT EXISTS book_metrics;

CREATE TABLE IF NOT EXISTS book_metrics.book_metrics_merge_tree (
    event_date DateTime DEFAULT now(),
    event LowCardinality(String),
    status LowCardinality(String),
    name String,
    user_name String
) ENGINE = MergeTree()
ORDER BY (event_date, event, status)
PARTITION BY toYYYYMM(event_date);


CREATE TABLE IF NOT EXISTS book_metrics.book_metrics_kafka (
    event_date DateTime,
    event LowCardinality(String),
    status LowCardinality(String),
    name String,
    user_name String
) ENGINE = Kafka()
SETTINGS 
    kafka_broker_list = 'kafka:9092',
    kafka_topic_list = 'book-metrics',
    kafka_group_id = 'clickhouse-group',
    kafka_format = 'JSONEachRow',
    kafka_num_consumers = 1;


CREATE MATERIALIZED VIEW IF NOT EXISTS book_metrics.book_metrics_mv
TO book_metrics.book_metrics_merge_tree
AS SELECT
    event_date,
    event,
    status,
    name,
    user_name
FROM book_metrics.book_metrics_kafka;
