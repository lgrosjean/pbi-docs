# Sales

## Columns

| Column        | Description | Type     |
| ------------- | ----------- | -------- |
| Currency Code |             | String   |
| CustomerKey   |             | Int64    |
| Delivery Date |             | DateTime |
| Exchange Rate |             | Double   |
| Line Number   |             | Int64    |
| Net Price     |             | Decimal  |
| Order Date    |             | DateTime |
| Order Number  |             | Int64    |
| ProductKey    |             | Int64    |
| Quantity      |             | Int64    |
| StoreKey      |             | Int64    |
| Unit Cost     |             | Decimal  |
| Unit Price    |             | Decimal  |

## Measures

| Measure        | Description | Expression                                              |
| -------------- | ----------- | ------------------------------------------------------- |
| Margin         |             | \[Sales Amount\] \- \[Total Cost\]                      |
| Sales Amount   |             | SUMX ( Sales, Sales\[Quantity\] \* Sales\[Net Price\] ) |
| Sales Quantity |             | SUM ( Sales\[Quantity\] )                               |
| Total Cost     |             | SUMX ( Sales, Sales\[Quantity\] \* Sales\[Unit Cost\] ) |
