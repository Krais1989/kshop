:: Orders database
start docker exec -it kshop_dbs_orders_1 mysql --password="asd" --database="db_orders" --execute="DROP DATABASE db_orders; CREATE DATABASE db_orders; GRANT ALL PRIVILEGES ON db_orders.* TO 'asd'@'%';"

:: Products database
start docker exec -it kshop_dbs_products_1 mysql --password="asd" --database="db_products" --execute="DROP DATABASE db_products; CREATE DATABASE db_products; GRANT ALL PRIVILEGES ON db_products.* TO 'asd'@'%';"

:: Payments database
start docker exec -it kshop_dbs_payments_1 mysql --password="asd" --database="db_payments" --execute="DROP DATABASE db_payments; CREATE DATABASE db_payments; GRANT ALL PRIVILEGES ON db_payments.* TO 'asd'@'%';"

:: Shipments database
start docker exec -it kshop_dbs_shipments_1 mysql --password="asd" --database="db_shipments" --execute="DROP DATABASE db_shipments; CREATE DATABASE db_shipments; GRANT ALL PRIVILEGES ON db_shipments.* TO 'asd'@'%';"

pause;