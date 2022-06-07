using TimeTracker;

var db = new DatabaseManager();
db.CreateDatabase();

var ui = new UIManager(db);
ui.MenuLoop();
