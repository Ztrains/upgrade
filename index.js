const express = require('express')
const mongodb = require('mongodb')
const bodyParser = require('body-parser')

const app = express()
app.use(bodyParser.json())

//const port = 3000
var db;
var localTestUrl = 'mongodb://localhost:27017/test'

mongodb.MongoClient.connect(process.env.MONGODB_URI || localTestUrl, function (err, database) {
  if (err) {
    console.log('error:', err);
    process.exit(1);
  }

  // Save database object from the callback for reuse.
  db = database;
  console.log("Database connection ready");

  // Initialize the app.
  var server = app.listen(process.env.PORT || 8080, function () {
    var port = server.address().port;
    console.log("App now running on port", port);
  });
});

app.get('/',(request,response)=>{
    response.send('Welcome to upgrade')
})

/*app.listen(port, (err)=>{
    if (err) return console.log('error', err)
    console.log('server is listening on ' + port)
})*/
