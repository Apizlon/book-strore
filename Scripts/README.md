npm i
npm run dev

пример запроса в клик 
SELECT
    COUNT(1) FROM "book_metrics"."book_metrics_merge_tree" WHERE $__timeFilter(event_date) AND event = 'Login' AND status = 'Failure'
настроить дашики и сохранить их, чтобы они автоматически запускались