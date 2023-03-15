```
Author:     Tyler DeBruin
Partner:    Rayyan Hamid
Date:       4-21-2022
Course:     CS 3500-001, University of Utah, School of Computing
GitHub ID:  TylerDeBruin & notrh99
Repo:       https://github.com/Utah-School-of-Computing-de-St-Germain/assignment-nine---web-server-sql-up-in-the-clouds.git
Commit #:   7f3acd337ce1349f6fa6f79140b30b183f7e6d89
Solution:   WebServer
Copyright:  CS 3500 and Tyler DeBruin and Rayyan Hamid - This work may not be copied for use in Academic Coursework.
```

# Comments to Evaluators:

To set the secrets file, right click the webserver project and 'Manage User Secrets'. We stored the secrets to utilize in our tests as well.

We heavily utilized the starter code - and the Networking DLL provided for the last assignment. 

I've done a lot of web development prior, with the exception of creating our own HTTP Listener - so this assignment was a little easier than the previous ones, we included the references
we ended up using below. Displaying the google chart required us to use the developer reference they provided - and even that was following their documentation, and formatting our data in a way that fit their model. 


# Assignment Specific Topics

We used the dependency injection framework to setup the WebServer as a console app. The Program.cs houses the container setup.
The secrets.json is loaded within that framework, and injected into the Repository.cs as a string. 

The tests utilize this same setup to run against the database. The integration tests allow us to test the database is working, and to debug. The Database is setup in a way to 
really only do CRUD operations, and the logic is in the Repository/Service Classes.

We created a secrets file following the documentation in reference 3. 

Lastly, we used Google's APIs to draw the fancy bar chart in a very easy format that matches the remaining way we implemented the service.


# Consulted Peers:
	Lectures, Lab, and references below. 


# References:

1. https://stackoverflow.com/questions/5488589/sql-data-adapter-insert-command
2. https://stackoverflow.com/questions/7917695/sql-server-return-value-after-insert
3. https://makolyte.com/how-to-add-user-secrets-in-a-dotnetcore-console-app/ 
4. https://developers.google.com/chart/interactive/docs/gallery/barchart 