# Lab1_SGBD
The problem requirment

For the database you modeled in the first semester, you need to implement a Windows Solution using the .NET Framework. 
The application must contain at least one form through which the user can manipulate the data of two tables. 
The two tables must be in a 1:M relationship. We call these tables parent table and child table.
The application must implement the following operations:

->Display of all tuples of the father table
->When selecting a tuple from the father table, all children of the tuple must be displayed.
->When selecting a tuple from the child table, the user can modify or delete the tuple.
(Note: you must be able to change at least two columns, one of which must be different from a string (i.e. it can be of type Integer, Float, Date, DateTime, Time, etc.).)
->When selecting a tuple from the father table, the user can insert a new child (a new tuple in the child table, which is subordinate to the selected tuple).

For the connection between application and database you have to use Datasets and DataAdapters. 

Note: If you do not have the database from the first semester, you must first model and create a database in SQL Server that meets the following conditions:
      -contains 10 tables
      -at least 2 tables are in a 1:M relationship
      -at least 2 tables are in an M:M relationship
