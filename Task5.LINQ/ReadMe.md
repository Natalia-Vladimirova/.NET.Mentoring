## Task 5 - LINQ

### General Info

To complete the sub tasks for this task it needs to modify the demo application. Modify it according to the following rules:
+ For each sub task create a new method in the LinqSamples class with the name LinqXXX where XXX is a number of the sub task
+ Specify Category, Title, Description attributes for the method
+ The method should contain necessary initialization, LINQ query and results
+ Run the application (F5) to check a result.

P.S. It is not obligatory to run the application; usual unit tests are enough (a test per a sub task)

### Sub tasks
1. Give a list of customers where total turnover (the sum of all orders) is more than a certain value of X. Demonstrate the query execution with different values of X (do it without copying the query)
2. For each customer create a list of suppliers located in the same country and the same city. Do this task using grouping and without it.
3. Find all customers who have orders exceeding the value of X by sum.
4. Give a list of customers indicating from which month of which year they became customers (get this month and this year from the very first order).
5. Give a list of customers indicating from which month of which year they became customers (get this month and this year from the very first order). Sort the list by year, month, orders total sum (from max to min) and customer id.
6. Find all customers who have a non-digital postal code or a region is empty or an operator code is not specified in the phone (for simplicity, phone has no round brackets at the beginning).
7. Group all products by categories, inside - by stock, inside the last group - order by price.
8. Group products by groups "cheap", "average price", "expensive". Choose borders of each group by yourselves.
9. Calculate the average profitability of each city (the average order sum for all customers from a given city) and the average intensity (the average amount of orders per customer from each city).
10. Create average annual activity statistics of customers by months (without years), statistics by years, by years and by months (that is, when one month in different years is important).
