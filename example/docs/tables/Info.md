# Info

## Columns

| Column    | Description | Type     |
| --------- | ----------- | -------- |
| Label     |             | String   |
| Text      |             | String   |
| Timestamp |             | DateTime |

## Measures

| Measure            | Description | Expression                                                    |
| ------------------ | ----------- | ------------------------------------------------------------- |
| Branch             |             | LOOKUPVALUE(Info\[Text\], Info\[Label\], "Branch")            |
| Data Updated (UTC) |             | LOOKUPVALUE(Info\[Timestamp\], Info\[Label\], "Data Updated") |
| Dataset Version    |             | LOOKUPVALUE(Info\[Text\], Info\[Label\], "Version")           |
| Environment        |             | LOOKUPVALUE(Info\[Text\], Info\[Label\], "Environment")       |
| GitHub RunID       |             | LOOKUPVALUE(Info\[Text\], Info\[Label\], "GitHub RunID")      |
| GitHub SHA         |             | LOOKUPVALUE(Info\[Text\], Info\[Label\], "GitHub SHA")        |
