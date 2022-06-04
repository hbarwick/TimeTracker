using System.Configuration;
using System.Collections.Specialized;
using CodingTracker;

var db = new DatabaseManager();
db.CreateDatabase();
var newSession = new Session();

DateTime start = DateTime.Parse("03/06/2022 09:55:23");
DateTime end = DateTime.Now;

newSession.StartTime = start;
newSession.EndTime = end;

Console.WriteLine(newSession.Duration);

var ui = new UIManager(db);


ui.MenuLoop();




