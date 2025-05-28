# Smart Web Scraper
This project is designed to help with SEO analysis using popular search engines like Google and Bing.

### Overview:
The image below shows the UI design. You enter the keyword(s) on the left and search. Rankings show on the right pane. You can then save the result or fetch the search history
<img width="1268" alt="Screenshot 2025-05-27 at 12 44 15 PM 1" src="https://github.com/user-attachments/assets/611b89d0-d3f7-4910-97a4-65fd787b95cd" />

### API Characteristics
* The API exposes three endpoints: Keyword search, save results, and search history
* **keyword-search** takes a search phrase and a target URL, carries out the search, scrapes the result, and returns a collection of rankings and corresponding URLs
* **save-results** allows the user to optionally save the results of their search
* **search-hisory** fetches the history of previous searches, and displays them on the UI. Clicking a specific search expands the rankings for that search on the right pane.

![image](https://github.com/user-attachments/assets/93e6bd2a-6f3e-4623-96a1-d405223c171d)

### How to set up and run the application
1. Create a .env file in the project root (same location as docker-compose and .sln)
2. Paste the following into it (you may replace "YourStr0ngP@ssword" with another strong password in both the password env variable and connection string):

```
MSSQL_SA_PASSWORD=YourStr0ngP@ssword
ConnectionString = "Server=smartwebscraper.db;Initial Catalog=SmartWebScrapperDb;Persist Security Info=False;User ID=sa;Password=YourStr0ngP@ssword;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Connection Timeout=30;"
```
(Note: Weak passwords like 'password' will fail)

3. Open a terminal in the project root directory or in VS Code (CLI recommended)
4. Run docker-compose down
5. Run docker-compose up -d --build --force-recreate
6. You might need to wait for a few minutes for all the containers to spin up (client, server, and db), and install all dependencies
7. The image below (from Docker logs) indicates that the client is ready:
   ![image](https://github.com/user-attachments/assets/11a32e20-af17-4b92-a35f-362abb7cd0ae)

9. When it is fully up, you can access the client on localhost:4200. The server is also accessible on port 8080 (localhost:8080/swagger to view API documentation)

### How to use the application (from the client)
* Type in a URL
* **Type in a keyword and press Enter** to add it (You can add multiple keywords)
* Click 'Search' to carry out the web scraping
* You might need to wait for a few seconds for the results to show (a deliberate time delay of 0.5 seconds is added for each page in the API, to obtain the correct result)
* To get the search history, you can click on the 'History' button, and the history appears on the left.
* Select any history entry to expand it on the right.
* You can test with 'www.gov.uk' as the URL and 'land registry search' as the search phrase. (I couldn't find any top 100 rankings for infotrack.co.uk on Bing)

### Core Libraries and Design
* UI - Angular, HTML, SCSS, TypeScript
* API - .NET 9, MediatR, FluentValidation
* Testing: xUnit, Moq, FluentAssertion
* Containerisation - Docker & Docker-Compose (To ensure stability across environments)
* Projects: API, Application, Domain, Infrastructure, Persistence

### Constraints
* Within the project timeframe, I was unable to figure out a way to manually scrape Google searches without the use of 3rd party libraries
* I only implemented Bing search. Future work will include Google and other search engines

### Improvement Areas/Future work
* Adding AI functionalities to analyse the rankings with the search results and recommend SEO adjustments
* Support for other search engines.
* Visualisation of the ranking over time.
* Result cashing to improve performance for similar searches

### Additional Notes:
* This project could be achieved using a minimal API due to its magnitude, but I have used a full controller-based API to demonstrate some architecture and design considerations.
* Authentication and authorisation could be added to ensure only authorised access to the API, but we have kept it simple for now.


